using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayButtonManager : MonoBehaviour {

    public GameObject GO_Spin;
    public GameObject GO_Stop;
    public GameObject GO_StopAutoSpin;
    public GameObject GO_GetScore;

    public SlotMachine slotMachine;
    public BetWheel betWheel;

    private Dictionary<string, UIButton> Buttons;

    void Awake()
    {
        Buttons = new Dictionary<string, UIButton>();
        Buttons.Add("Spin", GO_Spin.GetComponent<UIButton>());
        Buttons.Add("Stop", GO_Stop.GetComponent<UIButton>());
        Buttons.Add("StopAutoSpin", GO_StopAutoSpin.GetComponent<UIButton>());
        Buttons.Add("GetScore", GO_GetScore.GetComponent<UIButton>());
    }

    // Use this for initialization
    void Start () {

        //this.Set_Spin_Disable();

        GO_Stop.SetActive(false);
        GO_StopAutoSpin.SetActive(false);
        GO_GetScore.SetActive(false);
    }

    //**************************************************//
    // Spin
    //**************************************************//
    public void Set_Spin_OFF()
    {
        GO_Spin.SetActive(false);
    }
    public void Set_Spin_Normal()
    {
        GO_Spin.SetActive(true);
        Buttons["Spin"].SetState(UIButtonColor.State.Normal,true);
    }
    public void Set_Spin_Disable()
    {
        Buttons["Spin"].enabled = false;
        Buttons["Spin"].SetState(UIButtonColor.State.Disabled, true);
    }
    //**************************************************//


    //**************************************************//
    // Stop
    //**************************************************//
    public void Set_Stop_OFF()
    {
        GO_Stop.SetActive(false);
    }
    public void Set_Stop_Normal()
    {
        GO_Stop.SetActive(true);
        Buttons["Stop"].SetState(UIButtonColor.State.Normal, true);
    }
    public void Set_Stop_Disable()
    {
        Buttons["Stop"].enabled = false;
        Buttons["Stop"].SetState(UIButtonColor.State.Disabled, true);
    }
    //**************************************************//


    //**************************************************//
    // StopAutoSpin
    //**************************************************//
    public void Set_StopAutoSpin_OFF()
    {
        GO_StopAutoSpin.SetActive(false);
    }
    public void Set_StopAutoSpin_Normal()
    {
        GO_StopAutoSpin.SetActive(true);
        Buttons["StopAutoSpin"].SetState(UIButtonColor.State.Normal, true);
    }
    public void Set_StopAutoSpin_Disable()
    {
        Buttons["StopAutoSpin"].enabled = false;
        Buttons["StopAutoSpin"].SetState(UIButtonColor.State.Disabled, true);
    }
    //**************************************************//

    //**************************************************//
    // GO_GetScore
    //**************************************************//
    public void Set_GetScore_OFF()
    {
        GO_GetScore.SetActive(false);
    }
    public void Set_GetScore_Normal()
    {
        GO_GetScore.SetActive(true);
        Buttons["GetScore"].SetState(UIButtonColor.State.Normal, true);
    }
    public void Set_GetScore_Disable()
    {
        Buttons["GetScore"].enabled = false;
        Buttons["GetScore"].SetState(UIButtonColor.State.Disabled, true);
    }
    //**************************************************//

    public void OnClick_Spin()
    {
        Set_Spin_Disable();
        betWheel.Bet_Move_Close();
        slotMachine.OnClick_StartRun();
    }

    public void OnClick_Stop()
    {
        slotMachine.OnClick_Stop();
    }

    public void Allow_Stop()
    {
        Set_Spin_OFF();
        Set_Stop_Normal();
    }

    public void Allow_Spin()
    {
        betWheel.Bet_Move_Open();
    }
}
