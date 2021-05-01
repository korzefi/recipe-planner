#include <ingredient.h>

Ingredient::Ingredient(QString name, double amount, QString unit)
    : name(name),
      amount(amount),
      unit(unit) {}

Ingredient& Ingredient::operator=(const Ingredient& other) {
    if (this == &other) {
        return *this;
    }

    this->name = other.name;
    this->amount = other.amount;
    this->unit = other.unit;
    return *this;
}

Ingredient::Ingredient(const Ingredient& other) {
    *this = other;
}

bool Ingredient::operator==(const Ingredient& other) {
    return (name == other.name && unit == other.unit);
}
