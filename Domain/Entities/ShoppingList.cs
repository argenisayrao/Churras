namespace Domain.Entities
{
    public class ShoppingList
    {
        public ShoppingList()
        {
            QuantityMeatInKilos = 0;
            QuantityVegetablesInKilos = 0;
        }

        public double QuantityMeatInKilos { get; set; }
        public double QuantityVegetablesInKilos { get; set; }
    }
}
