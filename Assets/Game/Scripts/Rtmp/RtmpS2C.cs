using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class RtmpS2C : MonoBehaviour
{

    public RtmpS2CReceiverBase IRtmpS2C;

    // Use this for initialization
    void Start()
    {
        string str = @"OnClose[{""OnClose"":true}]";

        OnServerMSG(str);
    }
        
    // 訊息分流
    public void OnServerMSG(string _result)
    {

        int offset = _result.IndexOf('[');

        string event_name = _result.Substring(0, offset);

        string event_msg = _result.Substring(offset);

        print("event_name " + event_name + "\nevent_msg " + event_msg);

        switch(event_name)
        {
            case "OnClose":
                break;

            case "OnConnect":

                JsonData jd_connect = JsonMapper.ToObject(event_msg);
                if (!((bool)jd_connect[0]["disconnected"]))
                    IRtmpS2C.OnConnect();
                break;
            case "onLogin":

                IRtmpS2C.OnLogin();
                break;
            case "onGetMachineList":

                IRtmpS2C.OnGetMachineList();
                break;
            case "onTakeMachine":

                IRtmpS2C.OnTakeMachine();
                break;
            case "onOnLoadInfo2":

                IRtmpS2C.OnonLoadInfo2(event_msg);
                break;
            case "onCreditExchange":

                break;

            default:

                print("Can't found this event . event_name " + event_name);
                break;

        }
    }

}
