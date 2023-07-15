using System;
using System.Text;

public class FourRelaysFourInputs
{
    private const int ERROR = -1;
    private const int OK = 0;
    private const int SLAVE_OWN_ADDRESS_BASE = 0x20;
    private const int I2C_MEM_REVISION_MAJOR_ADD = 0;
    private const int ARG_CNT_ERR = -1;
    private const string warranty = @"		       Copyright (c) 2016-2022 Sequent Microsystems
                                                            	 
		This program is free software; you can redistribute it and/or modify
		it under the terms of the GNU Leser General Public License as published
		by the Free Software Foundation, either version 3 of the License, or
		(at your option) any later version.
                                    
		This program is distributed in the hope that it will be useful,
		but WITHOUT ANY WARRANTY; without even the implied warranty of
		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
		GNU Lesser General Public License for more details.
			
		You should have received a copy of the GNU Lesser General Public License
		along with this program. If not, see <http://www.gnu.org/licenses/>.";

    public static void Usage()
    {
        int i = 0;
        while (gCmdArray[i] != null)
        {
            if (gCmdArray[i].name != null)
            {
                if (gCmdArray[i].usage1.Length > 2)
                {
                    Console.Write(gCmdArray[i].usage1);
                }
                if (gCmdArray[i].usage2.Length > 2)
                {
                    Console.Write(gCmdArray[i].usage2);
                }
            }
            i++;
        }
        Console.WriteLine("Type 4rel4in -h <command> for more help");
    }

    public static int DoBoardInit(int stack)
    {
        int dev = 0;
        int add = 0;
        byte[] buff = new byte[8];

        if (stack < 0 || stack > 7)
        {
            Console.WriteLine("Invalid stack level [0..7]!");
            return ERROR;
        }

        add = SLAVE_OWN_ADDRESS_BASE + stack;
        dev = I2CSetup(add);
        if (dev == -1)
        {
            return ERROR;
        }
        if (ERROR == I2CMem8Read(dev, I2C_MEM_REVISION_MAJOR_ADD, buff, 1))
        {
            Console.WriteLine("Four Relays Four Inputs card did not detected!");
            return ERROR;
        }
        return dev;
    }

    public static int BoardCheck(int stack)
    {
        int dev = 0;
        int add = 0;
        byte[] buff = new byte[8];

        if (stack < 0 || stack > 7)
        {
            Console.WriteLine("Invalid stack level [0..7]!");
            return ERROR;
        }
        add = SLAVE_OWN_ADDRESS_BASE + stack;
        dev = I2CSetup(add);
        if (dev == -1)
        {
            return ERROR;
        }
        if (ERROR == I2CMem8Read(dev, I2C_MEM_REVISION_MAJOR_ADD, buff, 1))
        {
            return ERROR;
        }
        return OK;
    }

    private static int DoHelp(int argc, string[] argv)
    {
        int i = 0;
        if (argc == 3)
        {
            while (gCmdArray[i] != null)
            {
                if (gCmdArray[i].name != null)
                {
                    if (string.Equals(argv[2], gCmdArray[i].name, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Write($"{gCmdArray[i].help}{gCmdArray[i].usage1}{gCmdArray[i].usage2}{gCmdArray[i].example}");
                        break;
                    }
                }
                i++;
            }
            if (gCmdArray[i] == null)
            {
                Console.WriteLine($"Option \"{argv[2]}\" not found");
                i = 0;
                while (gCmdArray[i] != null)
                {
                    if (gCmdArray[i].name != null)
                    {
                        Console.Write(gCmdArray[i].help);
                        break;
                    }
                    i++;
                }
            }
        }
        else
        {
            i = 0;
            while (gCmdArray[i] != null)
            {
                if (gCmdArray[i].name != null)
                {
                    Console.Write(gCmdArray[i].help);
                }
                i++;
            }
        }
        return OK;
    }

    private static int DoVersion(int argc, string[] argv)
    {
        Console.WriteLine($"4rel4in v{VERSION_BASE}.{VERSION_MAJOR}.{VERSION_MINOR} " +
                          "Copyright (c) 2016 - 2022 Sequent Microsystems");
        Console.WriteLine("\nThis is free software with ABSOLUTELY NO WARRANTY.");
        Console.WriteLine("For details type: 4rel4in -warranty");
        return OK;
    }

    private static int DoWarranty(int argc, string[] argv)
    {
        Console.WriteLine(warranty);
        return OK;
    }

    private static int DoList(int argc, string[] argv)
    {
        int[] ids = new int[8];
        int cnt = 0;

        foreach (var cmd in argv)
        {
            Console.WriteLine(cmd);
        }

        for (int i = 0; i < 8; i++)
        {
            if (BoardCheck(i) == OK)
            {
                ids[cnt] = i;
                cnt++;
            }
        }
        Console.WriteLine($"{cnt} board(s) detected");
        if (cnt > 0)
        {
            Console.Write("Id:");
        }
        while (cnt > 0)
        {
            cnt--;
            Console.Write($" {ids[cnt]}");
        }
        Console.WriteLine();
        return OK;
    }

    private class CliCmdType
    {
        public string name;
        public int namePos;
        public Func<int, string[], int> pFunc;
        public string help;
        public string usage1;
        public string usage2;
        public string example;

        public CliCmdType(string name, int namePos, Func<int, string[], int> pFunc, string help, string usage1, string usage2, string example)
        {
            this.name = name;
            this.namePos = namePos;
            this.pFunc = pFunc;
            this.help = help;
            this.usage1 = usage1;
            this.usage2 = usage2;
            this.example = example;
        }
    }

    private static CliCmdType CMD_VERSION = new CliCmdType(
        "-v",
        1,
        DoVersion,
        "\t-v		Display the 4rel4in command version number\n",
        "\tUsage:		4rel4in -v\n",
        "",
        "\tExample:		4rel4in -v  Display the version number\n");

    private static CliCmdType CMD_HELP = new CliCmdType(
        "-h",
        1,
        DoHelp,
        "\t-h		Display the list of command options or one command option details\n",
        "\tUsage:		4rel4in -h    Display command options list\n",
        "\tUsage:		4rel4in -h <param>   Display help for <param> command option\n",
        "\tExample:		4rel4in -h rread    Display help for \"rread\" command option\n");

    private static CliCmdType CMD_WAR = new CliCmdType(
        "-warranty",
        1,
        DoWarranty,
        "\t-warranty	Display the warranty\n",
        "\tUsage:		4rel4in -warranty\n",
        "",
        "\tExample:		4rel4in -warranty  Display the warranty text\n");

    private static CliCmdType CMD_LIST = new CliCmdType(
        "-list",
        1,
        DoList,
        "\t-list:		List all 4rel4in boards connected\n\t\t\treturn the # of boards and stack level for every board\n",
        "\tUsage:		4rel4in -list\n",
        "",
        "\tExample:		4rel4in -list display: 1,0 \n");

    private static CliCmdType[] gCmdArray =
    {
        CMD_VERSION,
        CMD_HELP,
        CMD_WAR,
        CMD_LIST,
        null
    };

    private static int I2CSetup(int address)
    {
        // TODO: Implement I2CSetup method
        return -1;
    }

    private static int I2CMem8Read(int dev, int address, byte[] buff, int length)
    {
        // TODO: Implement I2CMem8Read method
        return -1;
    }

    private static bool Strcasecmp(string str1, string str2)
    {
        return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }

    public static int Main(string[] args)
    {
        int i = 0;
        int ret = OK;

        if (args.Length == 1)
        {
            Usage();
            return -1;
        }
        while (gCmdArray[i] != null)
        {
            if (gCmdArray[i].name != null && gCmdArray[i].namePos < args.Length)
            {
                if (Strcasecmp(args[gCmdArray[i].namePos], gCmdArray[i].name))
                {
                    ret = gCmdArray[i].pFunc(args.Length, args);
                    if (ret == ARG_CNT_ERR)
                    {
                        Console.WriteLine("Invalid parameters number!");
                        Console.Write(gCmdArray[i].usage1);
                        if (gCmdArray[i].usage2.Length > 2)
                        {
                            Console.Write(gCmdArray[i].usage2);
                        }
                    }
                    return ret;
                }
            }
            i++;
        }
        Console.WriteLine("Invalid command option");
        Usage();

        return -1;
    }
}
