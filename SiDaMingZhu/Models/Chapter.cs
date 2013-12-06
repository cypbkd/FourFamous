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
    public class Chapter
    {
        public Chapter()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// 所属书本Id
        /// </summary>
        public Guid BookId { get; set; }
        public Guid Id { get; set; }

        public String Name { get; set; }
        public String Url { get; set; }
        public int Count { get; set; }
    }
}
