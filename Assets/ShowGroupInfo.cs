﻿using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class ShowGroupInfo : MonoBehaviour
{
    public Text groupName;
    public async void join()
    {
        Debug.Log("button log clicked");
        using (var webSocket = new ClientWebSocket()) {
            await webSocket.ConnectAsync(new Uri("ws://185.188.183.51:13756"), CancellationToken.None);
            string query = "[shwgrp]" + groupName;
            var arraySegment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(query));
            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            string response = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            if (response.Contains("error"))
                Debug.Log(response);
            else
            {
                Debug.Log("Group info");
                List<string[]> objectsData = JsonConvert.DeserializeObject<List<string[]>>(response);
                string text = "";
                foreach (string[] obj in objectsData)
                {
                    text += obj[0] + ";" + obj[1] + ";" + obj[2] + "\n";
                }
                Debug.Log(text);
            }
        }
    }
}
