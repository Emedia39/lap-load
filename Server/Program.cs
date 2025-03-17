using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace lap_Load_Server
{
    internal class Program
    {
        static int clientCount = 0;
        static int playerTarn = 1;
        static string[] SplitText(string input)
        {
            return input.Split('/');
        }
        static async Task Main(string[] args)
        {
            List<TcpClient> tcpClientList = new List<TcpClient>();
            await StartListener(tcpClientList);
        }
        static async Task StartListener(List<TcpClient> tcpClientList)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 20001);
            TcpListener tcpListener = new TcpListener(localEndPoint);
            tcpListener.Start();
            Console.WriteLine("接続受付を開始");
            try
            {
                while (true)
                {
                    Console.WriteLine($"現在の接続数:{tcpClientList.Count}");
                    List<Socket> socketList = new List<Socket>();

                    foreach (TcpClient tcpClient in tcpClientList)
                    {
                        // 接続が生きているか確認
                        if (tcpClient.Connected && !(tcpClient.Client.Poll(0, SelectMode.SelectRead) && tcpClient.Client.Available == 0))
                        {
                            socketList.Add(tcpClient.Client);
                        }
                    }
                    socketList.Add(tcpListener.Server);

                    Socket.Select(socketList, null, null, -1);

                    foreach (Socket socket in socketList)
                    {
                        if (socket == tcpListener.Server)
                        {
                            // 新しいクライアントが接続
                            clientCount++;
                            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                            tcpClientList.Add(tcpClient);

                            string sendString = $"Player{clientCount}";
                            byte[] buffer = Encoding.UTF8.GetBytes(sendString);
                            NetworkStream stream = tcpClient.GetStream();
                            await stream.WriteAsync(buffer, 0, buffer.Length);
                            Console.WriteLine($"Client{clientCount}が接続しました");
                        }
                        else
                        {
                            try
                            {
                                byte[] buffer = new byte[1024];
                                int length = socket.Receive(buffer, 1024, SocketFlags.None);

                                if (length <= 0)
                                {
                                    // クライアントが切断された
                                    clientCount--;
                                    Console.WriteLine("クライアントが切断しました");
                                    TcpClient disconnectedClient = tcpClientList.FirstOrDefault(client => client.Client == socket);
                                    if (disconnectedClient != null)
                                    {
                                        tcpClientList.Remove(disconnectedClient);
                                        disconnectedClient.Close();
                                    }
                                }
                                else
                                {
                                    string receiveString = Encoding.UTF8.GetString(buffer, 0, length);
                                    string[] result = SplitText(receiveString);
                                    Console.WriteLine(receiveString);
                                    Console.WriteLine(result[1]);
                                    Console.WriteLine($"Player{playerTarn}");
                                    if (result[0].Length > 0 && result[1] == $"Player{playerTarn}" )
                                    {
                                        string sendString = receiveString;
                                        buffer = new byte[1024];
                                        buffer = Encoding.UTF8.GetBytes(sendString);

                                        foreach (TcpClient tcpClient in tcpClientList)
                                        {
                                            // 接続できているクライアントにだけ送信
                                            if (tcpClient.Connected)
                                            {
                                                NetworkStream stream = tcpClient.GetStream();
                                                await stream.WriteAsync(buffer, 0, buffer.Length);
                                            }
                                        }
                                        playerTarn++;
                                        if(playerTarn > tcpClientList.Count)
                                        {
                                            playerTarn = 1;
                                        }
                                    }
                                    if (receiveString == "__end")
                                    {
                                        // クライアントが終了要求を送信
                                        clientCount--;
                                        Console.WriteLine("クライアントからの切断要求を受けました");
                                        TcpClient disconnectedClient = tcpClientList.FirstOrDefault(client => client.Client == socket);
                                        if (disconnectedClient != null)
                                        {
                                            tcpClientList.Remove(disconnectedClient);
                                            disconnectedClient.Close();
                                        }
                                    }
                                }
                            }
                            catch (SocketException ex)
                            {
                                // クライアントが強制切断された場合
                                clientCount--;
                                Console.WriteLine($"クライアントが強制的に切断されました: {ex.Message}");
                                TcpClient disconnectedClient = tcpClientList.FirstOrDefault(client => client.Client == socket);
                                if (disconnectedClient != null)
                                {
                                    tcpClientList.Remove(disconnectedClient);
                                    disconnectedClient.Close();
                                }
                            }
                        }
                    }

                    // 接続が切れたクライアントをリストから削除
                    tcpClientList.RemoveAll(tcpClient => !tcpClient.Connected);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー発生: {ex}");
            }
        }
    }
}
