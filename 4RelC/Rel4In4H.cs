using System;

namespace YourNamespace
{
    public static class Rel4in4
    {
        public const int CHANNEL_NR_MIN = 1;
        public const int RELAY_CH_NO = 4;
        public const int LED_CH_NO = 4;
        public const int IN_CH_NO = 4;
        public const int COUNT_SIZE = 4;
        public const int ENC_COUNT_SIZE = 4;
        public const int ENC_NO = 2;
        public const int SCAN_FREQ_SIZE = 2;
        public const int PWM_IN_FILL_SIZE = 2;
        public const float PWM_IN_FILL_SCALE = 100f;
        public const int IN_FREQENCY_SIZE = 2;
        public const int RETRY_TIMES = 10;

        public enum I2CMemory : int
        {
            I2C_MEM_RELAY_VAL = 0,
            I2C_MEM_RELAY_SET,
            I2C_MEM_RELAY_CLR,
            I2C_MEM_DIG_IN,
            I2C_MEM_AC_IN,
            I2C_MEM_LED_VAL,
            I2C_MEM_LED_SET,
            I2C_MEM_LED_CLR,
            I2C_MEM_LED_MODE, // 0-auto, 1 - manual;
            I2C_MEM_EDGE_ENABLE,
            I2C_MEM_ENC_ENABLE,
            I2C_MEM_SCAN_FREQ,
            I2C_MEM_PULSE_COUNT_START = I2C_MEM_SCAN_FREQ + SCAN_FREQ_SIZE,
            I2C_MEM_PPS = I2C_MEM_PULSE_COUNT_START + (IN_CH_NO * COUNT_SIZE),
            I2C_MEM_ENC_COUNT_START = I2C_MEM_PPS + IN_CH_NO * IN_FREQENCY_SIZE,
            I2C_MEM_PWM_IN_FILL = I2C_MEM_ENC_COUNT_START + (ENC_NO * ENC_COUNT_SIZE),
            I2C_MEM_IN_FREQENCY = I2C_MEM_PWM_IN_FILL + (IN_CH_NO * PWM_IN_FILL_SIZE),
            I2C_MEM_IN_FREQENCY_END = I2C_MEM_IN_FREQENCY + (IN_CH_NO * IN_FREQENCY_SIZE) - 1,
            I2C_MEM_PULSE_COUNT_RESET, // 2 bytes to be one modbus register
            I2C_MEM_ENC_COUNT_RESET = I2C_MEM_PULSE_COUNT_RESET + 2, // 2 bytes to be one modbus register
            I2C_MODBUS_SETINGS_ADD = I2C_MEM_ENC_COUNT_RESET + 2,
            I2C_NBS1,
            I2C_MBS2,
            I2C_MBS3,
            I2C_MODBUS_ID_OFFSET_ADD,
            I2C_MEM_EXTI_ENABLE,
            I2C_MEM_BUTTON, // bit0 - state, bit1 - latch

            I2C_MEM_REVISION_HW_MAJOR_ADD = 0x78,
            I2C_MEM_REVISION_HW_MINOR_ADD,
            I2C_MEM_REVISION_MAJOR_ADD,
            I2C_MEM_REVISION_MINOR_ADD,

            SLAVE_BUFF_SIZE
        }

        public const int ERROR = -1;
        public const int OK = 0;
        public const int FAIL = -1;
        public const int ARG_CNT_ERR = -2;
        public const int ARG_RANGE_ERROR = -3;
        public const int IO_ERROR = -4;

        public const int SLAVE_OWN_ADDRESS_BASE = 0x0e;

        public enum OutStateEnumType
        {
            OFF = 0,
            ON,
            STATE_COUNT
        }

        public struct CliCmdType
        {
            public string name;
            public int namePos;
            public Func<int, string[], int> pFunc;
            public string help;
            public string usage1;
            public string usage2;
            public string example;
        }

        public struct ModbusSetingsType
        {
            public uint mbBaud;
            public uint mbType;
            public uint mbParity;
            public uint mbStopB;
            public uint add;
        }

        public static int DoBoardInit(int stack)
        {
            // Implementation goes here
            throw new NotImplementedException();
        }

        // RS-485 CLI structures
        public static readonly CliCmdType CMD_RS485_READ;
        public static readonly CliCmdType CMD_RS485_WRITE;

        public static readonly CliCmdType CMD_RELAY_TEST;
        public static readonly CliCmdType CMD_RELAY_READ;
        public static readonly CliCmdType CMD_RELAY_WRITE;

        public static readonly CliCmdType CMD_IN_READ;
        public static readonly CliCmdType CMD_AC_IN_READ;
        public static readonly CliCmdType CMD_CFG_COUNT_READ;
        public static readonly CliCmdType CMD_CFG_COUNT_WRITE;
        public static readonly CliCmdType CMD_COUNT_READ;
        public static readonly CliCmdType CMD_COUNT_RESET;
        public static readonly CliCmdType CMD_COUNT_PPS_READ;
        public static readonly CliCmdType CMD_CFG_ENCODER_READ;
        public static readonly CliCmdType CMD_CFG_ENCODER_WRITE;
        public static readonly CliCmdType CMD_ENCODER_READ;
        public static readonly CliCmdType CMD_ENCODER_RESET;
        public static readonly CliCmdType CMD_PWM_READ;
        public static readonly CliCmdType CMD_IN_FREQ_READ;
        public static readonly CliCmdType[] gCmdArray;

        static Rel4in4()
        {
            // Initialize the static readonly fields
            CMD_RS485_READ = new CliCmdType();
            CMD_RS485_WRITE = new CliCmdType();

            CMD_RELAY_TEST = new CliCmdType();
            CMD_RELAY_READ = new CliCmdType();
            CMD_RELAY_WRITE = new CliCmdType();

            CMD_IN_READ = new CliCmdType();
            CMD_AC_IN_READ = new CliCmdType();
            CMD_CFG_COUNT_READ = new CliCmdType();
            CMD_CFG_COUNT_WRITE = new CliCmdType();
            CMD_COUNT_READ = new CliCmdType();
            CMD_COUNT_RESET = new CliCmdType();
            CMD_COUNT_PPS_READ = new CliCmdType();
            CMD_CFG_ENCODER_READ = new CliCmdType();
            CMD_CFG_ENCODER_WRITE = new CliCmdType();
            CMD_ENCODER_READ = new CliCmdType();
            CMD_ENCODER_RESET = new CliCmdType();
            CMD_PWM_READ = new CliCmdType();
            CMD_IN_FREQ_READ = new CliCmdType();
            gCmdArray = new CliCmdType[] { /* Add your CliCmdType instances here */ };
        }
    }
}
