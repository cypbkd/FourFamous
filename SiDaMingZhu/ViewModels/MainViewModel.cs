using System;
using System.Collections.ObjectModel;
using SiDaMingZhu.ViewModels;
using GalaSoft.MvvmLight;
using SiDaMingZhu.Models;
using Microsoft.Practices.Prism.Commands;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using Microsoft.Phone.Tasks;

namespace SiDaMingZhu
{
    public class MainViewModel :ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ListPanelViewModel class.
        /// </summary>
        public MainViewModel()
        {
            BookViewModels = new ObservableCollection<BookViewModel>();
            MoreViewModels = new ObservableCollection<MoreViewModel>();

            FirstUseInitialApp();

            foreach (var b in App.BookStore.Items)
            {
                BookViewModel bvm = new BookViewModel();
                bvm.Book = b;
                String Id = b.Id.ToString();
                bvm.BookNavigateCommand = new DelegateCommand(() => { ((App)App.Current).RootFrame.Navigate(new Uri("/BookDetailPage.xaml?bookid=" + Id, UriKind.RelativeOrAbsolute)); });

                BookViewModels.Add(bvm);
            }

            

            MoreViewModels.Add(new MoreViewModel() { Title = "设置", Color = Colors.Orange, MoreNavigateCommand = new DelegateCommand(() => { ((App)App.Current).RootFrame.Navigate(new Uri("/SettingPage.xaml", UriKind.RelativeOrAbsolute)); }) });
            MoreViewModels.Add(new MoreViewModel() { Title = "" });
            MoreViewModels.Add(new MoreViewModel() { Title = "关于", Color = Colors.Green, MoreNavigateCommand = new DelegateCommand(() => { ((App)App.Current).RootFrame.Navigate(new Uri("/AboutPage.xaml", UriKind.RelativeOrAbsolute)); }) });
            MoreViewModels.Add(new MoreViewModel() { Title = "重置软件", Color = Colors.Green, MoreNavigateCommand = new DelegateCommand(ResetApp) });
            MoreViewModels.Add(new MoreViewModel() { Title = "" });           
        }

        private void FirstUseInitialApp()
        {
            #region Initial Books
            if (App.BookStore.Items.Count == 0)
            {
                App.BookStore.Items.Add(new Book()
                {
                    Title = "三国演义",
                    TitlePinyin = "SanGuoYanYi",
                    Author = "罗贯中/明",
                    Detail = "《三国演义》，原名《三国志通俗演义》。小说以东汉末年为历史背景，以曹操、诸葛亮、关羽为主角，以蜀汉为中心，讲述黄巾之乱至魏、蜀汉及吴三国鼎立，到西晋统一为终结。",
                    ChapterCount = 120
                });
                App.BookStore.Items.Add(new Book()
                {
                    Title = "西游记",
                    TitlePinyin = "XiYouJi",
                    Author = "吴承恩/明",
                    Detail = "《西游记》，又名《西游释厄传》。此书描写的是孙悟空、猪八戒、沙和尚保护唐僧西天取经，历经九九八十一难的传奇历险故事。",
                    ChapterCount = 100
                });
                App.BookStore.Items.Add(new Book()
                {
                    Title = "水浒传",
                    TitlePinyin = "ShuiHuZhuan",
                    Author = "施耐庵/明",
                    Detail = "《水浒传》，又名《忠义水浒传》，是中国历史上以白话文写成的章回小说。其内容讲述北宋山东梁山泊以宋江为首的绿林好汉，由被迫落草，发展壮大，直至受到朝廷招安，东征西讨的历程。",
                    ChapterCount = 120
                });
                App.BookStore.Items.Add(new Book()
                {
                    Title = "红楼梦",
                    TitlePinyin = "HongLouMeng",
                    Author = "曹雪芹/清",
                    Detail = "《红楼梦》，原名《石头记》，是一部中国末期封建社会的百科全书。小说以上层贵族社会为中心图画，极其真实、生动地描写了十八世纪上半叶中国末期封建社会的全部生活，是这段历史生活的一面镜子和缩影。",
                    ChapterCount = 120
                });
                App.BookStore.Commit();
            }
            #endregion

            #region Initial Chapters
            if (App.ChapterStore.Items.Count == 0)
            {
                foreach (var _book in App.BookStore.Items)
                {
                    XDocument loadedData = XDocument.Load("Datas/" + _book.TitlePinyin + ".xml");

                    var data = from c in loadedData.Descendants("UrlName")
                               select new Chapter()
                               {
                                   Url = String.Format("{0:000}",Convert.ToInt16(c.Attribute("count").Value))+".txt",
                                   BookId = _book.Id,
                                   Count = Convert.ToInt16(c.Attribute("count").Value),
                                   Name = c.Attribute("chapterName").Value
                               };

                    foreach (var c in data)
                    {
                        App.ChapterStore.Items.Add(c);
                    }

                }
            }
            #endregion
        }

        private void ResetApp()
        {
            if (MessageBox.Show("确定重置应用数据？书签将会被清除。", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.BookMarkStore.Items.Clear();
                App.BookMarkStore.Commit();
                foreach (var item in App.BookStore.Items)
                {
                    item.LastRead.Content = "尚未开始阅读";
                    // TODO: 第一章
                    item.LastRead.ChapterId = new Guid();
                    item.LastRead.SymbolCount = 0;
                }
                MessageBox.Show("重置完成.");
            }
        }

        public ObservableCollection<BookViewModel> BookViewModels
        {
            get;
            private set;
        }

        public ObservableCollection<MoreViewModel> MoreViewModels
        {
            get;
            private set;
        }
    }
}