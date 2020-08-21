using System;
using System.Security.Cryptography;
using System.Text;
using static Newtonsoft.Json.JsonConvert;


namespace App
{
    public abstract class root : object
    {
        private int _hash;


        public root() {
            _hash = ComputeHash(this);
        }

        public override string ToString()
        {
            return SerializeObject(this);
        }

        public override bool Equals(object obj) => ComputeHash(obj) == GetHashCode();

        private int ComputeHash(object obj)
        {
            Int32 retVal = 0;

            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(SerializeObject(obj)));
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                retVal =  BitConverter.ToInt32(bytes, 0);
            };

            return retVal;    

        }

       public int Hash => _hash;

        public override int GetHashCode() => ComputeHash(this);


    }

}

