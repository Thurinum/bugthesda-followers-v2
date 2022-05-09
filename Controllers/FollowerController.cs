using SessionProject2W5.Models;
using SessionProject2W5.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Linq;
using System.Collections.Generic;

namespace SessionProject2W5.Controllers
{
	public class FollowerController : Controller
	{
		private readonly Database Database;

		public FollowerController(Database database)
		{
			this.Database = database;
		}

		/// <summary>
		/// Passe l'erreur de la base de donnees au ViewData avant que la première action soit exécutée.
		/// Il est impossible de set le ViewData dans le constructeur.
		/// </summary>
		/// <param name="context">Le contexte de l'action.</param>
		/// <seealso cref="Database"/>
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			ViewData["sDatabaseError"] = Database.ErrorString; // Montrer certains messages d'erreur
		}

		#region Lister les compagnions, avec ou sans filtre simple ou complexe
		/// <summary>
		/// Affiche la page de recherche avec les compagnions de tous les jeux et initialise les filtres par défaut.
		/// </summary>
		/// <returns>La vue Search contenant tous les compagnions, ou une page 404 si la recherche ne donne aucun résultat.</returns>
		[Route("/search")]
		[Route("/recherche")]
		public IActionResult Search()
		{
			// initialiser les modèles de recherche
			SearchViewModel search = new SearchViewModel();
			SearchCriteriaViewModel searchCriteria = new SearchCriteriaViewModel(Database.Games);
			List<Follower> followers = Database.Followers;			

			search.Criteria = searchCriteria;
			search.Results = followers;

			ViewData["sPageTitle"] = "Bethesda's Followers - Recherche";
			return followers.Count > 0 ? View(search) : View("404_SearchNoResults", searchCriteria.Keywords);
		}

		/// <summary>
		/// Affiche la page de recherche avec la liste des compagnions filtrée en fonction d'un CriteriaViewModel.
		/// </summary>
		/// <remarks>
		/// Les consignes nommaient cette action "Filtre". J'ai préféré un overload pour éviter d'avoir à spécifier le nom de la vue (plus intuitif).
		/// </remarks>
		/// <param name="criteria">Le modèle contenant les critères de recherche.</param>
		/// <returns>La vue Search contenant les compagnions ayant passé à travers le filtre, ou une page 404 si la recherche ne donne aucun résultat.</returns>
		[Route("/search/filter")]
		[Route("/recherche/filtrer")]
		public IActionResult Search(SearchCriteriaViewModel criteria)
		{
			// récupérer les données
			List<Follower> followers = Database.Followers;

			// filtrer les données
			for (int i = 0; i < followers.Count; i++)
			{
				Follower follower = followers[i];
				
				if ( // les filtres sont insensibles à la casse
					(criteria.Keywords != null && !follower.Name.ToLower().Contains(criteria.Keywords.ToLower())) ||
					(criteria.GamesFilters.Where(f => f.Name == follower.Parent.ShortName).First().Allowed == false) ||
					(criteria.RacesFilters.Where(f => f.Name == follower.Race.ShortName).First().Allowed == false) ||
					(criteria.ClassesFilters.Where(f => f.Name == follower.Class.ShortName).First().Allowed == false) ||
					(criteria.FavoriteFilter != FavoriteFilter.Ignore && ((follower.IsFavorite ? FavoriteFilter.Favorite : FavoriteFilter.NotFavorite) != criteria.FavoriteFilter)) ||
					(criteria.ProtectionFilter != ProtectionFilter.Ignore && ((follower.IsEssential ? ProtectionFilter.Essential : (follower.IsProtected ? ProtectionFilter.Protected : ProtectionFilter.None)) != criteria.ProtectionFilter)) || // "true production code"
					(criteria.MinAlignment != null && (follower.Alignment < criteria.MinAlignment)) ||
					(criteria.MaxAlignment != null && (follower.Alignment > criteria.MaxAlignment)) ||
					(criteria.MinHitpoints != null && (follower.Hitpoints < criteria.MinHitpoints)) ||
					(criteria.MaxHitpoints != null && (follower.Hitpoints > criteria.MaxHitpoints)) ||
					(criteria.MinEnergy != null && (follower.Energy < criteria.MinEnergy)) ||
					(criteria.MaxEnergy != null && (follower.Energy > criteria.MaxEnergy))
				)
				{
					// si une des conditions discriminantes ci-haut est présente, on disqualifie le compagnion du résultat
					followers.Remove(follower);
					i--;
				}
			}

			// construire le modèle des résultats
			SearchViewModel search = new SearchViewModel
			{
				Criteria = criteria,
				Results = followers
			};

			ViewData["sPageTitle"] = "Bethesda's Followers - Recherche";
			return followers.Count > 0 ? View(search) : View("404_SearchNoResults", criteria.Keywords);
		}

		/// <summary>
		/// Affiche la page de recherche avec les compagnions d'un jeu spécifique.
		/// </summary>
		/// <remarks>
		/// <param name="name">Le ShortName du jeu.</param>
		/// <returns>La vue Search contenant les compagnions du jeu cible, ou une page de status si non trouvé.</returns>
		[Route("/search/{name}/")]
		[Route("/recherche/{name}/")]
		public IActionResult Search(string name)
		{
			Game game = Database.Games.Where(g => g.ShortName == name).FirstOrDefault();

			if (game == null)
				return View("404_GameNotFound", name);

			if (game.Followers.Count == 0)
				return View("204_GameEmpty", game.Name);

			// on bloque tous les jeux sauf la cible
			SearchViewModel search = new SearchViewModel();
			SearchCriteriaViewModel criteria = new SearchCriteriaViewModel(Database.Games, false, true, true);
			criteria.GamesFilters.Where(f => f.Name == name).First().Allowed = true;
			search.Criteria = criteria;
			search.Results = game.Followers;			

			ViewData["sPageTitle"] = "Bethesda's Followers - Recherche - " + game.Name;
			return View(search);
		}

		/// <summary>
		/// Affiche page de recherche avec les compagnions d'un jeu specifique.
		/// Cet overload de l'action précédente supporte un ID dans sa route.
		/// </summary>
		/// <param name="id">Le ID du jeu.</param>
		/// <returns>La vue Search contenant les compagnions du jeu cible, ou une page de status si non trouvé.</returns>
		[Route("/search/{id:int}/")]
		[Route("/recherche/{id:int}/")]
		public IActionResult Search(int id)
		{
			Game game = Database.Games[id];

			if (game == null)
				return PartialView("404_GameNotFound", id);

			if (game.Followers.Count == 0)
				return PartialView("204_GameEmpty", game.Name);

			SearchViewModel search = new SearchViewModel();
			SearchCriteriaViewModel criteria = new SearchCriteriaViewModel(Database.Games, false, true, true);
			criteria.GamesFilters.Where(f => f.Name == game.ShortName).First().Allowed = true;
			search.Results = game.Followers;

			ViewData["sPageTitle"] = "Bethesda's Followers - Recherche - " + game.Name;
			return View(search);
		}
		#endregion

		#region Afficher les détails d'un compagnion
		/// <summary>
		/// Affiche les details d'un compagnion selon son nom. Supporte plusieurs routes.
		/// </summary>
		/// <param name="name">Le nom du compagnion.</param>
		/// <returns>La page Details pour le compagnion spécifié, ou une page de status si le compagnion est introuvable.</returns>
		[Route("/{name}")]
		[Route("/details/{name}")]

		[Route("/follower/{name}")]
		[Route("/follower/details/{name}")]

		[Route("/enfant/{name}")]
		[Route("/enfant/detail/{name}")] // grr
		[Route("/enfant/details/{name}")]
		public IActionResult Details(string name)
		{
			Follower follower = Database.Followers.Where(f => f.ShortName == name.ToLower()).FirstOrDefault();

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(null, name));

			ViewData["sPageTitle"] = $"Bethesda's Followers - {follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		/// <summary>
		/// Affiche les details d'un compagnion selon son nom. Supporte plusieurs routes.
		/// Cet overload supporte les routes utilisant l'ID du compagnion.
		/// </summary>
		/// <param name="id">Le ID du compagnion.</param>
		/// <returns>La page Details pour le compagnion spécifié, ou une page de status si le compagnion est introuvable.</returns>
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

			ViewData["sPageTitle"] = $"Bethesda's Followers - {follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}

		/// <summary>
		/// Affiche les details d'un compagnion selon le chemin parent/enfant. Cette route est la plus intuitive!
		/// </summary>
		/// <param name="gamename">Le nom du jeu cible.</param>
		/// <param name="followername">Le nom du compagnion dans le jeu cible.</param>
		/// <returns>La vue Details pour le compagnion spécifié, ou une page de status si l'un des deux composants du chemin est introuvable.</returns>
		[Route("/{gamename}/{followername}")]
		public IActionResult Details(string gamename, string followername)
		{
			Game game = Database.Games.Where(g => g.ShortName == gamename.ToLower()).FirstOrDefault();
			if (game == null)
				return View("404_GameNotFound", gamename);

			Follower follower = game.Followers.Where(f => f.ShortName == followername.ToLower()).FirstOrDefault();
			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(game.Name, followername));

			ViewData["sPageTitle"] = $"Bethesda's Followers - {game.Name} - {follower.Name}";
			return View(follower);
		}

		/// <summary>
		/// Affiche les details d'un compagnion selon le chemin parent/enfant. Cette route est la plus intuitive!
		/// Cet overload de l'action précédente prend en charge les ID.
		/// </summary>
		/// <param name="gameid">Le ID du jeu cible.</param>
		/// <param name="followerid">Le ID du compagnion dans le jeu cible.</param>
		/// <returns>La vue Details pour le compagnion spécifié, ou une page de status si l'un des deux composants du chemin est introuvable.</returns>
		[Route("/{gameid:int}/{followerid:int}")]
		public IActionResult Details(int gameid, int followerid)
		{
			Follower follower = Database.Games[gameid].Followers[followerid];

			if (follower == null)
				return View("404_FollowerNotFound", new KeyValuePair<string, string>(gameid.ToString(), followerid.ToString()));

			ViewData["sPageTitle"] = $"Bethesda's Followers - {follower.Parent.Name} - {follower.Name}";
			return View(follower);
		}
		#endregion
	}
}
