using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApp2.Classes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для SingleGameWindow.xaml
    /// </summary>
    public partial class SingleGameWindow : Window
    {
        public SingleGameWindow(Window previouswindow, int size)
        {
            InitializeComponent();
            Size = size;
            Previous_Window = previouswindow;
            Cells = new Cell[Size, Size];
            Cells = CalculateCells(Size);
            lbUserScore.Content = 0;
        }

        private int Size;
        private Window Previous_Window;
        private Color[] ColorQueue = {Color.FromRgb(255, 0, 0),   Color.FromRgb(172, 172, 0), Color.FromRgb(0, 255, 0),
                                      Color.FromRgb(0, 172, 172), Color.FromRgb(0, 0, 255),   Color.FromRgb(172, 0, 172)};
        private StackPanel stackpanel = new StackPanel();
        private Cell[,] Cells;
        private const int WidthWaste = 170;
        private const int HeightWaste = 100;

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) {
                double width = ActualWidth - WidthWaste;
                double height = ActualHeight - HeightWaste;

                double widthstep = width / Size;
                double heightstep = height / Size;

                Point pt = e.GetPosition(this);
                pt.X -= 20;
                pt.Y -= 20;

                int col = (int)(pt.X / widthstep);
                int row = (int)(pt.Y / heightstep);

                for (int i = 0; i < Size; i++)
                {
                    ChangeColor(Cells[row, i]);
                    if (i != row)
                        ChangeColor(Cells[i, col]);
                }

                lbUserScore.Content = (int)lbUserScore.Content + CalculateScore();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateCells(Cells);
            DrawCells(Cells);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            DrawCells(Cells);
        }

        private Cell[,] CalculateCells(int Size)
        {
            double width  = GameWindow.Width - WidthWaste;
            double height = GameWindow.Height - HeightWaste;

            double widthstep  = width  / Size;
            double heightstep = height / Size;

            Cell[,] cells = new Cell[Size, Size];

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    Point lefttop = new Point(0 + widthstep * j, 0 + heightstep * i);
                    cells[i, j] = new Cell(lefttop, widthstep, heightstep);
                }

            return cells;
        }
        private void CalculateCells(Cell[,] cells)
        {
            double widthstep = (ActualWidth - WidthWaste) / Size;
            double heightstep = (ActualHeight - HeightWaste) / Size;

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    Point lefttop = new Point(0 + widthstep * j, 0 + heightstep * i);
                    Color thiscolor = cells[i, j].Color;
                    cells[i, j].ChangeData(lefttop, widthstep, heightstep);
                    Canvas.SetTop(cells[i,j].RectangleCell, i * cells[i, j].RectangleCell.Height);
                    Canvas.SetLeft(cells[i, j].RectangleCell, j * cells[i, j].RectangleCell.Width);

                }
        }

        private void DrawCell(Cell cell, int i, int j)
        {
            //cell.RectangleCell.RenderSize = new Size(cell.Width, cell.Height);
            GameCanvas.Children.Add(cell.RectangleCell);
            Canvas.SetTop(cell.RectangleCell, i * cell.RectangleCell.Height);
            Canvas.SetLeft(cell.RectangleCell, j * cell.RectangleCell.Width);
        }
        private void DrawCells(Cell[,] cells)
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    DrawCell(cells[i, j], i, j);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Previous_Window.Show();
        }
        private void ChangeColor(Cell cell)
        {
            int i = Array.IndexOf(ColorQueue, cell.Color);
            cell.SetColor(ColorQueue[(i + 1) % ColorQueue.Length]);
        }
        private void ChangeColor(Cell cell, Color color)
        {
            cell.SetColor(color);
        }

        private int CalculateScore()
        {
            int score = 0;
            for (int i = 0; i < Size - 1; i++)
                for (int j = 0; j < Size - 1; j++)
                {
                    //if (!Cells[i,j].IsDefaultColor() && Cells[i,j].Color == Cells[i, j + 1].Color)
                    //{
                    if (!Cells[i,j].IsDefaultColor())
                        score += CheckSquare(i, j);
                    //}
                }
            return score;
        }
        private int CheckSquare(int i_pos, int j_pos)
        {
            int rank = 1;
            
            for (int i = 1; i < Size - i_pos && i < Size - j_pos; i++)
            {
                if (Cells[i_pos + i, j_pos + i].Color.Equals(Cells[i_pos, j_pos].Color) && /*Cells[i_pos + i, j_pos + i].Color != Cell.Defaultcolor &*/
                    Cells[i_pos + i, j_pos    ].Color.Equals(Cells[i_pos, j_pos].Color) && /*Cells[i_pos + i, j_pos    ].Color != Cell.Defaultcolor &*/
                    Cells[i_pos    , j_pos + i].Color.Equals(Cells[i_pos, j_pos].Color) /* & Cells[i_pos    , j_pos + i].Color != Cell.Defaultcolor*/)
                {
                    rank++;
                    Cells[i_pos + i, j_pos + i].SetDefaultColor();
                    Cells[i_pos + i, j_pos].SetDefaultColor();
                    Cells[i_pos, j_pos + i].SetDefaultColor();
                }
            }

            if (rank > 1)
                Cells[i_pos, j_pos].SetDefaultColor();

            return rank <= 1 ? 0 : (int)Math.Pow(10, rank - 1);
        }

    }
}
