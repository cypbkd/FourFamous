using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using SiDaMingZhu.Models;
using System.Windows.Input;
using System.IO;
using System.Windows.Controls;
using System.IO.IsolatedStorage;

namespace SiDaMingZhu.ViewModels
{
    public class ReadPageViewModel : ViewModelBase
    {
        private Chapter cp;
        private Book book;

        public Book Book { get { return book; } }
        public Chapter Chapter { get { return cp; } }

        public ReadPageViewModel(Chapter cp)
        {
            this.cp = cp;
            book = App.BookStore.Items.Where(b => b.Id == cp.BookId).First();
        }

        public Uri Resource
        {
            get { return new Uri("Datas/" + book.TitlePinyin + "/" + cp.Url, UriKind.Relative);}
        }

        public String ChapterTitle
        {
            get { return "第" + cp.Count + "回 " + cp.Name; }
        }

        public int LineHeight
        {
            get
            {
                return Configs.LineHeight;
            }
        }

        public int FontSize
        {
            get
            {
                return Configs.FontSize;
            }
        }

        private bool _isBookMarkShow = false;
        public bool IsBookMarkShow
        {
            get { return _isBookMarkShow; }
            set
            {
                _isBookMarkShow = value;
                RaisePropertyChanged("IsBookMarkShow");
            }
        }
    }
}
