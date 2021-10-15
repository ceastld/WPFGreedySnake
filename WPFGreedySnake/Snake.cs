using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFGreedySnake
{
    public static class Ext
    {
        public static double DistanceOf(this Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
    public enum MoveDirection
    {
        Left,
        Up,
        Right,
        Down
    }
    public class SnakePart : GameElement
    {
        private bool isHead;
        public bool IsHead
        {
            get { return isHead; }
            set { isHead = value; OnPropertyChanged(nameof(IsHead)); }
        }

    }
    public class Snake
    {
        public Snake(Canvas canvas)
        {
            GameArea = canvas;
            SnakeParts.Add(new SnakePart() { IsHead = true });
            SnakeLength = SnakeParts.Count;
        }
        private SnakePart Head => SnakeParts[0];
        const double SnakeSquareSize = 20;
        private List<SnakePart> SnakeParts = new List<SnakePart>();
        private Canvas GameArea;
        public void DrawAll() => SnakeParts.ForEach(x => DrawPart(x));
        private void DrawPart(SnakePart snakePart)
        {
            if (snakePart.UiElement == null)
            {
                snakePart.UiElement = new Controls.SnakePartControl(snakePart);
                DrawElement(snakePart);
            }
        }
        private void DrawElement(GameElement element)
        {
            GameArea.Children.Add(element.UiElement);
            Canvas.SetTop(element.UiElement, element.Position.Y);
            Canvas.SetLeft(element.UiElement, element.Position.X);
        }
        public int SnakeLength { get; set; }
        public void Move(MoveDirection direction)
        {
            while (SnakeLength < SnakeParts.Count)
            {
                if (SnakeParts.Count == 0) break;
                var last = SnakeParts.Last();
                SnakeParts.Remove(last);
                GameArea.Children.Remove(last.UiElement);
            }
            while (FoodList.Count < 3)
            {
                GenerateFood();
            }
            var head = Head;
            var hp = head.Position;
            SnakeParts.ForEach(x => x.IsHead = false);
            var newHead = new SnakePart() { IsHead = true };
            switch (direction)
            {
                case MoveDirection.Left:
                    newHead.Position = new Point(hp.X - SnakeSquareSize, hp.Y);
                    break;
                case MoveDirection.Up:
                    newHead.Position = new Point(hp.X, hp.Y - SnakeSquareSize);
                    break;
                case MoveDirection.Right:
                    newHead.Position = new Point(hp.X + SnakeSquareSize, hp.Y);
                    break;
                case MoveDirection.Down:
                    newHead.Position = new Point(hp.X, hp.Y + SnakeSquareSize);
                    break;
                default:
                    break;
            }
            SnakeParts.Insert(0, newHead);
            DrawPart(newHead);
            CheckEat();
            CheckCollection();
        }

        internal void Reset()
        {
            FoodList.Clear();
            SnakeParts.Clear();
            SnakeParts.Add(new SnakePart() { IsHead = true });
            DrawAll();
            SnakeLength = SnakeParts.Count;
            GameArea.Children.Clear();
        }

        public List<Food> FoodList = new List<Food>();
        private void CheckEat()
        {
            foreach (var food in FoodList)
            {
                if (Head.Position.DistanceOf(food.Position) < 40)
                {
                    EatFood(food);
                    return;
                }
            }
        }
        private Random random = new Random();
        public void GenerateFood()
        {
            var x = random.Next(0, 20) * 20;
            var y = random.Next(0, 20) * 20;
            var food = new Food()
            {
                Score = random.Next(1, 3),
                Position = new Point(x, y)
            };
            FoodList.Add(food);
            ShowFood(food);
        }

        private void ShowFood(Food food)
        {
            if (food.UiElement == null)
            {
                food.UiElement = new FoodControl() { Score = food.Score };
                DrawElement(food);
            }
        }
        public void EatFood(Food food)
        {
            FoodList.Remove(food);
            GameArea.Children.Remove(food.UiElement);
            var e = new GrowUpEventArgs() { score = food.Score };
            GrowUp?.Invoke(food, e);
        }
        private void CheckCollection()
        {
            Rect rect = new Rect(-1, -1, GameArea.Width, GameArea.Height);
            if (!rect.Contains(Head.Position))
            {
                //DrawElement(new GameElement()
                //{
                //    UiElement = new Controls.CrashControl(),
                //    Position = Head.Position
                //});
                Collided?.Invoke(Head, new EventArgs());
            }
        }
        public event EventHandler Collided;

        public event GrowUpEventHandler GrowUp;
        public delegate void GrowUpEventHandler(object sender, GrowUpEventArgs e);
        public class GrowUpEventArgs
        {
            public int score { get; set; } = 1;
        }
    }
}
