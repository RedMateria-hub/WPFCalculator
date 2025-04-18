using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2;

namespace WbfCalculator;

public partial class MainWindow : Window
{
    private string currentValue = "0";

    private string history = "";
    private bool ClearScreen = true;
    private bool decimalInserted = false;
    private int noDecimals = 0;
    private ObservableCollection<double> memoryValues = new ObservableCollection<double>();

    public MainWindow()
    {
        InitializeComponent();
        Update();
    }

    public void SetCurrentValue(string currentValue)
    {
        noDecimals = GetDecimalPlaces(double.Parse(currentValue));
        if (noDecimals > 0 )
            decimalInserted = true;
        this.currentValue = currentValue;
        Update();
    }

    private bool HasDecimal()
    {
        return double.Parse(currentValue) % 1 != 0;
    }
    private static int GetDecimalPlaces(double num)
    {
        string str = num.ToString("G17");
        int index = str.IndexOf('.');
        return (index == -1) ? 0 : str.Length - index - 1;
    }
    private void InputNumber(object sender, RoutedEventArgs e)
    {
        string number = (sender as Button)?.Content.ToString();

        if (number == null) 
            return;

        if (currentValue[0] == '0' && number == "0")
            return;

        if (ClearScreen)
            currentValue = "";

        if (decimalInserted)
            noDecimals++;

        currentValue += number;
        ClearScreen = false;

        Update();
    }

    private void AddDecimal(object sender, RoutedEventArgs e)
    {
        if (ClearScreen) 
            currentValue = "0";
        
        if (decimalInserted || currentValue == "Invalid input") 
            return;

        currentValue += ".";
        decimalInserted = true;
        ClearScreen = false;

        Update();
    }

    private void InputOperator(object sender, RoutedEventArgs e)
    {
        if (currentValue == "" || currentValue == "Invalid input") 
            return;

        ClearScreen = true;
        decimalInserted = false;
        noDecimals = 0;
        history = currentValue + $" {(sender as Button).Content} ";
        currentValue = "0";

        Update();
    }

    private void EqualOperator(object sender, RoutedEventArgs e)
    {
        if (history == "") 
            return;

        string inputOperator;

        if (history.Contains("="))
        {
            string[] temp = history.Split(' ');
            temp[0] = currentValue;
            inputOperator = String.Join(" ", temp);

            history = inputOperator;
        }
        else
        {
            inputOperator = history + currentValue;
            history = inputOperator + " = ";
        }

        currentValue = Calculator.CalculateValue(inputOperator);
        ClearScreen = true;
        decimalInserted = false;
        Update();
    }

    private void MutiplyMinusOne(object sender, RoutedEventArgs e)
    {
        if (currentValue == "0") 
            return;
        
        float tempNum = -1 * float.Parse(currentValue);
        currentValue = tempNum.ToString();
        
        Update();
    }

    private void InputClear(object sender, RoutedEventArgs e)
    {
        currentValue = "0";
        history = "";
        noDecimals = 0;
        decimalInserted = false;
        ClearScreen = true;

        Update();
    }

    private void Backspace(object sender, RoutedEventArgs e)
    {
        if (currentValue == "0") 
            return;
        
        if (currentValue == "Invalid input" ||
            (currentValue.Length == 1 || currentValue.Length == 2 && currentValue.Contains("-")))
        {
            currentValue = "0";
            ClearScreen = true;
        }
        else
        {
            if (currentValue[currentValue.Length - 1] == '.')
               decimalInserted = false;
            if (decimalInserted)
                noDecimals--;
            currentValue = currentValue.Remove(currentValue.Length - 1, 1);
        }
        Update();
    }
    private void DivideOneByNum(object sender, RoutedEventArgs e)
    {
        if (currentValue == "0")
            return;

        currentValue = Calculator.DivideOneBy(currentValue);
        noDecimals = GetDecimalPlaces(double.Parse(currentValue));
        if (HasDecimal())
            decimalInserted = true;

        Update();
    }

    private void SquareNum(object sender, RoutedEventArgs e)
    {
        if (currentValue == "0") 
            return;

        currentValue = Calculator.Square(currentValue);
        noDecimals = GetDecimalPlaces(double.Parse(currentValue));
        if (HasDecimal())
            decimalInserted = true;

        Update();
    }

    private void SquareRootNum(object sender, RoutedEventArgs e)
    {
        if (currentValue == "0") 
            return;

        if (double.Parse(currentValue) < 0)
        {
            currentValue = "Invalid input";
            Update();
            return;
        }

        currentValue = Calculator.SquareRoot(currentValue);
        noDecimals = GetDecimalPlaces(double.Parse(currentValue));
        if (HasDecimal())
            decimalInserted = true;

        Update();
    }

    private void MemoryAdd(object sender, RoutedEventArgs e)
    {
        if (currentValue == "" || currentValue == "Invalid input")
            return;

        if (memoryValues.Count == 0)
            memoryValues.Add(double.Parse(currentValue));
        else
        {
            string newValue = Calculator.CalculateValue(currentValue + " + " + memoryValues[0]);
            memoryValues[0] = double.Parse(newValue);
        }

        Update();
    }

    private void MemorySubstract(object sender, RoutedEventArgs e)
    {
        if (currentValue == "" || currentValue == "Invalid input")
            return;

        if (memoryValues.Count == 0)
            memoryValues.Add(double.Parse(currentValue));
        else
        {
            string newValue = Calculator.CalculateValue(currentValue + " - " + memoryValues[0]);
            memoryValues[0] = double.Parse(newValue);
        }

        Update();
    }

    private void MemoryStore(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(currentValue, out double value))
        {
            memoryValues.Add(value);
            currentValue = "0";
            history = "";
            noDecimals = 0;
            decimalInserted = false;
            ClearScreen = true;

            Update();
        }
    }

    private void OpenMemoryWindow(object sender, RoutedEventArgs e)
    {
        MemoryWindow memoryWindow = new MemoryWindow(memoryValues, currentValue);
        memoryWindow.Show();
    }

    private void OpenInfoWindow(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Calculator made by:\nEnache Nicolae\nGroup:331");
    }

    private void MemoryRecall(object sender, RoutedEventArgs e)
    {
        if (memoryValues.Count > 0)
            SetCurrentValue(memoryValues[0].ToString());
    }

    private void MemoryClear(object sender, RoutedEventArgs e)
    {
        memoryValues.Clear();
    }

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key >= Key.D0 && e.Key <= Key.D9)
        {
            InputNumberFromKeyboard(e.Key - Key.D0);
        }
        else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
        {
            InputNumberFromKeyboard(e.Key - Key.NumPad0);
        }
        else if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
        {
            AddDecimal(null, null);
        }
        else if (e.Key == Key.Back)
        {
            Backspace(null, null);
        }
    }

    private void InputNumberFromKeyboard(int number)
    {
        InputNumber(number.ToString());
    }

    private void InputNumber(string number)
    {
        if (currentValue == "0" && number == "0")
            return;

        if (ClearScreen)
            currentValue = "";

        if (decimalInserted)
            noDecimals++;

        currentValue += number;
        ClearScreen = false;

        Update();
    }

    private void Update()
    {
        InputBlock.Text = currentValue;
        if (!currentValue.Contains("E"))
        {
            if (double.TryParse(currentValue, out double number))
            {
                if (currentValue.Contains("."))
                {
                    string newCode = "N" + noDecimals.ToString();
                    InputBlock.Text = number.ToString(newCode);
                }
                else
                    InputBlock.Text = number.ToString("N0");
            }
        }
        
        if (decimalInserted && noDecimals == 0)
            InputBlock.Text += ".";

        InputHistory.Text = history;
        if (InputBlock.Text == "Invalid input")
        {
            InputHistory.Text = "";
            history = "";
            ClearScreen = true;
        }
    }
}