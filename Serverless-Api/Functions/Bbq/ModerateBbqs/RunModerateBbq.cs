using CrossCutting;
using Domain.Entities;
using Domain.Events;
using Domain.Repositories;
using Domain.Services.RunModerateBbq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunModerateBbq
    {
        private readonly SnapshotStore _snapshots;
        private readonly IPersonRepository _persons;
        private readonly IBbqRepository _repository;
        private readonly IModerateBbqService _moderateBbqService;

        public RunModerateBbq(IBbqRepository repository, SnapshotStore snapshots, IPersonRepository persons, IModerateBbqService moderateBbqService)
        {
            _persons = persons;
            _snapshots = snapshots;
            _repository = repository;
            _moderateBbqService = moderateBbqService;
        }

        [Function(nameof(RunModerateBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "churras/{id}/moderar")] HttpRequestData req, string id)
        {
            var moderationRequest = await req.Body<ModerateBbqRequest>();

            if (moderationRequest == null)
                return await req.CreateResponse(HttpStatusCode.BadRequest, "input is required.");

            var response = await _moderateBbqService.Run(new ModerateBbqInput(moderationRequest.GonnaHappen, moderationRequest.TrincaWillPay, id));

            if (response.Barbecue is null)
                return await req.CreateResponse(HttpStatusCode.NotFound, "Barbecue not found.");


            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response.Barbecue.TakeSnapshot());
        }
    }
}
