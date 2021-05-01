#include "filehandler.h"

#include <QMessageBox>
#include <exception>
#include <QJsonDocument>


FileHandler::FileHandler(RecipeParser* parser, QString filename)
    : parser(parser),
      filename(filename), //QScopedPointer does not support std::move
      file(QFile(filename))
{
    loadJsonFile();
}

FileHandler::~FileHandler() {
    if (not recipes.isEmpty()) {
        recipes.clear();
    }
    if (parser) {
        delete parser;
        parser = nullptr;
    }
}

void FileHandler::loadJsonFile() {
    openFileIfCorrect(read);
    data_object = getObjectAndCloseFile();
    recipes = parser->parseToRecipes(data_object);
}

void FileHandler::openFileIfCorrect(FileOpenType type) {
    auto open_type = getOpenFileType(type);
    if (not file.open(open_type)) {
        throw std::runtime_error("Cannot open file: " + filename.toStdString());
    }
}

QFlags<QIODeviceBase::OpenModeFlag> FileHandler::getOpenFileType(FileOpenType type) {
    switch (type) {
    case read:
        return (QIODevice::ReadOnly | QIODevice::Text);
    case write:
        return QIODevice::WriteOnly;
    default:
        throw std::logic_error("Not supported open type");
    }
}

QJsonObject FileHandler::getObjectAndCloseFile() {
    QString file_content = file.readAll();
    file.close();
    auto json_doc = QJsonDocument::fromJson(file_content.toUtf8());
    return json_doc.object();
}

void FileHandler::saveJsonFile() {
    openFileIfCorrect(write);
    data_object = parser->parseToObject(recipes);
    writeAndCloseFile();
}

void FileHandler::writeAndCloseFile() {
    QJsonDocument json_doc(data_object);
    file.write(json_doc.toJson(QJsonDocument::Indented));
    file.close();
}

void FileHandler::addRecipe(Recipe recipe) {
    recipes.append(recipe);
}

void FileHandler::removeRecipe(const QString& recipe_name) {
    int size = recipes.size();
    for (int i=0; i<size; i++) {
        if (recipes[i].name == recipe_name) {
            recipes.removeAt(i);
            break;
        }
    }
}

Recipe* FileHandler::getRecipe(const QString& recipe_name) {
    for (auto& recipe : recipes) {
        if (recipe.name == recipe_name) {
            return &recipe;
        }
    }

    throw std::runtime_error("There is no recipe with given name: " + recipe_name.toStdString());
}

QList<Recipe>* FileHandler::getAllRecipes() {
    return &recipes;
}

QStringList FileHandler::getRecipesNames() {
    QStringList result;
    for (const auto& recipe : recipes) {
        result.append(recipe.name);
    }
    return result;
}


