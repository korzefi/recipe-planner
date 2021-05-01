#include "cookinglistbuilder.h"

CookingListBuilder::CookingListBuilder() {
    cooking_list = QList<Recipe>();
    ingredients_list = QList<Ingredient>();
}

void CookingListBuilder::addRecipe(Recipe recipe) {
    cooking_list.append(recipe);
}

void CookingListBuilder::removeRecipe(int index) {
    cooking_list.removeAt(index);
}

void CookingListBuilder::removeRecipe(const QString& recipe_name) {
    int size = cooking_list.size();
    for (int i=0; i<size; i++) {
        if (cooking_list[i].name == recipe_name) {
            cooking_list.removeAt(i);\
            break;
        }
    }
}

QList<Ingredient> CookingListBuilder::getIgredientsList() {
    createSummedUpIngredientsList();
    sortIngredientList();
    auto  result = ingredients_list;
    ingredients_list.clear();
    return result;
}

void CookingListBuilder::createSummedUpIngredientsList() {
    for (const auto& recipe : cooking_list) {
        sumUpIngredients(recipe.ingredients);
    }
}

void CookingListBuilder::sumUpIngredients(const QList<Ingredient>& ingredients) {
    for (const auto& current_ingredient : ingredients) {
        addIngredientToSummedList(current_ingredient);
    }
}

void CookingListBuilder::addIngredientToSummedList(const Ingredient& current_ingredient) {
    for (auto& summed_ingredient : ingredients_list) {
        if (summed_ingredient == current_ingredient) {
            summed_ingredient.amount += current_ingredient.amount;
            return;
        }
    }
    ingredients_list.append(current_ingredient);
}

void CookingListBuilder::sortIngredientList() {
    std::sort(ingredients_list.begin(),
              ingredients_list.end(),
              [&](const Ingredient& ingredient1, const Ingredient& ingredient2)
    { return ingredient1.name < ingredient2.name; }
    );
}

QStringList CookingListBuilder::getIngredientsAsStringList() {
    createSummedUpIngredientsList();
    sortIngredientList();
    auto result = getStringListIngredients();
    ingredients_list.clear();
    return result;
}

QStringList CookingListBuilder::getStringListIngredients() {
    QStringList result;
    for (const auto& ingredient : ingredients_list) {
        QString ingredient_string = buildIngredientString(ingredient);
        result.append(ingredient_string);
    }
    return result;
}

QString CookingListBuilder::buildIngredientString(const Ingredient& ingredient) {
    return ingredient.name + ": " + QString::number(ingredient.amount) + " " + ingredient.unit;
}

void CookingListBuilder::reset() {
    ingredients_list.clear();
    cooking_list.clear();
}
