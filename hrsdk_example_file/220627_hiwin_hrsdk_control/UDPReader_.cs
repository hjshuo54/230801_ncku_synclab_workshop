using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace _220627_hiwin_hrsdk_control
{
    class UDPReader_
    {
        public static string message;
        //static string lastmesg = "";
        static Socket server;
        static void Main(string[] args)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(new IPEndPoint(IPAddress.Parse("192.168.1.107"), 6001));//繫結埠號和IP
            Console.WriteLine("服務端已經開啟");
            Thread t = new Thread(ReciveMsg);//開啟接收訊息執行緒
            t.Start();

        }

        static void ReciveMsg()
        {
            while (true)
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用來儲存傳送方的ip和埠號
                byte[] buffer = new byte[1024];
                int length = server.ReceiveFrom(buffer, ref point);//接收資料報
                message = Encoding.UTF8.GetString(buffer, 0, length);
                //Console.WriteLine(point.ToString());
                Console.WriteLine(message);



            }
        }

        /*static void sendMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            while (true)
            {
                string msg = Console.ReadLine();
                server.SendTo(Encoding.UTF8.GetBytes(msg), point);
            }
        }*/


    }
}
