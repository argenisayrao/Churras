using System.Threading.Tasks;

namespace Domain.Services.CreateNewBbq
{
    public interface ICreateNewBbqService
    {
        Task<object> Run(CreateNewBbqServiceInput input);
    }
}
