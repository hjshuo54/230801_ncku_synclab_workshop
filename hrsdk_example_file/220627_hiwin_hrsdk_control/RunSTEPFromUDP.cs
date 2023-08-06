using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using SDKHrobot;

namespace _220627_hiwin_hrsdk_control
{
    class RunSTEPFromUDP
    {
        public static string message;
        static string lastmesg = "";
        static Socket server;
        static int Robot_ID;
        static int ProgramTool = 8;
        static int HomeTool = 7;


        unsafe static void Main(string[] args)
        {

            Robot_ID = HRobot.open_connection("192.168.1.203", 1, Test);
            HRobot.set_operation_mode(Robot_ID, 1);

            HRobot.set_override_ratio(Robot_ID, 30);
            HRobot.set_ptp_speed(Robot_ID, 30);
            HRobot.set_lin_speed(Robot_ID, 160);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.52"), 6001));
            Console.WriteLine("服務端已經開啟");
            Thread t = new Thread(ReciveMsg);
            t.Start();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    HRobot.motion_abort(Robot_ID);
                    HRobot.remove_command(Robot_ID, 0);
                    HRobot.disconnect(Robot_ID);
                    Console.WriteLine("Disconnect and exit"); return;

                }
            }
        }   

        static void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);
                message = Encoding.UTF8.GetString(buffer, 0, length);

                if (message == lastmesg)
                {
                    //...
                }
                else
                {
                    if (message.Contains("hrsdk!"))
                    {
                        Console.WriteLine(message);
                        string[] removehead = message.Split(new string[] { "hrsdk!" }, StringSplitOptions.RemoveEmptyEntries);
                        if (removehead[0].Contains("Tool:HomePos"))
                        {
                            HRobot.set_tool_number(Robot_ID, HomeTool);
                        }
                        else if (removehead[0].Contains("axis:"))
                        {
                            string[] axis_ = removehead[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            double[] axis_dou = axis_[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select((num) => Double.Parse(num)).ToArray();
                            HRobot.ptp_axis(Robot_ID, 1, axis_dou);
                        }
                        else if (removehead[0].Contains("ptp:"))
                        {
                            HRobot.set_tool_number(Robot_ID, ProgramTool);
                            string[] ptp_ = removehead[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            double[] ptp_dou = ptp_[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select((num) => Double.Parse(num)).ToArray();
                            HRobot.ptp_pos(Robot_ID, 1, ptp_dou);
                        }
                        else if (removehead[0].Contains("lin:"))
                        {
                            HRobot.set_tool_number(Robot_ID, ProgramTool);
                            string[] lin_ = removehead[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            double[] lin_dou = lin_[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select((num) => Double.Parse(num)).ToArray();
                            HRobot.lin_pos(Robot_ID, 1, 15, lin_dou);
                        }
                        else if (removehead[0].Contains("set_DO"))
                        {
                            string[] do_ = removehead[0].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] do_set = do_[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            HRobot.set_digital_output(Robot_ID, int.Parse(do_set[0]), bool.Parse(do_set[1]));
                        }
                        else if (removehead[0].Contains("WAIT SEC "))
                        {
                            string[] wait_ = removehead[0].Split(new string[] { "WAIT SEC " }, StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(wait_[0]);
                        }
                    }
                    lastmesg = message;
                }
            }
        }
        unsafe static void Test(ushort cmd, ushort rlt, char* msg, int len)
        {

        }
    }
}
