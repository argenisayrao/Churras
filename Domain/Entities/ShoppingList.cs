namespace Domain.Entities
{
    public class ShoppingList
    {
        public ShoppingList(double quantityMeatInKilos, double quantityVegetablesInKilos)
        {
            QuantityMeatInKilos = quantityMeatInKilos;
            QuantityVegetablesInKilos = quantityVegetablesInKilos;
        }

        public double QuantityMeatInKilos {get;set;}
        public double QuantityVegetablesInKilos { get; set; }
    }
}
