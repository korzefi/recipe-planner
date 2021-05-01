#ifndef FILEHANDLER_H
#define FILEHANDLER_H

#include <recipeparser.h>
#include <recipe.h>
#include <QString>
#include <QFile>
#include <QJsonObject>


/**
 * builder type
 * it is possible to call addRecipe, removeRecipe and then saveJsonFile to store the result
 * it is possible to call loadJsonFile to overwrite the results with the last written version
 */
class FileHandler {
public:
    FileHandler(RecipeParser* parser, QString filename);
    ~FileHandler();

    void loadJsonFile();
    void saveJsonFile();
    void addRecipe(Recipe recipe);
    void removeRecipe(const QString& recipe_name);
    Recipe* getRecipe(const QString& recipe_name);
    QList<Recipe>* getAllRecipes();
    QStringList getRecipesNames();

private:
    enum FileOpenType{
        read,
        write
    };
    RecipeParser* parser;
    QString filename;
    QFile file;
    QJsonObject data_object;
    QList<Recipe> recipes;

    void openFileIfCorrect(FileOpenType type);
    QFlags<QIODeviceBase::OpenModeFlag> getOpenFileType(FileOpenType type);
    QJsonObject getObjectAndCloseFile();
    void writeAndCloseFile();
};

#endif // FILEHANDLER_H
