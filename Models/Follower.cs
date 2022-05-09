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
		#region Clés
		/// <summary>
		/// La clé primaire d'un compagnion.
		/// </summary>
		public int    Id;

		/// <summary>
		/// L'id du parent (jeu) du compagnion. Pas vraiment utilisé puisque le parent
		/// est bound au compagnion par référence circulaire lors de la dé-sérialisation.
		/// </summary>
		public int    ParentId;

		/// <summary>
		/// Les deux IDs du compagnion utilisés par le game engine de Bethesda.
		/// </summary>
		public string BaseId;	
		public string RefId;
		#endregion

		#region Infos de Type
		/// <summary>
		/// Le parent (jeu) du compagnion.
		/// </summary>
		public Game   Parent;	

		/// <summary>
		/// La race du compagnion.
		/// </summary>
		/// <seealso cref="SharedInfo"/>
		public Race   Race;

		/// <summary>
		/// La classe de personnage du compagnion.
		/// </summary>
		/// <seealso cref="SharedInfo"/>
		public Class  Class;
		#endregion

		#region Biographie
		/// <summary>
		/// Le nom complet du compagnion.
		/// </summary>
		public string Name;

		/// <summary>
		/// Le nom écourté du compagnion. Il s'agit d'un seul mot en minuscule
		/// utilisé pour former certaines routes et obtenir les chemins d'images.
		/// </summary>
		public string ShortName;

		/// <summary>
		/// La description du compagnion.
		/// </summary>
		public string Description;

		/// <summary>
		/// Le contexte où le compagnion peut être obtenu.
		/// </summary>
		public string UnlockContext;
		#endregion

		#region Statistiques
		/// <summary>
		/// Le nombre de points de vie du compagnion.
		/// En réalité l'unité varie grandement de jeu en jeu, mais mon site en a fait abstraction.
		/// </summary>
		public int    Hitpoints;

		/// <summary>
		/// Le nombre de point d'énergie du compagnion.
		/// Les jeux ont différentes manières de représenter cette valeur, mais mon site en 
		/// fait abstraction.
		/// </summary>
		public int	  Energy;

		/// <summary>
		/// L'alignement moral du personnage. Les jeux ont différentes manières de représenter cette 
		/// valeur, mais mon site en fait abstraction.
		/// </summary>
		public int    Alignment;
		#endregion

		#region Attributs
		/// <summary>
		/// Indique si le compagnion est un favori.
		/// </summary>
		public bool   IsFavorite;

		/// <summary>
		/// Si le compagnion est essentiel. Un compagnion essentiel ne peut pas mourir.
		/// </summary>
		public bool   IsEssential;

		/// <summary>
		/// Si le compagnion est protégé. Un compagnion protégé peut être tué seulement par le joueur.
		/// </summary>
		public bool   IsProtected;
		#endregion

		#region Faits et Habiletés
		/// <summary>
		/// Une liste de faits à propos du compagnion.
		/// </summary>
		public List<string> Facts;

		/// <summary>
		/// Une liste de citations notoires du compagnion.
		/// </summary>
		public List<Quote>  Quotes;

		/// <summary>
		/// Une liste d'habiletés du compagnion. Le terme est plutôt vague et comprend toute sortes
		/// de perks et de skills.
		/// </summary>
		public List<Ability> Abilities;
		#endregion

		/// <summary>
		/// Instantie un nouveau compagnion.
		/// </summary>
		public Follower()
		{
			Facts = new List<string>();
			Quotes = new List<Quote>();
			Abilities = new List<Ability>();
		}
	}
}
