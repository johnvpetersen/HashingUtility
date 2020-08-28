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
        public void CanReadConfiguration() {

            var config = "{\"Locked\":false,\"CustomAlgorithm\":false,\"EncodingOption\":\"UTF8\",\"AlgorithmOption\":\"SHA256\",\"HashName\":\"\"}";
            var sut = HashUtility.Create(config);
            var hash = sut.ComputeHash("FOO");
            var val1 = sut.ConvertToInt(hash);
            var val2 = sut.ConvertTo<Int32>(hash);
            Assert.Equal(val1,val2);

        }

        [Fact]
        public void CanVerifyPassedConfigWorks() {
            var config = "{\"Locked\":false,\"CustomAlgorithm\":false,\"EncodingOption\":\"UTF8\",\"AlgorithmOption\":\"SHA256\",\"HashName\":\"\"}";
            var sut = HashUtility.Create(config);
            Assert.Equal(config,sut.ToString());
            Assert.Equal(-1292211014,sut.ComputeHashAndConvertToInt("FOO"));
            sut.SetAlgorithmOption(AlgorithmOptions.SHA512).SetEncodingOption(EncodingOptions.UTF32);
            Assert.Equal(-267213619,sut.ComputeHashAndConvertToInt("FOO"));
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanVerifyIfCustomAlgorithmIsSet(bool useCustomAlgorithm) {
            ICustomAlgorithm customAlgorithm = useCustomAlgorithm ? new MyAlgorithm() : null;
            var sut = HashUtility.Create(customAlgorithm);
            Assert.Equal(useCustomAlgorithm,sut.CustomAlgorithm);
        }
        [Fact]
        public void CanVerifyLockFlagWorks() {
            var sut = HashUtility.Create(AlgorithmOptions.SHA512,EncodingOptions.UTF32,true);
            var json = sut.ToString();
            sut.SetCustomAlgorithm(new MyAlgorithm());
            Assert.Equal(json,sut.ToString());
        }
        [Fact]
        public void CanComputeHashIntFromObject() {
           var sut = HashUtility.Create();
           var actual =  sut.SetEncodingOption(EncodingOptions.UTF8).ComputeHashAndConvertToInt("FOO");
           var expected = -1292211014;

            Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanComputeHashBytesFromObject() {
           var sut = HashUtility.Create();
           var bytes =  sut.SetEncodingOption(EncodingOptions.UTF8).ComputeHash("FOO");
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
           sut.SetAlgorithmOption(AlgorithmOptions.SHA1);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA384WhenSelected() {
           var expected = 144774935;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA384);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA512WhenSelected() {
           var expected = -479653650;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA512);     
           var actual = sut.ComputeHashAndConvertToInt(new byte[] {0});
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA256WhenSelected() {
           var expected = 397385757 ;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA256);     
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
            var sut = HashUtility.Create().SetAlgorithmOption(AlgorithmOptions.SHA256);
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
            var sut = HashUtility.Create().SetEncodingOption(EncodingOptions.UTF8);
            Assert.Equal(EncodingOptions.UTF8, sut.EncodingOption);
        }
    }

    public class MyAlgorithm : ICustomAlgorithm
    {
        public byte[] ComputeHash(byte[] bytes) => new byte[] { 0 };
    }


}