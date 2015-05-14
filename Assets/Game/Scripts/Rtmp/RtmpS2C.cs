using UnityEngine;

using LitJson;

public class RtmpS2C : MonoBehaviour
{

    public RtmpS2CReceiverBase IRtmpS2C;
            
    // 訊息分流
    public void OnServerMSG(string _result)
    {

        int offset = _result.IndexOf('[');

        string event_name = _result.Substring(0, offset);

        string event_msg = _result.Substring(offset);
        
        LogServer.Instance.print("event_name " + event_name + "\nevent_msg " + event_msg);

        switch (event_name)
        {
            case "OnClose":
                IRtmpS2C.OnClose(event_msg);
                break;

            case "OnConnect":

                JsonData jd_connect = JsonMapper.ToObject(event_msg);
                if (!((bool)jd_connect[0]["disconnected"]))
                    IRtmpS2C.OnConnect(event_msg);
                break;
            case "onLogin":

                IRtmpS2C.OnLogin(event_msg);
                break;
            case "onGetMachineList":

                IRtmpS2C.OnGetMachineList(event_msg);
                break;
            case "onTakeMachine":

                IRtmpS2C.OnTakeMachine(event_msg);
                break;
            case "onOnLoadInfo2":

                IRtmpS2C.OnonLoadInfo2(event_msg);
                break;
            case "onCreditExchange":

                IRtmpS2C.onCreditExchange(event_msg);
                break;
            case "onBalanceExchange":

                IRtmpS2C.onBalanceExchange(event_msg);
                break;
            case "onBeginGame":

                IRtmpS2C.onBeginGame(event_msg);
                break;
            case "onEndGame":
                IRtmpS2C.onEndGame(event_msg);
                break;
            default:

                LogServer.Instance.print("Can't found this event . event_name " + event_name);
                break;

        }
    }

}
