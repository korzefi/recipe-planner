#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "QMessageBox"
#include <iostream>
#include <recipe.h>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    list_model = new QStringListModel(this);
    QString filepath = "/Users/filip/Desktop/INFA/EGUI/github-korzefi/QT/data.json";
    try {
        parser = new RecipeParser();
        file_handler = new FileHandler(parser, filepath);
        recipe_dialog = new RecipeDialog(file_handler, this);
        cooking_dialog = new CookingDialog(this);
    }
    catch (std::runtime_error& e) {
        QMessageBox::critical(this, "ERROR", e.what());
    }
    reload_names();
}

MainWindow::~MainWindow() {
    file_handler->saveJsonFile(); //saves all the data at the end;
    delete ui;
    delete cooking_dialog;
    delete recipe_dialog;

    delete parser;
    delete list_model;
}

void MainWindow::reload_names() {
    QStringList names = file_handler->getRecipesNames();
    list_model->setStringList(names);
    ui->listView->setModel(list_model);
}

void MainWindow::on_addButton_clicked() {
    auto window_dimensions = getWindowSize();
    recipe_dialog->resize(window_dimensions.first, window_dimensions.second);
    recipe_dialog->addModeEnabled(true);
    recipe_dialog->show();
    hide();
}

void MainWindow::on_editButton_clicked()
{
    QModelIndex index = ui->listView->currentIndex();
    QString selected_recipe_name = index.data(Qt::DisplayRole).toString();
    if(selected_recipe_name.isEmpty()) {
        QMessageBox::warning(this, "ERROR", "None recipe was selected");
        return;
    }
    auto recipe_to_edit = file_handler->getRecipe(selected_recipe_name);
    auto window_dimensions = getWindowSize();
    recipe_dialog->resize(window_dimensions.first, window_dimensions.second);
    recipe_dialog->addModeEnabled(false);
    recipe_dialog->setRecipeToBeEdited(recipe_to_edit);
    recipe_dialog->loadRecipeToEdit();
    recipe_dialog->show();
    hide();
}

void MainWindow::on_deleteButton_clicked() {
    QModelIndex index = ui->listView->currentIndex();
    QString selected_recipe_name = index.data(Qt::DisplayRole).toString();
    if(selected_recipe_name.isEmpty()) {
        QMessageBox::warning(this, "ERROR", "None recipe was selected");
        return;
    }
    file_handler->removeRecipe(selected_recipe_name);
    list_model->removeRows(0, list_model->rowCount());
    reload_names();
}

void MainWindow::on_cookingButton_clicked() {
    cooking_dialog->setCookingList(file_handler->getAllRecipes());
    auto window_dimensions = getWindowSize();
    cooking_dialog->resize(window_dimensions.first, window_dimensions.second);
    cooking_dialog->clearPickedList();
    cooking_dialog->show();
    hide();
}

void MainWindow::on_saveButton_clicked() {
    file_handler->saveJsonFile();
}

QPair<int,int> MainWindow::getWindowSize() {
    QPair<int,int> result;
    result.first = this->size().width();
    result.second = this->size().height();
    return result;
}

void MainWindow::on_loadButton_clicked() {
    reload_names();
}
