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

namespace SiDaMingZhu.Models
{
    public class Book
    {
        public Book()
        {
            this.Id = Guid.NewGuid();
            LastRead = new BookMark();
            LastRead.Content = "尚未开始阅读";
        }

        public Guid Id { get; set; }

        public String Title { get; set; }

        public String TitlePinyin { get; set; }

        public String Author { get; set; }

        public String Detail { get; set; }

        public BookMark LastRead { get; set; }

        public int ChapterCount { get; set; }
    }
}
