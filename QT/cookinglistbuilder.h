#ifndef COOKINGLISTBUILDER_H
#define COOKINGLISTBUILDER_H

#include <recipe.h>
#include <QList>


class CookingListBuilder {
public:
    CookingListBuilder();
    ~CookingListBuilder() {};

    void addRecipe(Recipe recipe);
    void removeRecipe(int index);
    void removeRecipe(const QString& recipe_name);
    QList<Ingredient> getIgredientsList();
    QStringList getIngredientsAsStringList();
    void reset();

private:
    QList<Recipe> cooking_list;
    QList<Ingredient> ingredients_list;

    void createSummedUpIngredientsList();
    void sumUpIngredients(const QList<Ingredient>& ingredients);
    void addIngredientToSummedList(const Ingredient& ingredient);
    QList<Ingredient>::Iterator getIngredient();
    void sortIngredientList();
    QStringList getStringListIngredients();
    QString buildIngredientString(const Ingredient& ingredient);
};

#endif // COOKINGLISTBUILDER_H
