using UnityEngine;
using System.Collections.Generic;

public class SettingPanel : MonoBehaviour {

    ISetting2GM Isetting2GM;

    public MusicManager musicMgr;
    public SoundManager soundMgr;

    public UIPanel Win_Setting;
    public GameObject GO_UserPanel;

    public UIWidget Win_SelectLanguage;

    public UISprite[] SP_Languages;
    public Material[] Mat_Languages;
    public Texture2D[] Tex_Resource;

    public GF_ButtonObject[] GF_Buttons;
    public UILocalize[] UILocalizes; 

    private Dictionary<string, bool> Switchs;

	// Use this for initialization
	void Start () {

        Isetting2GM = GameManager.Instance;

        this.OnClick_Close();

        Switchs = new Dictionary<string, bool>();
        Switchs.Add("Win_SelectLanguage", true);
        Switchs.Add("But_Music", true);
        Switchs.Add("But_Sound", true);

        this.OnClick_ShowLanguageSelect();
        this.OnClick_SL_CN();

        // 按鍵初始化
        GF_Buttons[0].SetSpriteStatesName(0, "music_up.PNG", "music_down.PNG", "");
        GF_Buttons[1].SetSpriteStatesName(0, "sound_up.PNG", "sound_down.PNG", "");
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
        if(Switchs["But_Sound"])
        {
            soundMgr.SetVolume(0.0f);

            Switchs["But_Sound"] = false;
            GF_Buttons[1].SetSpriteStatesName(0, "stop_sound_up.PNG", "stop_sound_down.PNG", "");
            GF_Buttons[1].gameObject.SetActive(false);
            GF_Buttons[1].gameObject.SetActive(true);
            UILocalizes[1].key = "SoundOff";
            UILocalizes[1].enabled = false;
            UILocalizes[1].enabled = true;

        }
        else
        {
            soundMgr.SetVolume(1.0f);

            Switchs["But_Sound"] = true;
            GF_Buttons[1].SetSpriteStatesName(0, "sound_up.PNG", "sound_down.PNG", "");
            GF_Buttons[1].gameObject.SetActive(false);
            GF_Buttons[1].gameObject.SetActive(true);
            UILocalizes[1].key = "SoundOn";
            UILocalizes[1].enabled = false;
            UILocalizes[1].enabled = true;

        }
    }

    public void OnClick_Music()
    {
        if (Switchs["But_Music"])
        {
            musicMgr.SetVolume(0.0f);

            Switchs["But_Music"] = false;
            GF_Buttons[0].SetSpriteStatesName(0, "stop_music_up.PNG", "stop_music_down.PNG", "");
            GF_Buttons[0].gameObject.SetActive(false);
            GF_Buttons[0].gameObject.SetActive(true);
            UILocalizes[0].key = "MusicOff";
            UILocalizes[0].enabled = false;
            UILocalizes[0].enabled = true;
        }
        else
        {
            musicMgr.SetVolume(1.0f);

            Switchs["But_Music"] = true;
            GF_Buttons[0].SetSpriteStatesName(0, "music_up.PNG", "music_down.PNG", "");
            GF_Buttons[0].gameObject.SetActive(false);
            GF_Buttons[0].gameObject.SetActive(true);
            UILocalizes[0].key = "MusicOn";
            UILocalizes[0].enabled = false;
            UILocalizes[0].enabled = true;
        }

    }
    public void OnClick_BetRecord()
    {

    }
    public void OnClick_Rules()
    {

    }
    public void OnClick_Logout()
    {
        Isetting2GM.Logout();
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