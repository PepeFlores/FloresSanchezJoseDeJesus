using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;

namespace ApiTwitter.models
{
    public class Linq2TwitterManager
    {
        static Lazy<Linq2TwitterManager> lazy = new Lazy<Linq2TwitterManager>(() => new Linq2TwitterManager());
        public static Linq2TwitterManager SharedInstance { get => lazy.Value; }

        //Variables
        SingleUserAuthorizer authorizer;
        TwitterContext twitterContext;
        CancellationTokenSource cancellationTokenSource;

        //Claves para acceder a Twitter
        Linq2TwitterManager()
        {
            authorizer = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "dMIietdGMkqi7aMaevtx72yeF",
                    ConsumerSecret = "qZshbazX32UuGDHhUK3sTCpwhqyUOyUskS1laUOf0BV5rLTncm",
                    AccessToken = "993947802324500480-zfc7XaPGwQ1Ul1d9sDxSXoyfmbxs5vQ",
                    AccessTokenSecret = "2TccdwcwBptVKjaEMUg6Vxv6yyHfIjrwV1iY9UBeOC9rn"
                }
            };
            twitterContext = new TwitterContext(authorizer);
            cancellationTokenSource = new CancellationTokenSource();
        }

        #region Events
        public event EventHandler<TraerTweetsEventArgs> TraerTweets;
        public event EventHandler<TraerTweetsFalloEventArgs> TraerTweetsFallo;
        #endregion

        #region public functionality
        /*
         * var searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == "\"LINQ to Twitter\""
                 select search)
                .SingleOrDefaultAsync();
         */

        public void SearchTweets(string query)
        {
            
            if (cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Task.Factory.StartNew(async () => {
                try
                {
                    var tweets = await SearchTweetsAsync(query, cancellationToken);

                    var e = new TraerTweetsEventArgs(tweets);

                    TraerTweets?.Invoke(this,e);
                }
                catch(Exception ex)
                {
                    var e = new TraerTweetsFalloEventArgs(ex.Message);
                    TraerTweetsFallo?.Invoke(this, e);
                }
            }, cancellationToken);

        }
       /* public void SearchTweets(string query)
        {
            if (cancellationTokensource.IsCancellationRequested)
                cancellationTokensource.Cancel();

            cancellationTokensource = new CancellationTokenSource();
            var cancellationTokensource = cancellationTokensource.Token;

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var tweets = await SearchTweetAsync(query, cancellationTokensource);

                    var e = new TweetsFetchedEventArgs(tweets);
                    TweetsFetched?.Invoke(this, e);
                }
                catch (Exception ex)
                {
                    var e = new FetchedTweetsFailEventArgs(ex.Message);
                    FetchedTweetsFailed?.Invoke(this, e);
                }
            }, cancellationToken);
        }*/
        #endregion

        //Busca Lostweets
        async Task<List<Status>> SearchTweetsAsync(string query, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace (query))
            {
                return new List<Status>();
            }

            cancellationToken.ThrowIfCancellationRequested();

            Search searchResponse = await
                (from search in twitterContext.Search 
                 where search.Type == SearchType.Search && 
                 search.Query == query &&
                 search.TweetMode == TweetMode.Extended
                 select search)
                .SingleOrDefaultAsync();


            cancellationToken.ThrowIfCancellationRequested();

            return searchResponse?.Statuses;
        }

        /*
          Search searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == searchTerm &&
                       search.IncludeEntities == true &&
                       search.TweetMode == TweetMode.Extended
                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse?.Statuses != null)
                searchResponse.Statuses.ForEach(tweet =>
                    Console.WriteLine(
                        "\n  User: {0} ({1})\n  Tweet: {2}", 
                        tweet.User.ScreenNameResponse,
                        tweet.User.UserIDResponse,
                        tweet.Text ?? tweet.FullText));
         */
        /*async Task<List<Status>> SearchTweetAsync (string query, CancellationToken cancellation)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Status>();

            CancellationToken.ThrowIfCancellationRequested();

            Search searchResponse = await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                 search.Query == query &&
                 search.TweetMode == TweetMode.Extended
                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse?.Statuses != null)
                searchResponse.Statuses.ForEach(tweet =>
                    Console.WriteLine(
                        "\n  User: {0} ({1})\n  Tweet: {2}",
                        tweet.User.ScreenNameResponse,
                        tweet.User.UserIDResponse,
                        tweet.Text ?? tweet.FullText));
        }*/
    }

    public class TraerTweetsEventArgs : EventArgs
    {
        public List<Status> Tweets {get; private set;}

        public TraerTweetsEventArgs (List<Status> tweets)
        {
            Tweets = tweets;
        }

    }

    public class TraerTweetsFalloEventArgs : EventArgs
    {
        public string ErrorMessage { get; private set; }

        public TraerTweetsFalloEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

    }
}
