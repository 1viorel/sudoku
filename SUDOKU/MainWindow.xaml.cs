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
        SolveSudoku();
        RemoveNumbers(diff);
        return sudoku;
    }
    private bool SolveSudoku()
    {
        int row, col;

        // Find an empty cell
        if (!FindEmptyCell(out row, out col))
        {
            // If there are no empty cells, the Sudoku is solved
            return true;
        }

        // Try filling the empty cell with a valid number
        for (int num = 1; num <= 9; num++)
        {
            if (IsSafe(row, col, num))
            {
                // Place the number if it's safe
                sudoku[row, col] = num;

                // Recursively try to fill the rest of the Sudoku
                if (SolveSudoku())
                {
                    return true;
                }

                // If placing the number leads to an invalid solution, backtrack
                sudoku[row, col] = 0;
            }
        }

        // If no number can be placed, backtrack
        return false;
    }

    private bool IsSafe(int row, int col, int num)
    {
        // Check if 'num' is not already in the current row, column, and 3x3 subgrid
        return !UsedInRow(row, num) && !UsedInColumn(col, num) && !UsedInSubgrid(row - row % 3, col - col % 3, num);
    }

    private bool UsedInRow(int row, int num)
    {
        for (int col = 0; col < 9; col++)
        {
            if (sudoku[row, col] == num)
            {
                return true;
            }
        }

        return false;
    }

    private bool UsedInColumn(int col, int num)
    {
        for (int row = 0; row < 9; row++)
        {
            if (sudoku[row, col] == num)
            {
                return true;
            }
        }

        return false;
    }

    private bool UsedInSubgrid(int startRow, int startCol, int num)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (sudoku[startRow + row, startCol + col] == num)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool FindEmptyCell(out int row, out int col)
    {
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                if (sudoku[row, col] == 0)
                {
                    return true; // Found an empty cell
                }
            }
        }
        row = col = -1; // No empty cell found
        return false;
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