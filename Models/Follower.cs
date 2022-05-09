using System.Collections.Generic;

namespace SessionProject2W5.Models
{
	/// <summary>
	/// Un compagnion (l'un des kernels de le DB).
	/// C'est un personnage non-joueur qui peut être recruté au service du joueur.
	/// Les personnalités des compagnions (et bugs associés) les rendent souvent célèbres.
	/// </summary>
	public class Follower
	{
		public int    Id;
		public bool   IsFavorite;
		public int    ParentId;
		public string BaseId;	
		public string RefId;
		public Game   Parent;	
		public Race   Race;
		public Class  Class;

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
		public List<Ability>  Abilities;

		public Follower()
		{
			Facts = new List<string>();
			Quotes = new List<Quote>();
			Abilities = new List<Ability>();
		}
	}
}
