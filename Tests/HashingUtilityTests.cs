using Xunit;
using App;
using System.Text;
using System.Security.Cryptography;
using Moq;
using System;

namespace Tests
{

    public class HashingUtilityTests
    {
        [Fact]
        public void CanSetCustomAlgorithm() {

          var expected = new byte[] {0};  
          var mock = new Mock<ICustomAlgorithm>();
          mock.Setup(foo => foo.ComputeHash(It.IsAny<byte[]>())).Returns((byte[] bytes) => expected);
          var sut = HashUtility.Build().SetCustomAlgorithm(mock.Object);
          var actual = sut.ComputeHash(new byte[] {1});
          Assert.Equal(expected,actual);
        } 
        [Fact]
        public void CanComputeSHA256Hash()
        {

            var expected = 397385757 ;
            using (var sha256 = SHA256.Create())
           {
               var x =  sha256.ComputeHash(new byte[] {0});
                               if (BitConverter.IsLittleEndian)
                    Array.Reverse(x);
                var actual = BitConverter.ToInt32(x, 0);

                Assert.Equal(expected,actual);

           }

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