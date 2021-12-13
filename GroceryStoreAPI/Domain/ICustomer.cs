namespace GroceryStoreAPI.Domain
{
    public interface ICustomer
    {
        int Id { get; set; }
        string Name { get; set; }

        void SetData(string name);
    }
}