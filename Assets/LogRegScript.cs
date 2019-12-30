using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LogRegScript : MonoBehaviour
{
    public Text loginText, passText, logText;
    public async void checkLogInAsync()
    {
        Debug.Log("button log clicked");
        logText.text = "";
        using (var webSocket = new ClientWebSocket()) {
            await webSocket.ConnectAsync(new Uri("ws://185.188.183.51:13756"), CancellationToken.None);

            string query = "[log]" + loginText.text + "@" + passText.text;

            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(query));
            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            string response = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            Debug.Log(response);
            if (response.Contains("error"))
                logText.text = "Ошибка логина или пароля";
            if (response.Contains("check"))
            {
                GameObject.Find("TextMyLogin").GetComponent<Text>().text = loginText.text;
                Destroy(GameObject.Find("CanvasLogInForm"));
                new StartGame().getObjectsData(response);
            }
        }
    }
}
