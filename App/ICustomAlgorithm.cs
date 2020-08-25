namespace App {
    public interface ICustomAlgorithm
    {
        byte[] ComputeHash(byte[] bytes);
    }
}