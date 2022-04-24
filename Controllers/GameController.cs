using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace SessionProject2W5.Controllers
{
	[Route("/")]
	public class GameController : Controller
	{
		public GameController(Database db)
		{
			this.database = db;
			this.generator = new Random();

			ViewData["sDatabaseError"] = db.ErrorString; // Montrer certains messages d'erreur
		}

		// affiche les infos primaires des jeux dans un carousel
		public IActionResult Index()
		{
			ViewData["sPageTitle"] = "Bethesda's Followers";
			return View(database.Games);
		}

		// affiche les infos secondaires sur un jeu
		[Route("{Name}")]
		public IActionResult Index_Details(string Name)
		{
			/*Game game = (Game)(database.Games.Where(g => g.Name == Name).ToList()[0]);

			if (game.Followers.Count == 0)
				return PartialView("Index_Details_NoContent", game.Name);

			return PartialView(game);*/
			return Content("dsfsdfsdf");
		}

		// affiche les infos secondaires sur un jeu (overload)
		[Route("{Id:int}")]
		public IActionResult Index_Details(int Id)
		{
			Game game = database.Games[Id];

			if (game.Followers.Count == 0)
				return PartialView("Index_Details_NoContent", game.Name);

			return PartialView(game);
		}

		// obtient l'url d'une image aleatoire pour le jeu
		[Route("{Id:int}/randomthumbnail")]
		public IActionResult RandomThumbnail(int Id)
		{
			Game game = database.Games[Id];

			if (game.Followers.Count == 0)
				return Content("empty");

			int rand = generator.Next(0, game.Followers.Count - 1);
			string name = $"/images/games/{game.ShortName}/followers/{game.Followers[rand].ShortName}/thumbnail.jpg";

			return Content(name);
		}

		private Database database;
		private Random generator;
	}
}
