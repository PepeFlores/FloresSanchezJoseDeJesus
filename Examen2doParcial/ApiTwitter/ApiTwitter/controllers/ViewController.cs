using System;
using System.Collections.Generic;
using ApiTwitter.models;
using Foundation;
using UIKit;
using LinqToTwitter;
//using ApiTwitter.view;

namespace ApiTwitter
{
    public partial class ViewController : UIViewController, IUITableViewDataSource, IUITableViewDelegate, IUISearchResultsUpdating
    {
        //Variables
        UISearchController searchController;
        List<Status> Lostweets;


        #region Contrusctor
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
		#endregion


		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            InitialComponent();
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
            return UIStatusBarStyle.LightContent;
		}

        //Eventos que tren los tweets
        void Linq2TwitterManager_GetTweets(object sender, TraerTweetsEventArgs e)
        {
            Lostweets = e.Tweets;
            InvokeOnMainThread(() => TwitterTableView.ReloadData());
        }

        void Linq2TwitterManager_GetTweetsFallo(object sender, TraerTweetsFalloEventArgs e)
        {
            Console.WriteLine(e.ErrorMessage);
        }


        void InitialComponent()
        {
            Lostweets = new List<Status>();

            Linq2TwitterManager.SharedInstance.TraerTweets += Linq2TwitterManager_GetTweets;
            Linq2TwitterManager.SharedInstance.TraerTweetsFallo += Linq2TwitterManager_GetTweetsFallo;

            searchController = new UISearchController(searchResultsController: null)
            {
                SearchResultsUpdater = this,
                DimsBackgroundDuringPresentation = false
            };

            TwitterTableView.DataSource = this;
            TwitterTableView.Delegate = this;
            TwitterTableView.TableHeaderView = searchController.SearchBar;
          
        }

        //Barra de busqueda
        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            Linq2TwitterManager.SharedInstance.SearchTweets(searchController.SearchBar.Text);
        }

        //Boton de Iconos (URL)
        partial void BtnIcons8_TouchUpInside(NSObject sender)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("https://icons8.com"));
        }

        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }


        public nint RowsInSection(UITableView tableView, nint section)
        {
            return Lostweets.Count;
        }

        /*public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if(tweets.Count -- 1 >= indexPath.Row)
            {
                lazyLoad = true;
                Linq2TwitterManager.SharedInstance.SearchTweets(searchController);
            }

            var tweet = tweets [indexPath.Row];

            var cell = tableView.DequeueReusableCell(TweetsViewTableView.Key, indexPath) as TweetsViewTableView;

            cell.Tweet = tweet.FullText;
            cell.FavoritedCount = tweet.FavoritedCount.ToString();

            return cell;
        }*/

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var tweet = Lostweets[indexPath.Row];
            var celda = tableView.DequeueReusableCell(TweetsViewTableView.Key , indexPath) as TweetsViewTableView ;
            celda.Tweet = tweet.FullText;
            celda.RetweetedCount = tweet.RetweetCount.ToString();
            //cell.UsuarioName = tweet.UserID.ToString();

            return celda;
            throw new NotImplementedException();
        }
    }
}
