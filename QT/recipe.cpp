#include "recipe.h"

Recipe::Recipe(QString name, QString description, QList<Ingredient> ingredients)
    : name(name),
      description(description),
      ingredients(ingredients) {}

Recipe::Recipe(QString name, QString description)
    : name(name),
      description(description)
{
    this->ingredients = QList<Ingredient>();
}

Recipe::Recipe(QString name)
    : name(name)
{
    this->description = "";
    this->ingredients = QList<Ingredient>();
}

Recipe& Recipe::operator=(const Recipe& other) {
    if (this == &other) {
        return *this;
    }

    this->name = other.name;
    this->description = other.description;
    for (const auto& ingredient : other.ingredients) {
        this->ingredients.append(Ingredient(ingredient));
    }
    return *this;
}

Recipe::Recipe(const Ingredient& other) {
    *this = other;
}

bool Recipe::operator==(const Recipe& other) {
    return name == other.name;
}
