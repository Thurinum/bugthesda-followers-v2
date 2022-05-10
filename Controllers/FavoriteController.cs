using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Linq;
using SessionProject2W5.Extensions;

namespace SessionProject2W5.Controllers
{
	public class FavoriteController : Controller
	{
		private readonly Database Database;

		public FavoriteController(Database database)
		{
			Database = database;
		}

		[Route("/favoris")]
		[Route("/favorites")]
		public IActionResult Index()
		{
			// obtenir ids favoris session
			List<int> favoriteIds = HttpContext.Session.Get<List<int>>("favoriteIds");

			if (favoriteIds == null)
				favoriteIds = new List<int>();

			List<Follower> favorites = Database.Followers.Where(f => favoriteIds.Contains(f.Id)).ToList();

			Database.Serialize();

			return View(favorites);
		}

		[Route("/favorites/add/{favoriteid}")]
		[Route("/favoris/ajouter/{favoriteid}")]
		public IActionResult Add(int id)
		{
			List<int> favoriteIds = HttpContext.Session.Get<List<int>>("favoriteIds");

			if (favoriteIds == null)
				favoriteIds = new List<int>();

			if (!favoriteIds.Contains(id))
				favoriteIds.Add(id);

			HttpContext.Session.Set<List<int>>("favoriteIds", favoriteIds);
			return RedirectToAction("Index");
		}

		[Route("/favorites/remove/{favoriteid}")]
		[Route("/favoris/supprimer/{favoriteid}")]
		public IActionResult Remove(int id)
		{
			List<int> favoriteIds = HttpContext.Session.Get<List<int>>("favoriteIds");

			if (favoriteIds == null)
				favoriteIds = new List<int>();

			if (favoriteIds.Contains(id))
				favoriteIds.Remove(id);

			HttpContext.Session.Set<List<int>>("favoriteIds", favoriteIds);
			return RedirectToAction("Index");
		}
	}
}
