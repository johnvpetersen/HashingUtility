using Xunit;
using App;
using System;
namespace Tests
{
    public class HashingUtilityTests
    {
        [Fact]
        public void CanGenerateConfigString() {
         Assert.Equal("{\"CustomAlgorithm\":null,\"UseBase64Encoding\":false,\"AlgorithmOption\":\"SHA256\",\"Base64FormattingOption\":\"None\",\"EncodingOption\":\"UTF8\",\"HashName\":null}", HashUtilityConfig.GenerateConfigString(AlgorithmOptions.SHA256,EncodingOptions.UTF8));
        }
        [Fact]
        public void CanCreateBase64StringViaConfig() {
            var config = new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8,null,Base64FormattingOptions.None,true);
            Assert.Equal("bjQLnP+zepicpUTmu3gKLHiQHT+zNzh2hRGjBhevoB0=",HashUtility.Create(config.ToString()).ComputeHash(new byte[] {0}).ConvertToString());
        }
        [Fact]
        public void CanGetBase64StringViaConvertToString() {
           Assert.Equal("bjQLnP+zepicpUTmu3gKLHiQHT+zNzh2hRGjBhevoB0=",HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").ComputeHash(new byte[] {0}).ConvertToString(startIndex:0,createBase64String:true));
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanVerifyIfCustomAlgorithmIsSet(bool useCustomAlgorithm) {
            ICustomAlgorithm customAlgorithm = useCustomAlgorithm ? new StubAlgorithm() : null;
            var sut = HashUtility.Create(customAlgorithm);
            Assert.Equal(useCustomAlgorithm,sut.CustomAlgorithm);
        }
        [Fact]
        public void CanVerifyWhenLockIsTrueObjectIsLocked() {
            var sut = HashUtility.Create(new StubAlgorithm());
            var json = sut.ToString();
            Assert.Equal(json,sut.ToString());
        }
        [Fact]
        public void CanComputeHashIntFromObject() {
            Assert.Equal(660221365,HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").ComputeHash("FOO").GetHashCode());
        }
        [Fact]
        public void CanSetAndUseCustomAlgorithm() {
          Assert.Equal(new byte[] {0},HashUtility.Create(new StubAlgorithm()).ComputeHash(new byte[] {1}).GetHash());
        } 
        [Theory]
        [InlineData(38610104,AlgorithmOptions.SHA512)]
        [InlineData(-1676987282,AlgorithmOptions.SHA256)]
        [InlineData(-1272856386,AlgorithmOptions.SHA384)]
        [InlineData(-1656968869,AlgorithmOptions.SHA1)]
        public void CanVerifyAlgorithmOptionWhenSelected(Int32 expected, AlgorithmOptions selectedOption) {
           Assert.Equal(expected,HashUtility.Create(selectedOption,EncodingOptions.UTF8,"").ComputeHash(new byte[] {0}).ConvertToInt());
        }
       [Fact]
        public void CanVerifySHA256IsDefault()
        {
            Assert.Equal(-1676987282,HashUtility.Create().ComputeHash(new byte[] {0}).ConvertToInt());
        }
        [Fact]
        public void HashNameCanBeSet()
        {
            Assert.Equal("SHA256", HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"SHA256").HashName);
        }
        [Fact]
        public void DefaultEncodingIsSet()
        {
            Assert.Equal(EncodingOptions.Default, HashUtility.Create().EncodingOption);
        }
        [Fact]
        public void CanSelectEncoding()
        {
            Assert.Equal(EncodingOptions.UTF8, HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").EncodingOption);
        }
    }
    public class StubAlgorithm : ICustomAlgorithm
    {
        public byte[] ComputeHash(byte[] bytes, int startIndex = 0, int length = -1) => new byte[] { 0 };
        public byte[] ComputeHash(object obj)  => new byte[] { 0 };
        public void Dispose()
        {
            Console.WriteLine("Custom algorithm is disposing.");
        }
    }
}