using UnityEngine;

using System;
using System.Collections;
using LitJson;

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

public struct ButtonAllowTable
{
    public bool JackPot;
    public bool Exchange;
    public bool Settings;
    public bool Maxbet;
    public bool Dollar;

    public ButtonAllowTable(bool a,bool b,bool c,bool d,bool e)
    {
        JackPot = a;
        Exchange = b;
        Settings = c;
        Maxbet = d;
        Dollar = e;
    }
}

public enum SM_State
{
    NORMAL = 0,
    AUTOSPIN = 1,
    FREEGAME = 2
}

public class GameManager : RtmpS2CReceiverBase , IExchange2GM , ISlotMachine2GM , IGUIManager2GM , IBetWheel2GM ,ISetting2GM
{

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    public GUIManager guiManager;

    public SlotMachine slotmachine;
    
    public static GameManager Instance;

    GameInfo m_GameInfo;
    ButtonAllowTable m_but_allowInfo;

    JsonData m_jd_onBegingame;
    JsonData m_jd_onEndGame;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        m_GameInfo = new GameInfo(0,0,0);

        m_but_allowInfo = new ButtonAllowTable(false, false, false, false, false);

        m_but_allowInfo.Maxbet = true;
        m_but_allowInfo.Exchange = true;
        m_but_allowInfo.Settings = true;
        m_but_allowInfo.Dollar = true;

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
        
        // 恢復某些按鈕，設定Display可用分數
        guiManager.OnCreateExchange(betBase, allowspin, get_Int[0]);
    }

    public override void onBalanceExchange(string str)
    {
        m_GameInfo.score_own = 0;
        guiManager.OnBalanceExchange();

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
            LogServer.Instance.print("[Debug] tileinfo " + str_show);

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
    
    bool IExchange2GM.OpenAllow()
    {
        if (m_but_allowInfo.Exchange)
            return true;
        else
        {
            //guiManager.
            return false;
        }
    }

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
                LogServer.Instance.print(www.error);
            }
            else
            {
                LogServer.Instance.print(www.text);

                Application.LoadLevel("Login");
            }
        }
    }
    
    void ISlotMachine2GM.OnClick_Spin(bool autospin)
    {
        // 關閉不允許Spin開啟的按鍵
        m_but_allowInfo.Maxbet = false;
        m_but_allowInfo.Exchange = false;
        m_but_allowInfo.Dollar = false;

        
        int betscore = m_GameInfo.score_betoneline * 50;


        LogServer.Instance.print("betscore " + betscore);
        if (m_GameInfo.score_own >= betscore)
        {
            if (autospin)
                m_GameInfo.f_sm_state = SM_State.AUTOSPIN;

            LogServer.Instance.print("1 score_own " + m_GameInfo.score_own);
            m_GameInfo.score_own -= betscore;

            LogServer.Instance.print("2 score_own " + m_GameInfo.score_own);

            guiManager.OnClick_Spin(m_GameInfo.score_own);

            // 可用分數足夠
            RtmpC2S.BeginGame("beginGame2", 50, m_GameInfo.score_betoneline);

            slotmachine.StartSpin();
            
        }
        else
        {
            // 可用分數不足，跳出通知訊息。
            string title = "";
            string context = "";
            string language_id = Localization.language;
            if (language_id == "TW")
            {
                context = tw_ErrorMsg[0];
            }
            else if (language_id == "CN")
            {
                context = cn_ErrorMsg[0];
            }
            else
            {
                context = en_ErrorMsg[0];
            }

            guiManager.ShowWindowMsg(context);
            guiManager.AllowSpin();
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

        int num_score = 0;
        string[] values = score.Split('.');
        if(values.Length == 2)
            num_score = Convert.ToInt32(values[0]);
        else
            num_score = Convert.ToInt32(score);
        // 更新可用分數
        m_GameInfo.score_own = num_score;
        exchangePanel.OnEndGame(num_score);
    }

    // slotmachine totally stop.
    void ISlotMachine2GM.OnStop()
    {

        if (m_GameInfo.f_sm_state == SM_State.FREEGAME)
        {

        }
        else if(m_GameInfo.f_sm_state == SM_State.AUTOSPIN)
        {
            if (m_jd_onBegingame[0]["data"]["Lines"].Count > 0)
            {
                // 執行等待得分流程
                guiManager.OnStop(true,m_GameInfo.f_sm_state,m_jd_onBegingame);
            }
            else
            {
                guiManager.OnStop(false, m_GameInfo.f_sm_state, m_jd_onBegingame[0]["data"]);
                ((ISlotMachine2GM)this).OnClick_Spin(true);
            }
        }
        else
        {
            if(m_jd_onBegingame[0]["data"]["Lines"].Count > 0)
            {
                LogServer.Instance.print("執行等待得分流程.");
                // 執行等待得分流程
                guiManager.OnStop(true,m_GameInfo.f_sm_state, m_jd_onBegingame[0]["data"]);
            }
            else
            {
                guiManager.OnStop(false, m_GameInfo.f_sm_state, m_jd_onBegingame[0]["data"]);
                guiManager.AllowSpin();
            }
        }
    }    

    bool IBetWheel2GM.MaxBetAllow()
    {
        if (m_but_allowInfo.Maxbet)
            return true;
        else
        {
            return false;
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

            m_but_allowInfo.Maxbet = true;
            m_but_allowInfo.Exchange = true;
            m_but_allowInfo.Dollar = true;
        }
    }

    bool ISetting2GM.OpenAllow()
    {
        return m_but_allowInfo.Settings;            
    }

    private string[] en_ErrorMsg =
   {
        "NOT_ENOUGH_CREDIT",
        "DISCONNECT",
        "USER_IS_NOT_EXIST",
        "SID_IS_NOT_EXIST",
        "MACHINE_WAS_OCCUPIED",
        "CURRENCY_IS_NOT_EXIST",
        "MACHINE_IS_NOT_EXIST",
        "BETBASE_IS_ILLEGAL",
        "CREDIT_IS_ILLEGAL",
        "BALANCE_IS_NOT_ENOUGH",
        "UPDATE_BALANCE_FAILED",
        "END_WAGERS_FAILED",
        "UPDATE_CREDIT_FAILED",
        "NOT_ENOUGH_BALANCE",
        "MACHINE_IS_EMPTY",
        "MACHINE_LEAVE_FAILED",
        "DEAL_REPEATED",
        "MACHINE_IS_UNAVAILABLE",
        "FREEGAME_IS_NOT_EXIST",
        "FREEGAME_IS_ERROR",
        "WAGERS_IS_NOT_EXIST",
        "CONTENT_IS_NOT_EXIST",
        "CONTENT_UPDATE_ERROR",
        "WAGERS_AGINFO_ERROR",
        "RENT_POINT_IS_NOT_ENOUGH",
        "WAGERS_UPDATE_ERROR",
        "WAGERS_WAS_ENDED",
        "Please re-enter the game.",
        "Internet disconnection,please restart the game or check the connection status",
        "Load BonusGame resource fail",
        "Not Enough rent point.",             //30

    };

    private string[] tw_ErrorMsg =
    {
        "餘額不足",             //0
        "未成功連上Server",     //1
        "會員帳號有誤",         //2   
        "SID不存在",            //3 
        "機台已被佔",           //4
        "幣別有誤",             //5 
        "機台不存在",           //6 
        "基注有誤",             //7 
        "分數不足",             //8 
        "額度不足",             //9
        "更新額度失敗",         //10
        "結單失敗",            //11
        "更新分數失敗",        //12 
        "額度不足更換分數",    //13 
        "機台為空",           //14 
        "離開機台失敗",       //15
        "重複下單",           //16 
        "機台鎖定中",         //17 
        "免費遊戲不存在",     //18
        "免費遊戲資料有誤",   //19
        "注單不存在",        //20
        "內容不存在",        //21
        "內容更新有誤",      //22
        "注單體系資料錯誤",  //23 
        "租卡點數不足",     //24
        "注單更新失敗",     //25
        "遊戲已得分",      //26 
        "請重新登入遊戲", //27
        "網路斷線,請重新開啟遊戲或檢查連線狀態", //28
        "載入BonusGame資源包失敗",              //29
        "租卡餘額不足",                        //30

    };

    private string[] cn_ErrorMsg =
    {
        "余额不足",
        "未成功连上Server",
        "会员帐号有误",
        "SID不存在",
        "机台已被佔",
        "币别有误",
        "机台不存在",
        "基注有误",
        "分数不足",
        "额度不足",
        "更新额度失败",
        "结单失败",
        "更新分数失败",
        "额度不足更换分数",
        "机台为空",
        "离开机台失败",
        "重複下单",
        "机台锁定中",
        "免费游戏不存在",
        "免费游戏资料有误",
        "注单不存在",
        "内容不存在",
        "内容更新有误",
        "注单体系资料错误",
        "租卡点数不足",
        "注单更新失败",
        "游戏已得分",
        "请重新登入游戏",
        "网路断线,请重新开启游戏或检查连线状态",
        "载入BonusGame资源包失败",
        "租卡余额不足",                 //30

    };
}