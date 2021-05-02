﻿using System;
using System.Collections.Generic;
namespace recipe_planer.Models
{
    public class Recipe : IEquatable<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe(string Name)
        {
            this.Name = Name;
            this.Description = "";
            this.Ingredients = new List<Ingredient>();
        }

        public Recipe(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
            this.Ingredients = new List<Ingredient>();
        }

        public Recipe(string Name, string Description, List<Ingredient> Ingredients)
        {
            this.Name = Name;
            this.Description = Description;
            this.Ingredients = Ingredients;
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