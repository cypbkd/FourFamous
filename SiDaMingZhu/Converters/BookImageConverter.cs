using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Linq;
using SiDaMingZhu.Models;
using System.Windows.Media.Imaging;

namespace SiDaMingZhu.Converters
{
    public class BookImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Guid id = new Guid(value.ToString());

            Book book = App.BookStore.Items.Where(i => i.Id == id).FirstOrDefault();
            if (book != null)
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.UriSource = new Uri("Datas/" + book.TitlePinyin + ".jpg", UriKind.Relative);
                return bi3;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
