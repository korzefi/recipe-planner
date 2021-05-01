#ifndef COOKINGDIALOG_H
#define COOKINGDIALOG_H

#include <QDialog>
#include <cookinglistbuilder.h>
#include <QStringListModel>

namespace Ui {
class CookingDialog;
}

class CookingDialog : public QDialog
{
    Q_OBJECT

public:
    explicit CookingDialog(QWidget *parent = nullptr);
    ~CookingDialog();

    void setCookingList(QList<Recipe>* recipes_list);
    void loadAllRecipeNames();
    void clearPickedList();

private slots:
    void on_returnButton_clicked();

    void on_addButton_clicked();

    void on_removeButton_clicked();

    void on_clearButton_clicked();

private:
    Ui::CookingDialog *ui;
    CookingListBuilder* list_builder;
    QStringListModel* all_recipes_list_model;
    QStringListModel* picked_list_model;
    QStringListModel* ingredients_list_model;

    QList<Recipe>* recipes;
    QStringList picked_names;

    void reloadPickedNames();
    void reloadIngredietsList();
    void addRecipeToPickedList(const QString& recipe_name);
    void addIngredientsToList(const QString& recipe_name);
    void getIngredientsAndReload();
    void removeRecipeFromPickedList(const QString& recipe_name, int index);
    void removeIngredientsFromList(const QString& recipe_name);
    Recipe getRecipe(const QString& recipe_name);
    void clearIngredientsList();
};

#endif // COOKINGDIALOG_H
