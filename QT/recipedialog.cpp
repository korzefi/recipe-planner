#include "recipedialog.h"
#include "ui_recipedialog.h"

#include <QStringList>
#include <QMessageBox>

RecipeDialog::RecipeDialog(FileHandler* file_handler, QWidget *parent) :
    QDialog(parent),
    file_handler(file_handler),
    ui(new Ui::RecipeDialog)
{
    ui->setupUi(this);
    ui->ingredientsField->setColumnWidth(0,300);
    ui->ingredientsField->setColumnWidth(1,100);
    ui->ingredientsField->setColumnWidth(2,150);
    add_mode = true;
    edited_recipe = nullptr;
}

RecipeDialog::~RecipeDialog() {
    delete ui;
}

void RecipeDialog::addModeEnabled(bool trigger) {
    add_mode = trigger;
}

void RecipeDialog::setRecipeToBeEdited(Recipe* recipe) {
    edited_recipe = recipe;
}

void RecipeDialog::loadRecipeToEdit() {
    loadName();
    loadDescription();
    loadIngredients();
}

void RecipeDialog::loadName() {
    ui->nameField->setText(edited_recipe->name);
}

void RecipeDialog::loadDescription() {
    ui->descriptionField->setText(edited_recipe->description);
}

void RecipeDialog::loadIngredients() {
    int number_of_ingredients = edited_recipe->ingredients.size();
    ui->ingredientsField->setRowCount(number_of_ingredients);
    int current_row = 0;
    for (const auto& ingredient : edited_recipe->ingredients) {
        ui->ingredientsField->setItem(current_row, 0, new QTableWidgetItem(ingredient.name));
        QString amount_string = QString::number(ingredient.amount);
        ui->ingredientsField->setItem(current_row, 1, new QTableWidgetItem(amount_string));
        ui->ingredientsField->setItem(current_row, 2, new QTableWidgetItem(ingredient.unit));
        current_row++;
    }
}

void RecipeDialog::clear_fields() {
    ui->nameField->clear();
    ui->descriptionField->clear();
    ui->ingredientsField->clear();
    ui->ingredientsField->setRowCount(0);
    QStringList column_labels {"name", "amount", "unit"};
    ui->ingredientsField->setHorizontalHeaderLabels(column_labels);
}

void RecipeDialog::on_returnButton_clicked() {
    hide();
    parentWidget()->show();
}

void RecipeDialog::on_addRowButton_clicked() {
    addRowInIngredientsField();
}

void RecipeDialog::addRowInIngredientsField() {
    int rows = ui->ingredientsField->rowCount();
    ui->ingredientsField->setRowCount(rows+1);
}

void RecipeDialog::on_saveButton_clicked() {
    try {
        verifyCorrectness();
        QString name = getName();
        QString description = getDescription();
        QList<Ingredient> ingredients = getIngredients();
        handleModification(name, description, ingredients);
        file_handler->saveJsonFile();
    }
    catch (std::runtime_error& e) {
        QMessageBox::warning(this, "ERROR", e.what());
    }
}

void RecipeDialog::verifyCorrectness() {
    verifyNameNotEmpty();
    verifyIngredientsFieldsNotEmpty();
    verifyIngredientsAmountIsDouble();
}

void RecipeDialog::verifyNameNotEmpty() {
    if (ui->nameField->text().isEmpty()) {
        throw std::runtime_error("Name of recipe cannot be empty");
    }
}

void RecipeDialog::verifyIngredientsFieldsNotEmpty() {
    int rows = ui->ingredientsField->rowCount();
    int columns = ui->ingredientsField->columnCount();
    for (int i=0; i<rows; i++) {
        for(int j=0; j<columns; j++) {
            auto item = ui->ingredientsField->item(i,j);
            if (not item) {
                throw std::runtime_error("Ingredients fields cannot be empty");
            }
        }
    }
}

void RecipeDialog::verifyIngredientsAmountIsDouble() {
    int rows = ui->ingredientsField->rowCount();
    int amount_index = 1;
    bool is_ok = true;
    for (int i=0; i<rows; i++) {
        auto amount = ui->ingredientsField->item(i, amount_index);
        amount->text().toDouble(&is_ok);
        if(not is_ok) {
            throw std::runtime_error("Ingredients amount should be double");
        }
    }
}

QString RecipeDialog::getName() {
    return ui->nameField->text();
}

QString RecipeDialog::getDescription() {
    return ui->descriptionField->toPlainText();
}

QList<Ingredient> RecipeDialog::getIngredients() {
    QList<Ingredient> result;
    int rows = ui->ingredientsField->rowCount();
    for (int i=0; i<rows; i++) {
        QString name = ui->ingredientsField->item(i,0)->text();
        double amount = ui->ingredientsField->item(i,1)->text().toDouble();
        QString unit = ui->ingredientsField->item(i,2)->text();
        result.append(Ingredient(name, amount, unit));
    }
    return result;
}

void RecipeDialog::handleModification(QString name, QString description, QList<Ingredient> ingredients) {
    if (add_mode == true) {
        Recipe recipe(name, description, ingredients);
        file_handler->addRecipe(recipe);
        clear_fields();
    }
    else {
        edited_recipe->name = name;
        edited_recipe->description = description;
        edited_recipe->ingredients = ingredients;
    }
}

void RecipeDialog::on_clearButton_clicked() {
    if (add_mode == true) {
        clear_fields();
        return;
    }
    QMessageBox::warning(this, "ERROR", "To remove recipe please use main window interface");
}
