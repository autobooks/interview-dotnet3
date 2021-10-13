using System.IO;
using System.Reflection;

namespace GroceryStoreAPI.DataAccess
{
    public static class Queries
    {
        public static string SelectAllCustomers { get; }
        public static string SelectCustomerById { get; }
        public static string DeleteCustomer { get; }
        public static string UpdateCustomerById { get; }
        public static string InsertCustomer { get; }

        static Queries()
        {
            SelectAllCustomers = GetFileText("select-all-customers.sql");
            SelectCustomerById = GetFileText("select-customer-by-id.sql");           
            DeleteCustomer = GetFileText("delete-customer.sql");
            UpdateCustomerById = GetFileText("update-customer-by-id.sql");
            InsertCustomer = GetFileText("insert-customer.sql");
        }

        private static string GetFileText(string fileName)
        {
            string fileText;
            var assembly = typeof(Queries).GetTypeInfo().Assembly;

            using (var stream =
                assembly.GetManifestResourceStream(
                    $"{typeof(Queries).FullName}.{fileName}"))
            {
                using (var reader = new StreamReader(stream))
                {
                    fileText = reader.ReadToEnd();
                }
            }

            return fileText;
        }
    }
}