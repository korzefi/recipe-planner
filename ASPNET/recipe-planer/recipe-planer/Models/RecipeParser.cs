using System;
using System.Text.Json;
using System.Collections.Generic;

namespace recipe_planer.Models
{
    public class RecipeParser
    {
        public List<Recipe> parseToRecipes(string jsonString)
        {
            return new List<Recipe>();
        }

        public JsonDocument parseToDocument(List<Recipe> recipes)
        {
            string mock_data = "";
            return JsonDocument.Parse(mock_data);
        }

        public RecipeParser()
        {
        }
    }
}
