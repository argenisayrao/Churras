using Domain.Entities;

namespace Domain.Services.GetInvite
{
    public class GetInviteOutput
    {
        public Person Person { get; set; }
        public GetInviteOutput(Person person)
        {
            Person = person;
        }
    }
}
