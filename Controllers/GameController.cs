using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SessionProject2W5.Controllers
{
	public class GameController : Controller
	{
		private Database database;
		private Random generator;

		public GameController(Database db)
		{
			this.database = db;
			this.generator = new Random();

		}

		// passer l'erreur de la base de donnees au ViewData avant que
		// la premiere action soit executee
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			ViewData["sDatabaseError"] = database.ErrorString; // Montrer certains messages d'erreur
		}

		// affiche les infos primaires des jeux dans un carousel
		[Route("/")]
		public IActionResult Index()
		{
			ViewData["sPageTitle"] = "Bethesda's Followers";
			//ViewData["sDatabaseError"] = database.ErrorString;
			return View(database.Games);
		}

		#region Details d'un jeu
		// affiche les infos secondaires sur un jeu
		[Route("/game/{name}")]
		[Route("/parent/{name}")]
		public IActionResult Index_Details(string name)
		{
			List<Game> match = database.Games.Where(g => g.ShortName == name.ToLower()).ToList();

			if (match.Count != 1)
				return PartialView("404_GameNotFound", name);

			Game game = match[0];
			
			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			return PartialView(game);
		}

		// affiche les infos secondaires sur un jeu (overload)
		[Route("/game/{id:int}")]
		[Route("/parent/{id:int}")]
		public IActionResult Index_Details(int id)
		{
			Game game = database.Games[id];

			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			return PartialView(id);
		}

		// obtient l'url d'une image aleatoire pour le jeu
		[Route("/game/{id:int}/random_thumbnail")]
		[Route("/parent/{id:int}/random_thumbnail")]
		[Obsolete("Dont use this shit")]
		public IActionResult RandomThumbnail(int id)
		{
			Game game = database.Games[id];

			if (game.Followers.Count == 0)
				return Content("empty");

			int rand = generator.Next(0, game.Followers.Count - 1);
			string name = $"/images/games/{game.ShortName}/followers/{game.Followers[rand].ShortName}/thumbnail.jpg";

			return Content(name);
		}
		#endregion

	}
}
