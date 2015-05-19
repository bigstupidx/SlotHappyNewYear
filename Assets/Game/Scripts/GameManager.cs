using UnityEngine;

using System;
using System.Collections;
using LitJson;

public struct Info_GameApplication
{
    public bool sw_autorun;
    public bool sw_freegame_waitslot;
    public bool sw_freegame_waitgui;
    public int idx_freegame;

    /*  0 : Normal
        1 : Autospin
        2 : FreeGame    */
    public SM_State f_sm_state;

    public Info_GameApplication(bool pa1, SM_State state)
    {
        sw_autorun = pa1;
        f_sm_state = state;
        sw_freegame_waitslot = false;
        sw_freegame_waitgui = false;
        idx_freegame = 0;
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
    , IWinFreeGame2GM
{

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    public GUIManager guiManager;

    public SlotMachine slotmachine;
    
    public static GameManager Instance;

    Info_GameApplication m_GameAppInfo;
    ButtonAllowTable m_but_allowInfo;

    JsonData m_jd_onBegingame;

    void Awake()
    {
        LogServer.Instance.print("Screen.width : " + Screen.width + " Screen.height : " + Screen.height);
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        m_GameAppInfo = new Info_GameApplication(false , SM_State.NORMAL);

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
        try
        {
            JsonData jd = JsonMapper.ToObject(str);
            if ((bool)jd[0]["OnClose"])
            {
                string domain = LoginManager.loginInfo.Domain;
                string accountname = LoginManager.loginInfo.AccountName;

                string url = "http://" + domain + "/app/WebService/view/display.php/Logout?username=" + accountname;

                StartCoroutine(DoLoginout(url));
            }
        }
        catch(Exception EX)
        {
            LogServer.Instance.print("OnClose Exception " + EX);
        }
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
        try
        {
            JsonData jd = JsonMapper.ToObject(str);

            if ((bool)jd[0]["event"])
            {
                string balance = (jd[0]["data"]["Balance"]).ToString();
                string betBase = (jd[0]["data"]["BetBase"]).ToString();

                string credit = (jd[0]["data"]["Credit"]).ToString();

                string[] get_Int = credit.Split('.');

                exchangePanel.OnCreditExchange(balance, betBase, get_Int[0]);

                int score_now = 0;
                int score_bet = guiManager.GetBetScore();

                // 設定玩家可用分數
                score_now = Convert.ToInt32(get_Int[0]);

                bool allowspin = false;
                if (score_bet > 0 && score_now > 0)
                    allowspin = true;

                // 恢復某些按鈕，設定Display可用分數
                guiManager.OnCreateExchange(betBase, allowspin, get_Int[0]);
            }
            else
            {
                LogServer.Instance.print("onCreditExchange event : false" );
            }
        }
        catch(Exception EX)
        {
            LogServer.Instance.print("onCreditExchange Exception " + EX);
        }
    }

    public override void onBalanceExchange(string str)
    {
        try
        {
            guiManager.OnBalanceExchange();

            JsonData jd = JsonMapper.ToObject(str);
            string transcredit = (jd[0]["data"]["TransCredit"]).ToString();
            string amount = (jd[0]["data"]["Amount"]).ToString();
            string balance = (jd[0]["data"]["Balance"]).ToString();

            exchangePanel.OnBalanceExchange(transcredit, amount, balance);
        }
        catch (Exception EX)
        {
            LogServer.Instance.print("onBalanceExchange Exception " + EX);
        }
    }

    public override void onBeginGame(string str)
    {

        m_jd_onBegingame = JsonMapper.ToObject(str);
        
        if ((bool)m_jd_onBegingame[0]["event"])
        {
            string WagersID = (m_jd_onBegingame[0]["data"]["WagersID"]).ToString();
                        
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
        JsonData jd = JsonMapper.ToObject(str);

        if(!(bool)jd[0]["event"])
        {
            // 錯誤資訊
            
        }
        else
        {
            // 剖析 Cards 欄位
            string cards = (m_jd_onBegingame[0]["data"]["Cards"]).ToString();
            string[] tileinfo = cards.Split(',', '-');

            string str_show = "";
            for (int i = 0; i < tileinfo.Length; i++)
            {
                int num = Convert.ToInt32(tileinfo[i]);
                tileinfo[i] = num.ToString("000");
                str_show += tileinfo[i] + " ";
            }
            LogServer.Instance.print("[Debug] tileinfo " + str_show);

            // 將資料塞入拉霸機
            slotmachine.SetTileSpriteInfo(tileinfo);

            // 依拉霸機的狀態選擇下一個按鈕的種類
            if (m_GameAppInfo.f_sm_state == SM_State.AUTOSPIN)
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
    
    void ISlotMachine2GM.OnClick_Spin()
    {
        // 關閉不允許Spin開啟的按鍵
        m_but_allowInfo.Maxbet = false;
        m_but_allowInfo.Exchange = false;
        m_but_allowInfo.Dollar = false;

        Spin();
    }

    void Spin()
    {
        int score_now = guiManager.GetNowScore();
        int betscore = guiManager.GetBetScore();

        if (score_now >= betscore)
        {
            // 計算可用分數
            score_now -= betscore;

            // Disable 4組按鍵 ， 更改可用分數顯示
            guiManager.OnClick_Spin(score_now);

            RtmpC2S.BeginGame("beginGame2", 50, betscore/50);

            slotmachine.StartSpin();
        }
        else
        {
            if (m_GameAppInfo.f_sm_state == SM_State.AUTOSPIN)
            {
                // 結束自動轉
                m_GameAppInfo.f_sm_state = SM_State.NORMAL;
            }

            // 可用分數不足，跳出通知訊息。
            string context = "";
            string language_id = Localization.language;
            if (language_id == "TW")
                context = tw_ErrorMsg[0];
            else if (language_id == "CN")
                context = cn_ErrorMsg[0];
            else
                context = en_ErrorMsg[0];

            guiManager.ShowWindowMsg(context);

            // 復原按鍵
            guiManager.AllowSpin();

            m_but_allowInfo.Maxbet = true;
            m_but_allowInfo.Exchange = true;
            m_but_allowInfo.Dollar = true;
        }
    }
    
    void ISlotMachine2GM.OnClick_AutoSpin()
    {
        m_GameAppInfo.f_sm_state = SM_State.AUTOSPIN;

        m_GameAppInfo.sw_autorun = true;
        Spin();
    }

    void ISlotMachine2GM.OnClick_StopAutoSpin()
    {
        if(m_GameAppInfo.f_sm_state == SM_State.AUTOSPIN)
            m_GameAppInfo.f_sm_state = SM_State.NORMAL;

        m_GameAppInfo.sw_autorun = false;
    }

    void ISlotMachine2GM.OnClick_GetScore()
    {
        // 關閉動畫、結束流程。
        guiManager.OnClick_GetScore();
    }

    // slotmachine totally stop.
    void ISlotMachine2GM.OnStop()
    {
        if (m_GameAppInfo.f_sm_state == SM_State.FREEGAME)
        {
            m_GameAppInfo.sw_freegame_waitslot = false;
        }
        else
        {
            // 贏分
            bool b_win = false;
            bool b_scatter = false;
            int cnt_line = m_jd_onBegingame[0]["data"]["Lines"].Count;
            if (cnt_line > 0)
                b_win = true;
            if (m_jd_onBegingame[0]["data"]["Scatter"].IsObject)
                b_scatter = true;
            guiManager.OnStop(b_win, b_scatter, m_GameAppInfo.f_sm_state, m_jd_onBegingame[0]["data"]);
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
        int score_now = guiManager.GetNowScore();

        guiManager.UpdateBetValue(betvalue);

        if (score_now > 0)
            guiManager.AllowSpin();
    }
    
    void IGUIManager2GM.Finish_OnStop()
    {
        try
        {

            bool sw_freegame = false;
            if (m_GameAppInfo.f_sm_state != SM_State.FREEGAME)
            {
                // 檢查本次Spin有無免費遊戲
                if (m_jd_onBegingame[0]["data"]["FreeGame"].IsObject)
                {
                    sw_freegame = true;
                    m_GameAppInfo.f_sm_state = SM_State.FREEGAME;
                }
            }

            // 如果有免費遊戲
            if (sw_freegame)
            {
                sw_freegame = false;
                // 通知玩家獲得免費遊戲以及次數
                guiManager.ShowGetFreeGame(m_jd_onBegingame[0]["data"]["FreeGame"]["BonusInfo"].Count);

                LogServer.Instance.print("Finish_OnStop sw_freegame is true");
            }
            else
            {
                double dou = (double)m_jd_onBegingame[0]["data"]["EndCredit"];
                int EndCredit = Convert.ToInt32(dou);

                // 更新可用分數
                exchangePanel.OnChangeNowScore(EndCredit);

                if (m_GameAppInfo.f_sm_state == SM_State.FREEGAME)
                    m_GameAppInfo.sw_freegame_waitgui = false;
                else if (m_GameAppInfo.f_sm_state == SM_State.AUTOSPIN)
                    Spin();
                else if (m_GameAppInfo.f_sm_state == SM_State.NORMAL)
                {
                    guiManager.AllowSpin();

                    m_but_allowInfo.Maxbet = true;
                    m_but_allowInfo.Exchange = true;
                    m_but_allowInfo.Dollar = true;

                }
            }
        }
        catch(Exception EX)
        {
            LogServer.Instance.print("Finish_OnStop Exception " + EX);
        }
    }    

    bool ISetting2GM.OpenAllow()
    {
        return m_but_allowInfo.Settings;
    }

    void IWinFreeGame2GM.OnClick_CloseWinFreeGame()
    {
        // 執行免費遊戲
        StartCoroutine(FreeGame_Spin());
    }
    
    IEnumerator FreeGame_Spin()
    {
        int cnt_freegame = m_jd_onBegingame[0]["data"]["FreeGame"]["BonusInfo"].Count;

        LogServer.Instance.print("cnt_freegame " + cnt_freegame);

        int idx = 0;
        do
        {
            yield return new WaitForSeconds(1.0f);

            slotmachine.StartSpin();

            yield return new WaitForSeconds(1.0f);
            
            // 剖析 Cards 欄位
            string cards = (m_jd_onBegingame[0]["data"]["FreeGame"]["BonusInfo"][idx]["Cards"]).ToString();
            string[] tileinfo = cards.Split(',', '-');

            string str_show = "";
            for (int i = 0; i < tileinfo.Length; i++)
            {
                int num = Convert.ToInt32(tileinfo[i]);
                tileinfo[i] = num.ToString("000");
                str_show += tileinfo[i] + " ";
            }
            LogServer.Instance.print("[Debug] tileinfo [" + idx + "] " + str_show);

            // 將資料塞入拉霸機
            slotmachine.SetTileSpriteInfo(tileinfo);

            slotmachine.OnClick_StartStop_Immediate();

            m_GameAppInfo.sw_freegame_waitslot = true;
            while(m_GameAppInfo.sw_freegame_waitslot)
            {
                yield return new WaitForEndOfFrame();
            }

            m_GameAppInfo.sw_freegame_waitgui = true;
            guiManager.OnStop_FreeGameSpinStop(m_jd_onBegingame[0]["data"]["FreeGame"]["BonusInfo"][idx]);
            
            while (m_GameAppInfo.sw_freegame_waitgui)
            {
                yield return new WaitForEndOfFrame();
            }


            cnt_freegame--;
            idx++;            
        }
        while (cnt_freegame > 0);

        if(m_GameAppInfo.sw_autorun)
        {
            m_GameAppInfo.f_sm_state = SM_State.AUTOSPIN;
            Spin();
        }
        else
        {

            m_GameAppInfo.f_sm_state = SM_State.NORMAL;

            guiManager.AllowSpin();

            m_but_allowInfo.Maxbet = true;
            m_but_allowInfo.Exchange = true;
            m_but_allowInfo.Dollar = true;
        }
    }

    #region Table
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
    #endregion
}