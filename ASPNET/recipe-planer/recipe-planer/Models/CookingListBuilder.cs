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

        public void AddRecipe(Recipe recipe)
        {
            cooking_list.Add(recipe);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            cooking_list.Remove(recipe);
        }

        public List<Ingredient> GetSummmedUpIngredients()
        {
            CreateSummedUpIngredientsList();
            var result = summed_ingredients;
            summed_ingredients.Clear();
            return result;
        }

        // TODO SPRAWDZIC PRZEKAZYWANIE I PRZYPISYWANIE JAKO REFERENCJA, MOZE NIE DZIALAC!!!!!!!
        private void CreateSummedUpIngredientsList()
        {
            for (int i=0; i<cooking_list.Count; i++)
            {
                SumUpIngredients(ref cooking_list[i].Ingredients);
            }
            
        }

        private void SumUpIngredients(ref List<Ingredient> ingredients)
        {
            foreach (Ingredient current_ingredient in ingredients) {
                AddIngredientToSummedList(current_ingredient);
            }
        }

        private void AddIngredientToSummedList(Ingredient current_ingredient)
        {
            for (int i=0; i<summed_ingredients.Count; i++)

            foreach (Ingredient summed_ingredient in summed_ingredients)
            {
                if (summed_ingredients[i] == current_ingredient)
                {
                    summed_ingredients[i].Amount += current_ingredient.Amount;
                    return;
                }
            }
    }
}
