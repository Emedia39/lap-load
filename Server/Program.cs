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
        class Player
        {
            public TcpClient Client { get; set; }
            public int Id { get; set; }
            public bool IsAlive { get; set; } = true;
        }

        static List<Player> players = new List<Player>();
        static string backmessage;
        static int clientCount = 0;
        static int turnCount = 1;

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
                var player = new Player { Client = client, Id = clientCount };
                players.Add(player);

                string playerName = $"{player.Id}\n";
                byte[] playerNameBytes = Encoding.UTF8.GetBytes(playerName);
                await client.GetStream().WriteAsync(playerNameBytes, 0, playerNameBytes.Length);

                Console.WriteLine($"Player{player.Id} が接続しました");
                Console.WriteLine($"現在の接続数:{clientCount}");
                _ = HandleClientAsync(player);
            }
        }

        static async Task HandleClientAsync(Player player)
        {
            NetworkStream stream = player.Client.GetStream();
            byte[] buffer = new byte[1024];
            StringBuilder sb = new StringBuilder();

            try
            {
                while (player.Client.Connected)
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

                        if (message == "endturn")
                        {
                            NextTurn();
                            await BroadcastMessageAsync($"turn/{turnCount}");
                            backmessage = $"turn/{turnCount}";
                        }
                        else if (message == "getturn")
                        {
                            await BroadcastMessageAsync($"turn/{turnCount}");
                            backmessage = $"turn/{turnCount}";
                        }
                        else if (message == "dead")
                        {
                            player.IsAlive = false;
                            Console.WriteLine($"Player{player.Id} が死亡しました");
                            await BroadcastMessageAsync($"dead/{player.Id}");
                            await CheckForWinner();
                            backmessage = $"dead/{player.Id}";

                            // 死亡したらターンを次に送る
                            if (player.Id == turnCount)
                            {
                                NextTurn();
                                await BroadcastMessageAsync($"turn/{turnCount}");
                                backmessage = $"turn/{turnCount}";
                            }
                        }
                        else
                        {
                            await BroadcastMessageAsync(message);
                            backmessage = message;
                        }

                        if (message == "__end")
                        {
                            players.Remove(player);
                            player.Client.Close();
                            clientCount--;
                            if (clientCount == 0)
                            {
                                turnCount = 1;
                            }
                            Console.WriteLine($"現在の接続数:{clientCount}");
                            Console.WriteLine("クライアント切断");
                        break;
                        }
                        Console.WriteLine($"送信:{backmessage}");
                        Console.WriteLine("-------------------------------------");
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
                players.Remove(player);
                player.Client.Close();
                clientCount--;
                if (clientCount == 0)
                {
                    turnCount = 1;
                }
                Console.WriteLine($"現在の接続数:{clientCount}");
                Console.WriteLine("クライアント切断");
            }
        }

        static void NextTurn()
        {
            // 生存プレイヤー数確認
            CheckForWinner().Wait();

            int attempts = 0;
            do
            {
                turnCount++;
                if (turnCount > players.Count)
                    turnCount = 1;

                attempts++;

                if (attempts > players.Count)
                {
                    Console.WriteLine("全員死亡またはエラー: ターンを回せません");
                    break;
                }

            } while (players.Find(p => p.Id == turnCount && p.IsAlive) == null);
        }

        static async Task CheckForWinner()
        {
            var alivePlayers = players.FindAll(p => p.IsAlive);
            if (alivePlayers.Count == 1)
            {
                var winner = alivePlayers[0];
                Console.WriteLine($"Player{winner.Id} が勝利！");

                // 勝者に win 送信
                await SendMessageToPlayer(winner, "win");

                // その他プレイヤーに gameover 送信
                foreach (var p in players)
                {
                    if (p != winner && p.Client.Connected)
                    {
                        await SendMessageToPlayer(p, "gameover");
                    }
                }

                turnCount = 1;
            }
            else if (alivePlayers.Count == 0)
            {
                Console.WriteLine("全員死亡、勝者なし");
                await BroadcastMessageAsync("gameover");
                turnCount = 1;
            }
        }

        static async Task SendMessageToPlayer(Player player, string message)
        {
            try
            {
                if (player.Client.Connected)
                {
                    byte[] sendBuffer = Encoding.UTF8.GetBytes(message + "\n");
                    await player.Client.GetStream().WriteAsync(sendBuffer, 0, sendBuffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"送信エラー: {ex.Message}");
            }
        }

        static async Task BroadcastMessageAsync(string message)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message + "\n");

            foreach (var player in players.ToArray())
            {
                if (player.Client.Connected)
                {
                    try
                    {
                        await player.Client.GetStream().WriteAsync(sendBuffer, 0, sendBuffer.Length);
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