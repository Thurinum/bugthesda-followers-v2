using SessionProject2W5.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionProject2W5.ViewModels
{
	public enum ProtectionFilter
	{
		[Display(Name = "Peu importe")]
		Ignore,

		[Display(Name = "Aucun")]
		None,

		[Display(Name = "Protégé")]
		Protected,

		[Display(Name = "Essentiel")]
		Essential
	}

	public enum FavoriteFilter
	{
		Ignore,
		NotFavorite,
		Favorite
	}

	public class SearchCriteriaViewModel
	{
		[StringLength(69, ErrorMessage = "Le champ est trop long. Max. 69 caractères.")]
		[RegularExpression("^[a-zA-Z0-9 àâéèêùûÀÂÉÈÊÙÛ.,:;!?-]*$", ErrorMessage = "La recherche contient des caractères invalides! Lettres, chiffres et ponctuation seulement.")]
		public string Keywords   { get; set; }

		[StringLength(69, ErrorMessage = "Le champ est trop long. Max. 69 caractères.")]
		[RegularExpression("^[a-zA-Z0-9 àâéèêùûÀÂÉÈÊÙÛ.,:;!?-]*$", ErrorMessage = "La recherche contient des caractères invalides! Lettres, chiffres et ponctuation seulement.")]
		public string ExtraKeywords { get; set; }

		public List<TypeFilterViewModel> GamesFilters   { get; set; } = new List<TypeFilterViewModel>();
		public List<TypeFilterViewModel> RacesFilters   { get; set; } = new List<TypeFilterViewModel>();
		public List<TypeFilterViewModel> ClassesFilters { get; set; } = new List<TypeFilterViewModel>();

		[Display(Name = "Alignment Min.")]
		[Range(-100, 100, ErrorMessage = "Veuillez entrer une valeur entre -100 et 100.")]
		public int? MinAlignment { get; set; } = -100;

		[Display(Name = "Alignment Max.")]
		[Range(-100, 100, ErrorMessage = "Veuillez entrer une valeur entre -100 et 100.")]
		public int? MaxAlignment { get; set; } = 100;

		[Display(Name = "Mininum")]
		[Range(0, 999, ErrorMessage = "Veuillez entrer une valeur entre 0 et 999.")]
		public int? MinHitpoints { get; set; } = null;

		[Display(Name = "Maximum")]
		[Range(0, 999, ErrorMessage = "Veuillez entrer une valeur entre 0 et 999.")]
		public int? MaxHitpoints { get; set; } = null;

		[Display(Name = "Minimum")]
		public int? MinEnergy    { get; set; } = null;

		[Display(Name = "Maximum")]
		public int? MaxEnergy    { get; set; } = null;

		public ProtectionFilter ProtectionFilter { get; set; } = ProtectionFilter.Ignore;
		public FavoriteFilter FavoriteFilter { get; set; } = FavoriteFilter.Ignore;

		/// <summary>
		/// Initiailse le modèle de critères de recherche.
		/// Ce constructeur est utilisé par le model binding.
		/// </summary>
		public SearchCriteriaViewModel() { }

		/// <summary>
		/// Initialise le modèle de critères de recherche avec la liste de jeux spécifiée.
		/// Ce constructeur est nécessaire pour pré-populer les filtres de type.
		/// </summary>
		/// <param name="games">La liste des jeux de la base de données</param>
		public SearchCriteriaViewModel(List<Game> games, bool allowGames = true, bool allowRaces = true, bool allowClasses = true)
		{
			foreach (Game game in games)
				GamesFilters.Add(new TypeFilterViewModel() { Name = game.Name, Allowed = allowGames });

			foreach (Race race in games[0].SharedInfo.Races)
				RacesFilters.Add(new TypeFilterViewModel() { Name = race.NativeName, Allowed = allowRaces });

			foreach (Class @class in games[0].SharedInfo.Classes)
				ClassesFilters.Add(new TypeFilterViewModel() { Name = @class.Name, Allowed = allowClasses });
		}
	}
}
