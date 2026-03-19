using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace ToDoMaui_Listview;

public partial class MainPage : ContentPage
{
    ObservableCollection<ToDoClass> todoList = new ObservableCollection<ToDoClass>();
    int selectedId = -1;

    public MainPage()
    {
        InitializeComponent();
        todoLV.ItemsSource = todoList;
    }

    private void AddToDoItem(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text) ||
            string.IsNullOrWhiteSpace(detailsEditor.Text))
        {
            DisplayAlert("Error", "Please enter title and details", "OK");
            return;
        }

        var newItem = new ToDoClass
        {
            id = todoList.Any() ? todoList.Max(x => x.id) + 1 : 1,
            title = titleEntry.Text,
            detail = detailsEditor.Text
        };

        todoList.Add(newItem);
        ClearFields();
    }

    private void TodoLV_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;
        var item = e.SelectedItem as ToDoClass;
        titleEntry.Text = item.title;
        detailsEditor.Text = item.detail;
        selectedId = item.id;

        addBtn.IsVisible = false;
        editBtn.IsVisible = true;
        cancelBtn.IsVisible = true;
    }

    private void EditToDoItem(object sender, EventArgs e)
    {
        var item = todoList.FirstOrDefault(x => x.id == selectedId);
        if (item != null)
        {
            item.title = titleEntry.Text;
            item.detail = detailsEditor.Text;
        }
        ResetUI();
    }

    private void CancelEdit(object sender, EventArgs e) => ResetUI();

    private void DeleteToDoItem(object sender, EventArgs e)
    {
        var btn = sender as Button;
        int id = int.Parse(btn.ClassId);
        var item = todoList.FirstOrDefault(x => x.id == id);
        if (item != null) todoList.Remove(item);
        ResetUI();
    }

    private void ResetUI()
    {
        ClearFields();
        selectedId = -1;
        addBtn.IsVisible = true;
        editBtn.IsVisible = false;
        cancelBtn.IsVisible = false;
        todoLV.SelectedItem = null;
    }

    private void ClearFields()
    {
        titleEntry.Text = "";
        detailsEditor.Text = "";
    }

    private void todoLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }
}
