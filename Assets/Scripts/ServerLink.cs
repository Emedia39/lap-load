using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ServerLink : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] ObjectSponer sponer;
    [SerializeField] Text playerText;
    [SerializeField] Text turnText;
    [SerializeField] CardEffectLists effectLists;
    [SerializeField] GameMane gameMane;
    public string playerName;
    bool isGetTurn = false;
    public bool isPlay = false;
    public bool isdead = false;
    public int playerID;
    public int playerCount = 0;
    private static TcpClient tcpClient;

    static string[] SplitText(string input)
    {
        return input.Trim().Split('/');
    }

    static float StringToFloat(string input)
    {
        if (float.TryParse(input, out float result))
        {
            return result;
        }
        Debug.LogWarning($"変換失敗: {input} を float に変換できません");
        return 0f;
    }

    static int StringToInt(string input)
    {
        if (int.TryParse(input, out int result))
        {
            return result;
        }
        Debug.LogWarning($"変換失敗: {input} を int に変換できません");
        return 0;
    }

    private async Task StartClient(string ipaddress, int port)
    {
        tcpClient = new TcpClient();

        try
        {
            await tcpClient.ConnectAsync(ipaddress, port);
            Debug.Log("サーバーとの通信確立");

            NetworkStream stream = tcpClient.GetStream();
            byte[] recvBuffer = new byte[1024];
            int playerIdlength = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);
            string playerIdString = Encoding.UTF8.GetString(recvBuffer, 0, playerIdlength).Trim();
            playerName = $"{playerIdString}";
            playerID = StringToInt(playerIdString);
            playerText.text = "Player" + playerName;
            StringBuilder sb = new StringBuilder();

            while (tcpClient.Connected)
            {
                if (!isGetTurn)
                {
                    Transmission("getturn");
                    isGetTurn = true;
                }
                int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);
                if (length > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(recvBuffer, 0, length));

                    string fullText = sb.ToString();
                    int newLineIndex;
                    while ((newLineIndex = fullText.IndexOf('\n')) >= 0)
                    {
                        string receiveString = fullText.Substring(0, newLineIndex).Trim();
                        fullText = fullText.Substring(newLineIndex + 1);

                        string[] result = SplitText(receiveString);
                        if (result[0] == "turn")
                        {
                            var moveManager = GameObject.FindObjectOfType<MoveObjectManager>();
                            moveManager.ResetEndTurnFlag();
                            gameMane.isUseCard = true;
                            if (result[1] == playerName)
                            {
                                isPlay = true;
                                if (!isdead)
                                {
                                    turnText.text = "あなたの番です";
                                }
                            }
                            else
                            {
                                isPlay = false;
                                if (!isdead)
                                {
                                    turnText.text = $"Player{result[1]}の番です";
                                }
                            }
                        }
                        else if (result[0] == "drop")
                        {
                            sponer.DropObject(
                                StringToFloat(result[2]),
                                StringToFloat(result[3]),
                                StringToInt(result[4]),
                                new Vector3(
                                    StringToFloat(result[5]),
                                    StringToFloat(result[5]),
                                    StringToFloat(result[5])
                                )
                            );
                        }
                        else if (result[0] == "use" && (result[2] == playerName || result[2] == "all"))
                        {
                            Debug.Log($"CardID:{result[3]}");
                            effectLists.CardEffects(StringToInt(result[3]),false);
                        }
                        else if (result[0] == "dead" && result[1] == playerName)
                        {
                            turnText.text = "あなたは敗北しました";
                            isdead = true;
                            gameOverText.SetActive(true);
                        }
                        else if (result[0] == "win")
                        {
                            turnText.text = "あなたが勝者です！";
                            gameOverText.SetActive(true);
                            Debug.Log("勝利メッセージを受信しました");
                        }
                        else if (result[0] == "playercount")
                        {
                            playerCount = StringToInt(result[1]);
                        }
                        else
                        {
                            Debug.Log($"受信データ: {receiveString}");
                        }
                        if (isdead)
                        {
                            turnText.text = "あなたは敗北しました";
                        }
                    }

                    sb.Clear();
                    sb.Append(fullText);
                }
                else
                {
                    Debug.LogWarning("通信が切断されました");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"通信エラー: {ex.Message}");
        }
    }

    public async void Transmission(string text)
    {
        if (tcpClient == null || !tcpClient.Connected)
        {
            Debug.LogWarning("サーバーに接続されていません");
            return;
        }

        try
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] sendBuffer = Encoding.UTF8.GetBytes(text + "\n");
            await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await stream.FlushAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"送信エラー: {ex.Message}");
        }
    }

    private async void Start()
    {
        await StartClient("20.222.249.157", 20001);
    }
}
