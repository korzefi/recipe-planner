#include "cookingdialog.h"
#include "ui_cookingdialog.h"
#include <QMessageBox>
#include <iostream>

CookingDialog::CookingDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::CookingDialog)
{
    ui->setupUi(this);
    list_builder = new CookingListBuilder();
    all_recipes_list_model = new QStringListModel();
    picked_list_model = new QStringListModel();
    ingredients_list_model = new QStringListModel();
}

CookingDialog::~CookingDialog()
{
    delete ui;
    delete list_builder;
    delete all_recipes_list_model;
    delete picked_list_model;
    delete ingredients_list_model;
}

void CookingDialog::setCookingList(QList<Recipe>* recipes_list) {
    recipes = recipes_list;
    loadAllRecipeNames();
}

void CookingDialog::loadAllRecipeNames() {
    QStringList names;
    for (const auto& recipe : *recipes) {
        names.append(recipe.name);
    }
    all_recipes_list_model->setStringList(names);
    ui->allRecipesListView->setModel(all_recipes_list_model);
}

void CookingDialog::on_returnButton_clicked() {
    hide();
    parentWidget()->show();
}

void CookingDialog::on_addButton_clicked() {
    QModelIndex index = ui->allRecipesListView->currentIndex();
    QString selected_recipe_name = index.data(Qt::DisplayRole).toString();
    addRecipeToPickedList(selected_recipe_name);
    addIngredientsToList(selected_recipe_name);
}

void CookingDialog::addRecipeToPickedList(const QString& recipe_name) {
    if(recipe_name.isEmpty()) {
        QMessageBox::warning(this, "ERROR", "None recipe was selected");
        return;
    }
    picked_names.append(recipe_name);
    reloadPickedNames();
}

void CookingDialog::reloadPickedNames() {
    picked_list_model->setStringList(picked_names);
    ui->pickedListView->setModel(picked_list_model);
}

void CookingDialog::addIngredientsToList(const QString& recipe_name) {
    Recipe selected_recipe = getRecipe(recipe_name);
    list_builder->addRecipe(selected_recipe);
    getIngredientsAndReload();
}

void CookingDialog::getIngredientsAndReload() {
    QStringList ingredients_list = list_builder->getIngredientsAsStringList();
    ingredients_list_model->setStringList(ingredients_list);
    ui->ingredientsListView->setModel(ingredients_list_model);
}

Recipe CookingDialog::getRecipe(const QString& recipe_name) {
    for (auto& recipe : *recipes) {
        if (recipe.name == recipe_name) {
            return recipe;
        }
    }
    throw std::runtime_error("Recipe does not exist");
}

void CookingDialog::on_removeButton_clicked() {
    QModelIndex index = ui->pickedListView->currentIndex();
    QString selected_recipe_name = index.data(Qt::DisplayRole).toString();
    int list_index = index.row();
    removeRecipeFromPickedList(selected_recipe_name, list_index);
    removeIngredientsFromList(selected_recipe_name);
}

void CookingDialog::removeRecipeFromPickedList(const QString& recipe_name, int index) {
    if(recipe_name.isEmpty()) {
        QMessageBox::warning(this, "ERROR", "None recipe was selected");
        return;
    }
    picked_list_model->removeRows(0, picked_list_model->rowCount());
    picked_names.removeAt(index);
    reloadPickedNames();
}

void CookingDialog::removeIngredientsFromList(const QString& recipe_name) {
    list_builder->removeRecipe(recipe_name);
    getIngredientsAndReload();
}

void CookingDialog::on_clearButton_clicked() {
    clearPickedList();
    clearIngredientsList();
}

void CookingDialog::clearPickedList() {
    picked_names.clear();
    reloadPickedNames();
}

void CookingDialog::clearIngredientsList() {
    list_builder->reset();
    QStringList empty;
    ingredients_list_model->setStringList(empty);
    ui->ingredientsListView->setModel(ingredients_list_model);
}
