namespace Domain.Entities
{
    public class ConfirmedGuest
    {
        public ConfirmedGuest(string id, bool isVeg)
        {
            Id = id;
            IsVeg = isVeg;
        }

        public string Id { get; set; }
        public bool IsVeg{ get; set; }
    }
}
