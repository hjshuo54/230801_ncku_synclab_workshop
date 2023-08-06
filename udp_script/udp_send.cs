using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class udp_send : MonoBehaviour
{

    
    private IPEndPoint ipEndPoint;
    private UdpClient udpClient;
    private byte[] sendByte;

    public int port = 5555;
    public string IPlocation = "192.168.88.1";
    void Start()
    {
        //SendUDPData("Started");

    }

    void Update()
    { 
        
    }

    public void SendUDPData(string send_)
    {
        ipEndPoint = new IPEndPoint(IPAddress.Parse(IPlocation), port);
        udpClient = new UdpClient();
        
        sendByte = System.Text.Encoding.UTF8.GetBytes(send_);
        udpClient.Send(sendByte, sendByte.Length, ipEndPoint);


    }


}
