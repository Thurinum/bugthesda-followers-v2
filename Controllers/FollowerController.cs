using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace SessionProject2W5.Controllers
{
	public class FollowerController : Controller
	{
		private Database Database;

		public FollowerController(Database database)
		{
			this.Database = database;
			ViewData["sDatabaseError"] = database.ErrorString; // Montrer certains messages d'erreur
		}

		#region Lister compagnions
		// Affiche page de recherche avec les compagnions de TOUS les jeux
		[Route("/search")]
		public IActionResult Search()
		{
			List<Follower> allFollowers = new List<Follower>();

			foreach (Game game in this.Database.Games)
				allFollowers.AddRange(game.Followers);

			ViewData["sPageTitle"] = "Recherche de follower";
			return View(allFollowers);
		}

		// Affiche page de recherche avec les compagnions d'UN jeu specifique
		[Route("/search/{name}/")]
		[Route("/recherche/{name}/")]
		public IActionResult Search(string name)
		{
			Game game = Database.Games.Where(g => g.ShortName == name).FirstOrDefault();

			if (game == null)
				return View("404_GameNotFound", name);

			if (game.Followers.Count == 0)
				return View("204_GameEmpty", game.Name);

			ViewData["sPageTitle"] = "Recherche de follower pour " + game.Name;
			return View(game.Followers.Where(f => f.Parent.ShortName == name).ToList());
		}

		// Affiche page de recherche avec les compagnions d'UN jeu specifique (par ID)
		[Route("/search/{id:int}/")]
		[Route("/recherche/{id:int}/")]
		public IActionResult Search(int id)
		{
			Game game = Database.Games[id];

			if (game == null)
				return PartialView("404_GameNotFound", id);

			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			ViewData["sPageTitle"] = "Recherche de follower pour " + game.Name;
			return View(game.Followers.Where(f => f.Parent.Id == id).ToList());
		}
		#endregion

		#region Afficher details d'un compagnion
		// Affiche les details d'un compagnion selon son nom (supporte plusieurs routes)
		[Route("/{name}")]
		[Route("/details/{name}")]

		[Route("/follower/{name}")]
		[Route("/follower/details/{name}")]

		[Route("/enfant/{name}")]
		[Route("/enfant/detail/{name}")]
		[Route("/enfant/details/{name}")]
		public IActionResult Details(string name)
		{
			Follower follower = Database.Followers.Where(f => f.ShortName == name).FirstOrDefault();

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(null, name));

			ViewData["sPageTitle"] = $"{follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon son id (supporte plusieurs routes)
		[Route("/{id:int}")]
		[Route("/details/{id:int}")]

		[Route("/follower/{id:int}")]
		[Route("/follower/details/{id:int}")]

		[Route("/enfant/{id:int}")]
		[Route("/enfant/detail/{id:int}")]
		[Route("/enfant/details/{id:int}")]
		public IActionResult Details(int id)
		{
			Follower follower = Database.Followers[id];

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(null, id.ToString()));

			ViewData["sPageTitle"] = $"{follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon le chemin parent/enfant
		[Route("/{gamename}/{followername}")]
		public IActionResult Details(string gamename, string followername)
		{
			Game game = Database.Games.Where(g => g.ShortName == gamename).FirstOrDefault();
			if (game == null)
				return View("204_GameEmpty");

			Follower follower = game.Followers.Where(f => f.ShortName == followername).FirstOrDefault();
			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(game.Name, followername));

			ViewData["sPageTitle"] = $"{game.Name} - {follower.Name}";
			return View(follower);
		}

		// Affiche les details d'un compagnion selon le chemin parent/enfant (par ID)
		[Route("/{gameid:int}/{followerid:int}")]
		public IActionResult Details(int gameid, int followerid)
		{
			Follower follower = Database.Games[gameid].Followers[followerid];

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(gameid.ToString(), followerid.ToString()));

			ViewData["sPageTitle"] = "Recherche du follower " + follower.Name;
			return View(follower);
		}
		#endregion
	}
}
