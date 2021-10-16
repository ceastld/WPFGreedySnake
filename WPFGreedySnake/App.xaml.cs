using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFGreedySnake
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static void Run(string cmd)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new MainWindow();
                window.Show();
                window.Activate();
            });
        }
    }
}
