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
namespace WPFGreedySnake
{
    /// <summary>
    /// FoodControl.xaml 的交互逻辑
    /// </summary>
    public partial class FoodControl : UserControl, INotifyPropertyChanged
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

        public FoodControl()
        {
            InitializeComponent();
        }
        private int score = 1;

        public int Score
        {
            get { return score; }
            set { score = value; OnPropertyChanged(nameof(Score)); }
        }

    }
}
