using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using recipe_planer.Models;

namespace recipe_planer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static RecipeHandler handler;
        static CookingListBuilder builder = new CookingListBuilder();
        static List<Ingredient> ingr_to_be_added;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var filepath = "/Users/filip/Desktop/INFA/EGUI/github-korzefi/ASPNET/recipe-planer/recipe-planer/recipes.json";
            handler = new RecipeHandler(filepath);
            handler.loadJsonFile();

            return View(handler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            var name = Convert.ToString(form["Name"]);
            var description = Convert.ToString(form["Description"]);
            handler.Recipes.Add(new Recipe(name, description));
            handler.saveJsonFile();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var recipe_to_be_edited = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();

            return View(recipe_to_be_edited);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {
            var name = Convert.ToString(form["Name"]);
            var description = Convert.ToString(form["Description"]);

            var recipe_to_remove = handler.Recipes.Where(r => r.Name == name).FirstOrDefault();
            var ingredients = recipe_to_remove.Ingredients;
            handler.Recipes.Remove(recipe_to_remove);
            handler.Recipes.Add(new Recipe(name, description, ingredients));
            handler.saveJsonFile();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var recipe_to_remove = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();
            handler.Recipes.Remove(recipe_to_remove);
            handler.saveJsonFile();

            return RedirectToAction("Index");
        }

        public IActionResult AddIngredient()
        {
            return RedirectToAction("Index");
        }

        public IActionResult EditIngredient(int id, string name, string unit)
        {
            return RedirectToAction("Index");
        }

        public IActionResult DeleteIngredient(int id, string name, string unit)
        {
            handler.deleteIngredient(id, name, unit);

            return RedirectToAction("Edit", new { id = id });

        }

        public IActionResult CookingList()
        {
            //var ingredient_list = new List<Ingredient>();
            //ingredient_list.Add(new Ingredient("ingredient", 3.0, "pcs"));
            //recipes.addRecipe(new Recipe("test_recipe", "with some\nmultiline description", ingredient_list));
            //recipes.saveJsonFile();
            builder.AvailableRecipes = handler.Recipes;
            builder.sumUpIngredients();

            return View(builder);
        }

        public IActionResult ClearCookingList()
        {
            builder.CookingList.Clear();

            return RedirectToAction("CookingList");
        }

        public IActionResult AddToCookingList(int id)
        {
            var recipe = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();
            builder.addRecipe(recipe);

            return RedirectToAction("CookingList");
        }

        public IActionResult RemoveFromCookingList(int id)
        {
            var recipe = builder.CookingList.Where(r => r.RecipeID == id).FirstOrDefault();
            builder.CookingList.Remove(recipe);

            return RedirectToAction("CookingList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
