using System;
using App;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ObjectsWithEquivPropsAreEqual()
        {
            Assert.True(TestClass.Create(1,"One").Equals(TestClass.Create(1,"One")));
        }

        [Fact]
        public void HashPropertyIsPopulated()
        {
            Assert.True(TestClass.Create(1,"One").Hash == 491241268);
        }


    }

    public class TestClass : root {
        public TestClass(int amount, string description) : base() {
            Amount = amount;
            Description = description;
        }

        public int Amount {get; private set;}
        public string Description {get; private set;}

        public static TestClass Create(int amount, string description) => new TestClass(amount,description);
    }
}
