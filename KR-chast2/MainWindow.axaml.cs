using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using Action = KR_chast2.Entities.Action;

namespace KR_chast2;

public partial class MainWindow : Window
{
    public string _connString = "server = 10.10.1.24; database = pro1_10; port = 3306; User = user_01; password = user01pro";
    private List<Action> _actions;
    private MySqlConnection _connection;
    public MainWindow()
    {
        InitializeComponent();
        ShowTable();
    }

    private void ShowTable()
    {
        string sql = " select Id,Организатор,Волонтер,Собранные_средства,Дата from Акция";
        _actions = new List<Action>();
        _connection = new MySqlConnection(_connString);
        _connection.Open();
        MySqlCommand command = new MySqlCommand(sql, _connection);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentAction = new Action()
            {
                ActionId = reader.GetInt32(column:"Id"),
                Organizator = reader.GetInt32(column:"Организатор"),
                Volonter = reader.GetInt32(column:"Волонтер"),
                Cash = reader.GetString(column:"Собранные_средства"),
                Date = reader.GetDateTime(column:"Дата")
            };
            _actions.Add(currentAction);
        }
        _connection.Close();
        VolonterGrid.ItemsSource = _actions;
    }

    private void DelBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var selectedItem = VolonterGrid.SelectedItem as Action;
        if (selectedItem is null)
        {
            return;
        }
        _connection.Open();
        string sql = "delete from Акция where Id = @id";
        MySqlCommand command = new MySqlCommand(sql, _connection);
        command.Parameters.AddWithValue("@Id", selectedItem.ActionId);
        command.ExecuteNonQuery();
        _connection.Close();
        var newList = VolonterGrid.ItemsSource.Cast<Action>().ToList();
        var itemToRemove = newList.FirstOrDefault(x => x.ActionId == selectedItem.ActionId);
        newList.Remove(itemToRemove);
        VolonterGrid.ItemsSource = newList;
    }
}