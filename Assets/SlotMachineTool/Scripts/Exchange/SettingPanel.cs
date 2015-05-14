using UnityEngine;
using System.Collections.Generic;

public class SettingPanel : MonoBehaviour {

    ISetting2GM Isetting2GM;

    public UIPanel Win_Setting;
    public GameObject GO_UserPanel;

    public UIWidget Win_SelectLanguage;

    public UISprite[] SP_Languages;
    public Material[] Mat_Languages;
    public Texture2D[] Tex_Resource;
    
    private Dictionary<string, bool> Switchs;

	// Use this for initialization
	void Start () {

        Isetting2GM = GameManager.Instance;

        this.OnClick_Close();

        Switchs = new Dictionary<string, bool>();
        Switchs.Add("Win_SelectLanguage", true);

        this.OnClick_ShowLanguageSelect();
        this.OnClick_SL_CN();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick_Open()
    {
        if (Isetting2GM.OpenAllow())
            Open();
    }

    void Open()
    {
        Win_Setting.alpha = 1.0f;
    }

    public void OnClick_Close()
    {
        Win_Setting.alpha = 0.0f;
    }

    public void OnClick_Sound()
    {

    }
    public void OnClick_Music()
    {

    }
    public void OnClick_BetRecord()
    {

    }
    public void OnClick_Rules()
    {

    }
    public void OnClick_Logout()
    {

    }
    public void OnClick_ShowLanguageSelect()
    {
        if(Switchs["Win_SelectLanguage"])
        {
            Switchs["Win_SelectLanguage"] = false;
            Win_SelectLanguage.alpha = 0.0f;
        }
        else
        {
            Switchs["Win_SelectLanguage"] = true;
            Win_SelectLanguage.alpha = 1.0f;
        }
    }

    public void OnClick_SL_TW()
    {
        Localization.language = "TW";
        Mat_Languages[0].mainTexture = Tex_Resource[0];
        Mat_Languages[1].mainTexture = Tex_Resource[3];

        for (int i = 0; i < SP_Languages.Length; i++)
        {
            if (i == 0)
                SP_Languages[i].enabled = true;
            else
                SP_Languages[i].enabled = false;
        }

        GO_UserPanel.SetActive(false);
        GO_UserPanel.SetActive(true);
    }
    public void OnClick_SL_CN()
    {
        Localization.language = "CN";
        Mat_Languages[0].mainTexture = Tex_Resource[1];
        Mat_Languages[1].mainTexture = Tex_Resource[4];

        for (int i = 0; i < SP_Languages.Length; i++)
        {
            if (i == 1)
                SP_Languages[i].enabled = true;
            else
                SP_Languages[i].enabled = false;
        }
        GO_UserPanel.SetActive(false);
        GO_UserPanel.SetActive(true);
    }
    public void OnClick_SL_EN()
    {
        Localization.language = "EN";
        Mat_Languages[0].mainTexture = Tex_Resource[2];
        Mat_Languages[1].mainTexture = Tex_Resource[5];

        for (int i = 0; i < SP_Languages.Length; i++)
        {
            if (i == 2)
                SP_Languages[i].enabled = true;
            else
                SP_Languages[i].enabled = false;
        }
        GO_UserPanel.SetActive(false);
        GO_UserPanel.SetActive(true);
    }
}