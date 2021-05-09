using System;
using System.Collections.Generic;

namespace recipe_planer.Models
{
    public class CookingListBuilder
    {
        public List<Recipe> CookingList { get; set; }
        // TODO SPRAWDZIC CZY TU MOZE BYC PRIVATE
        public List<Ingredient> SummedIngredients { get; set; }

        public CookingListBuilder()
        {
            CookingList = new List<Recipe>();
            SummedIngredients = new List<Ingredient>();
        }

        public void AddRecipe(Recipe recipe)
        {
            CookingList.Add(recipe);
            //AddToIngredients(recipe.Ingredients);
        }

        public void RemoveRecipe(Recipe recipe)
        {
            CookingList.Remove(recipe);
            //RemoveFromIngredients(recipe.Ingredients);
        }
        //TODO JESLI LISTA MUSI BYC PUBLIC, TO ZMIENIC NAZWE - PRZEMYSLEC
        public List<Ingredient> getSummedIngredients()
        {
            //createSummedUpIngredientsList();
            ////sortIngredientList();
            //List<Ingredient> result = SummedIngredients;
            //SummedIngredients.Clear();
            //return result;

            SummedIngredients.Clear();
            createSummedUpIngredientsList();
            //sortIngredientList();
            return SummedIngredients;
        }

        private void createSummedUpIngredientsList()
        {
            foreach(var recipe in CookingList)
            {
                sumUpIngredients(recipe.ingredients);
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
    }
}
