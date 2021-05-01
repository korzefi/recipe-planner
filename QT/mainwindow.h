#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QStringListModel>

#include <filehandler.h>
#include <recipeparser.h>
#include <recipedialog.h>
#include <cookingdialog.h>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow {
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

    void reload_names();

private slots:
    void on_addButton_clicked();

    void on_editButton_clicked();

    void on_deleteButton_clicked();

    void on_cookingButton_clicked();

    void on_saveButton_clicked();

    void on_loadButton_clicked();

private:
    Ui::MainWindow *ui;
    RecipeDialog* recipe_dialog;
    CookingDialog* cooking_dialog;
    RecipeParser* parser;
    FileHandler* file_handler;
    QStringListModel* list_model;

    QPair<int, int> getWindowSize(); //first is width, second is height
};
#endif // MAINWINDOW_H
