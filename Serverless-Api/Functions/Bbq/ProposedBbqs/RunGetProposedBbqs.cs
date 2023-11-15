using System.Net;
using Domain.Services.GetProposedBbqs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Serverless_Api
{
    public partial class RunGetProposedBbqs
    {
        private readonly IGetProposedBbqsService _getProposedBbqsService;
        public RunGetProposedBbqs(IGetProposedBbqsService getProposedBbqsService)
        {
            _getProposedBbqsService = getProposedBbqsService;
        }

        [Function(nameof(RunGetProposedBbqs))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "churras")] HttpRequestData req)
        {
            var proposedBbqsService = await _getProposedBbqsService.Run();

            if (proposedBbqsService is null)
                return req.CreateResponse(HttpStatusCode.NoContent);

            return await req.CreateResponse(HttpStatusCode.Created, proposedBbqsService);
        }
    }
}
