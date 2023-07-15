using System;

public enum OutStateEnumType
{
    OFF = 0,
    ON = 1,
    STATE_COUNT
}

public class InputClass
{
    private const int ERROR = -1;
    private const int OK = 0;
    private const int CHANNEL_NR_MIN = 1;
    private const int IN_CH_NO = 4;
    private const int RELAY_CH_NO = 4;
    private const int ENC_NO = 2;
    private const int COUNT_SIZE = 4;
    private const int I2C_MEM_DIG_IN = 0;
    private const int I2C_MEM_AC_IN = 1;
    private const int I2C_MEM_EDGE_ENABLE = 0;
    private const int I2C_MEM_ENC_ENABLE = 1;
    private const int I2C_MEM_PULSE_COUNT_START = 2;
    private const int I2C_MEM_PULSE_COUNT_RESET = 6;
    private const int I2C_MEM_ENC_COUNT_START = 10;
    private const int I2C_MEM_ENC_COUNT_RESET = 14;
    private const int I2C_MEM_PPS = 18;
    private const int IN_FREQENCY_SIZE = 2;
    private const int PWM_IN_FILL_SIZE = 2;
    private const int I2C_MEM_PWM_IN_FILL = 20;
    private const int I2C_MEM_IN_FREQENCY = 28;
    private const int PWM_IN_FILL_SCALE = 10000;

    public static int InChGet(int dev, byte channel, out OutStateEnumType state)
    {
        byte[] buff = new byte[2];
        state = OutStateEnumType.STATE_COUNT;

        if ((channel < CHANNEL_NR_MIN) || (channel > IN_CH_NO))
        {
            Console.WriteLine("Invalid input nr!");
            return ERROR;
        }

        if (I2CMem8Read(dev, I2C_MEM_DIG_IN, buff, 1) == FAIL)
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

    public static int InGet(int dev, out int val)
    {
        byte[] buff = new byte[2];
        val = 0;

        if (I2CMem8Read(dev, I2C_MEM_DIG_IN, buff, 1) == FAIL)
        {
            return ERROR;
        }
        val = buff[0];
        return OK;
    }

    public static int AcInChGet(int dev, byte channel, out OutStateEnumType state)
    {
        byte[] buff = new byte[2];
        state = OutStateEnumType.STATE_COUNT;

        if ((channel < CHANNEL_NR_MIN) || (channel > IN_CH_NO))
        {
            Console.WriteLine("Invalid input nr!");
            return ERROR;
        }

        if (I2CMem8Read(dev, I2C_MEM_AC_IN, buff, 1) == FAIL)
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

    public static int AcInGet(int dev, out int val)
    {
        byte[] buff = new byte[2];
        val = 0;

        if (I2CMem8Read(dev, I2C_MEM_AC_IN, buff, 1) == FAIL)
        {
            return ERROR;
        }
        val = buff[0];
        return OK;
    }

    private static int DoInRead(int argc, string[] argv)
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
            if ((pin < CHANNEL_NR_MIN) || (pin > RELAY_CH_NO))
            {
                Console.WriteLine("Input channel number value out of range!");
                return ERROR;
            }

            if (InChGet(dev, (byte)pin, out state) != OK)
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
            if (InGet(dev, out val) != OK)
            {
                Console.WriteLine("Fail to read!");
                return IO_ERROR;
            }
            Console.WriteLine(val);
            return OK;
        }
        return ARG_CNT_ERR;
    }

    private static int DoAcInRead(int argc, string[] argv)
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
            if ((pin < CHANNEL_NR_MIN) || (pin > RELAY_CH_NO))
            {
                Console.WriteLine("Input channel number value out of range!");
                return ERROR;
            }

            if (AcInChGet(dev, (byte)pin, out state) != OK)
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
            if (AcInGet(dev, out val) != OK)
            {
                Console.WriteLine("Fail to read!");
                return IO_ERROR;
            }
            Console.WriteLine(val);
            return OK;
        }
        return ARG_CNT_ERR;
    }

    private static int CfgCntChGet(int dev, byte channel, out OutStateEnumType state// ============================================================

static int doPrintHelp(int argc, char *argv[]);
const CliCmdType CMD_PRINT_HELP =
{
	"help",
	1,
	&doPrintHelp,
	"help:			Print a list of available commands\n",
	"",
	"",
	"",
	""};

static int doPrintHelp(int argc, char *argv[])
{
	int i = 0;

	for (i = 0; i < 21; i++)
	{
		printf("%s\n", commands[i].shortDescr);
	}

	return OK;
}

const CliCmdType commands[21] = {CMD_PRINT_HELP,
								CMD_IN_READ,
								CMD_AC_IN_READ,
								CMD_CFG_COUNT_READ,
								CMD_CFG_COUNT_WRITE,
								CMD_COUNT_READ,
								CMD_COUNT_RESET,
								CMD_COUNT_PPS_READ,
								CMD_CFG_ENCODER_READ,
								CMD_CFG_ENCODER_WRITE,
								CMD_ENCODER_READ,
								CMD_ENCODER_RESET,
								CMD_PWM_READ,
								CMD_IN_FREQ_READ};

int main(int argc, char *argv[])
{
	int i = 0;

	if (argc < 3)
	{
		printf("Invalid argument number!\n");
		return 1;
	}

	for (i = 0; i < 21; i++)
	{
		if (strcmp(argv[2], commands[i].name) == 0)
		{
			commands[i].handler(argc, argv);
			return 0;
		}
	}

	printf("Invalid command!\n");
	return 1;
}

