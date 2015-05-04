using UnityEngine;

using LitJson;

public class LoadingManager : RtmpS2CReceiverBase
{

    public UIProgressBar progressbar;

    private bool sw_loadingGame;

    private float progress_now;

    // Use this for initialization
    void Start () {

        progressbar.value = 0.0f;
        sw_loadingGame = false;

        progress_now = 0.0f;

        this.Connect();
    }
	
	// Update is called once per frame
	void Update () {
	    
        if(progressbar.value < progress_now)
        {
            progressbar.value += 0.2f * Time.deltaTime;

            if (progressbar.value >= 1.0f)
                Application.LoadLevel("Game_HEY");
        }
	}

    void Connect()
    {
        RtmpC2S.Connect();
    }

    public override void OnClose(string str)
    {
        print("Server disconnect !");
    }
    public override void OnConnect(string str)
    {
        progress_now = 0.20f;
        print("RtmpC2S.LoginBySid(5835);");
        //RtmpC2S.LoginBySid("5402");
        RtmpC2S.LoginBySid("5835"); // 喜福 5835
    }
    public override void OnLogin(string str)
    {
        progress_now = 0.40f;
        print("Login Success !");
    }
    public override void OnGetMachineList(string str)
    {
        progress_now = 0.60f;
        RtmpC2S.TakeMachine(null);
    }
    public override void OnTakeMachine(string str)
    {
        print("OnTakeMachine");
        progress_now = 0.80f;
        RtmpC2S.onLoadInfo2();
    }
    public override void OnonLoadInfo2(string str)
    {
        progress_now = 1.0f;

        JsonData jd = JsonMapper.ToObject(str);

        string str_balance      = (jd[0]["data"]["Balance"]).ToString();
        string str_dbase        = (jd[0]["data"]["DefaultBase"]).ToString();
        string str_loginname    = (jd[0]["data"]["LoginName"]).ToString();
        string str_base         = (jd[0]["data"]["Base"]).ToString();

        // 儲存資料
        Global_UserInfo.Gl_onloadInfo = new Global_UserInfo.onloadInfo(str_balance, str_dbase, str_loginname, str_base);
    }

    public override void onCreditExchange(string str)
    {
    }

    public override void onBalanceExchange(string str)
    {
    }

    public override void onBeginGame(string str)
    {
    }

    public override void onEndGame(string str)
    {
    }

    public override void updateJP(string str)
    {
    }

    public override void updateJPList(string str)
    {
    }

    public override void onHitJackpot(string str)
    {
    }

    public override void updateMarquee(string str)
    {
    }

    public override void onHitFree(string str)
    {
    }

    public override void onMachineLeave(string str)
    {
    }
}
