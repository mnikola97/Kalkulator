using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string result = "0";
        bool dotPressed = false;
        Stack<string> stack;
        List<int> OperationPriority;
        public MainWindow()
        {
            InitializeComponent();
            this.txtBoxResult.IsReadOnly = true;
            OperationPriority= new List<int>();
            stack = new Stack<string>();
        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            if (!dotPressed)
            {
                result += ",";
                dotPressed = true;
                this.txtBoxResult.Text = result;
            }
        }

        private void btnNumbersClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if(this.OperationPriority.Count>0) result= button.Content.ToString();

            else if(result=="0")
            {
                if(button.Content.ToString()!="0")
                {
                    result = button.Content.ToString();
                }
            }
            else
            {
                result += button.Content.ToString();
            }
            this.txtBoxResult.Text = result;
        }

        private async void DoOperation()
        {
            double number1 = Double.Parse(this.stack.Pop());
            string operation = this.stack.Pop();
            double number2 = Double.Parse(this.stack.Pop());
            if(operation=="/" && number1 == 0)
            {
                MessageBox.Show("It is not allowed to divide with 0. Everything will be cleared");
                this.Clear();
            }
            switch(operation)
            {
                case "+":  
                    this.result = (number2+number1).ToString();
                    this.OperationPriority.Remove(1);
                    break;
                case"-":
                    this.result = (number2 - number1).ToString();
                    this.OperationPriority.Remove(1);
                    break;
                case "*":
                    this.result = (number2 * number1).ToString();
                    this.OperationPriority.Remove(2);
                    break;
                case "/":
                    this.result = (number2 / number1).ToString();
                    this.OperationPriority.Remove(2);
                    break;
                default:
                    this.result = "0"; 
                    break;

            }
            this.txtBoxResult.Text = result;
            this.stack.Push(result);
        }

        //method for addition and subdivison
        private void OperationPriorityOne(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.stack.Push(result);

            if (this.OperationPriority.Count == 0 || this.OperationPriority.Max() < 1)
            {
                this.stack.Push(button.Content.ToString());
                this.OperationPriority.Add(1);
            }
            else
            {

                while (this.OperationPriority.Count != 0)
                    this.DoOperation();
                stack.Push(button.Content.ToString());
                this.OperationPriority.Add(1);
            }
        }

        //method for multiplication and division
        private void OperationPriorityTwo(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.stack.Push(result);

            if (this.OperationPriority.Count == 0 || this.OperationPriority.Max() < 2)
            {
                this.stack.Push(button.Content.ToString());
                this.OperationPriority.Add(2);
            }
            else
            {

                this.DoOperation();
                stack.Push(button.Content.ToString());
                this.OperationPriority.Add(2);
            }
        }


        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            this.stack.Push(result);
            while(stack.Count>1)
            {
                DoOperation();
            }
            this.txtBoxResult.Text = this.stack.Pop();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Clear();
            
        }

        private void Clear()
        {
            this.stack.Clear();
            this.result = "0";
            this.dotPressed = false;
            this.OperationPriority.Clear();
            this.txtBoxResult.Text = result;
        }
    }
}
