﻿using System;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;
namespace App
{
    public class Root<T> : object
    {
        private readonly Int32 _hash;
        private readonly string _hashConfig = HashUtilityConfig.GenerateConfigString(AlgorithmOptions.SHA256,EncodingOptions.UTF8);
        private readonly T _data;
        [JsonConstructor]
        public Root(T data, Int32 hash)
        {
            _data = data;
            _hash = hash;
        }
        public Root(T data)
        {
            _data = data;
            _hash = computeHash(_data);
        }
        public override string ToString()
        {
            return SerializeObject(new { Data = _data, Hash = _hash });
        }
        public override bool Equals(object obj) => obj.GetHashCode() == _hash;
        private int computeHash(object obj) {

         Int32 retVal = 0; 
         using (var hashUtility = HashUtility.Create(_hashConfig))
         {
             retVal = hashUtility.ComputeHash(_data).ConvertToInt();
         }
             return retVal; 
        } 
        public bool HasChanged() => computeHash(_data) != _hash;
        public override int GetHashCode() => computeHash(_data);
        public static Root<T> Create(T data)
        {
            return new Root<T>(data);
        }
        public static Root<T> Create(T data, Int32 hash)
        {
            return new Root<T>(data, hash);
        }
        public T Data => _data;
        public Int32 Hash => _hash;
    }
}