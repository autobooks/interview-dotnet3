using System;

namespace GroceryStoreAPI.Domain
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Empty constructor for EF
        private Customer()
        {

        }

        // Constructor to be called from Create method
        private Customer(string name)
        {
            Name = name;
        }

        // Create a new customer
        public static Customer Create(string name)
        {
            // Adding these validations here for demo purposes. These can be annotations on the DTO
            if (name is null)
            {
                throw(new ArgumentNullException(nameof(name), "Name is required"));
            }
            if (name.Length == 0)
            {
                throw(new ArgumentException("Name length must be greater than zero", nameof(name)));
            }
            if (name.Length > 30)
            {
                throw(new ArgumentException("Name length cannot exceed 30 characters", nameof(name)));
            }

            return new Customer(name);
        }

        // Update data 
        public void SetData(string name)
        {
            if (name is null)
            {
                throw(new ArgumentNullException(nameof(name), "Name is required"));
            }
            if (name.Length == 0)
            {
                throw(new ArgumentException("Name length must be greater than zero", nameof(name)));
            }
            if (name.Length > 30)
            {
                throw(new ArgumentException("Name length cannot exceed 30 characters", nameof(name)));
            }

            Name = name;
        }
    }
}
