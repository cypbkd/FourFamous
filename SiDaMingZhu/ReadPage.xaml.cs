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
using System.IO;
using System.Text;

namespace SiDaMingZhu
{
    public partial class ReadPage : PhoneApplicationPage
    {
        PhoneApplicationService PhoneService = PhoneApplicationService.Current;
        Dictionary<int, StackPanel> Dic = new Dictionary<int, StackPanel>();

        Dictionary<int, int> PageIndexs = new Dictionary<int, int>();
        int nowPage = 0;
        int nowSymbol = 0;
        bool IsLastPage = false;

        public ReadPage()
        {
            InitializeComponent();
            Dic.Add(0, zero);
            Dic.Add(1, one);
            Dic.Add(2, two);
            InitBook();
        }

        public void InitBook()
        {
            Dic.Clear();
            Dic.Add(0, zero);
            Dic.Add(1, one);
            Dic.Add(2, two);
            Init.Begin();

            PageIndexs = new Dictionary<int, int>();
            nowPage = 0;
            nowSymbol = 0;
            IsLastPage = false;

            PageIndexs.Add(0, 0);
        }

        private bool LoadBook(bool next)
        {
            if (next)
            {
                if (IsLastPage) return false;
                (Dic[0].Children.First() as TextBlock).Text = (Dic[1].Children.First() as TextBlock).Text;
                (Dic[1].Children.First() as TextBlock).Text = (Dic[2].Children.First() as TextBlock).Text;

                if (PageIndexs.ContainsKey(nowPage))
                {
                    nowSymbol = PageIndexs[nowPage];
                }
            }
            else
            {
                (Dic[2].Children.First() as TextBlock).Text = (Dic[1].Children.First() as TextBlock).Text;
                (Dic[1].Children.First() as TextBlock).Text = (Dic[0].Children.First() as TextBlock).Text;

                nowPage--;
                if (PageIndexs.ContainsKey(nowPage - 1))
                {
                    nowSymbol = PageIndexs[nowPage - 1];
                }
            }

            ReadPageViewModel rpv = this.DataContext as ReadPageViewModel;
            if (rpv != null)
            {
                using (Stream strm = App.GetResourceStream(rpv.Resource).Stream)
                {
                    using (StreamReader sr = new StreamReader(strm, true))
                    {
                        char[] data;
                        data = new char[nowSymbol];
                        sr.Read(data, 0, data.Length);
                        // 跳过回车
                        while (sr.Peek() == 0x00 || sr.Peek() == 0x0a)
                        {
                            sr.Read();
                        }

                        TextBlock textSizeMeasure = new TextBlock();
                        textSizeMeasure.FontSize = rpv.FontSize;
                        textSizeMeasure.LineHeight = rpv.LineHeight;
                        textSizeMeasure.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
                        string result = String.Empty;
                        StringBuilder sb = new StringBuilder();
                        int line = 0;
                        int sysbolCount = 0;

                        while (line < (int)(673 / rpv.LineHeight) && sr.Peek() != -1)
                        {
                            int i = sr.Read();
                            sysbolCount++;

                            if (i == 0x00) continue;

                            result += (char)i;
                            textSizeMeasure.Text = result;

                            if (i == 0x0a || textSizeMeasure.ActualWidth + textSizeMeasure.FontSize + 1 > 456)
                            {
                                sb.Append(result);
                                if (!result.EndsWith("\r\n"))
                                {
                                    sb.Append("\r\n");
                                }
                                result = String.Empty;
                                line++;
                            }
                        }

                        if (next)
                        {
                            (Dic[2].Children.First() as TextBlock).Text = sb.ToString();
                            nowSymbol += sysbolCount;
                            nowPage++;
                            if (!PageIndexs.ContainsKey(nowPage))
                            {
                                PageIndexs.Add(nowPage, nowSymbol);
                            }
                        }
                        else
                        {
                            (Dic[0].Children.First() as TextBlock).Text = sb.ToString();
                        }

                        rpv.Book.LastRead.Content = sb.ToString();
                        rpv.Book.LastRead.ChapterId = rpv.Chapter.Id;
                        rpv.Book.LastRead.SymbolCount = PageIndexs[nowPage - 1];

                        BookMark bm = App.BookMarkStore.Items.Where(b => b.ChapterId == rpv.Chapter.Id && b.SymbolCount >= PageIndexs[nowPage - 1]
                            && b.SymbolCount < PageIndexs[nowPage]
                            ).FirstOrDefault();
                        rpv.IsBookMarkShow = bm != null;

                        IsLastPage = sr.Peek() == -1;
                        sr.Close();

                        BookName.Text = rpv.Book.Title + "[" + rpv.Book.Author + "]第" + (nowPage) + "页";
                    }
                    strm.Close();
                }
            }

            return true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            String Origin = NavigationContext.QueryString["origin"];
            if (Origin != null && Origin != "")
            {
                if (Origin == "chapter")
                {
                    Chapter chapter = PhoneService.State["chapter"] as Chapter;
                    if (chapter != null)
                    {
                        DataContext = new ReadPageViewModel(chapter);
                        InitBook();
                        Taped(300);
                    }
                }
                else if (Origin == "bookmark")
                {               
                    Guid BookId = new Guid(NavigationContext.QueryString["bookid"]);
                    BookMark bm = PhoneService.State["bookmark"] as BookMark;
                    if(BookId == null){
                    BookId = App.ChapterStore.Items.Where(c => c.Id == bm.ChapterId).First().BookId;
                    }

                    if (bm != null)
                    {
                        // bm会在Tap时重新赋值，这里暂时存储
                        int SysbolCount = bm.SymbolCount;
                        Chapter cp = App.ChapterStore.Items.Where(p => p.Id == bm.ChapterId).FirstOrDefault();
                        if (cp == null)
                        {
                            cp = App.ChapterStore.Items.Where(c => c.BookId == BookId).OrderBy(c => c.Count).FirstOrDefault();
                        }
                        DataContext = new ReadPageViewModel(cp);
                        InitBook();
                        do
                        {
                            Taped(300);
                        } while (SysbolCount >= PageIndexs[nowPage - 1]);
                        Taped(20);
                    }
                }
            }
        }

        #region 响应用户操作
        bool isLoading = false;
        private void Taped(double X)
        {
            if (isLoading) return;
            isLoading = true;
            ApplicationBar.IsVisible = false;
            if (X <= 200)
            {
                if (nowPage == 1)
                {
                    ApplicationBar.IsVisible = true;
                }
                else if (nowPage > 1 && LoadBook(false))
                {
                    //上一页。
                    Prev.Stop();
                    Storyboard.SetTargetName(Prev.Children[0], Dic[0].Name);
                    Storyboard.SetTargetName(Prev.Children[1], Dic[1].Name);
                    Storyboard.SetTargetName(Prev.Children[2], Dic[2].Name);
                    StackPanel temp = Dic[2];
                    Dic[2] = Dic[1];
                    Dic[1] = Dic[0];
                    Dic[0] = temp;
                    Prev.Begin();
                }
            }
            else if (X > 280)
            {
                if (LoadBook(true))
                {
                    Next.Stop();
                    Storyboard.SetTargetName(Next.Children[0], Dic[0].Name);
                    Storyboard.SetTargetName(Next.Children[1], Dic[1].Name);
                    Storyboard.SetTargetName(Next.Children[2], Dic[2].Name);
                    StackPanel temp = Dic[0];
                    Dic[0] = Dic[1];
                    Dic[1] = Dic[2];
                    Dic[2] = temp;
                    Next.Begin();
                }
                if (IsLastPage)
                {
                    ApplicationBar.IsVisible = true;
                }
            }
            isLoading = false;
        }

        private void ContentPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (isLoading) e.Handled = true;

            Point point = new Point();
            point = e.GetPosition(LayoutRoot);
            Taped(point.X);
        }


        Point StartPt = new Point();
        Point EndPt = new Point();
        private void ContentPanel_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            EndPt.X = e.ManipulationOrigin.X;
            EndPt.Y = e.ManipulationOrigin.Y;

            if (EndPt.X - StartPt.X > 50)
            {
                Taped(10);
            }
            else if (EndPt.X - StartPt.X < -50)
            {
                Taped(300);
            }

            StartPt = new Point();
            EndPt = new Point();
        }

        private void ContentPanel_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            StartPt.X = e.ManipulationOrigin.X;
            StartPt.Y = e.ManipulationOrigin.Y;
        }

        private void ContentPanel_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ApplicationBar.IsVisible = true;
        }

        private void PreChapter_Click(object sender, System.EventArgs e)
        {
            ReadPageViewModel rpv = DataContext as ReadPageViewModel;
            Chapter Pre = App.ChapterStore.Items.Where(c => c.BookId == rpv.Chapter.BookId && c.Count == rpv.Chapter.Count - 1).FirstOrDefault();
            if (Pre == null)
            {
                if (MessageBox.Show("已是第一回，是否返回目录？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ChapterList_Click(null, null);
                }
                return;
            }
            DataContext = new ReadPageViewModel(Pre);
            InitBook();
            Taped(300);
            Taped(30);
        }

        private void NextChapter_Click(object sender, System.EventArgs e)
        {
            ReadPageViewModel rpv = DataContext as ReadPageViewModel;
            Chapter Next = App.ChapterStore.Items.Where(c => c.BookId == rpv.Chapter.BookId && c.Count == rpv.Chapter.Count + 1).FirstOrDefault();
            if (Next == null)
            {
                if (MessageBox.Show("已是最后一回，是否返回目录？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ChapterList_Click(null, null);
                }
                return;
            }
            DataContext = new ReadPageViewModel(Next);
            InitBook();
            Taped(300);
            Taped(30);
        }

        private void ChapterList_Click(object sender, System.EventArgs e)
        {
            ReadPageViewModel rpv = DataContext as ReadPageViewModel;

            NavigationService.Navigate(new Uri("/BookDetailPage.xaml?bookid=" + rpv.Book.Id + "&select=chapterlist", UriKind.RelativeOrAbsolute));
        }
        #endregion

        /// <summary>
        /// 添加书签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridHead_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point point = new Point();
            point = e.GetPosition(LayoutRoot);

            if (point.X > 350 && point.Y < 60)
            {
                AddBookMark_Click(null, null);
            }
        }

        private void AddBookMark_Click(object sender, System.EventArgs e)
        {
            ReadPageViewModel rpv = DataContext as ReadPageViewModel;
            if (!rpv.IsBookMarkShow)
            {
                BookMarkShow.Begin();
                rpv.IsBookMarkShow = true;

                BookMark bm = new BookMark();
                bm.ChapterId = rpv.Chapter.Id;
                bm.SymbolCount = PageIndexs[nowPage - 1];
                bm.Content = rpv.Book.LastRead.Content.Substring(0, 50) + "...";
                bm.Content = bm.Content.Replace("\r\n", "");
                bm.AddTime = DateTime.Now;

                App.BookMarkStore.Items.Add(bm);
            }
            else
            {
                BookMarkHide.Begin();
                BookMark bm = App.BookMarkStore.Items.Where(b => b.ChapterId == rpv.Chapter.Id && b.SymbolCount == PageIndexs[nowPage - 1]).FirstOrDefault();
                if (bm != null)
                {
                    App.BookMarkStore.Items.Remove(bm);
                }
                rpv.IsBookMarkShow = false;
            }
        }

        private void Setting_Click(object sender, System.EventArgs e)
        {
            NavigationContext.QueryString["origin"] = "bookmark";

            ReadPageViewModel rpv = DataContext as ReadPageViewModel;
            BookMark bm = new BookMark();
            bm.ChapterId = rpv.Chapter.Id;
            bm.SymbolCount = PageIndexs[nowPage - 1];
            bm.Content = rpv.Book.LastRead.Content.Substring(0, 50) + "...";
            bm.Content = bm.Content.Replace("\r\n", "");
            bm.AddTime = DateTime.Now;

            PhoneService.State["bookmark"] = bm;

            NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
