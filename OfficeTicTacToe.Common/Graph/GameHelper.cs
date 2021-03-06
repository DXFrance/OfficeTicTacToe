﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeTicTacToe.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OfficeTicTacToe.Common.Graph
{
    public class GameHelper
    {

        private const string ServerUri = "http://localhost:17225";

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

        public async Task<Game> CreateGame(Game move)
        {
            try
            {
                if (!move.CreatedDate.HasValue)
                    move.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(ServerUri + "/api/Games");

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(move));

                return await this.PostAsync<Game>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task UpdateGame(Game move)
        {
            try
            {
                if (!move.CreatedDate.HasValue)
                    move.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(ServerUri + "/api/Games/" + move.Id);

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(move));

                await this.PutAsync<Game>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        public async Task<Game> GetGame(int id)
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Games/" + id);
                return await this.GetAsync<Game>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<Game>> GetGames()
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Games");
                return await this.GetAsync<List<Game>>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<List<Game>> GetGamesByUserId(string userId)
        {
            try
            {
                 var uri = new Uri(ServerUri + "/api/Games/Users/" + System.Net.WebUtility.UrlEncode(userId) + "/");
                return await this.GetAsync<List<Game>>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }



        public async Task DeleteGame(int id)
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Games/" + id);
                await this.DeleteAsync<Game>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async Task<Move> CreateMove(Move move)
        {
            try
            {
                if (!move.CreatedDate.HasValue)
                    move.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(ServerUri + "/api/Moves");

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(move));

                return await this.PostAsync<Move>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task UpdateMove(Move move)
        {
            try
            {
                if (!move.CreatedDate.HasValue)
                    move.CreatedDate = DateTime.UtcNow;

                var uri = new Uri(ServerUri + "/api/Moves/" + move.Id);

                StringContent scontent = new StringContent(JsonConvert.SerializeObject(move));

                await this.PutAsync<Move>(uri, scontent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        public async Task<Move> GetMove(int id)
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Moves/" + id);
                return await this.GetAsync<Move>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<Move>> GetMoves()
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Moves");
                return await this.GetAsync<List<Move>>(uri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task DeleteMove(int id)
        {
            try
            {
                var uri = new Uri(ServerUri + "/api/Moves/" + id);
                await this.DeleteAsync<Move>(uri);
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
