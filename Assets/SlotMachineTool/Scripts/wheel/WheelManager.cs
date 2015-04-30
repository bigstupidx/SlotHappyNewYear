using System.Collections.Generic;
using UnityEngine;

// 滾輪管理器
public class WheelManager : MonoBehaviour {

    private Dictionary<string, bool> dic_wheelstate;

    public UISprite sprite_dark;

    //public LineWheel linewheel;
    public BetWheel betwheel;

    void Awake()
    {
        dic_wheelstate = new Dictionary<string, bool>();

        //dic_wheelstate.Add("Line", false);
        dic_wheelstate.Add("Bet", false);
        sprite_dark.enabled = false;
    }

    void OnClick()
    {

        //print("OnClicik " + dic_wheelstate["Line"].ToString() + dic_wheelstate["Bet"].ToString());

        //if (dic_wheelstate["Line"] || dic_wheelstate["Bet"])
        if (dic_wheelstate["Bet"])
        {
            //linewheel.CloseWheel();
            betwheel.CloseWheel();
        }
    }

    public void ChangeState(string str_type,bool state)
    {
        //print(str_type + " " + state.ToString());

        if (dic_wheelstate.ContainsKey(str_type))
        {
            dic_wheelstate[str_type] = state;

            if (state)
                sprite_dark.enabled = true;

            //if ((!dic_wheelstate["Line"]) && (!dic_wheelstate["Bet"]))
            if ((!dic_wheelstate["Bet"]))
                sprite_dark.enabled = false;
        }
        else
            print("Error ChangeState str_type " + str_type);
    }
}
