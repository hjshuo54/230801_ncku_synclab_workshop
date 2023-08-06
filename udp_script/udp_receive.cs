using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

public class udp_receive : MonoBehaviour
{
    public Socket server;
    public string receive_data;

    // Start is called before the first frame update
    void Start()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6001));
        Thread t = new Thread(ReciveMsg);
        t.Start();

    }

    // Update is called once per frame
    void Update()
    {



    }

    public void ReciveMsg()
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[1024];
            int length = server.ReceiveFrom(buffer, ref point);
            string message = Encoding.UTF8.GetString(buffer, 0, length);
            receive_data = message;
            print(receive_data);

           
        }

    }
}
