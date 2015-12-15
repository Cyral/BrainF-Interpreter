using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Cyral.BrainF.Interpreter;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Interpreter Interpreter { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClearInput_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
        }

        private void btnClearOutput_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (Interpreter == null || Interpreter.Stopped)
            {
                //If the interpreter has not been created or the source has changed, create it
                if (Interpreter == null ||
                    !txtSource.Text.Replace(" ", "").ToCharArray().SequenceEqual(Interpreter.Source))
                {
                    memoryGrid.Items.Clear();
                    Interpreter = new Interpreter(10000, txtSource.Text, (i, b) =>
                        ((MemItem)memoryGrid.Items[i]).Value = b);
                    for (var i = 0; i < Interpreter.Cells; i++)
                        memoryGrid.Items.Add(new MemItem() { Address = i, Value = 0 });
                }
                btnRun.Content = "Stop";

                //Run the interpreter in a new thread and display the results as they come (using yield)
                new Thread(() =>
                {
                    try
                    {
                       Interpreter.Run(c =>
                            Application.Current.Dispatcher.Invoke(() => txtOutput.Text += c), () => ' ');
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error");
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Interpreter.Stopped = true;
                        btnRun.Content = "Run";
                    });
                }).Start();
            }
            else
            {
                Interpreter.Stopped = true;
                btnRun.Content = "Run";
            }
            */
        }
    }

    public class MemItem : INotifyPropertyChanged
    {
        private int address;
        private byte value;

        public int Address
        {
            get { return address; }
            set
            {
                if (value != this.address)
                    OnPropertyChanged();
                address = value;
            }
        }

        public byte Value
        {
            get { return value; }
            set
            {
                if (value != this.value)
                {
                    OnPropertyChanged();
                    // ReSharper disable once ExplicitCallerInfoArgument
                    OnPropertyChanged("Char");
                }
                this.value = value;
            }
        }

        public char Char => char.IsLetterOrDigit((char)value) ? (char)value : ' ';

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}