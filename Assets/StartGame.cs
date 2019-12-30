using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class StartGame : MonoBehaviour
{
    WebSocketSharp.WebSocket ws = new WebSocketSharp.WebSocket("ws://185.188.183.51:13756");
    string data;
    public void getObjectsData(string checkNum)
    {
        Debug.Log("start getting data");
        ws.OnMessage += OnMessage;

        ws.Connect ();
        ws.Send (checkNum);
    }

    protected void OnMessage (object sender, MessageEventArgs e)
    {
        List<string[]> objectsData = JsonConvert.DeserializeObject<List<string[]>>(e.Data);
        string text = "";
        foreach (string[] obj in objectsData)
        {
            text += obj[0] + ";" + obj[1] + ";" + obj[2] + "\n";
        }
        Debug.Log(text);
    }
}