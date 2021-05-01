#ifndef RECIPE_H
#define RECIPE_H

#include <QString>
#include <QList>
#include <ingredient.h>


struct Recipe {
public:
    QString name;
    QString description;
    QList<Ingredient> ingredients;

    Recipe(QString name);
    Recipe(QString name, QString description);
    Recipe(QString name, QString description, QList<Ingredient> ingredients);
    ~Recipe() {};

    Recipe(const Ingredient& other);
    Recipe& operator=(const Recipe& other);
    bool operator==(const Recipe& other);
};


#endif // RECIPE_H
