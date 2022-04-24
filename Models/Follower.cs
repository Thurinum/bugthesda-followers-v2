using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SessionProject2W5.Models
{
	public class Follower
	{
		public int   Id;                    // numeric id (PK)
		public bool  IsFavorite;            // numeric id (PK)
		public int   ParentId;              // TODO: Find better workaround (see Database.cs)
		public string BaseId;				// Ids dans le jeu
		public string RefId;
		public Game  Parent;				// the follower's parent. Same as above
		public Race  Race;
		public Datum Class;

		public string Name;
		public string ShortName;
		public string Description;
		public string UnlockContext;
		public int    Hitpoints;
		public int	  Energy;
		public int    Alignment;
		public bool   IsEssential;
		public bool   DoesRespawn;

		public List<string> Facts;
		public List<Quote> Quotes;
		public List<Datum>  Abilities;

		public Follower()
		{
			Facts = new List<string>();
			Quotes = new List<Quote>();
			Abilities = new List<Datum>();
		}
	}
}