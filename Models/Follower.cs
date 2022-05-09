using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		public int    Id { get; set; }

		/// <summary>
		/// L'id du parent (jeu) du compagnion. Pas vraiment utilisé puisque le parent
		/// est bound au compagnion par référence circulaire lors de la dé-sérialisation.
		/// </summary>
		[Display(Name = "Jeu Parent")]
		public int    ParentId { get; set; }
		#endregion

		#region Infos de Type
		/// <summary>
		/// Le parent (jeu) du compagnion.
		/// </summary>
		public Game   Parent { get; set; }

		/// <summary>
		/// Le ID de la race du compagnion.
		/// </summary>
		public int RaceId { get; set; }

		/// <summary>
		/// La race du compagnion.
		/// </summary>
		/// <seealso cref="SharedInfo"/>
		public Race   Race { get; set; }

		/// <summary>
		/// Le ID de la classe du compagnion.
		/// </summary>
		public int ClassId { get; set; }

		/// <summary>
		/// La classe de personnage du compagnion.
		/// </summary>
		/// <seealso cref="SharedInfo"/>
		public Class  Class { get; set; }
		#endregion

		#region Biographie
		/// <summary>
		/// Le nom complet du compagnion.
		/// </summary>
		[Required]
		[Display(Name = "Nom complet")]
		[MaxLength(69)]
		public string Name { get; set; }

		/// <summary>
		/// Le nom écourté du compagnion. Il s'agit d'un seul mot en minuscule
		/// utilisé pour former certaines routes et obtenir les chemins d'images.
		/// </summary>
		public string ShortName { get; set; }

		/// <summary>
		/// La description du compagnion.
		/// </summary>
		[Required]
		[MaxLength(666)]
		public string Description { get; set; }

		/// <summary>
		/// Le contexte où le compagnion peut être obtenu.
		/// </summary>
		[Required]
		[Display(Name = "Contexte d'obtention")]
		[MaxLength(420)]
		public string UnlockContext { get; set; }
		#endregion

		#region Statistiques
		/// <summary>
		/// Le nombre de points de vie du compagnion.
		/// En réalité l'unité varie grandement de jeu en jeu, mais mon site en a fait abstraction.
		/// </summary>
		[Required]
		[Display(Name = "Points de vie")]
		[Range(0, 999)]
		public int    Hitpoints { get; set; }

		/// <summary>
		/// Le nombre de point d'énergie du compagnion.
		/// Les jeux ont différentes manières de représenter cette valeur, mais mon site en 
		/// fait abstraction.
		/// </summary>
		[Required]
		[Display(Name = "Points d'énergie")]
		[Range(0, 999)]
		public int	  Energy { get; set; }

		/// <summary>
		/// L'alignement moral du personnage. Les jeux ont différentes manières de représenter cette 
		/// valeur, mais mon site en fait abstraction.
		/// </summary>
		[Required]
		[Display(Name = "Alignement moral")]
		[Range(-100, 100)]
		public int    Alignment { get; set; }
		#endregion

		#region Attributs
		/// <summary>
		/// Indique si le compagnion est un favori.
		/// </summary>
		[Required]
		[Display(Name = "Est un favori?")]
		public bool   IsFavorite { get; set; }

		/// <summary>
		/// Le niveau de protection du compagnion. Un compagnion essentiel ne peut pas mourir.
		/// Un compagnion protégé peut être tué seulement par le joueur.
		/// </summary>
		public ProtectionLevel Protection { get; set; }
		#endregion

		#region Faits et Habiletés
		/// <summary>
		/// Une liste de faits à propos du compagnion.
		/// </summary>
		public List<string> Facts { get; set; }

		/// <summary>
		/// Une liste de citations notoires du compagnion.
		/// </summary>
		public List<Quote>  Quotes { get; set; }

		/// <summary>
		/// Une liste d'habiletés du compagnion. Le terme est plutôt vague et comprend toute sortes
		/// de perks et de skills.
		/// </summary>
		public List<Ability> Abilities { get; set; }
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

		public enum ProtectionLevel
		{
			None	    = 1,
			Protected = 2,
			Essential = 3
		}
	}
}
