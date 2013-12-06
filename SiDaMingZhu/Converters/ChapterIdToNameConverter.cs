using System;
using System.Windows.Data;
using System.Linq;

namespace SiDaMingZhu.Converters
{
    public class ChapterIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Guid Id = new Guid(value.ToString());
            return "第" + App.ChapterStore.Items.Where(i => i.Id == Id).FirstOrDefault().Count.ToString() + "回";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
