using UnityEngine;

using LitJson;

public class GameManager : RtmpS2CReceiverBase {

    public UIPanel Win_SystemMessage;

    public ExchangePanel exchangePanel;

    // Use this for initialization
    void Start () {

        Win_SystemMessage.alpha = 0.0f;

        Init();
    }
	
	// Update is called once per frame
	void Update () {
	
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
    }

    public override void onBalanceExchange(string str)
    {

        JsonData jd = JsonMapper.ToObject(str);
        string transcredit = (jd[0]["data"]["TransCredit"]["Amount"]["Balance"]).ToString();
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
}
