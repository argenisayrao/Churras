using System.Net;
using CrossCutting;
using Domain.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Domain.Services.CreateNewBbq;

namespace Serverless_Api
{
    public partial class RunCreateNewBbq
    {
        private readonly ICreateNewBbqService _createNewBbq;

        public RunCreateNewBbq(ICreateNewBbqService createNewBbq)
        {
            _createNewBbq = createNewBbq;
        }

        [Function(nameof(RunCreateNewBbq))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "churras")] HttpRequestData req)
        {
            var input = await req.Body<NewBbqRequest>();

            if (input == null)
                return await req.CreateResponse(HttpStatusCode.BadRequest, "input is required.");

            var inputService = new CreateNewBbqServiceInput(input.Date, input.Reason, input.IsTrincasPaying);
            var churrasSnapshot = await _createNewBbq.Run(inputService);

            return await req.CreateResponse(HttpStatusCode.Created, churrasSnapshot);
        }
    }
}
