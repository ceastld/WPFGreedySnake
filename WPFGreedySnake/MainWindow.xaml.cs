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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Diagnostics;

namespace WPFGreedySnake
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region For INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        protected void OnPropertyChanged(params string[] vs)
        {
            foreach (var s in vs)
            {
                OnPropertyChanged(s);
            }
        }
        #endregion

        private DispatcherTimer Timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            TheWindow.PreviewKeyDown += TheWindow_PreviewKeyDown;
            TheWindow.ContentRendered += TheWindow_ContentRendered;
            TheMenu.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(this.MenuItemClicked));
        }

        private void MenuItemClicked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem item)
            {
                switch ((string)item.Tag)
                {
                    case "Restart":
                        this.Restart();
                        break;
                    case "Introduction":
                        MessageHelper.ShowInformation(@"↑↓→← 或者 W,S,A,D 移动");
                        break;
                    case "SpeedPlus":
                        if (Speed > 20) return;
                        Speed++;
                        Timer.Interval = TimeSpan.FromSeconds(1.0 / Speed);
                        break;
                    case "SpeedSub":
                        if (Speed < 2) return;
                        Speed--;
                        Timer.Interval = TimeSpan.FromSeconds(1.0 / Speed);
                        break;
                    case "Stop": Timer.Stop(); break;
                    case "Start": Timer.Start(); break;
                    default: break;
                }
            }
        }

        private void Restart()
        {
            Snake.Reset();
            Score = 0;
            currentDirection = MoveDirection.Right;
            Timer.Start();
        }

        private Snake Snake;
        private MoveDirection currentDirection = MoveDirection.Right;
        private void TheWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //非当前方向当前运动的反方向
            switch (e.Key)
            {
                case Key.Left:
                case Key.A:
                    if (currentDirection != MoveDirection.Right) currentDirection = MoveDirection.Left; break;
                case Key.Up:
                case Key.W:
                    if (currentDirection != MoveDirection.Down) currentDirection = MoveDirection.Up; break;
                case Key.Right:
                case Key.D:
                    if (currentDirection != MoveDirection.Left) currentDirection = MoveDirection.Right; break;
                case Key.Down:
                case Key.S:
                    if (currentDirection != MoveDirection.Up) currentDirection = MoveDirection.Down; break;
                default: return;
            }
        }
        private void TheWindow_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
            Snake = new Snake(TheGameArea);
            Snake.Collided += (s1, e1) =>
            {
                MessageHelper.ShowInformation("游戏结束");
                Timer.Stop();
            };
            Snake.GrowUp += (s, ea) =>
            {
                Snake.SnakeLength += ea.score;
                Score += ea.score;
            };
            Timer.Interval = TimeSpan.FromSeconds(1.0 / Speed);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void DrawGameArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    //Fill = nextIsOdd ? Brushes.White : Brushes.Black
                    Fill = Brushes.White
                };
                TheGameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += SnakeSquareSize;
                if (nextX >= TheGameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SnakeSquareSize;
                    rowCounter++;
                    nextIsOdd = rowCounter % 2 != 0;
                }
                if (nextY >= TheGameArea.ActualHeight)
                {
                    doneDrawingBackground = true;
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Snake.Move(currentDirection);
        }

        private int _score = 0;
        private const int SnakeSquareSize = 20;

        public int Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged(nameof(Score)); }
        }
        private int speed = 10;
        public int Speed
        {
            get { return speed; }
            set { speed = value; OnPropertyChanged(nameof(Speed)); }
        }
    }
}
