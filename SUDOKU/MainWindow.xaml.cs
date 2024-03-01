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

 public class SudokuGenerator
{
    public int[,] sudoku;

    public int[,] GenerateSudoku(int diff)
    {
        sudoku = new int[9, 9];
        FillSudoku();
        RemoveNumbers(diff);
        return sudoku;
    }

    private void FillSudoku()
    {
        int[,] initialPattern = {
            {7, 9, 5, 6, 2, 1, 8, 4, 3},
            {1, 2, 3, 4, 8, 9, 5, 6, 7},
            {6, 4, 8, 3, 7, 5, 9, 1, 2},
            {8, 7, 2, 5, 1, 4, 3, 9, 6},
            {9, 5, 4, 8, 3, 6, 2, 7, 1},
            {3, 1, 6, 2, 9, 7, 4, 5, 8},
            {4, 8, 1, 7, 5, 3, 6, 2, 9},
            {2, 6, 7, 9, 4, 8, 1, 3, 5},
            {5, 3, 9, 1, 6, 2, 7, 8, 4}
        };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudoku[i, j] = initialPattern[i, j];
            }
        }
    }

    private void RemoveNumbers(int DIFFICULTY)
    {
       
        
        Random random = new Random();

        for (int i = 0; i < DIFFICULTY; i++)
        {
            int row = random.Next(9);
            int col = random.Next(9);
            

            sudoku[row, col] = 0;
        }
    }
}
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private int Difficulty()
    {
        RadioButton easy = (RadioButton)FindName("EasySel");
        RadioButton medium = (RadioButton)FindName("MediumSel");
        RadioButton hard = (RadioButton)FindName("HardSel");
        RadioButton impossible = (RadioButton)FindName("ImposibleSel");
        if (easy.IsChecked == true)
        {
            return 15;
        }
        else if (medium.IsChecked == true)
        {
            return 30;
        }
        else if (hard.IsChecked == true)
        {
            return 45;
        }
        else 
        {
            return 80;
        }
    }
    public void Transfer(SudokuGenerator generator)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                TextBox textBox = (TextBox)FindName("P" + (i + 1) + (j + 1));
                if ((generator.sudoku[i,j]) == 0)
                {
                    textBox.Text = "";
                }
                else {
                    textBox.Text = generator.sudoku[i, j].ToString();
                }
            }
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        int diff = Difficulty();
        SudokuGenerator generator = new SudokuGenerator();
        int[,] sudoku = generator.GenerateSudoku(diff);
        Transfer(generator);
    }
}