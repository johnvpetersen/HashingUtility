using System;

namespace App {
    public interface ICustomAlgorithm : IDisposable
    {
         byte[] ComputeHash(object obj); 
        byte[] ComputeHash(byte[] bytes, int startIndex , int length );
        new void Dispose();
    }
}