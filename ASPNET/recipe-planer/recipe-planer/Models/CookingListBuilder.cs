using System;
using System.Collections.Generic;

namespace recipe_planer.Models
{
    public class CookingListBuilder
    {
        private List<Recipe> cooking_list;
        private List<Ingredient> summed_ingredients;

        public CookingListBuilder()
        {
            cooking_list = new List<Recipe>();
        }


        // TODO decide whether update after EACH add/remove or EVERYTHING on one action
        public void AddRecipe(Recipe recipe)
        {
            cooking_list.Add(recipe);
            //AddToIngredients(recipe.Ingredients);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            cooking_list.Remove(recipe);
            //RemoveFromIngredients(recipe.Ingredients);
        }
    }
           // TODO SPRAWDZIC PRZEKAZYWANIE I PRZYPISYWANIE JAKO REFERENCJA, MOZE NIE DZIALAC!!!!!!
}
