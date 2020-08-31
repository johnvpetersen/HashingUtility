using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Newtonsoft.Json.JsonConvert;
namespace App {
    public class HashUtilityConfig {

        public HashUtilityConfig(ICustomAlgorithm customAlgorithm, bool locked = false ) {
            CustomAlgorithm = customAlgorithm;
            Locked = locked;
        }

        public HashUtilityConfig(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, bool locked = false, string hashName = null ) {
            AlgorithmOption = algorithmOption;
            EncodingOption = encodingOption;
            HashName = hashName;
            Locked = locked;
        }
        public ICustomAlgorithm CustomAlgorithm {get;private set;}
         [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;}
         [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;}
        public string HashName {get; private set;}
        public bool Locked {get; private set;}
        public override string  ToString() => SerializeObject(this);  
 
    }
}