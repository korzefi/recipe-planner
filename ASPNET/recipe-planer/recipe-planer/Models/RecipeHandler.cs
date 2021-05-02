using System;
using System.Collections.Generic;
namespace recipe_planer.Models
{
    public class RecipeHandler
    {
        private List<Recipe> Recipes { get; set; }

        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            Recipes.Remove(recipe);
        }

        public RecipeHandler() {}
    }
}
