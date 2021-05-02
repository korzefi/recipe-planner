using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using recipe_planer.Models;

namespace recipe_planer.Controllers
{
    public class RecipeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            // TODO load all recipes name here and pass it to View
            var recipe = new Recipe("example");
       
            return View(recipe);
            //return RedirectToAction("Index", "Home"); // redirects to index page of home
        }
    }
}
