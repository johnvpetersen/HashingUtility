using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;
using static Newtonsoft.Json.JsonConvert;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace App
{
    public  class HashUtility {


       public HashUtility() {}
       public HashUtility(ICustomAlgorithm customAlgorithm, bool isLocked = false) {
           _customAlgorithm = customAlgorithm;
           Locked = isLocked;
       } 

       public HashUtility(AlgorithmOptions algorithmOption,EncodingOptions encodingOption, bool isLocked = false ) {
           AlgorithmOption = algorithmOption;
           EncodingOption = encodingOption;
           Locked = isLocked;
       } 

       public HashUtility(string config, ICustomAlgorithm customAlgorithm = null) {
           var x = JObject.Parse(config);
           Locked = Boolean.Parse(x.GetValue("Locked").ToString());
           _customAlgorithm = customAlgorithm;
           EncodingOption =  (EncodingOptions)Enum.Parse(typeof(EncodingOptions),x.GetValue("EncodingOption").ToString());
           AlgorithmOption = (AlgorithmOptions)Enum.Parse(typeof(AlgorithmOptions),x.GetValue("AlgorithmOption").ToString());
           HashName = x.GetValue("HashName").ToString();
       }


       public override string ToString() => SerializeObject(this);


       public bool Locked {get; private set;}

       public bool CustomAlgorithm => _customAlgorithm != null;

       private byte[] _lastComputedHash = null;
       private ICustomAlgorithm _customAlgorithm;
       [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;} = EncodingOptions.Default;
       [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;} =  AlgorithmOptions.SHA256;
       public string HashName {get; private set;} = string.Empty;
       private int _byteOffset = 0;
       private int _byteCount  = 0;
       private byte[] _bytes;
       private Stream _stream;
       public static HashUtility Create() => new HashUtility();
       public static HashUtility Create(ICustomAlgorithm customAlgorithm) => new HashUtility(customAlgorithm);
       public static HashUtility Create(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, bool isLocked) => new HashUtility(algorithmOption,encodingOption, isLocked);
       public static HashUtility Create(string config, ICustomAlgorithm customAlgorithm = null) => new HashUtility(config,customAlgorithm);


       public Int32 ConvertToInt()  => ConvertToInt(_lastComputedHash);
       public Int32 ConvertToInt(byte[] hash) {
           if (BitConverter.IsLittleEndian)
              Array.Reverse(hash);
 
              return BitConverter.ToInt32(hash,0);
       }
        public Int32 ComputeHashAndConvertToInt<T>(T obj) => ComputeHashAndConvertToInt(getEncodingOption().GetBytes(SerializeObject(obj)));
        public Int32 ComputeHashAndConvertToInt(byte[] bytes) {
           _lastComputedHash = _customAlgorithm != null ? _customAlgorithm.ComputeHash(bytes) : computeHash(bytes);
           return ConvertToInt(_lastComputedHash);
        }
        public HashUtility Lock() {
            Locked = true;
            return this;
        }
        public byte[] ComputeHash(object obj) => ComputeHash(getEncodingOption().GetBytes(SerializeObject(obj)));
        public Int32 ComputeHashAndConvertToInt(object obj) => ComputeHashAndConvertToInt(getEncodingOption().GetBytes(SerializeObject(obj)));
        public byte[] ComputeHash(byte[] bytes) {
           _lastComputedHash = _customAlgorithm != null ? _customAlgorithm.ComputeHash(bytes) : computeHash(bytes);
           return _lastComputedHash;
        }
       private byte[] computeHash(byte[] bytes) {
           byte[] hash = null;
           
           using (var algorithm = getAlgorithm())
           {
              hash = algorithm.ComputeHash(bytes);               
           }
           return hash;
       } 

       public HashUtility SetCustomAlgorithm(ICustomAlgorithm customAlgorithm) {
           if (!Locked)
              _customAlgorithm = customAlgorithm;
           return this;
       }
        public HashUtility SetStream(Stream stream) {
               _stream = stream;
            return this;
        }
        public HashUtility SetBytes(byte[] bytes) {
            return SetBytes(bytes,0,bytes.Length);
        }
        public HashUtility SetBytes(byte[] bytes, int offset, int count) {
               _bytes = bytes;
               _byteOffset = offset;
               _byteCount = count;
            return this;
        }
        public HashUtility SetAlgorithmOption(AlgorithmOptions algorithmOption) {
           return SetAlgorithmOption(algorithmOption,string.Empty);        
        }
        public HashUtility SetAlgorithmOption(AlgorithmOptions algorithmOption, string hashName) {
            if (!Locked) {
           AlgorithmOption = algorithmOption;
           HashName = hashName;
            }
           return this;
        }
        public HashUtility SetEncodingOption(EncodingOptions encodingOption) {
           if (!Locked)
              EncodingOption = encodingOption;
           return this;
       }
        public HashUtility SetHashName(string hashName) {
        if (!Locked)
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
              case AlgorithmOptions.SHA1:   return string.IsNullOrEmpty(HashName)  ? SHA1.Create()   : SHA1.Create(HashName);
              case AlgorithmOptions.SHA256: return  string.IsNullOrEmpty(HashName) ? SHA256.Create() : SHA256.Create(HashName);
              case AlgorithmOptions.SHA384: return  string.IsNullOrEmpty(HashName) ? SHA384.Create() : SHA384.Create(HashName);
              case AlgorithmOptions.SHA512: return  string.IsNullOrEmpty(HashName) ? SHA512.Create() : SHA512.Create(HashName);
          }
          return null;
       }
   }
}