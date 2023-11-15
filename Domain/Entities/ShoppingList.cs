namespace Domain.Entities
{
    public class ShoppingList
    {
        public ShoppingList(int quantityMeatInKilos, int quantityVegetablesInKilos)
        {
            QuantityMeatInKilos = quantityMeatInKilos;
            QuantityVegetablesInKilos = quantityVegetablesInKilos;
        }

        public int QuantityMeatInKilos {get;set;}
        public int QuantityVegetablesInKilos { get; set; }
    }
}
