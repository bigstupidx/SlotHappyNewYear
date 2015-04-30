using UnityEngine;
using System.Collections;
using System;

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

    public override void OnClose()
    {
        print("Server disconnect !");
    }
    public override void OnConnect()
    {
        progress_now = 0.20f;
        RtmpC2S.LoginBySid("5402");
    }
    public override void OnLogin()
    {
        progress_now = 0.40f;
        print("Login Success !");
    }
    public override void OnGetMachineList()
    {
        progress_now = 0.60f;
        RtmpC2S.TakeMachine(null);
    }
    public override void OnTakeMachine()
    {
        print("OnTakeMachine");
        progress_now = 0.80f;
        RtmpC2S.onLoadInfo2();
    }
    public override void OnonLoadInfo2(string str)
    {
        print("OnonLoadInfo2 " + str);
        progress_now = 1.0f;

        JsonData jd = JsonMapper.ToObject(str);

        string str_balance      = (jd[0]["data"]["Balance"]).ToString();
        string str_dbase        = (jd[0]["data"]["DefaultBase"]).ToString();
        string str_loginname    = (jd[0]["data"]["LoginName"]).ToString();
        string str_base         = (jd[0]["data"]["Base"]).ToString();

        // 儲存資料
        Global_UserInfo.Gl_onloadInfo = new Global_UserInfo.onloadInfo(str_balance, str_dbase, str_loginname, str_base);
    }
}
