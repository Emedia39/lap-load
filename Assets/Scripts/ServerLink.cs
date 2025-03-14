using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class ServerLink : MonoBehaviour
{
    static TcpClient tcpClient;
    static byte[] sendBuffer;
    static async Task StartClient(string ipaddress, int port)
    {
        tcpClient = new TcpClient();
        tcpClient.SendTimeout = 1000;
        tcpClient.ReceiveTimeout = 1000;

        await tcpClient.ConnectAsync(ipaddress, port);
        Debug.Log("サーバーとの通信確立");
        try
        {
            NetworkStream stream = tcpClient.GetStream();
            while (true)
            {
                byte[] recvBuffer = new byte[1024];
                int length = await stream.ReadAsync(sendBuffer, 0, recvBuffer.Length);
                string receiveString = Encoding.UTF8.GetString(recvBuffer, 0, length);
                Debug.Log(receiveString);
            }
            //tcpClient.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public void Transmission(string text)
    {
        string sentString = text;
        NetworkStream stream = tcpClient.GetStream();
        sendBuffer = Encoding.UTF8.GetBytes(sentString);
        stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
    }
    private async void Start()
    {
        await StartClient("127.0.0.1", 20001);
    }
}
    

