using Domain.Events;
using Newtonsoft.Json;

namespace Domain.Repositories.Dtos
{
    public class PersonsHasBeenInvitedToBbqDto
    {
        public string StreamId { get; set; }
        [JsonProperty("Body")]
        public PersonHasBeenInvitedToBbq Body { get; set; }

    }
}
