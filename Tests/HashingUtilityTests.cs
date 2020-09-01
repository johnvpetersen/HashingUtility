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
       public void CanUseConfigObjectToGenerateString() {
           Assert.Equal("6E-34-0B-9C-FF-B3-7A-98-9C-A5-44-E6-BB-78-0A-2C-78-90-1D-3F-B3-37-38-76-85-11-A3-06-17-AF-A0-1D",HashUtility.GenerateStringFromHash(new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8),new byte[] {0}));
       }

       [Fact]
       public void CanUseConfigStringToGenerateString() {
           Assert.Equal("6E-34-0B-9C-FF-B3-7A-98-9C-A5-44-E6-BB-78-0A-2C-78-90-1D-3F-B3-37-38-76-85-11-A3-06-17-AF-A0-1D",HashUtility.GenerateStringFromHash(new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8).ToString(),new byte[] {0}));
       }
       [Fact]
       public void CanUseConfigObjectToGenerateInt() {
           Assert.Equal(-1676987282,HashUtility.GenerateIntFromHash(new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8),new byte[] {0}));
       }
       [Fact]
       public void CanUseConfigStringToGenerateInt() {
           Assert.Equal(-1676987282,HashUtility.GenerateIntFromHash(new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8).ToString(),new byte[] {0}));
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
            Assert.Equal(660221365,HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"",false).ComputeHash("FOO").GetHashCode());
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
           Assert.Equal(expected,HashUtility.Create(selectedOption,EncodingOptions.UTF8,"",false).ComputeHash(new byte[] {0}).ConvertToInt());
        }
       [Fact]
        public void CanVerifySHA256IsDefault()
        {
            Assert.Equal(-1676987282,HashUtility.Create().ComputeHash(new byte[] {0}).ConvertToInt());
        }
        [Fact]
        public void HashNameCanBeSet()
        {
            Assert.Equal("SHA256", HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"SHA256",false).HashName);
        }
        [Fact]
        public void DefaultEncodingIsSet()
        {
            Assert.Equal(EncodingOptions.Default, HashUtility.Create().EncodingOption);
        }
        [Fact]
        public void CanSelectEncoding()
        {
            Assert.Equal(EncodingOptions.UTF8, HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"",false).EncodingOption);
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