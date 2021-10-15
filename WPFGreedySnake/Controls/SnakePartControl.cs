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
namespace WPFGreedySnake.Controls
{
    public class SnakePartControl : Control, INotifyPropertyChanged
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

        public SnakePartControl(SnakePart snakePart)
        {
            SnakePart = snakePart;
            IsHead = snakePart.IsHead;
            snakePart.PropertyChanged += SnakePart_PropertyChanged;
        }

        private void SnakePart_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsHead")
            {
                IsHead = SnakePart.IsHead;
            }
        }

        public static readonly DependencyProperty IsHeadProperty = DependencyProperty.Register("IsHead", typeof(bool), typeof(SnakePartControl), new PropertyMetadata(false));
        public bool IsHead
        {
            get { return (bool)GetValue(IsHeadProperty); }
            set { SetValue(IsHeadProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SnakePartControl), new PropertyMetadata(new CornerRadius(0)));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }





        public static readonly DependencyProperty SnakePartProperty = DependencyProperty.Register("SnakePart", typeof(SnakePart), typeof(SnakePartControl), new PropertyMetadata(null));
        public SnakePart SnakePart
        {
            get { return (SnakePart)GetValue(SnakePartProperty); }
            set { SetValue(SnakePartProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
    }
}
