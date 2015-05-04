using UnityEngine;
using System.Collections;

public abstract class RtmpS2CReceiverBase : MonoBehaviour
{

    public abstract void OnClose(string str);
    public abstract void OnConnect(string str);
    public abstract void OnLogin(string str);
    public abstract void OnGetMachineList(string str);
    public abstract void OnTakeMachine(string str);
    public abstract void OnonLoadInfo2(string str);
    public abstract void onCreditExchange(string str);
    public abstract void onBalanceExchange(string str);
    public abstract void onBeginGame(string str);
    public abstract void onEndGame(string str);
    public abstract void updateJP(string str);
    public abstract void updateJPList(string str);
    public abstract void onHitJackpot(string str);
    public abstract void updateMarquee(string str);
    public abstract void onHitFree(string str);
    public abstract void onMachineLeave(string str);
}
