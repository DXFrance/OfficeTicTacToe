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

namespace OfficeTicTacToe.Server.Controllers
{
    public class GamesController : ApiController
    {
        private TicTacToeEntities db = new TicTacToeEntities();

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
        [Route("api/Games/Users/{userId}")]
        public IQueryable<Game> MakeMove(string userId, Game game)
        {

            var move = new Move();
            move.CreatedDate = DateTime.UtcNow;
            move.CurrentPlayerIndex = game.CurrentPlayerIndex;
            move.GameId = game.Id;
            move.NewBoard = game.Board;
            move.Winner = game.Winner;
            move.GameResult = game.GameResult;
            move.UserId = userId;

            var games = from g in db.Games
                        where ((g.UserIdCreator == userId) ||
                               (g.UserIdOpponent == userId))
                        select g;

            return games;
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
        public IHttpActionResult PutGame(int id, Game game)
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