using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using SDKHrobot;


namespace _220627_hiwin_hrsdk_control
{
    class TESTForConnect
    {
        static int Robot_ID;

        unsafe static void Main(string[] args)
        {

            Robot_ID = HRobot.open_connection("192.168.1.203", 1, Test);
            Console.WriteLine("Connect!");
            HRobot.set_operation_mode(Robot_ID, 1);
            
            HRobot.set_override_ratio(Robot_ID, 5);

            HRobot.disconnect(Robot_ID);
            Console.WriteLine("Disconnect");
            
        }

            unsafe static void Test(ushort cmd, ushort rlt, char* msg, int len)
        {
            Console.WriteLine(UDPReader_.message);
        }
    }
}
