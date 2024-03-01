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

    public int[,] GenerateSudoku()
    {
        sudoku = new int[9, 9];
        FillSudoku();
        RemoveNumbers();
        return sudoku;
    }

    private void FillSudoku()
    {
        int[,] initialPattern = {
            {5, 3, 0, 0, 7, 0, 0, 0, 0},
            {6, 0, 0, 1, 9, 5, 0, 0, 0},
            {0, 9, 8, 0, 0, 0, 0, 6, 0},
            {8, 0, 0, 0, 6, 0, 0, 0, 3},
            {4, 0, 0, 8, 0, 3, 0, 0, 1},
            {7, 0, 0, 0, 2, 0, 0, 0, 6},
            {0, 6, 0, 0, 0, 0, 2, 8, 0},
            {0, 0, 0, 4, 1, 9, 0, 0, 5},
            {0, 0, 0, 0, 8, 0, 0, 7, 9}
        };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudoku[i, j] = initialPattern[i, j];
            }
        }
    }

    private void RemoveNumbers()
    {
        const int DIFFICULTY = 40;
        
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
        SudokuGenerator generator = new SudokuGenerator();
        int[,] sudoku = generator.GenerateSudoku();
        Transfer(generator);
    }
    
    public void Transfer(SudokuGenerator generator)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                TextBox textBox = (TextBox)FindName("p" + (i + 1) + (j + 1));
                textBox.Text = generator.sudoku[i, j].ToString();
            }
        }
    }

}