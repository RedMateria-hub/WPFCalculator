using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WbfCalculator;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MemoryWindow.xaml
    /// </summary>
    public partial class MemoryWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<double> MemoryValues { get; set; }
        private string currentValue;
        public MemoryWindow(ObservableCollection<double> memoryValues, string selectedValue)
        {
            InitializeComponent();
            MemoryValues = new ObservableCollection<double>(memoryValues);
            DataContext = this;
            MemoryListBox.ItemsSource = MemoryValues;
            this.currentValue = selectedValue;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                Clipboard.SetText(selectedValue.ToString());
            }
        }

        private void ClearMemory_Click(object sender, RoutedEventArgs e)
        {
            MemoryValues.Clear();
        }

        private void RemoveFromListBox(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                MemoryValues.Remove(selectedValue);
            }
        }

        private void AddFromListBox(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var parentWindow = Application.Current.MainWindow as MainWindow;
            if (parentWindow != null && MemoryListBox.SelectedItem is double selectedValue)
            {
                parentWindow.SetCurrentValue(selectedValue.ToString());
            }

            this.Close();
        }
    }
}
