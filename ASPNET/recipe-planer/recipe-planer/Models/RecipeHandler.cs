using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace recipe_planer.Models
{
    public class RecipeHandler
    {
        public List<Recipe> Recipes { get; set; }
        private string filepath;
        private JObject data_object;

        public RecipeHandler(string filepath)
        {
            Recipes = new List<Recipe>();
            this.filepath = filepath;
        }

        public void addRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }

        public void removeRecipe(Recipe recipe)
        {
            Recipes.Remove(recipe);
        }

        public void loadJsonFile()
        {
            string jsonString = File.ReadAllText(filepath);
            data_object = JObject.Parse(jsonString);
            fillRecipesWithNames();
            fillRecipesWithDescriptions();
            fillRecipesWithIngredients();
        }

        public void saveJsoFile()
        {
            FileStream fs = File.Create(filepath);
            //fs.Write()
        }

        private void fillRecipesWithNames()
        {
            List<string> recipes_names = getRecipesNames();
            foreach (var recipe_name in recipes_names)
            {
                Recipes.Add(new Recipe(recipe_name));
            }
        }

        private List<string> getRecipesNames()
        {
            List<string> keys = new List<string>();
            foreach (JProperty property in data_object.Properties())
            {
                keys.Add(property.Name);
            }
            return keys;
        }

        private void fillRecipesWithDescriptions()
        {
            for(int i=0; i<Recipes.Count; i++)
            {
                Recipes[i].description = getDescription(Recipes[i].name);
            }
        }

        private string getDescription(string recipe_name)
        {
            string description = "";
            JToken description_json = data_object[recipe_name]["recipe"];
            //var description_lines = data_object.Values<string>();
            JArray description_lines = JArray.Parse(description_json.ToString());
            foreach (var line in description_lines)
            {
                description += line.ToString();
                description += "\n";
            }
            description.TrimEnd(); //remove last new line char
            return description;
        }

        private void fillRecipesWithIngredients()
        {
            for (int i=0; i<Recipes.Count; i++)
            {
                Recipes[i].ingredients = getIngredients(Recipes[i].name);
            }
        }

        private List<Ingredient> getIngredients(string recipe_name)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            List<string> ingredients_names = getIngredientsNames(recipe_name);
            foreach(var name in ingredients_names)
            {
                string amount_with_unit = data_object[recipe_name][name].ToString();
                double amount = getAmount(amount_with_unit);
                string unit = getUnit(amount_with_unit);
                ingredients.Add(new Ingredient(name, amount, unit));
            }
            return ingredients;
        }

        private List<string> getIngredientsNames(string recipe_name)
        {
            List<string> recipe_attributes = new List<string>();
            var recipe_attributes_json = data_object[recipe_name];
            foreach (JProperty property in recipe_attributes_json)
            {
                recipe_attributes.Add(property.Name);
            }
            recipe_attributes.Remove("recipe");
            return recipe_attributes;
        }

        private double getAmount(string amount_with_unit)
        {
            string[] amount_unit_array = amount_with_unit.Split(' ');
            int amount_index = 0;
            return Convert.ToDouble(amount_unit_array[amount_index]);
        }

        private string getUnit(string amount_with_unit)
        {
            string[] amount_unit_array = amount_with_unit.Split(' ');
            int unit_index = 1;
            return amount_unit_array[unit_index];
        }
    }
}
