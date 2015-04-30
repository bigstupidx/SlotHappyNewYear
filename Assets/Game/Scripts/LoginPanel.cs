using UnityEngine;
using System.Collections;

public class LoginPanel : MonoBehaviour {

    public LoginManager loginManager;

    public TweenPosition tween_Pos;

    public UIToggle toggle_StoreAcc;
    
    private bool sw_StoreAccount;
    
	// Use this for initialization
	void Start () {

        // 檢查儲存的帳號
        UIInput Acc_keyin = GameObject.Find("ACC_Keyin").GetComponent<UIInput>();
        string str_user_acc = "";
        str_user_acc =  PlayerPrefs.GetString("USER_ACC");

        if (string.IsNullOrEmpty(str_user_acc))
        {
            print("Null");
            sw_StoreAccount = false;
            toggle_StoreAcc.value = false;
        }
        else
        {
            sw_StoreAccount = true;
            toggle_StoreAcc.value = true;
            Acc_keyin.value = str_user_acc;
        }
    }

    // 檢查背景點擊
    public void OnClick_TouchBackGround()
    {
        Transform trans = gameObject.transform;
        tween_Pos.from = new Vector3(0.0f, trans.localPosition.y, 0.0f);
        tween_Pos.to = new Vector3(0.0f, 0.0f, 0.0f);

        tween_Pos.PlayForward();
    }
	    
    public void OnClick_Login(string str_acc, string str_pw)
    {

        if(!string.IsNullOrEmpty(str_acc) && !string.IsNullOrEmpty(str_pw))
        {
            // 如果需要儲存密碼
            if(sw_StoreAccount)
            {
                PlayerPrefs.SetString("USER_ACC", str_acc);
            }
            else
            {
                PlayerPrefs.DeleteKey("USER_ACC");
            }
        }

        loginManager.OnClick_UserLogin(str_acc, str_pw);
    }

    public void OnClick_StoreAccount()
    {
        if(sw_StoreAccount)
            sw_StoreAccount = false;
        else
            sw_StoreAccount = true;
    }

    public void OnKeyinAccount()
    {
        tween_Pos.from = new Vector3(0.0f, 0.0f, 0.0f);
        tween_Pos.to = new Vector3(0.0f, 125.0f, 0.0f);

        tween_Pos.PlayForward();
    }

    public void OnKeyinPW()
    {
        tween_Pos.from = new Vector3(0.0f, 0.0f, 0.0f);
        tween_Pos.to = new Vector3(0.0f, 125.0f, 0.0f);

        tween_Pos.PlayForward();
    }
}