using UnityEngine;
using System.Collections;

public class wheel : MonoBehaviour {

    public bool bCan_Start;

    bool sw_move;
    bool bBreak;
    bool bCountDown;

    public float fspeed;

    int SpecifyArea;
    // 
    int SpecifyNum;
    float SpecifyPos;

    float cnt_times;
    public float waittime;

    float[] table = { 15.0f, 45.0f,75.0f };

	// Use this for initialization
	void Start () {

        bCan_Start = true;
        bBreak = false;

    }
	
	// Update is called once per frame
	void Update () {        


        if(bCountDown)
        {
            cnt_times += Time.deltaTime;

            if(cnt_times > waittime)
            {
                bCountDown = false;
                bBreak = true;

                this.SpecifyArea = GetNextArea();
                this.SpecifyPos = CalStopPosition(this.SpecifyNum, this.SpecifyArea);
            }
        }

        if (sw_move)
        {
            float dz = fspeed * Time.deltaTime;
            float old_z = gameObject.transform.localRotation.eulerAngles.z;
            float next_z = old_z + dz;
            if (next_z >= 360.0f)
                next_z -= 360.0f;

            if (bBreak)
            {

                if(SpecifyArea == GetNowArea())
                {
                    if(next_z >= this.SpecifyPos)
                    {
                        gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, this.SpecifyPos);

                        sw_move = false;
                    }
                    else
                    {
                        // 直接換位置
                        gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, next_z);
                    }
                }
                else
                {
                    // 直接換位置
                    gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, next_z);
                }
            }
            else
            {
                // 直接換位置
                gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, next_z);
            }
        }
	}

    public void OnClick_Start()
    {
        bCan_Start = false;

        sw_move = true;
        bBreak = false;
        bCountDown = false;

    }

    public void OnClick_Stop()
    {
        bCan_Start = true;
        //bBreak = true;
        bCountDown = true;
        cnt_times = 0.0f;

    }

    public void SetSpeed(float value)
    {
        fspeed = value;
    }

    public void SetSpecifyNum(int value)
    {
        SpecifyNum = value;
    }

    int GetNowArea()
    {

        float pos_z = gameObject.transform.localRotation.eulerAngles.z;

        // id_area 為下一區域
        if (pos_z >= 0.0f && pos_z < 90.0f)
        {
            //id_area = 0;
            return 0;
        }
        else if (pos_z >= 90.0f && pos_z < 180.0f)
        {
            return 1;
        }
        else if (pos_z >= 180.0f && pos_z < 270.0f)
        {
            return 2;
        }
        else if (pos_z >= 270.0f && pos_z <= 360.0f)
        {
            return 3;
        }

        return 5;

    }

    int GetNextArea()
    {

        float pos_z = gameObject.transform.localRotation.eulerAngles.z;

        int id_next_area = -1;

        // id_area 為下一區域
        if (pos_z >= 0.0f && pos_z < 90.0f)
        {
            //id_area = 0;
            id_next_area = 1;
        }
        else if (pos_z >= 90.0f && pos_z < 180.0f)
        {
            id_next_area = 2;
        }
        else if (pos_z >= 180.0f && pos_z < 270.0f)
        {
            id_next_area = 3;
        }
        else if (pos_z >= 270.0f && pos_z < 360.0f)
        {
            id_next_area = 0;
        }
        else
        {
            return -1;
        }

        return id_next_area;

    }

    // 回傳對應位置
    float CalStopPosition(int specifynum, int id_area)
    {
        return table[specifynum] + (90.0f * id_area);
    }   
}