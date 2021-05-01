#ifndef RECIPEPARSER_H
#define RECIPEPARSER_H


#include <QJsonObject>
#include <QJsonValue>
#include <QJsonArray>
#include <QList>
#include <recipe.h>

class RecipeParser {
public:
    RecipeParser() {};

    QList<Recipe> parseToRecipes(const QJsonObject& data_object);
    QJsonObject parseToObject(const QList<Recipe>& data_recipes);

private:
    QList<Recipe> recipes;

    void fillRecipesWithNames(const QJsonObject& data_object);
    void fillRecipesWithDescriptions(const QJsonObject& data_object);
    void fillRecipesWithIngredients(const QJsonObject& data_object);

    QStringList getRecipesNames(const QJsonObject& data_object);
    QString getDescription(const QJsonObject &data_object, const QString& recipe_name);
    QList<Ingredient> getIngredients(const QJsonObject &data_object, const QString& recipe_name);
    QStringList getIngredientsNames(const QJsonObject &data_object, const QString& recipe_name);
    double getAmount(const QString& amount_with_unit);
    QString getUnit(const QString& amount_with_unit);

    QJsonObject buildJsonRecipe(const Recipe& recipe);
    QJsonArray buildJsonDescription(const QString& description);
    QJsonValue buildIngredientVal(double amount, const QString& unit);


    void reset();
};

#endif // RECIPEPARSER_H
