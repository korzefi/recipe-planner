using System;
using System.Collections.Generic;
namespace recipe_planer.Models
{
    public class Recipe : IEquatable<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public int RecipeID { get; set; }
        public int SelectedIngredientIndex { get; set; }

        static int counter = 0;


        public Recipe()
        {
        }

        public Recipe(Recipe recipe)
        {
            this.Name = recipe.Name;
            this.Description = recipe.Description;
            this.Ingredients = new List<Ingredient>(recipe.Ingredients);
            this.SelectedIngredientIndex = -1;
            RecipeID = recipe.RecipeID;
        }

        public Recipe(string Name)
        {
            this.Name = Name;
            this.Description = "";
            this.Ingredients = new List<Ingredient>();
            this.SelectedIngredientIndex = -1;
            counter++;
            RecipeID = counter;
        }

        public Recipe(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.Ingredients = new List<Ingredient>();
            this.SelectedIngredientIndex = -1;
            counter++;
            RecipeID = counter;
        }

        public Recipe(string Name, string Description, List<Ingredient> Ingredients)
        {
            this.Name = Name;
            this.Description = Description;
            this.Ingredients = Ingredients;
            this.SelectedIngredientIndex = -1;
            counter++;
            RecipeID = counter;
        }

        public void setSelectedIngredientIndex(string name, string unit)
        {
            for (int i=0; i<Ingredients.Count; i++)
            {
                if(Ingredients[i].Name == name && Ingredients[i].Unit == unit)
                {
                    SelectedIngredientIndex = i;
                    break;
                }
            }
        }

        public static bool operator ==(Recipe LHS, Recipe RHS)
        {
            return LHS.Equals(RHS);
        }

        public static bool operator !=(Recipe LHS, Recipe RHS)
        {
            return !(LHS.Equals(RHS));
        }

        public bool Equals(Recipe other)
        {
            return this.Name.Equals(other.Name);
        }

        public override bool Equals(object other)
        {
            return Equals(other as Recipe);
        }
    }
}
