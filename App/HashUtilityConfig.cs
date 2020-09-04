using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static Newtonsoft.Json.JsonConvert;
namespace App {
    public class HashUtilityConfig {

       


        private HashUtilityConfig() {}
        public HashUtilityConfig(ICustomAlgorithm customAlgorithm) {
            CustomAlgorithm = customAlgorithm;
        }

        public HashUtilityConfig(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName = null, Base64FormattingOptions base64FormattingOption = Base64FormattingOptions.None, bool useBase64Encoding = false ) {
           
           setOptions(algorithmOption,encodingOption,hashName,base64FormattingOption,useBase64Encoding);

        }

        private HashUtilityConfig setOptions(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName , Base64FormattingOptions base64FormattingOption, bool useBase64Encoding) {
            AlgorithmOption = algorithmOption;
            EncodingOption = encodingOption;
            HashName = hashName;
            Base64FormattingOption = base64FormattingOption;
            UseBase64Encoding = useBase64Encoding;

            return this;

        }

        public ICustomAlgorithm CustomAlgorithm {get;private set;}
        public bool UseBase64Encoding {get; private set;}
         [JsonConverter(typeof(StringEnumConverter))] public AlgorithmOptions AlgorithmOption {get; private set;}
         [JsonConverter(typeof(StringEnumConverter))] public Base64FormattingOptions Base64FormattingOption {get; private set;}
         [JsonConverter(typeof(StringEnumConverter))] public EncodingOptions EncodingOption {get; private set;}
        public string HashName {get; private set;}
        public override string  ToString() => SerializeObject(this);  

        private static HashUtilityConfig _staticInstance = new HashUtilityConfig();
        public static string GenerateConfigString(AlgorithmOptions algorithmOption, EncodingOptions encodingOption, string hashName = null, Base64FormattingOptions base64FormattingOption = Base64FormattingOptions.None, bool useBase64Encoding = false ) =>  _staticInstance.setOptions(algorithmOption,encodingOption,hashName,base64FormattingOption,useBase64Encoding).ToString();

    }
}