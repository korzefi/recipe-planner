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
            Recipe recipe1 = new Recipe("example1");
            Recipe recipe2 = new Recipe("example2");

            var recipes = new RecipeHandler();
            recipes.AddRecipe(recipe1);
            recipes.AddRecipe(recipe2);


            return View(recipe);
            //return RedirectToAction("Index", "Home"); // redirects to index page of home
        }
    }
}
