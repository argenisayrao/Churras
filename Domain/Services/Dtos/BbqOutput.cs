using Domain.Entities;

namespace Domain.Services.Dtos
{
    public class BbqOutput
    {
        public Bbq Barbecue { get; set; }
        public bool WasFound { get; set; }

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
    }
}
