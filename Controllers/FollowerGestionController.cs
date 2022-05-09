using SessionProject2W5.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SessionProject2W5.Controllers
{
	public class FollowerGestionController : Controller
	{
		private Database Database;

		public FollowerGestionController(Database database)
		{
			this.Database = database;
		}

		// GET: FollowerManagementController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: FollowerManagementController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: FollowerManagementController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: FollowerManagementController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
