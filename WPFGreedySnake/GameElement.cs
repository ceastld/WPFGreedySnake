using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace WPFGreedySnake
{
    public class GameElement : INotifyPropertyChanged
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

        public UIElement UiElement { get; set; }
        public Point Position { get; set; }
    }
}