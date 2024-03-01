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

namespace SUDOKU;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GenerateSudoku();
    }
    
    public int[,] GenerateSudoku()
    {
        int[,] sudoku = new int[9, 9];
        for (int i = 1; i <= 9; i++)
        {
            for (int j = 1; j <= 9; j++)
            {
                TextBox textBox = (TextBox)FindName("p" + i + j);
                textBox.Text = "7";
            }
        }
        return sudoku;
    }
}