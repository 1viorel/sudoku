﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SUDOKU;

public class SudokuGenerator
{
    public int[,] sudoku;
    public int[,] puzzle;
    
    public int[,] GenerateSudoku()
    {
        sudoku = new int[9, 9];
        SolveSudoku();
        return sudoku;
    }
    public int[,] MakePuzzle(int diff)
    {
        puzzle = (int[,])sudoku.Clone();
        RemoveNumbers(diff);
        return puzzle;
    }
    private bool SolveSudoku()
    {
        int row, col;

        if (!FindEmptyCell(out row, out col))
        {
            return true;
        }

        List<int> numbersToTry = GetShuffledNumbers(); // Shuffle numbers from 1 to 9

        foreach (int num in numbersToTry)
        {
            if (IsSafe(row, col, num))
            {
                sudoku[row, col] = num;

                if (SolveSudoku())
                {
                    return true;
                }

                sudoku[row, col] = 0;
            }
        }

        return false;
    }
    private List<int> GetShuffledNumbers()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        Random random = new Random();

        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        return numbers;
    }
    private bool IsSafe(int row, int col, int num)
    {
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
    private void RemoveNumbers(int difficulty)
    {
        
        Random random = new Random();

        for (int i = 0; i < difficulty; i++)
        {
            int row = random.Next(9);
            int col = random.Next(9);


            puzzle[row, col] = 0;
        }
    }
}

public partial class MainWindow 
{  
    SudokuGenerator _generator = new SudokuGenerator();
    private DispatcherTimer timer;
    private TimeSpan timeElapsed;
    public MainWindow()
    {
        InitializeComponent();
    }

   
    private void Timer_Tick(object sender, EventArgs e)
    {
        timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
        TimerTextBlock.Text = timeElapsed.ToString(@"hh\:mm\:ss");
    }
    private int Difficulty()
    {
        RadioButton easy = (RadioButton)FindName("EasySel");
        RadioButton medium = (RadioButton)FindName("MediumSel");
        RadioButton hard = (RadioButton)FindName("HardSel");
        RadioButton impossible = (RadioButton)FindName("ImpossibleSel");
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
        else  if (impossible.IsChecked == true)
        {
            return 80;
        }
        else
        {
            return 0;
        }
    }
    public void Transfer(SudokuGenerator generator)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                TextBox textBox = (TextBox)FindName("P" + (i + 1) + (j + 1));
                if ((generator.puzzle[i,j]) == 0)
                {
                    textBox.Text = "";
                    textBox.IsReadOnly = false;
                    textBox.Foreground = Brushes.Black;
                    
                }
                else {
                    textBox.Text = generator.puzzle[i, j].ToString();
                    textBox.IsReadOnly = true;
                    textBox.Foreground = Brushes.DarkBlue;
                }
            }
        }
    }
    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        TextBox currentTextBox = (TextBox)sender;

        
        int currentRow = int.Parse(currentTextBox.Name[1].ToString());
        int currentColumn = int.Parse(currentTextBox.Name[2].ToString());

       
        int nextRow = currentRow;
        int nextColumn = currentColumn;

        
        switch (e.Key)
        {
            case Key.Down:
                nextRow = (currentRow % 10) + 1; 
                break;
            case Key.Up:
                nextRow = ((currentRow - 2 + 10) % 10) + 1; 
                break;
            case Key.Left:
                nextColumn = ((currentColumn - 2 + 10) % 10) + 1; 
                break;
            case Key.Right:
                nextColumn = (currentColumn % 10) + 1; 
                break;
        }

        
        string nextTextBoxName = "P" + nextRow + nextColumn;
        
        
        TextBox nextTextBox = (TextBox)this.FindName(nextTextBoxName);
        if (nextTextBox != null)
        {
            nextTextBox.Focus();
        }
    }
    
    private void PrintSudoku(int[,] sudoku)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Console.Write(sudoku[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        int diff = Difficulty();
      
        int[,] sudoku = _generator.GenerateSudoku();
        int[,] puzzle = _generator.MakePuzzle(diff);
        Transfer(_generator);
        
        //timer
        if (timer!=null) {
            timer.Stop();
        }
       
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;

       
        timeElapsed = TimeSpan.Zero;
        
        timer.Start();
    }
    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
       
        TextBox textBox = (TextBox)sender;
        
        if (!IsNumeric(e.Text))
        {
            e.Handled = true; 
        }
        
        if (textBox.Text.Length + e.Text.Length > 1)
        {
            e.Handled = true; 
        }
        
        WinStatus();
    }

    private void WinStatus()
    {
        int[,] playerGrid =  GetPlayerGrid();
        if (AreGridsEqual(playerGrid, _generator.sudoku))
        {
            timer.Stop();
            MessageBox.Show("You won! Your time is " + timeElapsed.ToString(@"hh\:mm\:ss"));
           
            
        }
        /*Console.Out.WriteLine("Solution:");
        printSudoku(generator.sudoku);
        Console.Out.WriteLine("Player:");
        printSudoku(playerGrid);
        Console.Out.WriteLine("Puzzle:");
        printSudoku(generator.puzzle);*/
        
    }
    private bool AreGridsEqual(int[,] grid1, int[,] grid2)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid1[i, j] != grid2[i, j])
                {
                    return false; 
                }
            }
        }
        return true;
    }
    private bool IsNumeric(string text)
    {
        foreach (char c in text)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    private int[,] GetPlayerGrid()
    {
        int[,] playerGrid = new int[9, 9];

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                TextBox textBox = (TextBox)FindName("P" + (i + 1) + (j + 1));
                playerGrid[i , j] = textBox.Text == "" ? 0 : int.Parse(textBox.Text);
            }
        }

        return playerGrid;
    }
}