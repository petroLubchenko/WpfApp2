using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp2.Classes
{
    class Cell
    {
        public static readonly Color Defaultcolor = Color.FromRgb(128, 128, 128);
        public Point Position // Left top point of the rectangle
        { get; set; }

        public Double Width { get; set; }
        public Double Height { get; set; }
        public Color Color { get; private set; }
        public Boolean IsLocked { get; private set; }

        private readonly Color DefaultColor = Defaultcolor;

        public Rectangle RectangleCell { get; }
        public Cell()
        {
            Position = new Point(0, 0);
            Width = 0;
            Height = 0;
            Color = DefaultColor;
            IsLocked = false;
            RectangleCell = new Rectangle();
            RectangleInit();
        }
        public Cell(Point point, Double Width, Double Height)
        {
            Position = point;
            this.Width = Width;
            this.Height = Height;
            Color = DefaultColor;
            RectangleCell = new Rectangle();
            RectangleInit();
        }
        private void RectangleInit()
        {
            if (RectangleCell == null)
                throw new NullReferenceException("Rectangle was not initialized before calling a method");
            RectangleCell.Width = Width;
            RectangleCell.Height = Height;
            RectangleCell.Fill = new SolidColorBrush(Color);
            RectangleCell.Stroke = Brushes.Black;
            RectangleCell.RenderSize = new Size(Width, Height);
        }
        public void SetColor(Color NextColor)
        {
            if (!IsLocked)
                Color = NextColor;
            RectangleInit();
        }
        public void SetPosition(Point LeftTopPoint)
        {
            Position = LeftTopPoint;
        }
        public void ChangeWidth(Int32 NewWidth)
        {
            Width = NewWidth;

        }
        public void ChangeHeight(Int32 NewHeight)
        {
            Height = NewHeight;
        }
        public void ChangeData(Point NewPosition, Double NewWidth, Double NewHeight)
        {
            Position = NewPosition;
            Width = NewWidth;
            Height = NewHeight;
            RectangleInit();
        }
        public void SetDefaultColor()
        {
            Color = DefaultColor;
            RectangleInit();
        }
        public bool IsDefaultColor()
        {
            return (Color == DefaultColor);
        }
    }
}
