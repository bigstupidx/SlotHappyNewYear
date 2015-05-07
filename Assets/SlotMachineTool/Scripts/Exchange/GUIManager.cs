using UnityEngine;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour , ISlotMachine2GUI {

    // 滿線滿注
    public UIButton but_maxbetline;

    public UIButton but_dollar;

    public UIButton but_exchange;

    public UIButton but_settings;

    public UISprite sp_ExchangeRatio;

    public PlayButtonManager playbutManager;

    Dictionary<string, string> table_ratioFindsprite;

	static public ISlotMachine2GUI Islotmachine2GUI;

	public void Awake()
	{
		Islotmachine2GUI = this;
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

        playbutManager.Set_Spin_Disable();
    }

    public void OnCreateExchange(string str)
    {
        but_dollar.enabled = true;
        but_dollar.SetState(UIButtonColor.State.Normal, false);

        but_exchange.enabled = true;
        but_exchange.SetState(UIButtonColor.State.Normal, false);

        playbutManager.Allow_Spin();

        // 設定兌換比率
        sp_ExchangeRatio.spriteName = table_ratioFindsprite[str];
    }

	void ISlotMachine2GUI.OnSpin()
	{
	}

	void ISlotMachine2GUI.OnStop()
	{
	}
}
