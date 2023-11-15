using Domain.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Dtos
{
    public class PersonsHasBeenInvitedToBbqDto
    {
        public string StreamId { get; set; }
        [JsonProperty("Body")]
        public PersonHasBeenInvitedToBbq Body { get; set; }

    }
}
