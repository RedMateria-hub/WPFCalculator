using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WbfCalculator
{
    public class Calculator
    {
        public static string CalculateValue(string value)
        {
            if (value.Length == 0) 
                return "";

            string[] splitVal = value.Split(' ');

            float result = 0;
            float leftVal = float.Parse(splitVal[0]);
            float rightVal = float.Parse(splitVal[2]);

            switch (splitVal[1])
            {
                case "+":
                    result = leftVal + rightVal;
                    break;
                case "-":
                    result = leftVal - rightVal;
                    break;
                case "*":
                    result = leftVal * rightVal;
                    break;
                case "%":
                    result = leftVal % rightVal;
                    break;
                case "/":
                    if (rightVal == 0)
                        return "Invalid input";
                    else
                        result = leftVal / rightVal;
                    break;
            }

            return result.ToString();
        }

        public static string DivideOneBy(string value)
        {
            float temp = float.Parse(value);
            if (temp == 0)
                return "Invalid input";
            return (1 / temp).ToString();
        }

        public static string Square(string value)
        {
            float temp = float.Parse(value);
            return (temp * temp).ToString();
        }

        public static string SquareRoot(string value)
        {
            float temp = float.Parse(value);

            if (temp < 0)
                return "Invalid input";

            return Math.Sqrt(temp).ToString();
        }
    }
}
