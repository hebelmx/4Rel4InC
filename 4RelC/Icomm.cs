using System;

namespace YourNamespace
{
    public interface IComm
    {
        int I2CSetup(int addr);
        int I2CMem8Read(int dev, int add, byte[] buff, int size);
        int I2CMem8Write(int dev, int add, byte[] buff, int size);
    }

    public class CommImplementation : IComm
    {
        public int I2CSetup(int addr)
        {
            // Implementation goes here
            throw new NotImplementedException();
        }

        public int I2CMem8Read(int dev, int add, byte[] buff, int size)
        {
            // Implementation goes here
            throw new NotImplementedException();
        }

        public int I2CMem8Write(int dev, int add, byte[] buff, int size)
        {
            // Implementation goes here
            throw new NotImplementedException();
        }
    }
}
