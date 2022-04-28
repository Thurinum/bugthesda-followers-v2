using System;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SessionProject2W5.Models
{
	public class Database
	{
		public List<Game> Games;		  // Liste des games
		public List<Follower> Favorites;  // Liste des favoris
		public SharedInfo SharedInfo;     // Informations partagees entre les followers
		
		public string ErrorString;        // Message d'erreur passe au ViewData dans le controller	

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
				catch(Exception) 
				{ 
					this.ErrorString += $"Could not to convert attribute '{name}' with value '{value}' to integer in {reader.Name}.\n";
				}

				return converted;
			};

			// Helper: obtenir un attribut en bool
			bool attrb(string name)
			{
				return attr(name) == "true";
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
						Race race			= new Race();
						race.Id				= attri("id"); // TODO: Validate IDs, names, etc.
						race.NativeName		= attr("nativename");
						race.CommonName		= attr("commonname");
						race.Description	= attr("description");
						race.Color			= attr("color");
						this.SharedInfo.Races.Add(race);
						break;
					case "class":
						Datum _class = new Datum(); // :P
						_class.Id = attri("id");
						_class.Name = attr("name");
						_class.Description = attr("description");
						_class.Color = attr("color");
						this.SharedInfo.Classes.Add(_class);
						break;
					case "ability":						
						Datum ability		 = new Datum();
						ability.Id			 = attri("id");
						ability.Name		 = attr("name");
						ability.Description	 = attr("description");
						ability.Color		 = attr("color");
						follower.Abilities.Add(ability);
						break;
					case "game":
						game			  = new Game();
						game.Id			  = attri("id");
						game.Name		  = attr("name");
						game.ShortName	  = attr("shortname");
						game.Description  = attr("description");
						game.YearReleased = attri("released");
						game.Tagline	  = attr("tagline");
						game.Director	  = attr("director");
						game.Root		  = this;

						// parse the game's facts
						reader.ReadToDescendant("facts");
						while (reader.Read() && reader.Name == "fact")
							game.Facts.Add(reader.Value);

						this.Games.Add(game);
						break;
					case "follower":
						follower				= new Follower();
						follower.Id				= attri("id");
						follower.IsFavorite		= attrb("favorite");
						follower.BaseId		    = attr("baseid");
						follower.RefId			= attr("refid");
						follower.ShortName		= attr("shortname");
						follower.Name			= attr("name");
						follower.Description	= attr("description");
						follower.UnlockContext	= attr("unlockcontext");
						follower.Hitpoints		= attri("hitpoints");
						follower.Energy		    = attri("energy");
						follower.Alignment		= attri("alignment");
						follower.IsEssential	= attrb("essential");
						follower.DoesRespawn	= attrb("respawns");
						follower.Parent			= game;

						// FIXME: Will crash if access to race later (must do constructor)
						int raceid = attri("raceid");
						try
						{
							follower.Race = this.SharedInfo.Races[raceid];
						}
						catch(Exception)
						{
							this.ErrorString += $"Invalid race id {raceid} for {reader.Name}.\n";							
						}

						int classid = attri("classid");
						try
						{
							follower.Class = this.SharedInfo.Classes[classid];
						}
						catch (Exception)
						{
							this.ErrorString += $"Invalid class id {classid} for {reader.Name}.\n";
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