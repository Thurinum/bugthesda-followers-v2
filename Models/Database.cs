using System;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SessionProject2W5.Models
{
	/// <summary>
	/// Une base de données des compagnions pour des jeux vidéos. Sérialise et désérialise les données depuis un fichier XML.
	/// </summary>
	/// <remarks>
	/// Certains checks sont en place pour sanitize les données, mais tout est loin d'être parfait! Le design reste cependant suffisant pour ce projet...
	/// </remarks>
	public class Database
	{
		#region Propriétés publiques
		/// <summary>
		/// La liste des jeux de la base de données.
		/// </summary>
		public List<Game> Games { get; set; }

		/// <summary>
		/// La liste des tous les compagnions de la DB, tous jeux confondus.
		/// Cette propriété s'avère utile pour le filtrage de la recherche.
		/// </summary>
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

		/// <summary>
		/// La liste des compagnions marqués comme Favoris dans la base de données.
		/// </summary>
		public List<Follower> Favorites;

		/// <summary>
		/// Les informations partagées entre tous les personnages, soit la race, la classe, etc.
		/// Les compagnions font référence aux entités de cette classe.
		/// </summary>
		public SharedInfo SharedInfo;

		/// <summary>
		/// Une liste d'erreurs du chargement des données, séparées par des newlines.
		/// Cette valeur est passée au ViewData au chargement de la base de données.
		/// </summary>
		public string ErrorString { get; private set; }
		#endregion

		/// <summary>
		/// Instantie une nouvelle base et désérialise un fichier XML pour populer les données.
		/// </summary>
		/// <param name="path">Le chemin vers le fichier XML à désérialiser.</param>
		/// <exception cref="ArgumentException">Si le chemin vers le fichier XML n'existe pas (ce qui serait triste).</exception>
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
			int attri(string name)
			{
				string value = attr(name);
				int converted = -1;

				try
				{
					converted = int.Parse(value);
				}
				catch (Exception e)
				{
					if (value.Length > 10)
						value = value.Substring(0, 10) + "...";

					this.ErrorString += $"Could not to convert attribute '{name}' with value '{value}' to integer in follower '{follower?.Name}' of game '{game?.Name}': {e.Message}\n";
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
				catch (Exception)
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
						Class _class = new Class
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
						Ability ability = new Ability
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
							ShortName = attr("shortname"),
							Name = attr("name"),
							Description = attr("description"),
							UnlockContext = attr("unlockcontext"),
							Hitpoints = attri("hitpoints"),
							Energy = attri("energy"),
							Alignment = attri("alignment"),
							Protection = attrb("essential") 
									? Follower.ProtectionLevel.Essential 
									: (attrb("respawns") 
										? Follower.ProtectionLevel.Protected 
										: Follower.ProtectionLevel.None),
							Parent = game,
							ParentId = game.Id							
						};

						// ensure unique id
						if (followerids.Contains(follower.Id))
							this.ErrorString += $"Duplicate id {follower.Id} for follower '{follower.Name}' in game '{game.Name}'.\n";
						followerids.Add(follower.Id);

						// FIXME: Will crash if access to race later (must do constructor)
						int raceid = attri("raceid");
						try
						{
							follower.RaceId = raceid;
							follower.Race = this.SharedInfo.Races[raceid];
							follower.Race.Population++;
						}
						catch (Exception)
						{
							this.ErrorString += $"Race id {raceid} of follower {follower.Name} is invalid.\n";
						}

						int classid = attri("classid");
						try
						{
							follower.ClassId = classid;
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
						while (reader.Read() && reader.Name == "quote")
						{
							Quote quote = new Quote
							{
								context = attr("context"),
								text = reader.Value
							};

							follower.Quotes.Add(quote);
						}

						game.Followers.Add(follower);

						break;
				}
			}
		}
	}
}
