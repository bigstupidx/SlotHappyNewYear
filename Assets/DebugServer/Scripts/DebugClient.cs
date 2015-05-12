using UnityEngine;


using System;
using System.IO;
using System.Net.Sockets;

public class DebugClient : MonoBehaviour
{

    public string LoginName;

    TcpClient m_client;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;

    bool socketReady = false;

    void Start()
    {
        SetupSocket("192.168.152.205");
    }

    public void SetupSocket(String Host)
    {
        if (!string.IsNullOrEmpty(LoginName))
        {
            try
            {
                Int32 Port = 13000;
                m_client = new TcpClient(Host, Port);
                theStream = m_client.GetStream();
                theWriter = new StreamWriter(theStream);
                theReader = new StreamReader(theStream);
                socketReady = true;

                writeSocket("Login Username:" + LoginName);
            }
            catch (Exception e)
            {
                Debug.Log("Socket error:" + e);
            }
        }
        else
        {
            Debug.LogError("LoginName is Null.");
        }
    }

    public void writeSocket(string theLine)
    {
        if (!socketReady)
            return;

        String tmpString = theLine;
        theWriter.Write(tmpString);
        theWriter.Flush();
    }

    public String readSocket()
    {
        if (!socketReady)
            return "";

        if (theStream.DataAvailable)
            return theReader.ReadLine();

        return "";
    }

    public void closeSocket()
    {
        if (!socketReady)
            return;

        theWriter.Close();
        theReader.Close();
        m_client.Close();

        socketReady = false;
    }
    
}