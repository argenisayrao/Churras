using Domain.Services.RunModerateBbq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Serverless_Api
{
    public partial class RunModerateBbq
    {
        private readonly IModerateBbqService _moderateBbqService;

        public RunModerateBbq(IModerateBbqService moderateBbqService)
        {
            _moderateBbqService = moderateBbqService;
        }

        [Function(nameof(RunModerateBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "churras/{id}/moderar")] HttpRequestData req, string id)
        {
            var moderationRequest = await req.Body<ModerateBbqRequest>();

            if (moderationRequest == null)
                return await req.CreateResponse(HttpStatusCode.BadRequest, "churras is required.");

            var response = await _moderateBbqService.Run(new ModerateBbqInput(moderationRequest.GonnaHappen, moderationRequest.TrincaWillPay, id));

            if (response.WasFound is false)
                return await req.CreateResponse(HttpStatusCode.NotFound, "Barbecue not found.");

            return await req.CreateResponse(System.Net.HttpStatusCode.OK, response.Barbecue.TakeSnapshot());
        }
    }
}
