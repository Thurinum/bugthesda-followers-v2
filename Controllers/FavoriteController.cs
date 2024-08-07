﻿using SessionProject2W5.Models;
using SessionProject2W5.Extensions;

using Microsoft.AspNetCore.Mvc;

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
				if (favoriteIds.Contains(f.Id))
				{
					f.IsFavorite = true;
					return true;
				}

				return false;
			}).ToList();

			return View("Favorites", favorites);
		}

		[Route("/favorite/add/{favoriteid}")]
		[Route("/favorites/add/{favoriteid}")]
		[Route("/favoris/ajouter/{favoriteid}")]
		public IActionResult Add(int favoriteid)
		{
			List<int> favoriteIds = HttpContext.Session.Get<List<int>>("favoriteIds");

			if (favoriteIds == null)
				favoriteIds = new List<int>();

			if (!favoriteIds.Contains(favoriteid))
				favoriteIds.Add(favoriteid);

			Database.Followers.Single(f => f.Id == favoriteid).IsFavorite = true;

			HttpContext.Session.Set<List<int>>("favoriteIds", favoriteIds);
			return RedirectToAction("Index");
		}

		[Route("/favorite/remove/{favoriteid}")]
		[Route("/favorites/remove/{favoriteid}")]
		[Route("/favoris/supprimer/{favoriteid}")]
		public IActionResult Remove(int favoriteid)
		{
			List<int> favoriteIds = HttpContext.Session.Get<List<int>>("favoriteIds");

			if (favoriteIds == null)
				favoriteIds = new List<int>();

			if (favoriteIds.Contains(favoriteid))
				favoriteIds.Remove(favoriteid);

			Database.Followers.Single(f => f.Id == favoriteid).IsFavorite = false;

			HttpContext.Session.Set<List<int>>("favoriteIds", favoriteIds);
			return RedirectToAction("Index");
		}
	}
}
