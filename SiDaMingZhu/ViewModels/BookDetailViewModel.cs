using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiDaMingZhu.Models;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Collections;

namespace SiDaMingZhu.ViewModels
{
    public class BookDetailViewModel : ViewModelBase
    {
        private Book _book;
        public Book BookDetail
        {
            get { return _book; }
            set
            {
                _book = value;
                RaisePropertyChanged("BookDetail");
            }
        }

        public String LastReadContent
        {
            get { return BookDetail.LastRead.Content; }
        }

        public BookDetailViewModel(Guid guid)
        {
            BookDetail = App.BookStore.Items.Where(b => b.Id == guid).FirstOrDefault();
            ChapterList = new ObservableCollection<Chapter>();
            BookMarkList = new ObservableCollection<BookMark>();

            List<Chapter> listCp = App.ChapterStore.Items.Where(i=>i.BookId == guid).OrderBy(i=>i.Count).ToList();

            for (int i = 0; i < listCp.Count(); i++)
            {
                ChapterList.Add(listCp[i]);
            }

            foreach (var m in App.BookMarkStore.Items.Where(i => App.ChapterStore.Items.Where(c=>c.Id == i.ChapterId).FirstOrDefault().BookId == guid).OrderByDescending(i => i.AddTime))
            {
                BookMarkList.Add(m);
            }
        }

        public ObservableCollection<Chapter> ChapterList
        {
            get;
            private set;
        }

        public ObservableCollection<BookMark> BookMarkList
        {
            get;
            private set;
        }

        public bool IsBookMarkShow
        {
            get { return BookMarkList.Count == 0; }
        }
    }
}
