using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using SiDaMingZhu.Models;
using System.Windows.Input;

namespace SiDaMingZhu.ViewModels
{
    public class BookViewModel : ViewModelBase
    {
        private Book _book;
        public Book Book
        {
            get { return _book; }
            set
            {
                _book = value;
                RaisePropertyChanged("Book");
            }
        }

        private ICommand _bookNavigateCommand;
        public ICommand BookNavigateCommand
        {
            get { return _bookNavigateCommand; }
            set
            {
                _bookNavigateCommand = value;
                RaisePropertyChanged("BookNavigateCommand");
            }
        }
    }
}
