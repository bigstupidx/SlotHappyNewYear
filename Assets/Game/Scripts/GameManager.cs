using UnityEngine;
using System.Collections;

public class GameManager :RtmpS2CReceiverBase {

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
}
