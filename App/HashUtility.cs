using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace App
{
    public  class HashUtility {
       public EncodingOptions EncodingOption {get; private set;} = EncodingOptions.Default;
       public AlgorithmOptions AlgorithmOption {get; private set;} =  AlgorithmOptions.SHA256;
       public string HashName {get; private set;} = string.Empty;
       public int ByteOffset {get; private set;} = 0;
       public int ByteCount  {get; private set;} = 0;
       public byte[] Bytes {get; private set;}
       public Stream Stream {get; private set;}


       public static HashUtility Build() => new HashUtility();
        public HashUtility SetStream(Stream stream) {
            Stream = stream;
            return this;
        }
        public HashUtility SetBytes(byte[] bytes) {
           return SetBytes(bytes,0,bytes.Length);            return this;
        }
        public HashUtility SetBytes(byte[] bytes, int offset, int count) {
            Bytes = bytes;
            ByteOffset = offset;
            ByteCount = count;
            return this;
        }
        public HashUtility SelectAlgorithmOption(AlgorithmOptions algorithmOption) {
           return SelectAlgorithmOption(algorithmOption,string.Empty);        
        }
        public HashUtility SelectAlgorithmOption(AlgorithmOptions algorithmOption, string hashName) {
           AlgorithmOption = algorithmOption;
           HashName = hashName;
           return this;
        }
        public HashUtility SelectEncodingOption(EncodingOptions encodingOption) {

           EncodingOption = encodingOption;
           return this;
       }
        public HashUtility SetHashName(string hashName) {
           HashName = hashName;
           return this;
       }
      private Encoding getEncodingOption() {
          switch (EncodingOption)
          {
              case EncodingOptions.BigEndianUnicode: return Encoding.BigEndianUnicode;
              case EncodingOptions.Default:          return Encoding.Default;
              case EncodingOptions.Unicode:          return Encoding.Unicode;
              case EncodingOptions.UTF32:            return Encoding.UTF32;
              case EncodingOptions.UTF7:             return Encoding.UTF7;
              case EncodingOptions.UTF8:             return Encoding.UTF8;
          }
          return null;
      }
      private HashAlgorithm getAlgorithm() {
          switch (AlgorithmOption)
          {
              case AlgorithmOptions.SHA1:   return  SHA1.Create(HashName);
              case AlgorithmOptions.SHA256: return  SHA256.Create(HashName);
              case AlgorithmOptions.SHA384: return  SHA384.Create(HashName);
              case AlgorithmOptions.SHA512: return  SHA512.Create(HashName);
          }
          return null;
       }
   }
}