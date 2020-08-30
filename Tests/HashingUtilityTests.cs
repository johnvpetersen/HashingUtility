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
            Assert.True(HashUtility.Create(config).ComputeHash("FOO").GetHashCode() > 0);
        }
        [Fact]
        public void CanVerifyPassedConfigWorks() {
            var config = "{\"Locked\":false,\"CustomAlgorithm\":false,\"EncodingOption\":\"UTF8\",\"AlgorithmOption\":\"SHA256\",\"HashName\":\"\"}";

            Assert.Equal(660221365,HashUtility.Create(config).ComputeHash("FOO").GetHashCode());
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
            var sut = HashUtility.Create(AlgorithmOptions.SHA512,EncodingOptions.UTF32,true);
            var json = sut.ToString();
            sut.SetCustomAlgorithm(new StubAlgorithm());
            Assert.Equal(json,sut.ToString());
        }
        [Fact]
        public void CanComputeHashIntFromObject() {
            Assert.Equal(660221365,HashUtility.Create().SetEncodingOption(EncodingOptions.UTF8).ComputeHash("FOO").GetHashCode());
        }
        [Fact]
        public void CanSetAndUseCustomAlgorithm() {
          Assert.Equal(new byte[] {0},HashUtility.Create().SetCustomAlgorithm(new StubAlgorithm()).ComputeHash(new byte[] {1}).GetHash());
        } 
        [Theory]
        [InlineData(38610104,AlgorithmOptions.SHA512)]
        [InlineData(-1676987282,AlgorithmOptions.SHA256)]
        [InlineData(-1272856386,AlgorithmOptions.SHA384)]
        [InlineData(-1656968869,AlgorithmOptions.SHA1)]
        public void CanVerifyAlgorithmOptionWhenSelected(Int32 expected, AlgorithmOptions selectedOption) {
           Assert.Equal(expected,HashUtility.Create().SetAlgorithmOption(selectedOption).ComputeHash(new byte[] {0}).ConvertToInt());
        }
       [Fact]
        public void CanVerifySHA256IsDefault()
        {
            Assert.Equal(-1676987282,HashUtility.Create().ComputeHash(new byte[] {0}).ConvertToInt());
        }
        [Fact]
        public void HashNameCanBeSet()
        {
            Assert.Equal("SHA256", HashUtility.Create().SetHashName("SHA256").HashName);
        }
        [Fact]
        public void DefaultEncodingIsSet()
        {
            Assert.Equal(EncodingOptions.Default, HashUtility.Create().EncodingOption);
        }
        [Fact]
        public void CanSelectEncoding()
        {
            Assert.Equal(EncodingOptions.UTF8, HashUtility.Create().SetEncodingOption(EncodingOptions.UTF8).EncodingOption);
        }
    }
    public class StubAlgorithm : ICustomAlgorithm
    {
        public byte[] ComputeHash(byte[] bytes, int startIndex = 0, int length = -1) => new byte[] { 0 };

        public byte[] ComputeHash(object obj)  => new byte[] { 0 };

        public void Dispose()
        {
        
        }
    }


}