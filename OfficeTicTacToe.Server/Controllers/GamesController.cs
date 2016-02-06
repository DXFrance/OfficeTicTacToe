using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OfficeTicTacToe.Server.Models;
using Microsoft.Azure.NotificationHubs;
using System.Threading.Tasks;

namespace OfficeTicTacToe.Server.Controllers
{
    public class GamesController : ApiController
    {
        private const string jarvisName = "jarvis@tictactoe.com";
        private OfficeTicTacToeEntities db = new OfficeTicTacToeEntities();

        // GET: api/Games
        public IQueryable<Game> GetGames()
        {
            return db.Games;
        }


        [HttpGet]
        [Route("api/Games/Users/{userId}")]
        public IQueryable<Game> GetGamesByUsers(string userId)
        {
            var games = from g in db.Games
                        where ((g.UserIdCreator == userId) ||
                               (g.UserIdOpponent == userId))
                        select g;

            return games;
        }



        [HttpPost]
        [Route("api/Games/Move/Jarvis/{userId}")]
        [ResponseType(typeof(Game))]
        public IHttpActionResult GetMoveFromComputer(string userId, Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            TicTacToeEngine engine = new TicTacToeEngine();
            engine.Initialise(game);

         
            var isEnded = engine.MakeBestMove(true);
            game.UserIdCurrent = jarvisName;
            game.IsTerminated = isEnded;
            game.Board = engine.Board;
            if (game.IsTerminated)
            {
                var result = engine.GetResultState(game.Board);
                if (result == TicTacToeEngine.TicTacToeResult.MachineWin)
                    game.UserIdWinner = jarvisName;
                else
                    game.UserIdWinner = userId;
            }
            return Ok(game);

        }


        // GET: api/Games/5
        [ResponseType(typeof(Game))]
        public IHttpActionResult GetGame(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/Games/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGame(int id, Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.Id)
            {
                return BadRequest();
            }

            db.Entry(game).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                NotificationHubClient hub = NotificationHubClient
                                .CreateClientFromConnectionString("Endpoint=sb://tictactoenotifications.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+Au+w96izwXkztajDDeRB4r+6hsCsN0Gt1lN0Yg7lxM=", "OfficeTicTacToeNotificationHub");

                var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";


                await hub.SendWindowsNativeNotificationAsync(toast);

                var payload = @"<toast>
                                   <visual>
                                       <binding template=""ToastText01"">
                                           <text id=""1"">Hello from a .NET App!</text>
                                       </binding>
                                   </visual>
                                </toast>";

                //var headers = new Dictionary<string, string>();
                //headers.Add("Content-Type", "application/octet-stream");
                //headers.Add("X-WNS-Type", "wns/raw");

                //var tag = game.

                //Notification notification = new WindowsNotification(payload, headers, tag);
                //await hub.SendNotificationAsync(notification);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Games
        [ResponseType(typeof(Game))]
        public IHttpActionResult PostGame(Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Games.Add(game);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [ResponseType(typeof(Game))]
        public IHttpActionResult DeleteGame(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            db.SaveChanges();

            return Ok(game);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.Id == id) > 0;
        }
    }
}