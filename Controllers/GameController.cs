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
		private Database Database;
		private Random Generator;

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

			return PartialView(game);
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

			return PartialView(id);
		}

		/// <summary>
		/// Renvoie l'URL d'une image aléatoire pour le jeu ciblé. Cette action est appelée depuis JavaScript.
		/// </summary>
		/// <remarks>N'est plus utilisée. Je la laisse au cas où...</remarks>
		/// <param name="name">Le ShortName du jeu en question.</param>
		/// <returns>L'URL de l'image d'un follower aléatoire dans le jeu spécifié.</returns>
		[Obsolete("Pourquoi tu utilises ça???!!!!!")]
		[Route("/game/{id:int}/random_thumbnail")]
		[Route("/parent/{id:int}/random_thumbnail")]
		public IActionResult RandomThumbnail(int id)
		{
			Game game = Database.Games[id];

			if (game.Followers.Count == 0)
				return Content("empty");

			int rand = Generator.Next(0, game.Followers.Count - 1);
			string name = $"/images/games/{game.ShortName}/followers/{game.Followers[rand].ShortName}/thumbnail.jpg";

			return Content(name);
		}
		#endregion
	}
}
