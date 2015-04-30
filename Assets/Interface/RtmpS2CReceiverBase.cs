using UnityEngine;
using System.Collections;

public class RtmpS2CReceiverBase : MonoBehaviour
{

    public virtual void OnClose() { }
    public virtual void OnConnect() { }
    public virtual void OnLogin() { }
    public virtual void OnGetMachineList() { }
    public virtual void OnTakeMachine() { }
    public virtual void OnonLoadInfo2(string str) { }
    public virtual void onCreditExchange() { }
}
