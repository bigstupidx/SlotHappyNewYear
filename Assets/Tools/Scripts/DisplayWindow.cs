using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using System.Text;

public class DisplayWindow : MonoBehaviour {

    public int dis_width;
    public int dis_height;

    private int dis_max_width, dis_max_height;

    public UIScrollBar uiscrollbar;
    public UILabel uilabel_display;

    private List<string> list_strs;

    void Awake()
    {
        list_strs = new List<string>();

        dis_max_width = dis_width / 10;
        dis_max_height = dis_height / 20;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DeleteBehind()
    {
        if(list_strs.Count >= 1)
        {
            list_strs.Remove(list_strs[0]);
        }
        Update_DisPlay();
        uiscrollbar.value = 0.0f;
    }

    public void AddString(string str)
    {
        // Debug.Log("*********AddString " + str);

        int mover = 0;
        int len_sub = 0;
        int len_limit = str.Length;
        bool sw_loop = true;
        while (sw_loop)
        {
            len_sub = dis_max_width;
            if (len_sub >= len_limit - mover)
            {
                len_sub = len_limit - mover;
                sw_loop = false;
            }
            list_strs.Add(str.Substring(mover, len_sub));

            mover += len_sub;
        }

        // Debug.Log("now len " + list_strs.Count);

        Update_DisPlay();
        uiscrollbar.value = 1.0f;
    }

    public void Update_DisPlay()
    {
        // cal size of uiscrollbar
        int len_strs = list_strs.Count ;
       
        int pos_start = 0, pos_end = 0;
        if(len_strs <=  dis_max_height)
        {
            uiscrollbar.barSize = 1.0f;

            pos_start = 0;
            pos_end = len_strs;
        }
        else
        {
            float fsize = (float)dis_max_height / len_strs;
            uiscrollbar.barSize = fsize;

            //float uibar_size = uiscrollbar.barSize;
            float uibar_value = uiscrollbar.value;

            float ratio = (len_strs - dis_max_height);

            pos_start = (int)(((float)uibar_value * ratio) + 0.1f) ;
            pos_end = pos_start + dis_max_height;
            
            if (pos_end > len_strs)
                pos_end = len_strs;
        }
        uilabel_display.text = this.Get_DisString(pos_start, pos_end);
        
    }

    private string Get_DisString(int start,int end)
    {
        string str_ret = "";
        for (int i = start; i < end; i++)
            str_ret += list_strs[i] + "\n";

        //Debug.Log("start is " + start + " end is  " + end + " len is " + list_strs.Count);
        return str_ret;
    }

}
