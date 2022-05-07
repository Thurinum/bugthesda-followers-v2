using SessionProject2W5.Models;
using System.Collections.Generic;

namespace SessionProject2W5.ViewModels
{
	public class SearchViewModel
	{
		public List<Follower> Followers { get; set; }
		public SearchCriteriaViewModel Criteria { get; set; }
	}
}
