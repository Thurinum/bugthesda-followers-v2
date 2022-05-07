using SessionProject2W5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionProject2W5.ViewModels
{
	public enum Protection
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
		public string Keywords   { get; set; } = String.Empty;

		public Dictionary<string, bool> GamesFilter { get; set; } = new Dictionary<string, bool>();
		public Dictionary<string, bool> RacesFilter;
		public Dictionary<string, bool> ClassesFilter;

		public int? MinAlignment { get; set; } = null;
		public int? MaxAlignment { get; set; } = null;
		public int? MinHitpoints { get; set; } = null;
		public int? MaxHitpoints { get; set; } = null;
		public int? MinEnergy    { get; set; } = null;
		public int? MaxEnergy    { get; set; } = null;

		public Protection     Protection { get; set; } = Protection.Ignore;
		public FavoriteFilter IsFavorite { get; set; } = FavoriteFilter.Ignore;
	}
}
