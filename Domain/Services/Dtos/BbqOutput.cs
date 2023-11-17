using Domain.Entities;

namespace Domain.Services.Dtos
{
    public class BbqOutput
    {
        public Bbq Barbecue { get; set; }
        public bool WasFound { get; set; }
        public bool Accepted { get; set; }

        public BbqOutput(Bbq? barbecue)
        {
            if (barbecue == null)
                WasFound = false;
            else
            {
                Barbecue = barbecue;
                WasFound = true;
            }
        }

        public BbqOutput(bool accepted)
        {
            Accepted = accepted;
        }
    }
}
