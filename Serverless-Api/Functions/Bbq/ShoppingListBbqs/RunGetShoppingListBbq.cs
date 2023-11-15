using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Net;
using Domain.Services.ShoppingListBbq;
using CrossCutting;
using Domain.Entities;

namespace Serverless_Api.Functions.Bbq.ShoppingListBbqs
{
    internal class RunGetShoppingListBbq
    {
        private readonly IGetShoppingListBbqService _getShoppingListBbqService;
        private readonly SnapshotStore _snapshotStore;
        public RunGetShoppingListBbq(IGetShoppingListBbqService getShoppingListBbqService, SnapshotStore snapshotStore)
        {
            _getShoppingListBbqService = getShoppingListBbqService;
            _snapshotStore = snapshotStore;
        }

        [Function(nameof(RunGetShoppingListBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "person/{personId}/shopping-list/{bbqId}")] HttpRequestData req,
            string personId, string bbqId)
        {
            var coOwners = await _snapshotStore.AsQueryable<Lookups>("Lookups").SingleOrDefaultAsync();

            if(coOwners.ModeratorIds.Contains(personId) is false)
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            if(string.IsNullOrWhiteSpace(bbqId))
                return await req.CreateResponse(HttpStatusCode.BadRequest, "Barbecue id is required");

            var response = await _getShoppingListBbqService.Run(bbqId);

            if (response.WasFound is false)
                return req.CreateResponse(HttpStatusCode.NotFound);

            return await req.CreateResponse(HttpStatusCode.OK, response.Barbecue.TakeSnapshot());
        }
    }
}
