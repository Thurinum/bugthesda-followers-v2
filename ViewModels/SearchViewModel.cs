using SessionProject2W5.Models;
using System.Collections.Generic;

namespace SessionProject2W5.ViewModels
{
	public class SearchViewModel
	{
		public SearchCriteriaViewModel Criteria { get; set; }
		public List<Follower> Results { get; set; }
	}
}
