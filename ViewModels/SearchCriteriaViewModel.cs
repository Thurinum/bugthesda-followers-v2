using SessionProject2W5.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SessionProject2W5.ViewModels
{
	public enum ProtectionFilter
	{
		Ignore,
		None,
		Protected,
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
		[Required]
		[StringLength(69, ErrorMessage = "Fill the field moron")]
		public string Keywords   { get; set; } = String.Empty;

		public List<TypeFilterViewModel> GamesFilters   { get; set; } = new List<TypeFilterViewModel>();
		public List<TypeFilterViewModel> RacesFilters   { get; set; } = new List<TypeFilterViewModel>();
		public List<TypeFilterViewModel> ClassesFilters { get; set; } = new List<TypeFilterViewModel>();

		public int? MinAlignment { get; set; } = -100;
		public int? MaxAlignment { get; set; } = 100;
		public int? MinHitpoints { get; set; } = null;
		public int? MaxHitpoints { get; set; } = null;
		public int? MinEnergy    { get; set; } = null;
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
				GamesFilters.Add(new TypeFilterViewModel() { Name = game.ShortName, Allowed = allowGames });

			foreach (Race race in games[0].SharedInfo.Races)
				RacesFilters.Add(new TypeFilterViewModel() { Name = race.ShortName, Allowed = allowRaces });

			foreach (Class @class in games[0].SharedInfo.Classes)
				ClassesFilters.Add(new TypeFilterViewModel() { Name = @class.ShortName, Allowed = allowClasses });
		}
	}
}
