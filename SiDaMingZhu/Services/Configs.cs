using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace SiDaMingZhu.Models
{
    public class Configs
    {
        private static IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public static void Commit()
        {
            userSettings.Save();
        }

        public static int FontSize
        {
            get
            {
                if (!userSettings.Contains("fontsize"))
                {
                    userSettings["fontsize"] = 23;
                }
                return Convert.ToInt32(userSettings["fontsize"]);
            }
            set { userSettings["fontsize"] = value; }
        }

        public static int LineHeight
        {
            get
            {
                if (!userSettings.Contains("lineheight"))
                {
                    userSettings["lineheight"] = 30;
                }
                return Convert.ToInt32(userSettings["lineheight"]);
            }
            set { userSettings["lineheight"] = value; }
        }

    }
}
