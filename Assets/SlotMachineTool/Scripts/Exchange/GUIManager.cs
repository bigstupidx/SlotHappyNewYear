using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class GUIManager : MonoBehaviour
{
    IGUIManager2GM IguiManager2GM;

    // 滿線滿注
    public UIButton but_maxbetline;

    public UIButton but_dollar;

    public UIButton but_exchange;

    public UIButton but_settings;

    public UISprite sp_ExchangeRatio;

    public PlayButtonManager playbutManager;

    public DisplayManager displayManager;

    Dictionary<string, string> table_ratioFindsprite;

	static public GUIManager Instance;

	public void Awake()
	{
        Instance = this;

        IguiManager2GM = GameManager.Instance;
    }

    public void Start()
    {
        table_ratioFindsprite = new Dictionary<string, string>();

        table_ratioFindsprite.Add("1:100", "1:100_up.PNG");
        table_ratioFindsprite.Add("1:50", "1:50_up.PNG");
        table_ratioFindsprite.Add("1:20", "1:20_up.PNG");
        table_ratioFindsprite.Add("1:10", "1:10_up.PNG");
        table_ratioFindsprite.Add("1:5", "1:5_up.PNG");
        table_ratioFindsprite.Add("10:1", "10:1_up.PNG");
        table_ratioFindsprite.Add("2:1", "2:1_up.PNG");
        table_ratioFindsprite.Add("20:1", "20:1_up.PNG");
        table_ratioFindsprite.Add("5:1", "5:1_up.PNG");
        table_ratioFindsprite.Add("50:1", "50:1_up.PNG");
        table_ratioFindsprite.Add("100:1", "100:1_up.PNG");
        table_ratioFindsprite.Add("1000:1", "1000:1_up.PNG");
        table_ratioFindsprite.Add("10000:1", "10000:1_up.PNG");
        table_ratioFindsprite.Add("100K:1", "100K:1_up.PNG");
        table_ratioFindsprite.Add("200K:1", "200K:1_up.PNG");
        table_ratioFindsprite.Add("300K:1", "300K:1_up.PNG");
        table_ratioFindsprite.Add("50000:1", "50000:1_up.PNG");
    }

    public void OnWaitCreateExchange()
    {
        but_dollar.enabled = false;
        but_dollar.SetState(UIButtonColor.State.Disabled, false);

        but_exchange.enabled = false;
        but_exchange.SetState(UIButtonColor.State.Disabled, false);

        playbutManager.SetButtonState("Spin", "Disabled");
    }

    public void OnCreateExchange(string str,bool allowspins)
    {
        but_dollar.enabled = true;
        but_dollar.SetState(UIButtonColor.State.Normal, false);

        but_exchange.enabled = true;
        but_exchange.SetState(UIButtonColor.State.Normal, false);


        // 設定兌換比率
        sp_ExchangeRatio.spriteName = table_ratioFindsprite[str];
        
        if(allowspins)
        {
            playbutManager.Allow_Spin();
        }
    }

    public void AllowSpin()
    {
        playbutManager.Allow_Spin();
    }

    public void AllowStop()
    {
        playbutManager.Allow_Stop();
    }
    public void AllowAutoStop()
    {
        playbutManager.Allow_AutoStop();
    }

    public void OnClick_Spin()
    {
        playbutManager.OnClick_Spin();
    }

    public void OnClick_GetScore(string str_nowscore)
    {
        // 贏得分數歸零、現在分數增加
        displayManager.Set_WinScore("0");
        displayManager.Set_NowScore(str_nowscore);
    }

    public void OnStop(SM_State state)
    {
        StartCoroutine(GetFlow(state));
    }    
    
    public void UpdateBetValue(int betperline)
    {
        string str_betscore = (betperline * 50).ToString();
        displayManager.Set_BetScore(str_betscore);
    }

    IEnumerator GetFlow(SM_State state)
    {
        yield return new WaitForSeconds(1.0f);

        if(state == SM_State.AUTOSPIN)
        {
            IguiManager2GM.Finish_GetScore();
        }
        else if(state == SM_State.FREEGAME)
        {

        }
        else
        {
            // 執行等待得分流程


            IguiManager2GM.Finish_GetScore();
        }
    }
}
