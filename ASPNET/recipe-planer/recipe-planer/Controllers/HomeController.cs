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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var filepath = "recipes.json";
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
            if (name == "")
            {
                ModelState.AddModelError("name", "Please provide name of recipe");
                return View();
            }
            var recipe = new Recipe(name, description);
            handler.Recipes.Add(new Recipe(recipe));

            return RedirectToAction("Edit", new { id = recipe.RecipeID });
        }

        public IActionResult Edit(int id, string error_message = "")
        {
            var recipe_to_be_edited = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();
            if (error_message == "Please provide all ingredient data" ||
                error_message == "Amount field of ingredient should be floating-point type")
            {
                ModelState.AddModelError("empty_ingredient", error_message);
                return View(recipe_to_be_edited);
            }

            return View(recipe_to_be_edited);
        }

        public IActionResult EditNameDesc(int id)
        {
            var recipe_to_be_edited = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();

            return View(recipe_to_be_edited);
        }

        [HttpPost]
        public IActionResult EditNameDesc(IFormCollection form, Recipe recipe)
        {
            var name = Convert.ToString(form["Name"]);
            var description = Convert.ToString(form["Description"]);
            var id = recipe.RecipeID;

            var recipe_to_remove = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();
            List<Ingredient> ingredients = recipe_to_remove.Ingredients;
            if (name == "")
            {
                return View(new Recipe(name, description, new List<Ingredient>(ingredients)));
            }
            handler.Recipes.Remove(recipe_to_remove);
            handler.Recipes.Add(new Recipe(name, description, new List<Ingredient>(ingredients)));

            var added_recipe = handler.Recipes.Where(r => r.Name == name).FirstOrDefault();
            return RedirectToAction("Edit", new { id = added_recipe.RecipeID });
        }

        [HttpPost]
        public IActionResult Save(Recipe recipe)
        {
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

        public IActionResult AddIngredient(IFormCollection form, Recipe recipe)
        {
            var name = Convert.ToString(form["addIngredientName"]);
            var amount = Convert.ToString(form["addIngredientAmount"]);
            var unit = Convert.ToString(form["addIngredientUnit"]);
            var id = recipe.RecipeID;
            double amount_converted;
            if (name == "" || amount == "" || unit == "")
            {
                return RedirectToAction("Edit", new { id = id, error_message = "Please provide all ingredient data" });
            }
            try
            {
                amount_converted = Convert.ToDouble(amount);
            }
            catch (FormatException)
            {
                return RedirectToAction("Edit", new { id = id, error_message = "Amount field of ingredient should be floating-point type" });
            }

            handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault().Ingredients.Add(new Ingredient(name, amount_converted, unit));
         
            return RedirectToAction("Edit", new { id = id });
        }

        public IActionResult EditIngredient(int id, string name, string unit)
        {
            var current_recipe = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();
            current_recipe.setSelectedIngredientIndex(name, unit);

            return RedirectToAction("EditIngr", new { id = id });
        }

        public IActionResult EditIngr(int id)
        {
            var recipe = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault();

            return View(recipe);
        }

        public IActionResult DeleteIngredient(int id, string name, string unit)
        {
            handler.deleteIngredient(id, name, unit);

            return RedirectToAction("Edit", new { id = id });

        }

        public IActionResult SaveIngredient(IFormCollection form, Recipe recipe)
        {
            var id = recipe.RecipeID;
            var name = Convert.ToString(form["editIngredientName"]);
            var amount = Convert.ToString(form["editIngredientAmount"]);
            var unit = Convert.ToString(form["editIngredientUnit"]);
            var index = recipe.SelectedIngredientIndex;
            double amount_converted;

            if (name == "" || amount == "" || unit == "")
            {
                return RedirectToAction("Edit", new { id = id, error_message = "Please provide all ingredient data" });
            }
            try
            {
                amount_converted = Convert.ToDouble(amount);
            }
            catch (FormatException)
            {
                return RedirectToAction("Edit", new { id = id, error_message = "Amount field of ingredient should be floating-point type" });
            }

            handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault().SelectedIngredientIndex = -1;

            var modified_ingr = handler.Recipes.Where(r => r.RecipeID == id).FirstOrDefault().Ingredients[index];
            modified_ingr.Name = name;
            modified_ingr.Amount = amount_converted;
            modified_ingr.Unit = unit;

            return RedirectToAction("Edit", new { id = id });
        }

        public IActionResult CookingList()
        {
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
