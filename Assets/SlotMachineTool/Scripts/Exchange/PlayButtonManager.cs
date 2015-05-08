using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayButtonManager : MonoBehaviour {

    public GF_ButtonObject GFB_Spin;
    public GF_ButtonObject GFB_Stop;
    public GF_ButtonObject GFB_StopAutoSpin;
    public GF_ButtonObject GFB_GetScore;
    
    public BetWheel betWheel;

    private Dictionary<string, GF_ButtonObject> Buttons;

    void Awake()
    {
        Buttons = new Dictionary<string, GF_ButtonObject>();
        Buttons.Add("Spin", GFB_Spin);
        Buttons.Add("Stop", GFB_Stop);
        Buttons.Add("StopAutoSpin", GFB_StopAutoSpin);
        Buttons.Add("GetScore", GFB_GetScore);
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
    }
    
    public void SetButtonState(string key,string state)
    {
        Buttons[key].SetState(state);
    }

    public void Allow_Spin()
    {
        Buttons["Stop"].SetState("OFF");
        Buttons["Spin"].SetState("Normal");

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

}
