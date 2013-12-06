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
using Microsoft.Phone.Tasks;

namespace SiDaMingZhu
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Rate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MarketplaceReviewTask _marketplaceReviewTask = new MarketplaceReviewTask();
            _marketplaceReviewTask.Show();
        }

        private void Update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
            _marketPlaceDetailTask.ContentIdentifier = "c343389c-abd6-4873-bbf4-b760e1b25d0b";
            _marketPlaceDetailTask.Show();
        }

        private void Advice_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MarketplaceSearchTask _marketplaceSearchTask = new MarketplaceSearchTask();
            _marketplaceSearchTask.ContentType = MarketplaceContentType.Applications;
            _marketplaceSearchTask.SearchTerms = "cyp";
            _marketplaceSearchTask.Show();
        }

        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.To = "cyp_bkd@qq.com";
            ect.Show();
        }

        private void Weibo_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.Uri = new Uri("http://weibo.com/cypbkd");
            wbt.Show();
        }
    }
}
