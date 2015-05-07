using UnityEngine;


using LitJson;
using System.Collections;
using System;

public struct GameInfo
{
    //可用分數
    public int score_own;
    //每線下注分
    public int score_betoneline;
    
}

public class GameManager : RtmpS2CReceiverBase , IExchange2GM , ISlotMachine2GM , IGUIManager2GM
{

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    public GUIManager guiManager;

    public SlotMachine slotmachine;

    public static GameManager Instance;

    GameInfo m_GameInfo;

    JsonData m_jd_onBegingame;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        m_GameInfo = new GameInfo();

        Win_SystemMessage.alpha = 0.0f;

        Init();
    }	

    void Init()
    {
        // 初始化開分面板
        exchangePanel.Setup(Global_UserInfo.Gl_onloadInfo.balance,
            Global_UserInfo.Gl_onloadInfo.defaultBase,
            Global_UserInfo.Gl_onloadInfo.loginName,
            Global_UserInfo.Gl_onloadInfo.Base);
            
        RtmpS2C rtmps2c = GameObject.Find("ServerToClientObject").GetComponent<RtmpS2C>();
        rtmps2c.IRtmpS2C = this;
    }

    #region Rtmp Listener
    public override void OnClose(string str)
    {
        print("GameManager OnClose");
    }

    public override void OnConnect(string str)
    {
    }

    public override void OnLogin(string str)
    {
    }

    public override void OnGetMachineList(string str)
    {
    }

    public override void OnTakeMachine(string str)
    {
    }

    public override void OnonLoadInfo2(string str)
    {
        print("GameManager OnonLoadInfo2 !");
    }

    public override void onCreditExchange(string str)
    {
        JsonData jd = JsonMapper.ToObject(str);
        string balance = (jd[0]["data"]["Balance"]).ToString();
        string betBase = (jd[0]["data"]["BetBase"]).ToString();

        string credit = (jd[0]["data"]["Credit"]).ToString();

        string[] get_Int = credit.Split('.');

        exchangePanel.OnCreditExchange(balance, betBase, get_Int[0]);

        // 恢復某些按鈕
        guiManager.OnCreateExchange(betBase);
    }

    public override void onBalanceExchange(string str)
    {

        JsonData jd = JsonMapper.ToObject(str);
        string transcredit = (jd[0]["data"]["TransCredit"]).ToString();
        string amount = (jd[0]["data"]["Amount"]).ToString();
        string balance = (jd[0]["data"]["Balance"]).ToString();

        exchangePanel.OnBalanceExchange(transcredit , amount , balance); 
    }

    public override void onBeginGame(string str)
    {

        m_jd_onBegingame = JsonMapper.ToObject(str);
        
        if ((bool)m_jd_onBegingame["event"])
        {
            JsonData jd_fg = m_jd_onBegingame["data"]["FreeGame"];

            string WagersID = (m_jd_onBegingame["data"]["WagersID"]).ToString();

            if (jd_fg.IsObject)
            {

            }

            string cards = (m_jd_onBegingame["data"]["Cards"]).ToString();
            string[] temp_1 = cards.Split(',');
            string[] tileinfo = new string[15];

            int cnt = 0;
            for(int i = 0; i < temp_1.Length; i++)
            {
                string[] temp = temp_1[i].Split('-');

                for(int j = 0; j < temp.Length; j++)
                {
                    tileinfo[cnt++] = temp[j];
                }
            }

            slotmachine.SetTileSpriteInfo(tileinfo);
            slotmachine.OnClick_StartStop();

            // 顯示 停止鍵 可用
            //guiManager.

            RtmpC2S.EndGame(WagersID);
        }
        else
        {
            // 錯誤訊息
        }
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
    #endregion

    void IExchange2GM.CreateExchange(string ratio, int score)
    {
        // 關閉開分按鈕，直到開分結束恢復。
        guiManager.OnWaitCreateExchange();

        RtmpC2S.creditExchange(ratio, score.ToString());
    }

    void IExchange2GM.BalanceExchange(bool needclosegui)
    {
        if (needclosegui)
        {
            // 關閉開分按鈕
            guiManager.OnWaitCreateExchange();
        }

        RtmpC2S.BalanceExchange();
    }

    void IExchange2GM.CashoutQuit()
    {
        string domain = LoginManager.loginInfo.Domain;
        string accountname = LoginManager.loginInfo.AccountName;

        string url = "http://" + domain + "/app/WebService/view/display.php/Logout?username=" + accountname;

		RtmpC2S.Close ();
        StartCoroutine(DoLoginout(url));
    }

    // 登出 SID
    IEnumerator DoLoginout(string url)
    {
        using (WWW www = new WWW(url))
        {

            yield return www;

            if(!string.IsNullOrEmpty(www.error))
            {
                print(www.error);
            }
            else
            {
                print(www.text);

                Application.LoadLevel("Login");
            }
        }
    }
    

    void ISlotMachine2GM.OnClick_Spin()
    {
        guiManager.OnClick_Spin();

        int betscore = m_GameInfo.score_betoneline * 50;

        if (m_GameInfo.score_own >= betscore)
        {
            // 可用分數足夠
            RtmpC2S.BeginGame("beginGame2", 50, m_GameInfo.score_betoneline);

            slotmachine.StartSpin();

            //guiManager.
        }
        else
        {
            // 可用分數不足，跳出通知訊息。
        }
    }

    // slotmachine totally stop.
    void ISlotMachine2GM.OnStop()
    {

        // Disable 停止鍵
        //guiManager
        guiManager.OnStop();
    }

    void IGUIManager2GM.OnClick_Spin()
    {
        throw new NotImplementedException();
    }

    void IGUIManager2GM.OnSpin()
    {
        throw new NotImplementedException();
    }

    void IGUIManager2GM.OnStop()
    {
        throw new NotImplementedException();
    }
}
