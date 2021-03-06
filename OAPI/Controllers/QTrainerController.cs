﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.ServiceFabric.Actors;
using OfficeTicTacToe.Actors.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
//https://github.com/Azure-Samples/service-fabric-dotnet-getting-started/issues/6
// I was unable to add a project name that had more than 4 characters

namespace OfficeTicTacToe.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class QTrainerController : Controller
    {
        // GET: api/values
        [HttpGet()]
        [Route("[action]/{startTrans:int}")]
        public async Task<IActionResult> Start(int startTrans)
        {
            try
            {
                var actor = ActorProxy.Create<IQState>(ActorId.NewId(), "fabric:/OfficeTicTacToe.Actors.SF");

                await actor.StartTrainingAsync(startTrans);

                return Ok(startTrans);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet()]
        [Route("[action]/{stateToken}")]
        public async Task<int> NextValue(int stateToken)
        {
            var actor = ActorProxy.Create<IQTrainedState>(new ActorId(stateToken), "fabric:/OfficeTicTacToe.Actors.SF");

            var qs = await actor.GetChildrenQTrainedStatesAsync();

            return qs[new Random().Next(0, qs.Count)];
        }

    }
}
