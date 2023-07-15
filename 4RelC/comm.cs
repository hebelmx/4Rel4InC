using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace YourNamespace
{
    public class Comm:IComm
    {
        private const string I2CDeviceFilePath = "/dev/i2c-1";
        private const int I2CSlaveAddress = 0x0703;

        private FileStream i2cDeviceFile;

        public int I2CSetup(int addr)
        {
            string devicePath = I2CDeviceFilePath;
            try
            {
                i2cDeviceFile = new FileStream(devicePath, FileMode.Open);
                int slaveAddress = addr;
                i2cDeviceFile.IOControl(I2CSlaveAddress, BitConverter.GetBytes(slaveAddress), null);
                return 0;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Failed to open the I2C bus: " + ex.Message);
                return -1;
            }
        }

        public int I2CMem8Read(int dev, int add, byte[] buff, int size)
        {
            if (buff == null)
            {
                return -1;
            }

            if (size > 512) // Replace with the desired maximum block size
            {
                return -1;
            }

            try
            {
                byte[] writeBuffer = new byte[] { (byte)(add & 0xFF) };
                i2cDeviceFile.Write(writeBuffer, 0, 1);

                int bytesRead = 0;
                while (bytesRead < size)
                {
                    int readBytes = i2cDeviceFile.Read(buff, bytesRead, size - bytesRead);
                    if (readBytes <= 0)
                    {
                        return -1;
                    }
                    bytesRead += readBytes;
                }

                return 0; // OK
            }
            catch (IOException ex)
            {
                Console.WriteLine("I2C read error: " + ex.Message);
                return -1;
            }
        }

        public int I2CMem8Write(int dev, int add, byte[] buff, int size)
        {
            if (buff == null)
            {
                return -1;
            }

            if (size > 511) // Replace with the desired maximum block size minus 1
            {
                return -1;
            }

            try
            {
                byte[] writeBuffer = new byte[size + 1];
                writeBuffer[0] = (byte)(add & 0xFF);
                Array.Copy(buff, 0, writeBuffer, 1, size);

                i2cDeviceFile.Write(writeBuffer, 0, size + 1);

                return 0; // OK
            }
            catch (IOException ex)
            {
                Console.WriteLine("I2C write error: " + ex.Message);
                return -1;
            }
        }

        public void Dispose()
        {
            i2cDeviceFile?.Dispose();
        }
    }
}
