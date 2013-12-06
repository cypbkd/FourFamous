using System;
using System.Windows.Data;

namespace SiDaMingZhu.Converters
{
    public class SubStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String s = value.ToString();
            s = s.Replace("\r\n", " ");
            s = s.Replace("\t", "\r\n");
            if (s.Length > 100)
            {
                s = s.Substring(0,100) + "...";
            }
            if (!s.StartsWith("\t"))
            {
                s = "◆ " + s;
            }
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
