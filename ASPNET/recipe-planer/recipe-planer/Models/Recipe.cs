using System;
using System.Collections.Generic;
namespace recipe_planer.Models
{
    public class Recipe : IEquatable<Recipe>
    {
        public string name;
        public string description;
        public List<Ingredient> ingredients;

        public Recipe(string Name)
        {
            this.name = Name;
            this.description = "";
            this.ingredients = new List<Ingredient>();
        }

        public Recipe(string Name, string Description)
        {
            this.name = Name;
            this.description = Description;
            this.ingredients = new List<Ingredient>();
        }

        public Recipe(string Name, string Description, List<Ingredient> Ingredients)
        {
            this.name = Name;
            this.description = Description;
            this.ingredients = Ingredients;
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
            return this.name.Equals(other.name);
        }

        public override bool Equals(object other)
        {
            return Equals(other as Recipe);
        }
    }
}
