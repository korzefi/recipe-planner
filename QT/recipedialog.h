#ifndef RECIPEDIALOG_H
#define RECIPEDIALOG_H

#include <QDialog>
#include <filehandler.h>

namespace Ui {
class RecipeDialog;
}

class RecipeDialog : public QDialog
{
    Q_OBJECT

public:
    explicit RecipeDialog(FileHandler* file_handler, QWidget *parent = nullptr);
    ~RecipeDialog();

    void addModeEnabled(bool trigger);
    void setRecipeToBeEdited(Recipe* recipe);
    void loadRecipeToEdit();

private slots:
    void on_returnButton_clicked();

    void on_saveButton_clicked();

    void on_addRowButton_clicked();

    void on_clearButton_clicked();

private:
    Ui::RecipeDialog *ui;
    FileHandler* file_handler;
    Recipe* edited_recipe;

    bool add_mode;

    void clear_fields();
    void loadName();
    void loadDescription();
    void loadIngredients();
    void addRowInIngredientsField();

    void verifyCorrectness();
    void verifyNameNotEmpty();
    void verifyIngredientsFieldsNotEmpty();
    void verifyIngredientsAmountIsDouble();

    QString getName();
    QString getDescription();
    QList<Ingredient> getIngredients();
    void handleModification(QString name, QString description, QList<Ingredient> ingredients);

};

#endif // RECIPEDIALOG_H
