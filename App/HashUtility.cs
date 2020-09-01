using System.Text;
using System.Security.Cryptography;
using System.IO;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static Newtonsoft.Json.JsonConvert;
namespace App
{
    public  class HashUtility : IDisposable {
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
           HashName = configObject.GetValue("HashName").ToString();
       }
       public override string ToString() => SerializeObject(this);
       public bool CustomAlgorithm => _customAlgorithm != null;
       private byte[] _lastComputedHash = null;
       private ICustomAlgorithm _customAlgorithm;
       [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;} = EncodingOptions.Default;
       [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;} =  AlgorithmOptions.SHA256;
       public override int GetHashCode() => ConvertToInt(_lastComputedHash);
       public string HashName {get; private set;} = string.Empty;
       public static HashUtility Create() => new HashUtility();
       public static HashUtility Create(ICustomAlgorithm customAlgorithm) => new HashUtility(customAlgorithm);
       public static HashUtility Create(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName, bool isLocked) => new HashUtility(algorithmOption,encodingOption, hashName);
       public static HashUtility Create(string config, ICustomAlgorithm customAlgorithm = null) => new HashUtility(config,customAlgorithm);
       public Int32 ConvertToInt(int startIndex = 0) => ConvertToInt(_lastComputedHash,startIndex);
       public Int32 ConvertToInt(byte[] hash, int startIndex = 0) => BitConverter.ToInt32(hash,startIndex);
       public string ConvertToString(int startIndex = 0) => BitConverter.ToString(_lastComputedHash);
       public HashUtility ComputeHash(object obj) => ComputeHash(getEncodingOption().GetBytes(SerializeObject(obj)));
       public HashUtility ComputeHash(byte[] bytes, int startIndex = 0, int length = -1) {
               _lastComputedHash = _customAlgorithm != null ? _customAlgorithm.ComputeHash(bytes, startIndex, length) : computeHash(bytes,startIndex,length);
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
       private byte[] computeHash(byte[] bytes) {
           byte[] hash = null;
           using (var algorithm = getAlgorithm())
           {
              hash = algorithm.ComputeHash(bytes);               
           }
           return hash;
       } 
       public static string GenerateStringFromHash(HashUtilityConfig config, byte[] bytes, ICustomAlgorithm customAlgorithm = null)  => GenerateStringFromHash(config.ToString(),bytes,customAlgorithm);
       public static string GenerateStringFromHash(string config, byte[] bytes, ICustomAlgorithm customAlgorithm = null) {
           var retVal = string.Empty;

           using (var hashUtility = HashUtility.Create(config, customAlgorithm))
           {
              retVal  = hashUtility.ComputeHash(bytes).ConvertToString();
           }
           return retVal;
       }
       public static int GenerateIntFromHash(HashUtilityConfig config, byte[] bytes, ICustomAlgorithm customAlgorithm = null)  => GenerateIntFromHash(config.ToString(),bytes,customAlgorithm);
       public static int GenerateIntFromHash(string config, byte[] bytes, ICustomAlgorithm customAlgorithm = null) {
           Int32 retVal = 0;
           using (var hashUtility = HashUtility.Create(config, customAlgorithm))
           {
               retVal = hashUtility.ComputeHash(bytes).ConvertToInt();
           }

           return retVal;
       }
       public static int GenerateIntFromObject(HashUtilityConfig config, object obj, ICustomAlgorithm customAlgorithm = null) => GenerateIntFromObject(config.ToString(),obj,customAlgorithm);
       public static int GenerateIntFromObject(string config, object obj, ICustomAlgorithm customAlgorithm = null) {
           Int32 retVal = 0;

           using (var hashUtility = HashUtility.Create(config, customAlgorithm))
           {
               var bytes = hashUtility.getEncodingOption().GetBytes(SerializeObject(obj));
               retVal = hashUtility.ComputeHash(bytes).ConvertToInt();
           }
           return retVal;
       }
       public static string GenerateStringFromObject(HashUtilityConfig config, object obj, ICustomAlgorithm customAlgorithm = null) => GenerateStringFromObject(config.ToString(),obj,customAlgorithm);
       public static string GenerateStringFromObject(string config, object obj, ICustomAlgorithm customAlgorithm = null) { 
          var retVal = string.Empty;
           using (var hashUtility = HashUtility.Create(config, customAlgorithm))
           {
               var bytes = hashUtility.getEncodingOption().GetBytes(SerializeObject(obj));
               retVal = hashUtility.ComputeHash(bytes).ConvertToString();
           }
           return retVal;
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