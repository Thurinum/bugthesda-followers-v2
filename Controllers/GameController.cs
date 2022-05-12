using SessionProject2W5.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Linq;
using System.Collections.Generic;

namespace SessionProject2W5.Controllers
{
	public class GameController : Controller
	{
		private readonly Database Database;
		private readonly Random Generator;

		public GameController(Database db)
		{
			this.Database = db;
			this.Generator = new Random();
		}

		/// <summary>
		/// Passe l'erreur de la base de donnees au ViewData avant que la première action soit exécutée.
		/// Il est impossible de set le ViewData dans le constructeur.
		/// </summary>
		/// <param name="context">Le contexte de l'action.</param>
		/// <seealso cref="Models.Database"/>
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			ViewData["sDatabaseError"] = Database.ErrorString; // Montrer certains messages d'erreur
		}

		/// <summary>
		/// Affiche la page d'accueil permettant de sélectionner un jeu et de montrer ses détails.
		/// </summary>
		/// <returns>La vue Index contenant tous les jeux.</returns>
		[Route("/")]
		public IActionResult Index()
		{
			ViewData["sPageTitle"] = "Bethesda's Followers";
			return View(Database.Games);
		}

		#region Afficher les détails d'un jeu
		/// <summary>
		/// Affiche les détails d'un jeu sélectionné. Cette action est appelée depuis JavaScript dans la vue Index.
		/// </summary>
		/// <param name="name">Le ShortName du jeu en question.</param>
		/// <returns>Une vue partielle Index_Details contenant les détails du jeu cible.</returns>
		[Route("/game/{name}")]
		[Route("/parent/{name}")]
		public IActionResult Index_Details(string name)
		{
			List<Game> match = Database.Games.Where(g => g.ShortName == name.ToLower()).ToList();

			if (match.Count != 1)
				return PartialView("404_GameNotFound", name);

			Game game = match[0];
			
			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			return PartialView("_GameDetailsPartial", game);
		}

		/// <summary>
		/// Affiche les détails d'un jeu sélectionné. Cette action est appelée depuis JavaScript dans la vue Index.
		/// Cet overload supporte les ID.
		/// </summary>
		/// <param name="id">Le ID du jeu en question.</param>
		/// <returns>Une vue partielle Index_Details contenant les détails du jeu cible.</returns>
		[Route("/game/{id:int}")]
		[Route("/parent/{id:int}")]
		public IActionResult Index_Details(int id)
		{
			Game game = Database.Games[id];

			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			return PartialView("_GameDetailsPartial", id);
		}

		/// <summary>
		/// Obtient un fait aléatoire sur n'importe quel jeu de la DB.
		/// </summary>
		/// <returns>Un fait sur un jeu.</returns>
		[Route("/game/getrandomtrivia")]
		[Route("/parent/getrandomtrivia")]
		public IActionResult GetRandomTrivia()
		{
			List<string> trivia = new List<string>();

			foreach (Game game in Database.Games)
				trivia.AddRange(game.Facts);

			int index = Generator.Next(0, trivia.Count - 1);

			return Content(trivia[index]);
		}

		#endregion
	}
}
