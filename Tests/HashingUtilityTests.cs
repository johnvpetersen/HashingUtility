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

            var getHashCode = sut.GetHashCode();

            var val1 = sut.ConvertToInt(hash);
            Assert.Equal(val1,getHashCode);

        }

        [Fact]
        public void CanVerifyPassedConfigWorks() {
            var config = "{\"Locked\":false,\"CustomAlgorithm\":false,\"EncodingOption\":\"UTF8\",\"AlgorithmOption\":\"SHA256\",\"HashName\":\"\"}";
            var sut = HashUtility.Create(config);
            Assert.Equal(config,sut.ToString());
            var hash = sut.ComputeHash("FOO");
            Assert.Equal(660221365,sut.ConvertToInt(hash));
            sut.SetAlgorithmOption(AlgorithmOptions.SHA512).SetEncodingOption(EncodingOptions.UTF32);
            hash = sut.ComputeHash("FOO"); 
            Assert.Equal(1939473270,sut.ConvertToInt(hash));
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
           var hash =  sut.SetEncodingOption(EncodingOptions.UTF8).ComputeHash("FOO");
           var actual = sut.ConvertToInt(hash);
           var expected = 660221365;

            Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanComputeHashBytesFromObject() {
           var sut = HashUtility.Create();
           var bytes =  sut.SetEncodingOption(EncodingOptions.UTF8).ComputeHash("FOO");
           var actual = sut.ConvertToInt(bytes);
           var expected = 660221365;
            Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanComputeHashFromObjectInstance() {
           var sut = HashUtility.Create();
           var expected = 660221365;
           var hash = sut.ComputeHash("FOO");
           var actual = sut.ConvertToInt(hash);
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
           var expected = -1656968869;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA1);
           var hash = sut.ComputeHash(new byte[] {0});     
           var actual = sut.ConvertToInt(hash);
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA384WhenSelected() {
           var expected = -1272856386;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA384);
           var hash = sut.ComputeHash(new byte[] {0});     
           var actual = sut.ConvertToInt(hash);
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA512WhenSelected() {
           var expected = 38610104;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA512);     
           var hash = sut.ComputeHash(new byte[] {0});     
           var actual = sut.ConvertToInt(hash);
           Assert.Equal(expected,actual);
        }
        [Fact]
        public void CanVerifySHA256WhenSelected() {
           var expected = -1676987282 ;
           var sut = HashUtility.Create();
           sut.SetAlgorithmOption(AlgorithmOptions.SHA256);     
           var hash = sut.ComputeHash(new byte[] {0});     
           var actual = sut.ConvertToInt(hash);
           Assert.Equal(expected,actual);
        }
       [Fact]
        public void CanVerifySHA256IsDefault()
        {
            var expected = -1676987282 ;
            var sut = HashUtility.Create();
            var hash = sut.ComputeHash(new byte[] {0});
            var actual = sut.ConvertToInt(hash);
            Assert.Equal(expected,actual);
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