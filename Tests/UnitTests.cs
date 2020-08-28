using System;
using System.Text;
using App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using static Newtonsoft .Json.JsonConvert;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CanDetectChangedData() {
            var sut = new Root<string>("test");
            var json = sut.ToString().Replace("test","TEST");
            sut = DeserializeObject<Root<string>>(json);
            Assert.True(sut.HasChanged());


            
        }
       [Fact]
       public void Test() {
           var sut = new TestClass2(1,"One");
           var json = SerializeObject(sut);
           sut = new TestClass2(JObject.Parse(json));
           Assert.Equal(json, sut.ToString());
       } 

        [Fact]
        public void UnchangedObjectHasChangedIsFalse() {
            var sut = new Root<TestClass>(new TestClass(1,"one"));
            Assert.False(sut.HasChanged());
        }

        [Fact]
        public void CanInstantiateNewInstance()
        {
            var actual = new Root<TestClass>(new TestClass(1,"one")).ToString();
            var expected = "{\"Data\":{\"Amount\":1,\"Description\":\"one\"},\"Hash\":308362742}";
            Assert.Equal(expected,actual);
        }
    }

    public class TestClass2    {
        public TestClass2(JObject jObject)  {
            Amount = jObject.SelectToken("Amount").ToObject<Root<int>>();    //DeserializeObject<T>(jObject.SelectToken("Data").ToString());
            Description = jObject.SelectToken("Description").ToObject<Root<string>>();
        }
        [JsonConstructor]
        public TestClass2(int amount, string description)  {
            Amount = Root<int>.Create(amount);
            Description = Root<string>.Create(description);
        }
        public override string ToString() => SerializeObject(this);
        public Root<int> Amount {get; private set;}
        public Root<string> Description {get; private set;}
    }   

    public class TestClass    {
        public TestClass(int amount, string description)  {
            Amount = amount;
            Description = description;
        }
        public int Amount {get; private set;}
        public string Description {get; private set;}

        public static TestClass Create(int amount, string description) => new TestClass(amount,description);
    }
}