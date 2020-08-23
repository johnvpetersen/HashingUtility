using Xunit;
using App;
using System.Text;
using System.Security.Cryptography;


namespace Tests
{

    public class HashingUtilityTests
    {

        [Fact]
        public void CanComputeHash()
        {
        }
 
        [Fact]
        public void CanSelectAlgorithm()
        {
            var sut = HashUtility.Build().SelectAlgorithmOption(AlgorithmOptions.SHA256);
            Assert.Equal(AlgorithmOptions.SHA256, sut.AlgorithmOption);
        }
        [Fact]
        public void HashNameCanBeSet()
        {
            var sut = HashUtility.Build().SetHashName("SHA256");
            Assert.Equal("SHA256", sut.HashName);
        }
        [Fact]
        public void DefaultEncodingIsSet()
        {
            var sut = HashUtility.Build();
            Assert.Equal(EncodingOptions.Default, sut.EncodingOption);
        }
        [Fact]
        public void CanSelectEncoding()
        {
            var sut = HashUtility.Build().SelectEncodingOption(EncodingOptions.UTF8);
            Assert.Equal(EncodingOptions.UTF8, sut.EncodingOption);
        }
    }
}