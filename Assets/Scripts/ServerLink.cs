using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ServerLink : MonoBehaviour
{
    [SerializeField] ObjectSponer sponer;
    [SerializeField] Text playerText;
    [SerializeField] CardEffectLists effectLists;
    public string playerName;
    private static TcpClient tcpClient;

    static string[] SplitText(string input)
    {
        return input.Split('/');
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
        tcpClient = new TcpClient
        {
            SendTimeout = 500,
            ReceiveTimeout = 500
        };

        try
        {
            await tcpClient.ConnectAsync(ipaddress, port);
            Debug.Log("サーバーとの通信確立");

            NetworkStream stream = tcpClient.GetStream();
            byte[] recvBuffer = new byte[1024];
            int playerIdlength = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);
            string playerIdString = Encoding.UTF8.GetString(recvBuffer, 0, playerIdlength);
            playerText.text = playerIdString;
            playerName = playerIdString;

            while (tcpClient.Connected)
            {
                int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);
                if (length > 0)
                {
                    string receiveString = Encoding.UTF8.GetString(recvBuffer, 0, length);

                    string[] result = SplitText(receiveString);
                    if (result[0] == "drop")
                    {
                        sponer.DropObject(StringToFloat(result[2]), StringToFloat(result[3]), StringToInt(result[4]));
                    }
                    if (result[0] == "use" && result[2] != playerName)
                    {
                        Debug.Log($"CardID:{result[3]}");
                        effectLists.CardEffects(StringToInt(result[3]));
                    }
                    else
                    {
                        Debug.Log($"受信データ: {receiveString}");
                    }
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
            byte[] sendBuffer = Encoding.UTF8.GetBytes(text + "\n"); // 終端記号を追加
            await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await stream.FlushAsync(); // 送信バッファを即時フラッシュ
        }
        catch (Exception ex)
        {
            Debug.LogError($"送信エラー: {ex.Message}");
        }
    }

    private async void Start()
    {
        await StartClient("127.0.0.1", 20001);
    }
}
