using System;
using System.Collections.Generic;
namespace recipe_planer.Models
{
    public class RecipeHandler
    {
        private List<Recipe> recipes;

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            recipes.Remove(recipe);
        }

        public RecipeHandler() {}
    }
}
