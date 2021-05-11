using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;

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
            data_object = new JObject();
        }

        public void deleteIngredient(int recipeID, string ingredient_name, string unit)
        {
            var current_recipe = Recipes.Find(r => r.RecipeID == recipeID);
            var ingredient_to_remove = current_recipe.Ingredients.Find(i => (i.name == ingredient_name && i.unit == unit));
            current_recipe.Ingredients.Remove(ingredient_to_remove);
        }

        //public Recipe findRecipe(int id)
        //{
        //    for(int i=0; i<Recipes.Count; i++) {
        //        if(Recipes[i].RecipeID == id)
        //        {
        //            return Recipes[i];
        //        }
        //    }
        //    throw new InvalidOperationException("RECIPE ID FAIL NUMBER");
        //}

        public void loadJsonFile()
        {
            Recipes.Clear();
            string jsonString = File.ReadAllText(filepath);
            data_object = JObject.Parse(jsonString);
            fillRecipesWithNames();
            fillRecipesWithDescriptions();
            fillRecipesWithIngredients();
        }

        public void saveJsonFile()
        {
            loadRecipesToObject();
            string jsonResult = JsonConvert.SerializeObject(data_object, Formatting.Indented);
            File.WriteAllText(filepath, jsonResult);
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
                Recipes[i].Description = getDescription(Recipes[i].Name);
            }
        }

        private string getDescription(string recipe_name)
        {
            string description = "";
            JToken description_json = data_object[recipe_name]["recipe"];
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
                Recipes[i].Ingredients = getIngredients(Recipes[i].Name);
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

        private void loadRecipesToObject()
        {
            data_object.RemoveAll();

            foreach (var recipe in Recipes)
            {
                data_object[recipe.Name] = buildJsonRecipe(recipe);
            }
        }

        private JObject buildJsonRecipe(Recipe recipe)
        {
            JObject result = new JObject();
            result["recipe"] = buildDescription(recipe.Description);
            foreach (var ingredient in recipe.Ingredients)
            {
                string ingredient_value = buildIngredientValue(ingredient.amount, ingredient.unit);
                result[ingredient.name] = ingredient_value;
            }

            return result;
        }

        private JArray buildDescription(string description)
        {
            JArray result = new JArray();
            var description_lines = Regex.Split(description, "\r\n|\r|\n");
            foreach(var line in description_lines)
            {
                if (line == "")
                {
                    continue;
                }
                result.Add(line);
            }
            return result;
        }

        private string buildIngredientValue(double amount, string unit)
        {
            return amount.ToString() + " " + unit;
        }
    }
}
