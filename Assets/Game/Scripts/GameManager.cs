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

    /*
        0 : Normal
        1 : Autospin
        2 : FreeGame
    */
    public SM_State f_sm_state;

    public GameInfo(int pa1,int pa2, SM_State state)
    {
        score_own = pa1;
        score_betoneline = pa2;
        f_sm_state = state;
    }
}
public enum SM_State
{
    NORMAL = 0,
    AUTOSPIN = 1,
    FREEGAME = 2
}

public class GameManager : RtmpS2CReceiverBase , IExchange2GM , ISlotMachine2GM , IGUIManager2GM , IBetWheel2GM
{

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    public GUIManager guiManager;

    public SlotMachine slotmachine;
    
    public static GameManager Instance;

    GameInfo m_GameInfo;

    JsonData m_jd_onBegingame;
    JsonData m_jd_onEndGame;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        m_GameInfo = new GameInfo(0,0,0);

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

        // 設定玩家可用分數
        m_GameInfo.score_own = Convert.ToInt32(get_Int[0]);

        bool allowspin = false;
        if (m_GameInfo.score_betoneline > 0 && m_GameInfo.score_own > 0)
            allowspin = true;
        
        // 恢復某些按鈕
        guiManager.OnCreateExchange(betBase, allowspin);
    }

    public override void onBalanceExchange(string str)
    {
        m_GameInfo.score_own = 0;

        JsonData jd = JsonMapper.ToObject(str);
        string transcredit = (jd[0]["data"]["TransCredit"]).ToString();
        string amount = (jd[0]["data"]["Amount"]).ToString();
        string balance = (jd[0]["data"]["Balance"]).ToString();

        exchangePanel.OnBalanceExchange(transcredit , amount , balance); 
    }

    public override void onBeginGame(string str)
    {

        m_jd_onBegingame = JsonMapper.ToObject(str);
        
        if ((bool)m_jd_onBegingame[0]["event"])
        {
            JsonData jd_fg = m_jd_onBegingame[0]["data"]["FreeGame"];

            string WagersID = (m_jd_onBegingame[0]["data"]["WagersID"]).ToString();

            if (jd_fg.IsObject)
            {

            }

            // 剖析 Cards 欄位
            string cards = (m_jd_onBegingame[0]["data"]["Cards"]).ToString();
            string[] tileinfo = cards.Split(',', '-');

            string str_show = "";
            for(int i = 0; i < tileinfo.Length; i++)
            {
                int num = Convert.ToInt32(tileinfo[i]);
                tileinfo[i] = num.ToString("000");
                str_show += tileinfo[i] + " ";
            }
            print("[Debug] tileinfo " + str_show);

            // 將資料塞入拉霸機
            slotmachine.SetTileSpriteInfo(tileinfo);

            // 依拉霸機的狀態選擇下一個按鈕的種類
            if(m_GameInfo.f_sm_state == SM_State.AUTOSPIN)
            {
                slotmachine.OnClick_StartStop_Immediate();
                // 顯示停止自動轉的按鍵
                guiManager.AllowAutoStop();
            }
            else
            {
                slotmachine.OnClick_StartStop();
                
                // 顯示 停止鍵 
                guiManager.AllowStop();
            }

            // 清除上一筆資料緩存
            m_jd_onEndGame = null;
            RtmpC2S.EndGame(WagersID);
        }
        else
        {
            // 錯誤訊息
        }
    }

    public override void onEndGame(string str)
    {
        // 緩存資料
        m_jd_onEndGame = JsonMapper.ToObject(str);

        if(!(bool)m_jd_onEndGame[0]["event"])
        {
            // 錯誤資訊
        }

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
    
    void ISlotMachine2GM.OnClick_Spin(bool autospin)
    {

        int betscore = m_GameInfo.score_betoneline * 50;

        if (m_GameInfo.score_own >= betscore)
        {
            if (autospin)
                m_GameInfo.f_sm_state = SM_State.AUTOSPIN;

            guiManager.OnClick_Spin();

            // 可用分數足夠
            RtmpC2S.BeginGame("beginGame2", 50, m_GameInfo.score_betoneline);

            slotmachine.StartSpin();
            
        }
        else
        {
            // 可用分數不足，跳出通知訊息。
        }
    }
    
    void ISlotMachine2GM.OnClick_StopAutoSpin()
    {
        m_GameInfo.f_sm_state = SM_State.NORMAL;
    }

    void ISlotMachine2GM.OnClick_GetScore()
    {
        string score = (m_jd_onEndGame[0]["data"]["Credit"]).ToString();
        guiManager.OnClick_GetScore(score);
    }

    // slotmachine totally stop.
    void ISlotMachine2GM.OnStop()
    {
        if(m_GameInfo.f_sm_state == SM_State.FREEGAME)
        {

        }
        else if(m_GameInfo.f_sm_state == SM_State.AUTOSPIN)
        {
            if (m_jd_onBegingame[0]["data"]["Lines"].Count > 0)
            {
                // 執行等待得分流程
                guiManager.OnStop(m_GameInfo.f_sm_state,m_jd_onBegingame);
            }
            else
            {
                ((ISlotMachine2GM)this).OnClick_Spin(true);
            }
        }
        else
        {
            if(m_jd_onBegingame[0]["data"]["Lines"].Count > 0)
            {
                print("執行等待得分流程.");
                // 執行等待得分流程
                guiManager.OnStop(m_GameInfo.f_sm_state, m_jd_onBegingame[0]["data"]);
            }
            else
            {
                guiManager.AllowSpin();
            }
        }
    }    

    void IBetWheel2GM.UpdateBetValue(int betvalue)
    {
        m_GameInfo.score_betoneline = betvalue;

        guiManager.UpdateBetValue(betvalue);

        if (m_GameInfo.score_betoneline > 0 && m_GameInfo.score_own > 0)
            guiManager.AllowSpin();
    }
    
    void IGUIManager2GM.Finish_GetScore()
    {
        if (m_GameInfo.f_sm_state == SM_State.FREEGAME)
        {
        }
        else if (m_GameInfo.f_sm_state == SM_State.AUTOSPIN)
        {
            ((ISlotMachine2GM)this).OnClick_Spin(true);
        }
        else
        {
            guiManager.AllowSpin();
        }
    }
}
