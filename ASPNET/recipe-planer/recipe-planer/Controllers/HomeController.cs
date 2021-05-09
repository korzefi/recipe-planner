using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using recipe_planer.Models;

namespace recipe_planer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // TODO load recipes from file - FileHandler
            // TODO pass list of recipes to View - var recipes = FileHandler::GetRecipes()

            return View();
        }

        public IActionResult Privacy()
        {
            var filepath = "/Users/filip/Desktop/INFA/EGUI/github-korzefi/ASPNET/recipe-planer/recipe-planer/recipes.json";
            RecipeHandler recipes = new RecipeHandler(filepath);
            var ingredient_list = new List<Ingredient>();
            ingredient_list.Add(new Ingredient("ingredient", 3.0, "pcs"));
            recipes.addRecipe(new Recipe("test_recipe", "with some\nmultiline description", ingredient_list));
            recipes.saveJsonFile();
            recipes.loadJsonFile();

            return View(recipes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
