using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using static Newtonsoft.Json.JsonConvert;
namespace App
{
    public  class HashUtility : IDisposable {
       public delegate void HashGeneratedNotification(byte[] hash);
       public delegate void ConvertedToIntNotification(Int32 hash);
       public delegate void ConvertedToStringNotification(string hash);
       public event HashGeneratedNotification HashGenerated;
       public event ConvertedToIntNotification ConvertedToInt;
       public event ConvertedToStringNotification ConvertedToString;
       protected virtual void OnHashGenerated() {
          HashGenerated?.Invoke(_lastComputedHash);
       }
       protected virtual void OnIntGenerated(Int32 hash) {
          ConvertedToInt?.Invoke(hash);
       }
       protected virtual void OnStringGenerated(string hash) {
          ConvertedToString?.Invoke(hash);
       }
       public HashUtility() {}
       public HashUtility(ICustomAlgorithm customAlgorithm) {
           _customAlgorithm = customAlgorithm;
       } 
       public HashUtility(AlgorithmOptions algorithmOption,EncodingOptions encodingOption, string hashName = "") {
           AlgorithmOption = algorithmOption;
           EncodingOption = encodingOption;
           HashName = hashName;
       } 
       public HashUtility(string config, ICustomAlgorithm customAlgorithm = null) {
           var configObject = JObject.Parse(config);
           _customAlgorithm = customAlgorithm;
           EncodingOption =  (EncodingOptions)Enum.Parse(typeof(EncodingOptions),configObject.GetValue("EncodingOption").ToString());
           AlgorithmOption = (AlgorithmOptions)Enum.Parse(typeof(AlgorithmOptions),configObject.GetValue("AlgorithmOption").ToString());
           _useBase64EncodedString =  Boolean.Parse(configObject.GetValue("UseBase64Encoding").ToString());
           _base64FormattingOption = (Base64FormattingOptions)Enum.Parse(typeof(Base64FormattingOptions), configObject.GetValue("Base64FormattingOption").ToString());
           HashName = configObject.GetValue("HashName").ToString();
       }
       public override string ToString() => SerializeObject(this);
       public bool CustomAlgorithm => _customAlgorithm != null;
       private byte[] _lastComputedHash = null;
       private bool _useBase64EncodedString = false;
       private Base64FormattingOptions _base64FormattingOption = Base64FormattingOptions.None;
       private ICustomAlgorithm _customAlgorithm;
       [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;} = EncodingOptions.Default;
       [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;} =  AlgorithmOptions.SHA256;
       public override int GetHashCode() => ConvertToInt(_lastComputedHash);
       public string HashName {get; private set;} = string.Empty;
       public static HashUtility Create() => new HashUtility();
       public static HashUtility Create(ICustomAlgorithm customAlgorithm) => new HashUtility(customAlgorithm);
       public static HashUtility Create(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName) => new HashUtility(algorithmOption,encodingOption, hashName);
       public static HashUtility Create(string config, ICustomAlgorithm customAlgorithm = null) => new HashUtility(config,customAlgorithm);
       public Int32 ConvertToInt(int startIndex = 0) => ConvertToInt(_lastComputedHash,startIndex);
       public Int32 ConvertToInt(byte[] hash, int startIndex = 0) {
          var retVal = BitConverter.ToInt32(hash,startIndex);
          OnIntGenerated(retVal);
          return retVal;
       } 
       public string ConvertToString(int startIndex = 0) {
           var retVal = _useBase64EncodedString ? Convert.ToBase64String(_lastComputedHash,_base64FormattingOption) : BitConverter.ToString(_lastComputedHash);
           OnStringGenerated(retVal);
           return retVal;
       }   
       public string ConvertToString(int startIndex, bool createBase64String, Base64FormattingOptions base64FormattingOption = Base64FormattingOptions.None) {
          var retVal = createBase64String ? Convert.ToBase64String(_lastComputedHash,startIndex, _lastComputedHash.Length, base64FormattingOption)  :  BitConverter.ToString(_lastComputedHash);
          OnStringGenerated(retVal);
          return retVal;
       }  
       public HashUtility ComputeHash(object obj) => ComputeHash(getEncodingOption().GetBytes(SerializeObject(obj)));
       public HashUtility ComputeHash(byte[] bytes, int startIndex = 0, int length = -1) {
               _lastComputedHash = _customAlgorithm != null ? _customAlgorithm.ComputeHash(bytes, startIndex, length) : computeHash(bytes,startIndex,length);
               OnHashGenerated();
               return this;
        }
       public byte[] GetHash() => _lastComputedHash;
       private byte[] computeHash(Stream stream) { 
           byte[] hash = null;
           using (var algorithm = getAlgorithm())
           {
              hash = algorithm.ComputeHash(stream);               
           }
           return hash;
       }
       private byte[] computeHash(byte[] bytes, int offset, int count) { 
           byte[] hash = null;
           count = count < 0 ? bytes.Length : count;
           using (var algorithm = getAlgorithm())
           {
              hash = algorithm.ComputeHash(bytes,offset,count);               
           }
           return hash;
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
       public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }  
       public void Dispose(bool disposer)
        {
            if (disposer)
            {
                if (_customAlgorithm != null) {
                _customAlgorithm.Dispose();
                _customAlgorithm = null;
                }
            }
        }
       ~HashUtility()
        {
            Dispose(false);
        }
    }
}