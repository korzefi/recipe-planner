using System;
using System.Collections.Generic;
using System.Linq;

namespace recipe_planer.Models
{
    public class CookingListBuilder
    {
        public List<Recipe> AvailableRecipes { get; set; }
        public List<Recipe> CookingList { get; set; }
        public List<Ingredient> SummedIngredients { get; set; }

        public CookingListBuilder()
        {
            CookingList = new List<Recipe>();
            SummedIngredients = new List<Ingredient>();
        }

        public void addRecipe(Recipe recipe)
        {
            CookingList.Add(recipe);
        }
       
        public List<Ingredient> sumUpIngredients()
        {
            SummedIngredients.Clear();
            createSummedUpIngredientsList();
            sortIngredientList();
            return SummedIngredients;
        }

        private void createSummedUpIngredientsList()
        {
            foreach(var recipe in CookingList)
            {
                sumUpIngredients(recipe.Ingredients);
            }
        }

        private void sumUpIngredients(List<Ingredient> ingredients)
        {
            foreach (var current_ingredient in ingredients)
            {
                addIngredientToSummedList(current_ingredient);
            }
        }

        private void addIngredientToSummedList(Ingredient current_ingredient)
        {
            for (int i=0; i<SummedIngredients.Count; i++)
            {
                if (SummedIngredients[i] == current_ingredient)
                {
                    SummedIngredients[i].amount += current_ingredient.amount;
                    return;
                }
            }
            SummedIngredients.Add(current_ingredient);
        }

        private void sortIngredientList()
        {
            List<Ingredient> temp = SummedIngredients.OrderBy(o => o.name).ToList();
            SummedIngredients = temp;
        }
    }
}
