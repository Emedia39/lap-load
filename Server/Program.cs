using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace lap_Load_Server
{
    internal class Program
    {
        static List<TcpClient> tcpClientList = new List<TcpClient>();
        static int clientCount = 0;

        static async Task Main(string[] args)
        {
            await StartListener();
        }

        static async Task StartListener()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 20001);
            listener.Start();
            Console.WriteLine("サーバー開始");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clientCount++;
                tcpClientList.Add(client);

                string playerName = $"Player{clientCount}\n";
                byte[] playerNameBytes = Encoding.UTF8.GetBytes(playerName);
                await client.GetStream().WriteAsync(playerNameBytes, 0, playerNameBytes.Length);

                Console.WriteLine($"{playerName.Trim()} が接続しました");
                _ = HandleClientAsync(client);
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            StringBuilder sb = new StringBuilder();

            try
            {
                while (client.Connected)
                {
                    int length = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (length <= 0) break;

                    sb.Append(Encoding.UTF8.GetString(buffer, 0, length));

                    string fullText = sb.ToString();
                    int newLineIndex;
                    while ((newLineIndex = fullText.IndexOf('\n')) >= 0)
                    {
                        string message = fullText.Substring(0, newLineIndex).Trim();
                        fullText = fullText.Substring(newLineIndex + 1);

                        Console.WriteLine($"受信: {message}");
                        await BroadcastMessageAsync(message);

                        if (message == "__end")
                        {
                            Console.WriteLine("クライアントから切断要求");
                            break;
                        }
                    }

                    sb.Clear();
                    sb.Append(fullText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: {ex.Message}");
            }
            finally
            {
                tcpClientList.Remove(client);
                client.Close();
                clientCount--;
                Console.WriteLine("クライアント切断");
            }
        }

        static async Task BroadcastMessageAsync(string message)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message + "\n");

            foreach (var tcpClient in tcpClientList.ToArray())
            {
                if (tcpClient.Connected)
                {
                    try
                    {
                        await tcpClient.GetStream().WriteAsync(sendBuffer, 0, sendBuffer.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"送信エラー: {ex.Message}");
                    }
                }
            }
        }
    }
}
