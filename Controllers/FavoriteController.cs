using SessionProject2W5.Models;
using SessionProject2W5.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Linq;

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

			List<Follower> favorites = Database.Followers.Where(f =>
			{
				f.IsFavorite = true;
				return favoriteIds.Contains(f.Id);
			}).ToList();

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

			Database.Followers.Single(f => f.Id == id).IsFavorite = true;

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

			Database.Followers.Single(f => f.Id == id).IsFavorite = false;

			HttpContext.Session.Set<List<int>>("favoriteIds", favoriteIds);
			return RedirectToAction("Index");
		}
	}
}
