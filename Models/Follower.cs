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
		/// <summary>
		/// La clé primaire d'un compagnion.
		/// </summary>
		public int    Id { get; set; }

		/// <summary>
		/// L'id du parent (jeu) du compagnion. Pas vraiment utilisé puisque le parent
		/// est bound au compagnion par référence circulaire lors de la dé-sérialisation.
		/// </summary>
		[Required(ErrorMessage = "Le jeu parent est requis.")]
		[Display(Name = "Jeu Parent")]
		public int?   ParentId { get; set; }

		#region Infos de Type
		/// <summary>
		/// Le parent (jeu) du compagnion.
		/// </summary>
		public Game   Parent { get; set; }

		/// <summary>
		/// Le ID de la race du compagnion.
		/// </summary>
		[Required(ErrorMessage = "La race est requise.")]
		[Display(Name = "Race")]
		public int? RaceId { get; set; }

		/// <summary>
		/// La race du compagnion.
		/// </summary>
		/// <seealso cref="SharedInfo"/>
		public Race   Race { get; set; }

		/// <summary>
		/// Le ID de la classe du compagnion.
		/// </summary>
		[Required(ErrorMessage = "La classe est requise.")]
		[Display(Name = "Classe de personnage")]
		public int? ClassId { get; set; }

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
		[Required(ErrorMessage = "Le nom est requis.")]
		[Display(Name = "Nom complet")]
		[MaxLength(69, ErrorMessage = "Le nom est trop long.")]
		[RegularExpression("^[a-zA-Z0-9 àâéèêùûÀÂÉÈÊÙÛ.,:;!?-]*$", ErrorMessage = "Le nom contient des caractères invalides! Lettres, chiffres et ponctuation seulement.")]
		public string Name { get; set; }

		/// <summary>
		/// Le nom écourté du compagnion. Il s'agit d'un seul mot en minuscule
		/// utilisé pour former certaines routes et obtenir les chemins d'images.
		/// </summary>
		public string ShortName { get; set; }

		/// <summary>
		/// L'url d'une image distante, si le follower l'utilise. Sinon, l'image sera obtenue sur le
		/// serveur grâce au ShortName généré.
		/// </summary>
		[Display(Name = "URL Image")]
		[MaxLength(100, ErrorMessage = "L'url d'image est trop long! Max. 100 caractères.")]
		[RegularExpression("^[a-zA-Z0-9.:/@#=_-]*$", ErrorMessage = "L'url contient des caractères invalides!")]
		public string PictureUrl { get; set; }

		/// <summary>
		/// La description du compagnion.
		/// </summary>
		[Required(ErrorMessage = "La description est requise.")]
		[MaxLength(666, ErrorMessage = "C'est une description, pas un roman. Max. 666 caractères.")]
		[RegularExpression("^[a-zA-Z0-9 àâéèêùûÀÂÉÈÊÙÛ.,:;!?-]*$", ErrorMessage = "La description contient des caractères invalides! Lettres, chiffres et ponctuation seulement.")]
		public string Description { get; set; }

		/// <summary>
		/// Le contexte où le compagnion peut être obtenu.
		/// </summary>
		[Required(ErrorMessage = "Le contexte est requis.")]
		[Display(Name = "Contexte d'obtention")]
		[MaxLength(420, ErrorMessage = "Vous voulez qu'on s'endorme ou quoi? Max. 420 caractères.")]
		[RegularExpression("^[a-zA-Z0-9 àâéèêùûÀÂÉÈÊÙÛ.,:;!?-]*$", ErrorMessage = "Le contexte contient des caractères invalides! Lettres, chiffres et ponctuation seulement.")]
		public string UnlockContext { get; set; }
		#endregion

		#region Statistiques
		/// <summary>
		/// Le nombre de points de vie du compagnion.
		/// En réalité l'unité varie grandement de jeu en jeu, mais mon site en a fait abstraction.
		/// </summary>
		[Required(ErrorMessage = "Le nombre de points de vie est requis.")]
		[Display(Name = "Points de vie")]
		[Range(0, 999, ErrorMessage = "Le nombre de points de vie dépasse les limites permises. Il doit être entre 0 et 999.")]
		public int?    Hitpoints { get; set; }

		/// <summary>
		/// Le nombre de point d'énergie du compagnion.
		/// Les jeux ont différentes manières de représenter cette valeur, mais mon site en 
		/// fait abstraction.
		/// </summary>
		[Required(ErrorMessage = "Le nombre d'énergie est requis.")]
		[Display(Name = "Points d'énergie")]
		[Range(0, 999, ErrorMessage = "Le nombre de points d'énergie dépasse les limites permises. Il doit être entre 0 et 999.")]
		public int?	  Energy { get; set; }

		/// <summary>
		/// L'alignement moral du personnage. Les jeux ont différentes manières de représenter cette 
		/// valeur, mais mon site en fait abstraction.
		/// </summary>
		[Required(ErrorMessage = "L'alignement moral est requis.")]
		[Display(Name = "Alignement moral")]
		[Range(-100, 100, ErrorMessage = "L'alignement moral dépasse les limites permises. Il doit être entre -100 et 100.")]
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
		[Required(ErrorMessage = "Le niveau de protection doit être spécifié.")]
		public ProtectionLevel? Protection { get; set; }
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
			[Display(Name = "Aucun")]
			None	    = 1,

			[Display(Name = "Protégé")]
			Protected = 2,

			[Display(Name = "Essentiel")]
			Essential = 3
		}

		/// <summary>
		/// Obtient l'image du portrait du compagnion.
		/// Certains compagnions (créés dynamiquement) utilisent un URL pour leur image.
		/// En l'absence d'un tel URL, on forme le chemin de l'image sur le serveur local
		/// </summary>
		/// <param name="follower">Le follower dont l'image est recherchée.</param>
		/// <returns>Le chemin vers l'image du follower.</returns>
		public static string GetPictureSrc(Follower follower)
		{
			return follower.PictureUrl ?? $"/images/games/{follower.Parent.ShortName}/followers/{follower.ShortName}/thumbnail.jpg";
		}
	}
}
