using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Newtonsoft.Json.JsonConvert;
namespace App {
    public class HashUtilityConfig {

        public HashUtilityConfig(ICustomAlgorithm customAlgorithm) {
            CustomAlgorithm = customAlgorithm;
        }

        public HashUtilityConfig(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName = null ) {
            AlgorithmOption = algorithmOption;
            EncodingOption = encodingOption;
            HashName = hashName;
        }
        public ICustomAlgorithm CustomAlgorithm {get;private set;}
         [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;}
         [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;}
        public string HashName {get; private set;}
        public override string  ToString() => SerializeObject(this);  
    }
}