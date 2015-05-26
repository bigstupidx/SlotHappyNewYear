using UnityEngine;
using System.Collections;

using System;
using System.Net.Sockets;


public class LogServer : MonoBehaviour
{

    public string ServerIP;
    public string LogUserName;

    static public LogServer Instance;

    private TcpClient tcpClient;

    void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
            Connect();
        }
    }
    public void print(string str)
    {
        WriteToLServer(str + "\n");
    }

    void Connect()
    {
        try
        {
            //string IP = "192.168.152.205";
            int port = 13000;

            tcpClient = new TcpClient(ServerIP, port);

            if (tcpClient.Connected)
            {
                StartCoroutine(Read());

                WriteToLServer("Login Username:" + LogUserName);

            }
        }
        catch (Exception EX)
        {
            Debug.Log("Connect Exception : " + EX);
        }

    }

    void WriteToLServer(string message)
    {
     
        if (tcpClient.Connected)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = tcpClient.GetStream();
            
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
        }    
    }

    IEnumerator Read()
    {
        while (tcpClient.Connected)
        {
            byte[] data = new Byte[512];

            NetworkStream stream = tcpClient.GetStream();

            if (stream.DataAvailable)
            {
                Console.WriteLine("Real Read");
                stream.Read(data, 0, data.Length);

                //OperationReceive(data);
            }
            Console.WriteLine("Read");

            yield return new WaitForSeconds(1);

        }
        
        tcpClient.Close();

        Console.WriteLine("關閉聆聽");
    }

}