using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using SiDaMingZhu.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace SiDaMingZhu.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {

        public SettingViewModel()
        {
            FontSizeSets = new ObservableCollection<int>() { 18, 23, 28, 32, 35 };
            LineHeightSets = new ObservableCollection<int>() { 30, 40, 50 };
        }

        public int FontSize
        {
            get { return Configs.FontSize; }
            set
            {
                Configs.FontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }

        public int LineHeight
        {
            get { return Configs.LineHeight; }
            set
            {
                Configs.LineHeight = value;
                RaisePropertyChanged("LineHeight");
            }
        }

        public ObservableCollection<int> FontSizeSets
        {
            get;
            private set;
        }

        public ObservableCollection<int> LineHeightSets
        {
            get;
            private set;
        }

        public int FontSizeIndex
        {
            get { 
                int index = 0;
                for(int i = 0; i < FontSizeSets.Count;i++)
                {
                    if (FontSizeSets[i] == Configs.FontSize)
                    {
                        index = i;
                    }
                }
                return index;
            }
            set
            {
                FontSize = FontSizeSets[value];
                RaisePropertyChanged("FontSizeIndex");
            }
        }

        public int LineHeightIndex
        {
            get
            {
                int index = 0;
                for (int i = 0; i < LineHeightSets.Count; i++)
                {
                    if (LineHeightSets[i] == Configs.LineHeight)
                    {
                        index = i;
                    }
                }
                return index;
            }
            set
            {
                LineHeight = LineHeightSets[value];
                RaisePropertyChanged("LineHeightIndex");
            }
        }
    }
}
