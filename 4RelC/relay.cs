using System;
using System.IO;

public enum OutStateEnumType
{
    OFF = 0,
    ON = 1,
    STATE_COUNT
}

public class RelayClass
{
    private const int ERROR = -1;
    private const int OK = 0;
    private const int CHANNEL_NR_MIN = 1;
    private const int RELAY_CH_NO = 4;
    private const int I2C_MEM_RELAY_VAL = 2;

    public static int RelayChSet(int dev, byte channel, OutStateEnumType state)
    {
        byte[] buff = new byte[2];

        if ((channel < CHANNEL_NR_MIN) || (channel > RELAY_CH_NO))
        {
            Console.WriteLine("Invalid relay nr!");
            return ERROR;
        }

        if (I2CMem8Read(dev, I2C_MEM_RELAY_VAL, buff, 1) == FAIL)
        {
            return FAIL;
        }

        switch (state)
        {
            case OutStateEnumType.OFF:
                buff[0] &= ~(1 << (channel - 1));
                return I2CMem8Write(dev, I2C_MEM_RELAY_VAL, buff, 1);
            case OutStateEnumType.ON:
                buff[0] |= (1 << (channel - 1));
                return I2CMem8Write(dev, I2C_MEM_RELAY_VAL, buff, 1);
            default:
                Console.WriteLine("Invalid relay state!");
                return ERROR;
        }
    }

    public static int RelayChGet(int dev, byte channel, out OutStateEnumType state)
    {
        byte[] buff = new byte[2];
        state = OutStateEnumType.STATE_COUNT;

        if ((channel < CHANNEL_NR_MIN) || (channel > RELAY_CH_NO))
        {
            Console.WriteLine("Invalid relay nr!");
            return ERROR;
        }

        if (I2CMem8Read(dev, I2C_MEM_RELAY_VAL, buff, 1) == FAIL)
        {
            return ERROR;
        }

        if ((buff[0] & (1 << (channel - 1))) != 0)
        {
            state = OutStateEnumType.ON;
        }
        else
        {
            state = OutStateEnumType.OFF;
        }
        return OK;
    }

    public static int RelaySet(int dev, int val)
    {
        byte[] buff = new byte[2];
        buff[0] = (byte)(0x0F & val);
        return I2CMem8Write(dev, I2C_MEM_RELAY_VAL, buff, 1);
    }

    public static int RelayGet(int dev, out int val)
    {
        byte[] buff = new byte[2];
        val = 0;

        if (I2CMem8Read(dev, I2C_MEM_RELAY_VAL, buff, 1) == FAIL)
        {
            return ERROR;
        }

        val = buff[0];
        return OK;
    }

    private static int DoRelayWrite(int argc, string[] argv)
    {
        int pin = 0;
        OutStateEnumType state = OutStateEnumType.STATE_COUNT;
        int val = 0;
        int dev = 0;
        OutStateEnumType stateR = OutStateEnumType.STATE_COUNT;
        int valR = 0;
        int retry = 0;

        if (argc != 4 && argc != 5)
        {
            return ARG_CNT_ERR;
        }

        dev = DoBoardInit(int.Parse(argv[1]));
        if (dev <= 0)
        {
            return ERROR;
        }

        if (argc == 5)
        {
            pin = int.Parse(argv[3]);
            if (pin < CHANNEL_NR_MIN || pin > RELAY_CH_NO)
            {
                Console.WriteLine("Relay channel number value out of range");
                return ARG_RANGE_ERROR;
            }

            if (string.Equals(argv[4], "up", StringComparison.OrdinalIgnoreCase)
                || string.Equals(argv[4], "on", StringComparison.OrdinalIgnoreCase))
            {
                state = OutStateEnumType.ON;
            }
            else if (string.Equals(argv[4], "down", StringComparison.OrdinalIgnoreCase)
                || string.Equals(argv[4], "off", StringComparison.OrdinalIgnoreCase))
            {
                state = OutStateEnumType.OFF;
            }
            else
            {
                if (!int.TryParse(argv[4], out int stateValue) || stateValue >= (int)OutStateEnumType.STATE_COUNT || stateValue < 0)
                {
                    Console.WriteLine("Invalid relay state!");
                    return ARG_RANGE_ERROR;
                }
                state = (OutStateEnumType)stateValue;
            }

            retry = RETRY_TIMES;

            while (retry > 0 && stateR != state)
            {
                if (RelayChSet(dev, (byte)pin, state) != OK)
                {
                    Console.WriteLine("Fail to write relay");
                    return IO_ERROR;
                }
                if (RelayChGet(dev, (byte)pin, out stateR) != OK)
                {
                    Console.WriteLine("Fail to read relay");
                    return IO_ERROR;
                }
                retry--;
            }

            #ifdef DEBUG_I
            if (retry < RETRY_TIMES)
            {
                Console.WriteLine("retry {0} times", 3 - retry);
            }
            #endif

            if (retry == 0)
            {
                Console.WriteLine("Fail to write relay");
                return IO_ERROR;
            }
            return OK;
        }
        else
        {
            val = int.Parse(argv[3]);
            if (val < 0 || val > 0x0F)
            {
                Console.WriteLine("Invalid relays value");
                return ARG_RANGE_ERROR;
            }

            retry = RETRY_TIMES;
            valR = -1;
            while (retry > 0 && valR != val)
            {
                if (RelaySet(dev, val) != OK)
                {
                    Console.WriteLine("Fail to write relay!");
                    return IO_ERROR;
                }
                if (RelayGet(dev, out valR) != OK)
                {
                    Console.WriteLine("Fail to read relay!");
                    return IO_ERROR;
                }
            }
            if (retry == 0)
            {
                Console.WriteLine("Fail to write relay!");
                return IO_ERROR;
            }
            return OK;
        }
        return ARG_CNT_ERR;
    }

    private static int DoRelayRead(int argc, string[] argv)
    {
        int pin = 0;
        int val = 0;
        int dev = 0;
        OutStateEnumType state = OutStateEnumType.STATE_COUNT;

        dev = DoBoardInit(int.Parse(argv[1]));
        if (dev <= 0)
        {
            return ERROR;
        }

        if (argc == 4)
        {
            pin = int.Parse(argv[3]);
            if (pin < CHANNEL_NR_MIN || pin > RELAY_CH_NO)
            {
                Console.WriteLine("Relay channel number value out of range!");
                return ERROR;
            }

            if (RelayChGet(dev, (byte)pin, out state) != OK)
            {
                Console.WriteLine("Fail to read!");
                return IO_ERROR;
            }

            if (state != 0)
            {
                Console.WriteLine("1");
            }
            else
            {
                Console.WriteLine("0");
            }
            return OK;
        }
        else if (argc == 3)
        {
            if (RelayGet(dev, out val) != OK)
            {
                Console.WriteLine("Fail to read!");
                return IO_ERROR;
            }
            Console.WriteLine(val);
            return OK;
        }
        return ARG_CNT_ERR;
    }

    private static int DoRelayTest(int argc, string[] argv)
    {
        int dev = 0;
        int i = 0;
        int retry = 0;
        int trVal;
        int valR;
        int relayResult = 0;
        FileStream file = null;
        byte[] relayOrder = { 1, 2, 3, 4 };

        dev = DoBoardInit(int.Parse(argv[1]));
        if (dev <= 0)
        {
            return ERROR;
        }

        if (argc == 4)
        {
            file = new FileStream(argv[3], FileMode.Create);
            if (file == null)
            {
                Console.WriteLine("Fail to open result file");
                //return -1;
            }
        }

        // Relay test
        if (string.Equals(argv[2], "reltest", StringComparison.OrdinalIgnoreCase))
        {
            trVal = 0;
            Console.WriteLine("Are all relays and LEDs turning on and off in sequence?");
            Console.Write("Press 'y' for Yes or any key for No....");
            StartThread();
            while (relayResult == 0)
            {
                for (i = 0; i < RELAY_CH_NO; i++)
                {
                    relayResult = CheckThreadResult();
                    if (relayResult != 0)
                    {
                        break;
                    }
                    valR = 0;
                    trVal = (byte)(1 << (relayOrder[i] - 1));

                    retry = RETRY_TIMES;
                    while (retry > 0 && (valR & trVal) == 0)
                    {
                        if (RelayChSet(dev, relayOrder[i], OutStateEnumType.ON) != OK)
                        {
                            retry = 0;
                            break;
                        }

                        if (RelayGet(dev, out valR) != OK)
                        {
                            retry = 0;
                        }
                    }
                    if (retry == 0)
                    {
                        Console.WriteLine("Fail to write relay");
                        if (file != null)
                            file.Close();
                        return IO_ERROR;
                    }
                    BusyWait(150);
                }

                for (i = 0; i < RELAY_CH_NO; i++)
                {
                    relayResult = CheckThreadResult();
                    if (relayResult != 0)
                    {
                        break;
                    }
                    valR = 0xFF;
                    trVal = (byte)(1 << (relayOrder[i] - 1));
                    retry = RETRY_TIMES;
                    while (retry > 0 && (valR & trVal) != 0)
                    {
                        if (RelayChSet(dev, relayOrder[i], OutStateEnumType.OFF) != OK)
                        {
                            retry = 0;
                        }
                        if (RelayGet(dev, out valR) != OK)
                        {
                            retry = 0;
                        }
                    }
                    if (retry == 0)
                    {
                        Console.WriteLine("Fail to write relay!");
                        if (file != null)
                            file.Close();
                        return IO_ERROR;
                    }
                    BusyWait(150);
                }
            }
        }
        else
        {
            return ARG_CNT_ERR;
        }

        if (relayResult == YES)
        {
            if (file != null)
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine("Relay Test ............................ PASS");
                }
            }
            else
            {
                Console.WriteLine("Relay Test ............................ PASS");
            }
        }
        else
        {
            if (file != null)
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine("Relay Test ............................ FAIL!");
                }
            }
            else
            {
                Console.WriteLine("Relay Test ............................ FAIL!");
            }
        }
        if (file != null)
        {
            file.Close();
        }
        RelaySet(dev, 0);
        return OK;
    }
}
