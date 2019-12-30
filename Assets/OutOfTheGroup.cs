using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class OutOfTheGroup : MonoBehaviour
{
    public Text groupName, myLogin;
    public async void left()
    {
        Debug.Log("button log clicked");
        using (var webSocket = new ClientWebSocket()) {
            await webSocket.ConnectAsync(new Uri("ws://185.188.183.51:13756"), CancellationToken.None);
            string query = "[outgrp]" + groupName + ";" + myLogin;
            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(query));
            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            string response = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            if (response.Contains("error"))
                Debug.Log(response);
            else
            {
                Debug.Log("Group lefted");
            }
        }
    }
}
