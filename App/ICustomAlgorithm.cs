namespace App {
    public interface ICustomAlgorithm
    {
         byte[] ComputeHash(object obj); 
        byte[] ComputeHash(byte[] bytes, int startIndex , int length );
    }
}