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
    public class BookMark
    {
        public BookMark()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid ChapterId { get; set; }

        public DateTime AddTime { get; set; }

        public String Content { get; set; }

        public int SymbolCount { get; set; }
    }
}
