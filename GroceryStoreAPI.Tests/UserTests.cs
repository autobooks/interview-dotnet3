using System;
using Xunit;

namespace GroceryStoreAPI.Tests
{
    public class UserTests
    {
        //create local class to make the test succeed (failing to compile is a failed test).
        //then in the refactor phase, move the class where it belongs. 
        internal class User { }
        [Fact]
        public void UserClassShouldExist()
        {
            var user = new User();
            Assert.NotNull(user);            
        }

    }
}
