using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace SessionProject2W5.Controllers
{
	public class FollowerController : Controller
	{
		public FollowerController(Database database)
		{
			this.Database = database;
			ViewData["sDatabaseError"] = database.ErrorString; // Montrer certains messages d'erreur
		}

		// on evite conflits avec details dans HomeController
		// todo: meilleures routes
		[Route("/search")]
		public IActionResult Search()
		{
			ViewData["sPageTitle"] = "Recherche de follower";

			List<Follower> allFollowers = new List<Follower>();
			foreach (Game game in this.Database.Games)
				allFollowers.AddRange(game.Followers);

			return View(allFollowers);
		}

		[Route("/search/{gameid}/")]
		public IActionResult Search(int gameid)
		{
			ViewData["sPageTitle"] = "Recherche de follower pour " + Database.Games[gameid].Name;
			return View(Database.Games[gameid].Followers.Where(f => f.Parent.Id == gameid).ToList());
		}

		[Route("/details/{gameid}/{followerid}")]
		public IActionResult Details(int gameid, int followerid)
		{
			Follower follower = Database.Games[gameid].Followers[followerid];
			ViewData["sPageTitle"] = "Recherche du follower " + follower.Name;
			return View(follower);
		}

		private Database Database;
	}
}
