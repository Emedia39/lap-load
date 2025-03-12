using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lap_Load_Server
{
    internal class Program
    {
        static List<TcpClient> tcpClientList = new List<TcpClient>();
        static async Task Main(string[] args)
        {
            await StartListener();
        }
        static async Task StartListener()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 20001);
            System.Net.Sockets.TcpListener tcpListener = new TcpListener(localEndPoint);
            tcpListener.Start();
            Console.WriteLine("接続受付を開始");
            try
            {
                while (true)
                {
                    Console.WriteLine($"現在の接続数:{tcpClientList.Count}");
                    List<Socket> socketList = new List<Socket>();
                    Thread thread = new Thread(new ParameterizedThreadStart(DoWork));

                    foreach (TcpClient tcpClient in tcpClientList)
                    {
                        socketList.Add(tcpClient.Client);
                    }
                    socketList.Add(tcpListener.Server);

                    Socket.Select(socketList, null, null, -1);

                    foreach (Socket socket in socketList)
                    {
                        if (socket == tcpListener.Server)
                        {
                            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                            tcpClientList.Add(tcpClient);

                            string sendString = "OK";
                            byte[] buffer = new byte[1024];
                            buffer = Encoding.UTF8.GetBytes(sendString);
                            NetworkStream stream = tcpClient.GetStream();
                            await stream.WriteAsync(buffer, 0, buffer.Length);
                            Console.WriteLine("Clientが接続しました");
                        }
                        else
                        {
                            thread.Start(socket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        static void DoWork(object param)
        {
            Socket socket = (Socket)param;
            byte[] buffer = new byte[1024];
            int length = socket.Receive(buffer, 1024, SocketFlags.None);
            string receiveString = Encoding.UTF8.GetString(buffer, 0, length);
            Console.WriteLine(receiveString);
        }
    }
}
