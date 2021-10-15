using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFGreedySnake
{
    public static class MessageHelper
    {
        public static void ShowInformation(string info) => Process.Start($"quicker:showmessage:{info}");
        public static void ShowWarning(string warning) => Process.Start($"quicker:showmessage:warning:{warning}");
        public static void ShowSuccess(string success) => Process.Start($"quicker:showmessage:success:{success}");
        public static void ShowError(string success) => Process.Start($"quicker:showmessage:error:{success}");

    }
}
