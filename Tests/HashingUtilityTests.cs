using App;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Tests
{
    [TestClass]
    public class HashingUtilityTests
    {
       private byte[] _generatedHash;
       private string _generatedString;
       private Int32 _generatedInt;
       [TestMethod]
       public void GeneratedIntEventDoesFire() {
           _generatedInt = 0;
           var sut = HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,string.Empty);
           sut.ConvertedToInt += IntIsGenerated;
           sut.ComputeHash(new byte[] {0}).ConvertToInt();
           Assert.AreEqual(_generatedInt,sut.ConvertToInt());
       }
       [TestMethod]
       public void GeneratedStringEventDoesFire() {
           _generatedString = string.Empty;
           var sut = HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,string.Empty);
           sut.ConvertedToString += StringIsGenerated;
           sut.ComputeHash(new byte[] {0}).ConvertToString(0,createBase64String:true);
           Assert.AreEqual(_generatedString,sut.ConvertToString(0,createBase64String:true));
       }
       [TestMethod]
       public void GeneratedHashEventDoesFire() {
           _generatedHash = null;
           var sut = HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,string.Empty);
           sut.HashGenerated += HashIsGenerated;
           sut.ComputeHash(new byte[] {0});
           Assert.AreEqual(_generatedHash,sut.GetHash());
       }
       void IntIsGenerated(Int32 hash) {
           _generatedInt = hash;
       }
       void StringIsGenerated(string hash) {
           _generatedString = hash;
       }
       void HashIsGenerated(byte[] bytes) {
          _generatedHash = bytes;
       }
        [TestMethod]
        public void CanGenerateConfigString() {
         Assert.AreEqual("{\"CustomAlgorithm\":null,\"UseBase64Encoding\":false,\"AlgorithmOption\":\"SHA256\",\"Base64FormattingOption\":\"None\",\"EncodingOption\":\"UTF8\",\"HashName\":null}", HashUtilityConfig.GenerateConfigString(AlgorithmOptions.SHA256,EncodingOptions.UTF8));
        }
        [TestMethod]
        public void CanCreateBase64StringViaConfig() {
            var config = new HashUtilityConfig(AlgorithmOptions.SHA256,EncodingOptions.UTF8,null,Base64FormattingOptions.None,true);
            Assert.AreEqual("bjQLnP+zepicpUTmu3gKLHiQHT+zNzh2hRGjBhevoB0=",HashUtility.Create(config.ToString()).ComputeHash(new byte[] {0}).ConvertToString());
        }
        [TestMethod]
        public void CanGetBase64StringViaConvertToString() {
           Assert.AreEqual("bjQLnP+zepicpUTmu3gKLHiQHT+zNzh2hRGjBhevoB0=",HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").ComputeHash(new byte[] {0}).ConvertToString(startIndex:0,createBase64String:true));
        }
        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void CanVerifyIfCustomAlgorithmIsSet(bool useCustomAlgorithm) {
            ICustomAlgorithm customAlgorithm = useCustomAlgorithm ? new StubAlgorithm() : null;
            var sut = HashUtility.Create(customAlgorithm);
            Assert.AreEqual(useCustomAlgorithm,sut.CustomAlgorithm);
        }
        [TestMethod]
        public void CanVerifyWhenLockIsTrueObjectIsLocked() {
            var sut = HashUtility.Create(new StubAlgorithm());
            var json = sut.ToString();
            Assert.AreEqual(json,sut.ToString());
        }
        [TestMethod]
        public void CanComputeHashIntFromObject() {
            Assert.AreEqual(660221365,HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").ComputeHash("FOO").GetHashCode());
        }
        [TestMethod]
        public void CanSetAndUseCustomAlgorithm() {
          Assert.AreEqual((new byte[] {0}).Length,HashUtility.Create(new StubAlgorithm()).ComputeHash(new byte[] {1}).GetHash().Length);
        } 
        [DataTestMethod]
        [DataRow(38610104,AlgorithmOptions.SHA512)]
        [DataRow(-1676987282,AlgorithmOptions.SHA256)]
        [DataRow(-1272856386,AlgorithmOptions.SHA384)]
        [DataRow(-1656968869,AlgorithmOptions.SHA1)]
        public void CanVerifyAlgorithmOptionWhenSelected(Int32 expected, AlgorithmOptions selectedOption) {
           Assert.AreEqual(expected,HashUtility.Create(selectedOption,EncodingOptions.UTF8,"").ComputeHash(new byte[] {0}).ConvertToInt());
        }
       [TestMethod]
        public void CanVerifySHA256IsDefault()
        {
            Assert.AreEqual(-1676987282,HashUtility.Create().ComputeHash(new byte[] {0}).ConvertToInt());
        }
        [TestMethod]
        public void HashNameCanBeSet()
        {
            Assert.AreEqual("SHA256", HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"SHA256").HashName);
        }
        [TestMethod]
        public void DefaultEncodingIsSet()
        {
            Assert.AreEqual(EncodingOptions.Default, HashUtility.Create().EncodingOption);
        }
        [TestMethod]
        public void CanSelectEncoding()
        {
            Assert.AreEqual(EncodingOptions.UTF8, HashUtility.Create(AlgorithmOptions.SHA256,EncodingOptions.UTF8,"").EncodingOption);
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