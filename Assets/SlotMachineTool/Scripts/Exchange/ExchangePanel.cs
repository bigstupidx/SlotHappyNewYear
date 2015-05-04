using UnityEngine;

using System;
using System.Collections.Generic;

public class ExchangePanel : MonoBehaviour {
    
    // 顯示帳號登入名稱
    public UILabel Content_AccountName;
    // 顯示目前餘額
    public UILabel Content_BalanceMoney;
    // 顯示目前分數
    public UILabel Content_NowScore;
    // 顯示兌換分數
    public UILabel Content_ExchangeScore;
    // 顯示兌換比例
    public UILabel Context_Proportion;

    // exit - 遊戲分數
    public UILabel Context_Exit_NowScore;
    public UILabel Context_Exit_ExchangeScore;
    public UILabel Context_Exit_Balance;

    public UIButton UIBut_checkout;

    public UILabel exchangewindow_Proportion;

    public UISlider Content_Slider;
    
    public string Cal_BalanceMoney;

    private int Cal_NowScore;

    private int Cal_ExchangScore;


    public UIWidget window_other;
    public UIWidget window_number;
    public UIWidget window_exchangeratio;
    public UIWidget window_exit;

    
    // *****************************/
    // 目前比值
    string[] ratiobases;
    int idx_ratio_keep;
    int idx_ratio;
    int idx_ratio_temp;

    // ****************************/
    public UIWidget window_Tutorial;
    public GameObject[] Tutorial_pages;

    // 目前教學頁數
    int idx_tutorial;

    // 當收到 BalanceExchange 是否需要開分
    bool bNeedCreateExchange;
    
    void Start()
    {

        CloseOtherWindow();

        window_exit.alpha = 0.0f;

        Cal_BalanceMoney = "0.0";

        bNeedCreateExchange = false;
    }

    // Invoke this method when use ScoreExchange.
    public void Setup(string balance, string defaultBase, string LoginName, string bases)
    {
        // 登入顯示名稱
        Content_AccountName.text = LoginName;

        // 設定餘額
        Content_BalanceMoney.text = balance;
        Cal_BalanceMoney = balance;

        // 目前顯示分數
        Content_NowScore.text = "0";
        Cal_NowScore = 0;

        // 設定兌換分數
        Content_ExchangeScore.text = "0";
        Cal_ExchangScore = 0;

        // 設定支援的比例、兌換分數上目前的比例、索引、暫時索引位置
        ratiobases = bases.Split(',');
        exchangewindow_Proportion.text = defaultBase;
        idx_ratio = Array.FindIndex<string>(ratiobases, x => x == defaultBase);
        idx_ratio_keep = idx_ratio;
        idx_ratio_temp = idx_ratio;
    }


    public void OpenExchangeWindow()
    {
        gameObject.SetActive(true);
    }

    public void OnButton500()
    {
        Cal_ExchangScore += 500;

        if (Cal_ExchangScore + Cal_NowScore > 500000)
            Cal_ExchangScore = 500000 - Cal_NowScore;

        string[] values = Cal_BalanceMoney.Split('.');

        int max_ExchangeScore = (int)(Convert.ToDouble(values[0]) * RatioStrToFloat(Context_Proportion.text));

        if (max_ExchangeScore + Cal_NowScore > 500000)
            max_ExchangeScore = 500000 - Cal_NowScore;

        float slider_value = (float)Cal_ExchangScore / max_ExchangeScore;

        Content_Slider.value = slider_value;
    }

    public void OnButton5000()
    {
        Cal_ExchangScore += 5000;

        if (Cal_ExchangScore + Cal_NowScore > 500000)
            Cal_ExchangScore = 500000 - Cal_NowScore;

        string[] values = Cal_BalanceMoney.Split('.');

        int max_ExchangeScore = (int)(Convert.ToDouble(values[0]) * RatioStrToFloat(Context_Proportion.text));

        if (max_ExchangeScore + Cal_NowScore > 500000)
            max_ExchangeScore = 500000 - Cal_NowScore;

        float slider_value = (float)Cal_ExchangScore / max_ExchangeScore;

        Content_Slider.value = slider_value;
    }

    public void OnButton50000()
    {
        Cal_ExchangScore += 50000;

        if (Cal_ExchangScore + Cal_NowScore > 500000)
            Cal_ExchangScore = 500000 - Cal_NowScore;

        string[] values = Cal_BalanceMoney.Split('.');

        int max_ExchangeScore = (int)(Convert.ToDouble(values[0]) * RatioStrToFloat(Context_Proportion.text));

        if (max_ExchangeScore + Cal_NowScore > 500000)
            max_ExchangeScore = 500000 - Cal_NowScore;

        float slider_value = (float)Cal_ExchangScore / max_ExchangeScore;

        Content_Slider.value = slider_value;
    }

    public void OnButtonOther()
    {
        Content_Slider.value = 1.0f;
    }
    
    public void OnSliderValueChange(object value)
    {
        // integr [0] , float [1]
        string[] values = Cal_BalanceMoney.Split('.');

        int max_ExchangeScore = (int)(Convert.ToDouble(values[0]) * RatioStrToFloat(Context_Proportion.text));

        if (max_ExchangeScore + Cal_NowScore > 500000)
            max_ExchangeScore = 500000 - Cal_NowScore;

        int slider_rate = (int)((float)value * 10000000.0f);
        float min_ExchangeScore = (float)max_ExchangeScore / 10000000.0f;

        this.Cal_ExchangScore = Convert.ToInt32(min_ExchangeScore * slider_rate);

        float rate_toMoney = 1.0f / RatioStrToFloat(Context_Proportion.text);

        SetExchangeScore(this.Cal_ExchangScore);

        rate_toMoney = rate_toMoney * 100.0f;
        int cost = Convert.ToInt32(this.Cal_ExchangScore * rate_toMoney);
        cost = cost / 100;

        string new_BalanceMoney = String_Subtraction(Cal_BalanceMoney, cost);


        string[] values_1 = new_BalanceMoney.Split('.');

        this.SetUserBalance(new_BalanceMoney);
    }

    private string String_Subtraction(string value, int subtrahend)
    {
        string[] values = value.Split('.');

        string outcome = (Convert.ToInt32(values[0]) - subtrahend).ToString();

        outcome += "." + values[1];

        return outcome;

    }

    private void SetExchangeScore(int value)
    {
        Content_ExchangeScore.text = value.ToString();
    }

    private void SetUserBalance(string value)
    {
        Content_BalanceMoney.text = value;
    }

    public void OpenOtherWindow()
    {
        window_other.alpha = 1.0f;
        window_number.alpha = 0.0f;
    }

    public void CloseOtherWindow()
    {
        window_other.alpha = 0.0f;
        window_number.alpha = 1.0f;
    }

    public void OnClick_EnterGame()
    {
        gameObject.SetActive(false);

        // 分數小於 0 ，什麼事都不做。
        if (Cal_ExchangScore > 0)
        {
            if (idx_ratio_keep != idx_ratio)
            {
                bNeedCreateExchange = true;
                RtmpC2S.creditExchange(ratiobases[idx_ratio], Cal_ExchangScore.ToString());
                //LuaManager_new.Instance().CallLuaFuction("DoCreateExchange", true, ratiobases[idx_ratio], Cal_ExchangScore);
            }
            else
            {
                bNeedCreateExchange = false;
                //LuaManager_new.Instance().CallLuaFuction("DoCreateExchange", false, ratiobases[idx_ratio], Cal_ExchangScore);
            }
        }
    }
    

    public void OnClick_CashOut()
    {

    }

    public void OnCreditExchange(string balance,string betbase,string credit)
    {

        // 設定餘額
        Content_BalanceMoney.text = balance;
        Cal_BalanceMoney = balance;

        // 目前顯示分數
        Content_NowScore.text = credit;
        Cal_NowScore = Convert.ToInt32(credit);

        // 設定兌換分數
        Content_ExchangeScore.text = "0";
        Cal_ExchangScore = 0;

        // 設定支援的比例、兌換分數上目前的比例、索引、暫時索引位置
        exchangewindow_Proportion.text = betbase;
        idx_ratio = Array.FindIndex<string>(ratiobases, x => x == betbase);
        idx_ratio_keep = idx_ratio;
        idx_ratio_temp = idx_ratio;

        gameObject.SetActive(false);
    }
    
    public void OnEndGame(string credit)
    {
        // 目前顯示分數
        Content_NowScore.text = credit;
        string[] temp = credit.Split('.');
        Cal_NowScore = Convert.ToInt32(temp[0]);
    }

    // exit (check out)
    // ************************************************
    public void OnClick_DoBalanceExchange()
    {
        UIBut_checkout.enabled = false;

        // 如果目前分數大於0
        if (Cal_NowScore > 0)
        {
            //LuaManager_new.Instance().CallLuaFuction("DoBalanceExchange");
        }
        else
        {
            OnBalanceExchange("0", "0", Cal_BalanceMoney);
        }
    }

    // TransCredit 遊戲分數，amount 兌換金額，balance 可用餘額
    public void OnBalanceExchange(string transCredit, string amount, string balance)
    {
        print("OnBalanceExchange " + transCredit + amount + balance);

        Context_Exit_NowScore.text      = transCredit;
        Context_Exit_ExchangeScore.text = amount;
        Context_Exit_Balance.text       = balance;


        // 設定餘額
        Content_BalanceMoney.text = balance;
        Cal_BalanceMoney = balance;

        // 目前顯示分數
        Content_NowScore.text = "0";
        Cal_NowScore = 0;

        // 設定兌換分數
        Content_ExchangeScore.text = "0";
        Cal_ExchangScore = 0;

        // 開啟結算面板
        window_exit.alpha = 1.0f;
    }

    public void OnClick_ResumeGame()
    {
        UIBut_checkout.enabled = true;
        window_exit.alpha = 0.0f;
    }

    public void OnClick_CheckoutQuitGame()
    {

    }
    // ************************************************

    public void OnClick_OpenExchangeRatio()
    {
        window_exchangeratio.alpha = 1.0f;
    }

    public void OnClick_CloseExchangeRatio()
    {
        window_exchangeratio.alpha = 0.0f;

        idx_ratio_temp = idx_ratio;
        exchangewindow_Proportion.text = ratiobases[idx_ratio_temp];
    }

    public void OnClick_ChangeRation_Left()
    {
        idx_ratio_temp--;
        if (idx_ratio_temp < 0)
            idx_ratio_temp = 0;
        else
        {
            exchangewindow_Proportion.text = ratiobases[idx_ratio_temp];
        }
    }

    public void OnClick_ChangeRation_Right()
    {

        idx_ratio_temp++;
        if (idx_ratio_temp == ratiobases.Length)
            idx_ratio_temp = ratiobases.Length - 1;
        else
        {
            exchangewindow_Proportion.text = ratiobases[idx_ratio_temp];
        }
    }

    public void OnClick_MakeSureExchangeRatio()
    {

        if (idx_ratio != idx_ratio_temp)
        {
            idx_ratio = idx_ratio_temp;
            Context_Proportion.text = ratiobases[idx_ratio];

            Content_Slider.value = 0.0f;
        }

        window_exchangeratio.alpha = 0.0f;
    }

    public void OnClick_Tutorial_Left()
    {
        idx_tutorial--;
        if (idx_tutorial < 0)
            idx_tutorial = 0;
        else
        {
            SetTutorialPage(idx_tutorial);
        }
    }

    public void OnClick_Tutorial_Right()
    {
        idx_tutorial++;

        if(idx_tutorial == Tutorial_pages.Length)
            idx_tutorial = Tutorial_pages.Length - 1;
        else
        {
            SetTutorialPage(idx_tutorial);
        }
    }

    public void OnClick_OpenTutorial()
    {
        // Init Page to idx_tutorial
        SetTutorialPage(idx_tutorial);

        window_Tutorial.alpha = 1.0f;
    }

    public void OnClick_CloseTutorial()
    {
        window_Tutorial.alpha = 0.0f;
    }

    private void SetTutorialPage(int idx)
    {
        int size = Tutorial_pages.Length;
        if (idx >= 0 && idx < size)
        {
            // Update idx
            idx_tutorial = idx;

            for (int i = 0; i < size; i++)
            {
                if(i != idx)
                    Tutorial_pages[i].SetActive(false);
                else
                    Tutorial_pages[i].SetActive(true);
            }
        }
    }

    private float RatioStrToFloat(string str)
    {
        string[] values = str.Split(':');

        return (float)(Convert.ToDouble(values[1]) / Convert.ToDouble(values[0]));
    }
}