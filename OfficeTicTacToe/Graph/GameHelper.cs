using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OfficeTicTacToe.ViewModels;

namespace OfficeTicTacToe.Graph
{
    public class GameHelper
    {
        
        private const string _ServerUri = "http://localhost:17225";
        //private const string _ServerUri = "http://prod-officetictactoe.azurewebsites.net";

        private string _QServiceUrlSuffixe = "/api/qtrainer/nextvalue/";
        private const string _markovName = "markov@tictactoe.com";
        private string _stateToken;

        private static GameHelper current;

        public static GameHelper Current
        {
            get
            {
                if (current == null)
                    current = new GameHelper();

                return current;
            }
        }

        public async Task<GameViewModel> CreateGameAsync(GameViewModel game)
        {
            try
            {
                if (!game.CreatedDate.HasValue)
                    game.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(_ServerUri + "/api/Games");

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(game));

                return await this.PostAsync<GameViewModel>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<GameViewModel> UpdateGameAsync(GameViewModel move)
        {
            try
            {
                if (!move.CreatedDate.HasValue)
                    move.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(_ServerUri + "/api/Games/" + move.Id + "/update");

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(move));

                if (move.UserIdOpponent == _markovName)
                {
                    int cellId = 1; // temporary value -> to update afterwards
                    _stateToken = _stateToken + cellId;
                    using (var client = new HttpClient())
                    {
                        var r = new Uri(new Uri(_ServerUri + _QServiceUrlSuffixe), _stateToken);
                        _stateToken = await client.GetStringAsync(r);
                        // following two temporary lines -> to update afterwards
                        int lastMove = int.Parse(_stateToken.Last().ToString());
                        return await this.PostAsync<GameViewModel>(uri, scontent);
                    }
                }
                else
                    return await this.PostAsync<GameViewModel>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


    //public async Task<GameViewModel> GetJarvisMoveAsync(string userId, GameViewModel game)
    //{
    //    try
    //    {
    //        var uri = new Uri(ServerUri + "/api/Games/Move/Jarvis/" + userId + "/");

    //        StringContent scontent = new StringContent(JsonConvert.SerializeObject(game));

    //        var newGame = await this.PostAsync<GameViewModel>(uri, scontent);

    //        return newGame;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message);
    //        return null;
    //    }
    //}


    public async Task<GameViewModel> GetGameAsync(int id)
        {
            try
            {
                var uri = new Uri(_ServerUri + "/api/Games/" + id);
                return await this.GetAsync<GameViewModel>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<GameViewModel>> GetGamesAsync()
        {
            try
            {
                var uri = new Uri(_ServerUri + "/api/Games");
                return await this.GetAsync<List<GameViewModel>>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<List<GameViewModel>> GetGamesByUserIdAsync(string userId)
        {
            try
            {
                 var uri = new Uri(_ServerUri + "/api/Games/Users/" + System.Net.WebUtility.UrlEncode(userId) + "/");
                return await this.GetAsync<List<GameViewModel>>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }



        public async Task DeleteGameAsync(int id)
        {
            try
            {
                var uri = new Uri(_ServerUri + "/api/Games/" + id);
                await this.DeleteAsync<GameViewModel>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
     

        private async Task<T> DeleteAsync<T>(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {

                using (HttpResponseMessage response = await client.DeleteAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                }
            }
            return default(T);
        }
        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (HttpClient client = new HttpClient())
            {

                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                }
            }
            return default(T);
        }
        private async Task<T> PostAsync<T>(Uri uri, StringContent content)
        {
            using (HttpClient client = new HttpClient())
            {

                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                using (HttpResponseMessage response = await client.PostAsync(uri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                }
            }
            return default(T);
        }

        private async Task<T> PutAsync<T>(Uri uri, StringContent content)
        {
            using (HttpClient client = new HttpClient())
            {

                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");

                using (HttpResponseMessage response = await client.PutAsync(uri, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                }
            }
            return default(T);
        }
    }
}
