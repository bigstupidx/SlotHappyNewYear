using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class GUIManager : MonoBehaviour
{

    public delegate void CallBack();

    public struct GUIInfoBuffer
    {
        public string Score_endgame;
    }

    IGUIManager2GM IguiManager2GM;

    public BingoManager bingoManger;

    public WinFreeGame winFreeGame;

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

    private bool sw_animation;


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

        sw_animation = false;

    }

    public int GetNowScore()
    {
        return displayManager.GetNowScore();
    }

    public int GetBetScore()
    {
        return displayManager.GetBetScore();
    }

    public void OnWaitCreateExchange()
    {
        but_dollar.enabled = false;
        but_dollar.SetState(UIButtonColor.State.Disabled, false);

        but_exchange.enabled = false;
        but_exchange.SetState(UIButtonColor.State.Disabled, false);

        playbutManager.SetButtonState("Spin", "Disabled");
    }

    public void OnCreateExchange(string str, bool allowspins, string score_credit)
    {
        displayManager.Set_NowScore(score_credit);

        but_dollar.enabled = true;
        but_dollar.SetState(UIButtonColor.State.Normal, false);

        but_exchange.enabled = true;
        but_exchange.SetState(UIButtonColor.State.Normal, false);


        // 設定兌換比率
        sp_ExchangeRatio.spriteName = table_ratioFindsprite[str];

        if (allowspins)
        {

            playbutManager.Allow_Spin();
        }
    }

    public void OnBalanceExchange()
    {
        displayManager.Set_NowScore("0");
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

    public void OnClick_Spin(int score_own)
    {
        playbutManager.OnClick_Spin();

        // 更改可用分數
        displayManager.Set_NowScore(score_own.ToString());
    }

    public void OnClick_GetScore()
    {
        sw_animation = false;
    }

    public void OnStop(bool win, bool b_scatter, SM_State state, JsonData jd)
    {
        // 更改局號
        string WagersID = (jd["WagersID"]).ToString();
        displayManager.Set_TableNumber(WagersID);

        if (win || b_scatter)
            StartCoroutine(GetFlow(state, jd, b_scatter));
        else
        {
            StartCoroutine(WaitAWhile(1.0f, IguiManager2GM.Finish_OnStop));
        }
    }

    public void ShowGetFreeGame(int freegametime)
    {
        winFreeGame.OpenAndSetContext("獲得 Free Game " + freegametime + " 次");
    }

    public void UpdateBetValue(int betperline)
    {
        string str_betscore = (betperline * 50).ToString();
        displayManager.Set_BetScore(str_betscore);
    }

    public void ShowWindowMsg(string content)
    {
        displayManager.OpenAndSet_WindowMsg(content);
    }

    public void OnStop_FreeGameSpinStop(JsonData jd)
    {
        try
        {
            // 更改局號
            string WagersID = (jd["WagersID"]).ToString();
            displayManager.Set_TableNumber(WagersID);

            // 贏分
            if (jd["Lines"].Count > 0 || jd["Scatter"].IsObject)
                StartCoroutine(FreeGameSpinWin(jd));
            else
                StartCoroutine(WaitAWhile(1.0f, IguiManager2GM.Finish_OnStop));
        }
        catch (Exception EX)
        {
            LogServer.Instance.print("OnStop_FreeGameSpinStop Exception " + EX);
        }
    }

    IEnumerator FreeGameSpinWin(JsonData jd)
    {
        JsonData jd_lines = jd["Lines"];
        int len_arr = jd_lines.Count;
        if (jd["Scatter"].IsObject)
            len_arr += 1;

        int[] arr_lineid = new int[len_arr];
        string[] arr_payoff = new string[len_arr];
        int sum_line_payoff = 0;

        // 剖析每一條線的資料
        string output = "output :\n";
        for (int i = 0; i < jd_lines.Count; i++)
        {

            double dou = (double)jd_lines[i]["LineID"];
            arr_lineid[i] = Convert.ToInt32(dou);
            arr_payoff[i] = (jd_lines[i]["Payoff"]).ToString().Split('.')[0];
            sum_line_payoff += Convert.ToInt32(arr_payoff[i]);

            output += "i " + i + " , arr_lineid[i] " + arr_lineid[i] + " , arr_payoff[i] " + arr_payoff[i] + "\n";
        }

        if (jd["Scatter"].IsObject)
        {
            double dou_id = (double)jd["Scatter"]["ID"];
            int lineid_scatter = Convert.ToInt32(dou_id);
            double dou_payoff = (double)jd["Scatter"]["Payoff"];
            int payoff_scatter = Convert.ToInt32(dou_payoff);

            arr_lineid[len_arr - 1] = lineid_scatter;
            arr_payoff[len_arr - 1] = payoff_scatter.ToString();

            sum_line_payoff += payoff_scatter;
            LogServer.Instance.print("Add a line of Scatter. payoff is " + payoff_scatter);
        }

        output += "sum_line_payoff is " + sum_line_payoff;
        LogServer.Instance.print(output);

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < jd_lines.Count; i++)
        {
            bingoManger.OpenBingoLine(arr_lineid[i]);
        }

        // 顯示贏得分數
        displayManager.Set_WinScore(sum_line_payoff.ToString());


        yield return new WaitForSeconds(1.0f);

        bingoManger.CloseAllBingoLine();

        int score_now = GetNowScore();
        // 加上本次贏分
        score_now += sum_line_payoff;
        // 贏得分數歸零、現在分數增加
        displayManager.Set_WinScore("0");
        displayManager.Set_NowScore(score_now.ToString());

        IguiManager2GM.Finish_OnStop();

        // Debug 檢查目前分數
        double dou_1 = (double)jd["EndCredit"];
        int EndCredit = Convert.ToInt32(dou_1);

        LogServer.Instance.print("FreeGameSpinWin EndCredit " + EndCredit + " , score_now " + score_now);
    }

    IEnumerator WaitAWhile(float time, CallBack callback)
    {
        yield return new WaitForSeconds(time);

        callback();
    }

    IEnumerator GetFlow(SM_State state, JsonData jd, bool sw_scatter)
    {
        JsonData jd_lines = jd["Lines"];
        int len_arr = jd_lines.Count;
        if (sw_scatter)
            len_arr += 1;

        int[] arr_lineid = new int[len_arr];
        string[] arr_payoff = new string[len_arr];
        int sum_line_payoff = 0;

        // 剖析每一條線的資料
        string output = "output :\n";
        for (int i = 0; i < jd_lines.Count; i++)
        {

            double dou = (double)jd_lines[i]["LineID"];
            arr_lineid[i] = Convert.ToInt32(dou);
            arr_payoff[i] = (jd_lines[i]["Payoff"]).ToString().Split('.')[0];
            sum_line_payoff += Convert.ToInt32(arr_payoff[i]);

            output += "i " + i + " , arr_lineid[i] " + arr_lineid[i] + " , arr_payoff[i] " + arr_payoff[i] + "\n";
        }

        if (sw_scatter)
        {
            double dou_id = (double)jd["Scatter"]["ID"];
            int lineid_scatter = Convert.ToInt32(dou_id);
            double dou_payoff = (double)jd["Scatter"]["Payoff"];
            int payoff_scatter = Convert.ToInt32(dou_payoff);

            arr_lineid[len_arr - 1] = lineid_scatter;
            arr_payoff[len_arr - 1] = payoff_scatter.ToString();

            sum_line_payoff += payoff_scatter;
            LogServer.Instance.print("Add a line of Scatter. payoff is " + payoff_scatter);
        }

        output += "sum_line_payoff is " + sum_line_payoff;
        LogServer.Instance.print(output);

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < jd_lines.Count; i++)
        {
            bingoManger.OpenBingoLine(arr_lineid[i]);
        }

        // 顯示贏得分數
        displayManager.Set_WinScore(sum_line_payoff.ToString());

        yield return new WaitForSeconds(1.0f);

        bingoManger.CloseAllBingoLine();


        if (state == SM_State.AUTOSPIN)
        {
            int score_now = GetNowScore();
            // 加上本次贏分
            score_now += sum_line_payoff;
            // 贏得分數歸零、現在分數增加
            displayManager.Set_WinScore("0");
            displayManager.Set_NowScore(score_now.ToString());

            IguiManager2GM.Finish_OnStop();
        }
        else if (state == SM_State.FREEGAME)
        {

        }
        else
        {

            // 執行等待得分流程
            playbutManager.Allow_GetScore();
            sw_animation = true;
            int cnt_idx = 0;

            while (sw_animation)
            {
                bingoManger.OpenBingoLine(arr_lineid[cnt_idx]);

                bingoManger.ShowPayoff(arr_lineid[cnt_idx], arr_payoff[cnt_idx]);

                // 當按下得分
                if (!sw_animation)
                {
                    bingoManger.CloseAllBingoLine();

                    bingoManger.ClosePayoff();
                    break;
                }

                yield return new WaitForSeconds(1.0f);
                bingoManger.CloseAllBingoLine();

                bingoManger.ClosePayoff();


                cnt_idx++;

                if (cnt_idx == arr_lineid.Length)
                    cnt_idx = 0;
            }

            int score_now = GetNowScore();
            // 加上本次贏分
            score_now += sum_line_payoff;
            // 贏得分數歸零、現在分數增加
            displayManager.Set_WinScore("0");
            displayManager.Set_NowScore(score_now.ToString());

            IguiManager2GM.Finish_OnStop();
        }

    }
}
        

