using UnityEngine;

public class DisplayManager : MonoBehaviour {

    public UILabel Label_NowScore;
    public UILabel Label_BetScore;
    public UILabel Label_WinScore;
    public UILabel Label_TableNum;

    public GF_Window GF_WindowMsg;
      
    // Use this for initialization
    void Start () {
        Set_NowScore("0");
        Set_BetScore("0");
        Set_WinScore("0");
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
    public void OpenAndSet_WindowMsg(string content)
    {
        GF_WindowMsg.SetContext(content);
        GF_WindowMsg.Open();
    }
    public void Close_WindowMsg()
    {
        GF_WindowMsg.Close();
        GF_WindowMsg.SetContext("");
    }

}
