using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PullToRefresh
{
    public class CitiesManager
    {
        #region Singleton
        static readonly Lazy<CitiesManager> lazy = new Lazy<CitiesManager>(() => new CitiesManager());
        public static CitiesManager SharedInstance { get => lazy.Value; }
        #endregion

        #region Class Variables
        HttpClient httpClient;
        Dictionary<string, List<string>> cities;
        #endregion

        #region Events
        public event EventHandler<CitiesEventArgs> CitiesFetched;
        public event EventHandler<EventArgs> FetchCitiesFailed;
        #endregion

        #region Constructors
        CitiesManager()
        {
            httpClient = new HttpClient();

        }
        #endregion

        #region Public Functionality
        public Dictionary<string, List<string>> GetDefaultCities()
        {
            var citiesJson = File.ReadAllText("citites-incomplete.json");
            // Vamos a parsear el Json, usando Json.Net
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(citiesJson);
        }

        public void FetchCities()
        {
            Task.Factory.StartNew(FetchCitiesAsync);
            async Task FetchCitiesAsync()
            {
                try
                {

                    if (CitiesFetched == null)
                        return;

                    var citiesJson = await httpClient.GetStringAsync("https://dl.dropbox.com/s/0adq8yw6vd5r6bj/cities.json?dl=0");
                    cities = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(citiesJson);

                    //Avisarle al controller que ya estan disponible los datos
                    //1. A través de eventos
                    //2. A través de notificaciones
                    //3. (Sólo cuando estás dentro de un ViewController) A través de Unwind Segues

                    var e = new CitiesEventArgs(cities);
                    CitiesFetched(this, e);

                }
                catch (Exception ex)
                {

                    if (FetchCitiesFailed == null)
                    {
                        return;
                    }

                    FetchCitiesFailed(this, new EventArgs());
                    //Avisarle al controller que ya estan disponible los datos
                    //1. A través de eventos
                    //2. A través de notificaciones
                    //3. (Sólo cuando estás dentro de un ViewController) A través de Unwind Segues
                }
            }
        }
        #endregion
    }

    public class CitiesEventArgs : EventArgs
    {
        public Dictionary<string, List<string>> Cities { get; private set; }

        public CitiesEventArgs(Dictionary<string, List<string>> cities)
        {
            Cities = cities;
        }
    }
}
