using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services.GetProposedBbqs
{
    public interface IGetProposedBbqsService
    {
        Task<List<object>> Run();
    }
}
