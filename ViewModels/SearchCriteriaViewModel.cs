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
	}
}
