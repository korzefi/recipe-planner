#include "recipeparser.h"
#include <QJsonArray>

QList<Recipe> RecipeParser::parseToRecipes(const QJsonObject& data_object) {
    fillRecipesWithNames(data_object);
    fillRecipesWithDescriptions(data_object);
    fillRecipesWithIngredients(data_object);
    QList<Recipe> result = recipes;
    reset();
    return result;
}

void RecipeParser::fillRecipesWithNames(const QJsonObject& data_object) {
    QStringList recipes_names = getRecipesNames(data_object);
    for (const auto& recipe_name : recipes_names) {
        recipes.append(Recipe(recipe_name));
    }
}

QStringList RecipeParser::getRecipesNames(const QJsonObject& data_object) {
    return data_object.keys();
}

void RecipeParser::fillRecipesWithDescriptions(const QJsonObject &data_object) {
    for (auto& recipe : recipes) {
        recipe.description = getDescription(data_object, recipe.name);
    }
}

QString RecipeParser::getDescription(const QJsonObject &data_object, const QString& recipe_name) {
    QString description;
    QJsonValue description_json = data_object[recipe_name]["recipe"];
    QJsonArray description_lines = description_json.toArray();
    for (const QJsonValue& line : description_lines) {
        description += line.toString();
        description += "\n";
    }
    description.chop(1); //remove last new line char
    return description;
}

void RecipeParser::fillRecipesWithIngredients(const QJsonObject& data_object) {
    for (auto& recipe : recipes) {
        recipe.ingredients = getIngredients(data_object, recipe.name);
    }
}

QList<Ingredient> RecipeParser::getIngredients(const QJsonObject& data_object, const QString& recipe_name) {
    QList<Ingredient> ingredients;
    QStringList ingredients_names = getIngredientsNames(data_object, recipe_name);
    for (const auto& name : ingredients_names) {
        QString amount_with_unit = data_object[recipe_name][name].toString();
        double amount = getAmount(amount_with_unit);
        QString unit = getUnit(amount_with_unit);
        ingredients.append(Ingredient(name, amount, unit));
    }
    return ingredients;
}

QStringList RecipeParser::getIngredientsNames(const QJsonObject& data_object, const QString& recipe_name) {
    QJsonObject recipe_attributes_object = data_object[recipe_name].toObject();
    QStringList recipe_attributes = recipe_attributes_object.keys();
    recipe_attributes.removeOne("recipe");
    return recipe_attributes;
}

double RecipeParser::getAmount(const QString& amount_with_unit) {
    QStringList amount_unit_array = amount_with_unit.split(" ");
    int amount_index = 0;
    return amount_unit_array[amount_index].toDouble();
}

QString RecipeParser::getUnit(const QString &amount_with_unit) {
    QStringList amount_unit_array = amount_with_unit.split(" ");
    int unit_index = 1;
    return amount_unit_array[unit_index];
}

QJsonObject RecipeParser::parseToObject(const QList<Recipe>& data_recipes) {
    QJsonObject result;
    for (const auto& recipe : data_recipes) {
        auto json_recipe_data = buildJsonRecipe(recipe);
        result.insert(recipe.name, json_recipe_data);
    }
    return result;
}

QJsonObject RecipeParser::buildJsonRecipe(const Recipe& recipe) {
    QJsonObject recipe_obj;
    QJsonArray description = buildJsonDescription(recipe.description);
    recipe_obj.insert("recipe", description);
    for (const auto& ingredient : recipe.ingredients) {
        QJsonValue ingredient_val = buildIngredientVal(ingredient.amount, ingredient.unit);
        recipe_obj.insert(ingredient.name, ingredient_val);
    }
    return recipe_obj;
}

QJsonArray RecipeParser::buildJsonDescription(const QString& description) {
    QJsonArray description_array;
    QStringList description_lines = description.split("\n");
    for (const auto& line : description_lines) {
        description_array.append(QJsonValue::fromVariant(line));
    }
    return description_array;
}

QJsonValue RecipeParser::buildIngredientVal(double amount, const QString& unit) {
    QString result = QString::number(amount) + QString(" ") + unit;
    return QJsonValue::fromVariant(result);
}

void RecipeParser::reset() {
    recipes.clear();
}

