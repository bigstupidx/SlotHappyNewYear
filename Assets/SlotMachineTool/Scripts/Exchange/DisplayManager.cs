using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayManager : MonoBehaviour {

    public UILabel Label_NowScore;
    public UILabel Label_BetScore;
    public UILabel Label_WinScore;
    public UILabel Label_TableNum;

    // Use this for initialization
    void Start () {
        Set_NowScore("");
        Set_BetScore("");
        Set_WinScore("");
        Set_TableNumber("");
    }
	
    public void Set_NowScore(string str)
    {
        Label_NowScore.text = str;
    }
    public void Set_BetScore(string str)
    {
        Label_BetScore.text = str;
    }
    public void Set_WinScore(string str)
    {
        Label_WinScore.text = str;
    }
    public void Set_TableNumber(string str)
    {
        Label_TableNum.text = str;
    }

    
}
