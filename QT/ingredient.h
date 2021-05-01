#ifndef INGREDIENT_H
#define INGREDIENT_H

#include <QString>

struct Ingredient {
public:
    QString name;
    double amount;
    QString unit;

    Ingredient(QString name, double amount, QString unit);
    Ingredient(const Ingredient& other);
    Ingredient& operator=(const Ingredient& other);
    bool operator==(const Ingredient& other);
};

#endif // INGREDIENT_H
