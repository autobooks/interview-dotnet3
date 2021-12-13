using GroceryStoreAPI.Domain;
using System;
using Xunit;

namespace GroceryStoreAPI.UnitTest.Domain
{
    public class CustomerTest
    {        
        [Fact (DisplayName = "Create Should Fail When Name Is Null")]
        public void Create_ShouldFail_When_Name_Is_Null()
        {
            Assert.Throws<ArgumentNullException>("name", () =>  Customer.Create(null));
        }

        [Fact (DisplayName = "Create Should Fail When Name Length Is Zero")]
        public void Create_Should_Fail_When_Name_Length_Is_Zero()
        {
            Assert.Throws<ArgumentException>("name", () =>  Customer.Create(""));
        }

        [Fact (DisplayName = "Create Should Fail When Name Length Is More Than 30")]
        public void Create_Should_Fail_When_Name_Length_Is_More_Than_30()
        {
            Assert.Throws<ArgumentException>("name", () =>  Customer.Create("Testing 123456789012345678901234567890"));
        }

        [Fact (DisplayName = "SetData Should Fail When Name Is Null")]
        public void SetData_Should_Fail_When_Name_Is_Null()
        {
            Assert.Throws<ArgumentNullException>("name", () =>  Customer.Create(null));
        }

        [Fact (DisplayName = "SetData Should Fail When Name Length Is Zero")]
        public void SetData_Should_Fail_When_Name_Length_Is_Zero()
        {
            Assert.Throws<ArgumentException>("name", () =>  Customer.Create(""));
        }

        [Fact (DisplayName = "SetData Should Fail When Name Length Is More Than 30")]
        public void SetData_Should_Fail_When_Name_Length_Is_More_Than_30()
        {
            Assert.Throws<ArgumentException>("name", () =>  Customer.Create("Testing 123456789012345678901234567890"));
        }
    
        [Fact (DisplayName = "Create Should Succeed When Name Is Valid")]
        public void Create_Should_Succeed_When_Name_Is_Valid()
        {
            var customer = Customer.Create("Test Name");
            Assert.NotNull(customer);
            Assert.Equal("Test Name", customer.Name);
        }
    }
}
