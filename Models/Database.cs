using System;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SessionProject2W5.Models
{
	public class Database
	{
		public List<Game> Games;          // Liste des games
		public List<Follower> Followers
		{
			get
			{
				List<Follower> followers = new List<Follower>();

				foreach (Game game in Games)
					followers.AddRange(game.Followers);

				return followers;
			}
		}
		public List<Follower> Favorites;  // Liste des favoris
		public SharedInfo SharedInfo;     // Informations partagees entre les followers

		// Message d'erreur passe au ViewData dans le controller	
		public string ErrorString { get; private set; }

		public Database(string path)
		{
			if (!File.Exists(path))
				throw new ArgumentException("Le chemin n'existe pas.");

			// XmlSerializer ne supporte PAS les references circulaires (ref au parent dans l'enfant)
			// DataContractSerializer ne support PAS les... attributs (!)
			// On parse donc le XML manuellement et on instantie nos classes
			XmlTextReader reader = new XmlTextReader(path);

			// initialiser collections
			this.Games = new List<Game>();
			this.SharedInfo = new SharedInfo();

			// variables temporaires (membre actuel)
			Game game = null;
			Follower follower = null;
			List<int> gameids = new List<int>();
			List<int> followerids = new List<int>();

			#region Helper functions
			// Helper: obtenir un attribut
			string attr(string name)
			{
				string value = reader.GetAttribute(name);

				if (value == null)
				{
					this.ErrorString += $"Could not find data attribute '{name}' in {reader.Name}.\n";
					value = "<INVALID>";
				}

				return value;
			}

			// Helper: obtenir une attribut et convertir
			int attri(string name) {
				string value = attr(name);
				int converted = -1;

				try {					
					converted = int.Parse(value);
				}
				catch(Exception e) 
				{
					if (value.Length > 10)
						value = value.Substring(0, 10) + "...";
					this.ErrorString += $"Could not to convert attribute '{name}' with value '{value}' to integer in follower '{follower.Name}' of game '{game.Name}': {e.Message}\n";
				}

				return converted;
			};

			// Helper: obtenir un attribut en bool
			bool attrb(string name)
			{
				string val = attr(name);
				bool result = false;
				
				try
				{
					result = bool.Parse(val);
				}
				catch(Exception)
				{
					this.ErrorString += $"Could not convert value '{val}' to boolean.\n";
				}

				return result;
			}
			#endregion

			// voir wwwroot/resources/data.xml
			// FIXME: limitation: la balise sharedinfo doit etre serialisee en premier
			while (reader.Read())
			{
				// le reader lit aussi les balises de fermeture
				if (!reader.IsStartElement())
					continue;

				switch (reader.Name)
				{
					case "race":
						Race race = new Race
						{
							Id = attri("id"),
							ShortName = attr("shortname"),
							NativeName = attr("nativename"),
							CommonName = attr("commonname"),
							Description = attr("description"),
							Color = attr("color")
						};

						this.SharedInfo.Races.Add(race);
						break;
					case "class":
						Datum _class = new Datum
						{
							Id = attri("id"),
							ShortName = attr("shortname"),
							Name = attr("name"),
							Description = attr("description"),
							Color = attr("color")
						};
						this.SharedInfo.Classes.Add(_class);

						break;
					case "ability":
						Datum ability = new Datum
						{
							Name = attr("name"),
							Description = attr("description"),
							Color = attr("color")
						};
						follower.Abilities.Add(ability);

						break;
					case "game":
						game = new Game
						{
							Id = attri("id"),
							Name = attr("name"),
							Color = attr("color"),
							ShortName = attr("shortname"),
							Description = attr("description"),
							YearReleased = attri("released"),
							Tagline = attr("tagline"),
							Director = attr("director"),
							SharedInfo = this.SharedInfo
						};

						// ensure unique id
						if (gameids.Contains(game.Id))
							this.ErrorString += $"Duplicate id {game.Id} for a game named '{game.Name}'.\n";
						gameids.Add(game.Id);

						// parse the game's facts
						reader.ReadToDescendant("facts");
						while (reader.Read() && reader.Name == "fact")
							game.Facts.Add(reader.Value);

						this.Games.Add(game);

						break;
					case "follower":
						follower = new Follower
						{
							Id = attri("id"),
							IsFavorite = attrb("favorite"),
							BaseId = attr("baseid"),
							RefId = attr("refid"),
							ShortName = attr("shortname"),
							Name = attr("name"),
							Description = attr("description"),
							UnlockContext = attr("unlockcontext"),
							Hitpoints = attri("hitpoints"),
							Energy = attri("energy"),
							Alignment = attri("alignment"),
							IsEssential = attrb("essential"),
							DoesRespawn = attrb("respawns"),
							Parent = game
						};

						// ensure unique id
						if (followerids.Contains(follower.Id))
							this.ErrorString += $"Duplicate id {follower.Id} for follower '{follower.Name}' in game '{game.Name}'.\n";
						followerids.Add(follower.Id);

						// FIXME: Will crash if access to race later (must do constructor)
						int raceid = attri("raceid");
						try
						{
							follower.Race = this.SharedInfo.Races[raceid];
							follower.Race.Population++;
						}
						catch(Exception)
						{
							this.ErrorString += $"Race id {raceid} of follower {follower.Name} is invalid.\n";	
						}

						int classid = attri("classid");
						try
						{
							follower.Class = this.SharedInfo.Classes[classid];
						}
						catch (Exception)
						{
							this.ErrorString += $"Class id {classid} of follower {follower.Name} is invalid.\n";
						}

						// parse the follower's quotes
						reader.ReadToFollowing("facts");
						while (reader.Read() && reader.Name == "fact")
							follower.Facts.Add(reader.Value);

						// parse follower's quotes
						reader.ReadToFollowing("quotes");
						while (reader.Read() && reader.Name == "quote") {
							Quote quote = new Quote();
							quote.context = attr("context");
							quote.text = reader.Value;

							follower.Quotes.Add(quote);
						}

						game.Followers.Add(follower);

						break;
				}
			}
		}
	}
}