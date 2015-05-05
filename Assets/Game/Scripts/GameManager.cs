using UnityEngine;

using LitJson;
using System;

public class GameManager : RtmpS2CReceiverBase , IExchange2GM
{

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    public GUIManager guiManager;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

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
}
