using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BudgetTracker
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    namespace butszet
    {
        public partial class MainWindow : Window
        {
            private List<BudgetItem> budgetItems = new List<BudgetItem>();
            private CollectionViewSource budgetItemsView = new CollectionViewSource();

            public MainWindow()
            {
                InitializeComponent();
                budgetItemsView.Source = budgetItems;
                lstBudgetItems.ItemsSource = budgetItemsView.View;
            }

            private void btnAdd_Click(object sender, RoutedEventArgs e)
            {
                AddEditBudgetItemWindow addEditWindow = new AddEditBudgetItemWindow();
                addEditWindow.ShowDialog();
                BudgetItem newItem = addEditWindow.NewBudgetItem;
                if (newItem != null)
                {
                    budgetItems.Add(newItem);
                    budgetItemsView.View.Refresh();
                }
            }

            private void btnEdit_Click(object sender, RoutedEventArgs e)
            {
                BudgetItem selectedItem = lstBudgetItems.SelectedItem as BudgetItem;
                if (selectedItem != null)
                {
                    AddEditBudgetItemWindow addEditWindow = new AddEditBudgetItemWindow(selectedItem);
                    addEditWindow.ShowDialog();
                    budgetItemsView.View.Refresh();
                }
            }

            private void btnDelete_Click(object sender, RoutedEventArgs e)
            {
                BudgetItem selectedItem = lstBudgetItems.SelectedItem as BudgetItem;
                if (selectedItem != null)
                {
                    budgetItems.Remove(selectedItem);
                    budgetItemsView.View.Refresh();
                }
            }

            private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {

            }
        }
    }
    [Serializable]
    public class BudgetItem : INotifyPropertyChanged
    {
        private string _name;
        private string _type;
        private decimal _amount;
        private bool _isIncome;
        private DateTime _date;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public bool IsIncome
        {
            get { return _isIncome; }
            set
            {
                if (_isIncome != value)
                {
                    _isIncome = value;
                    OnPropertyChanged("IsIncome");
                }
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class Serializer
    {
        public static void Serialize<T>(T obj, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }
    } 
}