using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SiDaMingZhu.ViewModels;
using SiDaMingZhu.Models;
using Microsoft.Phone.Shell;

namespace SiDaMingZhu
{
    public partial class BookDetailPage : PhoneApplicationPage
    {
        PhoneApplicationService PhoneService = PhoneApplicationService.Current;
        public BookDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("select") && NavigationContext.QueryString["select"] == "chapterlist")
            {
                MyPivot.SelectedIndex = 1;
            }
            DataContext = new BookDetailViewModel(new Guid(NavigationContext.QueryString["bookid"]));
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel sp = sender as StackPanel;

            Chapter cp = sp.DataContext as Chapter;
            if (cp != null)
            {
                PhoneService.State["chapter"] = cp;
                NavigationService.Navigate(new Uri("/ReadPage.xaml?origin=chapter", UriKind.RelativeOrAbsolute));
            }
        }

        private void BookMark_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	StackPanel sp = sender as StackPanel;

            BookMark bm = sp.DataContext as BookMark;
            BookDetailViewModel bdvm = sp.DataContext as BookDetailViewModel;
            
            if(bdvm != null)
            {
                bm = bdvm.BookDetail.LastRead;
            }

            if (bm != null)
            {
                PhoneService.State["bookmark"] = bm;
                NavigationService.Navigate(new Uri("/ReadPage.xaml?origin=bookmark&bookid="+((BookDetailViewModel)DataContext).BookDetail.Id, UriKind.RelativeOrAbsolute));
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            MenuItem sp = sender as MenuItem;
            BookMark bm = sp.DataContext as BookMark;

            if (bm != null)
            {
                if (App.BookMarkStore.Items.Where(i => i.Id == bm.Id).Count() > 0)
                {
                    App.BookMarkStore.Items.Remove(bm);
                    App.BookMarkStore.Commit();

                    BookDetailViewModel bdvm = this.DataContext as BookDetailViewModel;
                    bdvm.BookMarkList.Remove(bm);
                }
            }
        }
    }
}