using Xunit;
using App;
using Moq;
using System;
using static Newtonsoft.Json.JsonConvert;

namespace Tests
{
    public class HashingUtilityTests
    {
        [Fact]
        public void CanComputeHashIntFromObject() {
           var sut = HashUtility.Create();
           var actual =  sut.SelectEncodingOption(EncodingOptions.UTF8).ComputeHashAndConvertToInt("FOO");
           var expected = -1292211014;

            Assert.Equal(expected,actual);
        }

        [Fact]
        public void CanComputeHashBytesFromObject() {
           var sut = HashUtility.Create();
           var bytes =  sut.SelectEncodingOption(EncodingOptions.UTF8).ComputeHash("FOO");
           var expected = -1292211014;

             if (BitConverter.IsLittleEndian)
               Array.Reverse(bytes);
            Assert.Equal(expected,BitConverter.ToInt32(bytes));
        }

        [Fact]
        public void CanComputeHashFromObjectInstance() {
           var sut = HashUtility.Create();
           var expected = -1292211014;
           var actual = sut.ComputeHashAndConvertToInt<string>("FOO");
           Assert.Equal(expected,actual);
        }

        [Fact]
        public void CanSetCustomAlgorithm() {
          var expected = new byte[] {0};  
          var mock = new Mock<ICustomAlgorithm>();
          mock.Setup(x => x.ComputeHash(It.IsAny<byte[]>())).Returns((byte[] bytes) => expected);
          var sut = HashUtility.Create().SetCustomAlgorithm(mock.Object);
          var actual = sut.ComputeHash(new byte[] {1});
          Assert.Equal(expected,actual);
        } 
        [Fact]
        public void CanVerifySHA1WhenSelected() {
           var expected = -308119473;
           var sut = HashUtility.Create();
           sut.SelectAlgorithmOption(AlgorithmOptions.SHA1);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA384WhenSelected() {
           var expected = 144774935;
           var sut = HashUtility.Create();
           sut.SelectAlgorithmOption(AlgorithmOptions.SHA384);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA512WhenSelected() {
           var expected = -479653650;
           var sut = HashUtility.Create();
           sut.SelectAlgorithmOption(AlgorithmOptions.SHA512);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA256WhenSelected() {
           var expected = 397385757 ;
           var sut = HashUtility.Create();
           sut.SelectAlgorithmOption(AlgorithmOptions.SHA256);     
            var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
            Assert.Equal(expected,actual);
        }
       [Fact]
        public void CanVerifySHA256IsDefault()
        {
            var expected = 397385757 ;
            var sut = HashUtility.Create();
            var actual = sut.ComputeHash(new byte[] {0});
            if (BitConverter.IsLittleEndian)
               Array.Reverse(actual);
            Assert.Equal(expected,BitConverter.ToInt32(actual));
        }
        [Fact]
        public void CanSelectAlgorithm()
        {
            var sut = HashUtility.Create().SelectAlgorithmOption(AlgorithmOptions.SHA256);
            Assert.Equal(AlgorithmOptions.SHA256, sut.AlgorithmOption);
        }
        [Fact]
        public void HashNameCanBeSet()
        {
            var sut = HashUtility.Create().SetHashName("SHA256");
            Assert.Equal("SHA256", sut.HashName);
        }
        [Fact]
        public void DefaultEncodingIsSet()
        {
            var sut = HashUtility.Create();
            Assert.Equal(EncodingOptions.Default, sut.EncodingOption);
        }
        [Fact]
        public void CanSelectEncoding()
        {
            var sut = HashUtility.Create().SelectEncodingOption(EncodingOptions.UTF8);
            Assert.Equal(EncodingOptions.UTF8, sut.EncodingOption);
        }
    }
}