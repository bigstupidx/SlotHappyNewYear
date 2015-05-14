using UnityEngine;
using System.Collections.Generic;

public class PlayButtonManager : MonoBehaviour {

    public GF_ButtonObject GFB_Spin;
    public GF_ButtonObject GFB_Stop;
    public GF_ButtonObject GFB_StopAutoSpin;
    public GF_ButtonObject GFB_GetScore;

    public GF_ButtonObject GFB_MaxBet;
    public GF_ButtonObject GFB_JackPot;
    public GF_ButtonObject GFB_CashExchange;
    public GF_ButtonObject GFB_Settings;
    public GF_ButtonObject GFB_Dollars;

    public BetWheel betWheel;

    private Dictionary<string, GF_ButtonObject> Buttons;

    void Awake()
    {
        Buttons = new Dictionary<string, GF_ButtonObject>();
        Buttons.Add("Spin", GFB_Spin);
        Buttons.Add("Stop", GFB_Stop);
        Buttons.Add("StopAutoSpin", GFB_StopAutoSpin);
        Buttons.Add("GetScore", GFB_GetScore);

        Buttons.Add("MaxBet", GFB_MaxBet);
        Buttons.Add("JackPot", GFB_JackPot);
        Buttons.Add("CashExchange", GFB_CashExchange);
        Buttons.Add("Settings", GFB_Settings);
        Buttons.Add("Dollars", GFB_Dollars);
    }

    // Use this for initialization
    void Start () {
                
        Buttons["Stop"].SetState("OFF");
        Buttons["StopAutoSpin"].SetState("OFF");
        Buttons["GetScore"].SetState("OFF");
    }
    

    public void OnClick_Spin()
    {
        betWheel.Bet_Move_Close();

        Buttons["MaxBet"].SetState("Disabled");
        Buttons["CashExchange"].SetState("Disabled");
        Buttons["Dollars"].SetState("Disabled");
    }
    
    public void SetButtonState(string key,string state)
    {
        Buttons[key].SetState(state);
    }

    public void Allow_Spin()
    {
        Buttons["Stop"].SetState("OFF");
        Buttons["GetScore"].SetState("OFF");
        Buttons["Spin"].SetState("Normal");


        Buttons["MaxBet"].SetState("Normal");
        Buttons["CashExchange"].SetState("Normal");
        Buttons["Dollars"].SetState("Normal");

        betWheel.Bet_Move_Open();
    }

    public void Allow_Stop()
    {
        Buttons["Spin"].SetState("OFF");
        Buttons["Stop"].SetState("Normal");
    }

    public void Allow_AutoStop()
    {
        Buttons["Spin"].SetState("OFF");
        Buttons["StopAutoSpin"].SetState("Normal");

    }

    public void Allow_GetScore()
    {
        Buttons["Stop"].SetState("OFF");
        Buttons["GetScore"].SetState("Normal");
    }
}