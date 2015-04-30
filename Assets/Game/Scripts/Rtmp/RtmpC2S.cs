
public class RtmpC2S
{
    public static string sid = "";
    public static string ip = "";
    public static string userid = "";
    public static string hallid = "";
    public static string gamecode = "";

#if UNITY_IOS || UNITY_IPHONE

    static public void Connect()
    {
        UnityEngine.Debug.Log("ip is " + ip + " sid is " + sid);
        myPlugin2.myConnect2("rtmp://" + ip, "slotMachine");
        //myPlugin2.myConnect2("rtmp://103.240.216.205:1935", "slotMachine");
    }
	static public void LoginBySid(string gamecode)
	{

		myPlugin2.myCallFmsApiWith2P ("loginBySid",sid,gamecode);
	}
	static public void TakeMachine(string setno)
	{		

		myPlugin2.myCallFmsApiWith1P ("takeMachine",setno);
    }
	
	static public void onLoadInfo2()
	{	
        	
		myPlugin2.myCallFmsApiWith1P ("onLoadInfo2",null);	
	}
	
	static public void creditExchange(string rate,string score)
	{
        		
		myPlugin2.myCallFmsApiWith2P ("creditExchange",rate,score);
	}
	
	static public void BeginGame(int BetPerLine, int SelectLine)
	{
		myPlugin2.myCallFmsApiWith3P("beginGame2",sid,BetPerLine.ToString(),SelectLine.ToString());
	}
	
	static public void EndGame(string wagersID)
	{
		myPlugin2.myCallFmsApiWith2P("endGame",sid,wagersID);
	}
	
	static public void HitFree(string wagersID,int itemID)
	{
		myPlugin2.myCallFmsApiWith3P("hitFree",sid,wagersID,itemID.ToString());
	}
	
	static public void HitBonus(int itemID)
	{
	}
	
	static public void EndBonus()
	{
	}
	
	static public void BalanceExchange()
	{
		myPlugin2.myCallFmsApiWith1P ("balanceExchange",null);
	}
    
    // 2015/3/13 coming soon.
    static public void Close()
    {
    }

#endif
#if UNITY_ANDROID

    static public void Connect()
    {
        UnityEngine.Debug.Log("Connect");
        myPlugin3.CallAndroidRtmpDo(sid, "rtmp://" + ip + "/SlotMachine");
        //myPlugin3.CallAndroidRtmpDo(sid, "rtmp://103.240.216.205:1935/SlotMachine");
    }

    static public void LoginBySid(string gamecode)
    {
        myPlugin3.CallAndroidStatic4("loginBySid", sid, gamecode);
    }
    static public void TakeMachine(string setno)
	{
		myPlugin3.CallAndroidStatic4 ("takeMachine",0);
	}

    static public void onLoadInfo2()
	{
		myPlugin3.CallAndroidStatic4 ("onLoadInfo2","null");		
	}
	
	static public void creditExchange(string rate,string score)
	{
		myPlugin3.CallAndroidStatic4("creditExchange",rate,score);
	}

    static public void BeginGame(int BetPerLine, int SelectLine)
	{
        myPlugin3.CallAndroidStatic4 ("beginGame2",sid,BetPerLine.ToString(),SelectLine.ToString());
    }

    static public void EndGame(string wagersID)
	{
		myPlugin3.CallAndroidStatic4 ("endGame",sid,wagersID);
	}

    static public void HitFree(string wagersID,int itemID)
	{
		myPlugin3.CallAndroidStatic4 ("hitFree",sid,wagersID,itemID.ToString());
	}

    static public void HitBonus(int itemID)
	{
	}

    static public void EndBonus()
	{
	}

    static public void BalanceExchange()
	{
		myPlugin3.CallAndroidStatic4 ("balanceExchange","null");
	}

    static public void MachineLeave()
    {
        UnityEngine.Debug.Log("machineLeave " + userid + " " + hallid + " " + gamecode);
        myPlugin3.CallAndroidStatic4("machineLeave", userid , hallid, gamecode);
    }

    // end the plugin loop.
    static public void Close()
    {
        UnityEngine.Debug.Log("RtmpC2S close");
        myPlugin3.CallAndroidStatic4("close", "null");
    }

#endif
}
