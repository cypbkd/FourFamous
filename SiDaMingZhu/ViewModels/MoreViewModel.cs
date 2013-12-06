using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using SiDaMingZhu.Models;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace SiDaMingZhu.ViewModels
{
    public class MoreViewModel : ViewModelBase
    {
        private String _title;
        public String Title
        {
            get { return _title; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _backColor = (Application.Current.Resources["WhiteAlphaBrush"] as SolidColorBrush).Color;
                }
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }

        private Color _backColor;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                RaisePropertyChanged("BackColor");
            }
        }

        private ICommand _moreNavigateCommand;
        public ICommand MoreNavigateCommand
        {
            get { return _moreNavigateCommand; }
            set
            {
                _moreNavigateCommand = value;
                RaisePropertyChanged("MoreNavigateCommand");
            }
        }
    }
}
