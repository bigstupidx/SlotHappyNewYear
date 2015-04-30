using UnityEngine;
using System.Collections;

public class TileLine : MonoBehaviour {

    private float fspeed;


    public TileObject[] tileObjects;

    private bool sw_Move_all;

    private bool[] sw_Move;
    private bool sw_Break;
    
    private bool btunround;
    private int cnt_stop = 0;

    public delegate void TL_FinishSpin();

    TL_FinishSpin tilefinishspin;

    // Use this for initialization
    void Start () {
        //this.sw_Move = false;

        sw_Move = new bool[8];

        this.sw_Move_all = false;
        this.sw_Break = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (sw_Move_all)
        {
            for (int i = 0; i < 8; i++)
            {
                if(sw_Move[i])
                    this.Move(i);
            }

            if (btunround)
            {
                Turnround();
                btunround = false;
            }
        }
	}

    public void SetSpeed(float speed)
    {
        this.fspeed = speed;
    }

    public float GetSpeed()
    {
        return this.fspeed;
    }

    public void Move(int idx)
    {
        float dis = this.fspeed * Time.deltaTime;

        float pos_new = tileObjects[idx].transform.localPosition.y - dis;
        float limit = -825.0f + idx * 165.0f;

        if (sw_Break)
        {
            if (pos_new <= limit)
            {
                tileObjects[idx].SetPosition(limit);

                sw_Move[idx] = false;
                cnt_stop++;

                if (cnt_stop == 8)
                {
                    sw_Move_all = false;
                    tilefinishspin();
                }
            }
            else
            {
                tileObjects[idx].SetPosition(pos_new);
            }
        }
        else
        {
            tileObjects[idx].SetPosition(pos_new);

            if (pos_new < -165.0f)
            {
                btunround = true;
            }

        }
    }

    public void StartRun()
    {
        sw_Move_all = true;

        for (int i = 0; i < 8; i++)
        {
            sw_Move[i] = true;
        }

        this.sw_Break = false;
        this.btunround = false;
        cnt_stop = 0;
    }
    public void StopRun(TL_FinishSpin finishspin)
    {

        this.sw_Break = true;
        tilefinishspin = new TL_FinishSpin(finishspin);
    }

    public void SetSprites(string[] idxs)
    {
        for (int i = 7, j = 0; j < idxs.Length; i--, j++)
            tileObjects[i].SetSprite(idxs[j]);
    }

    // 5 6 7 不動
    private void ResetPosition()
    {
        TileObject[] temps = new TileObject[8];

        for (int i = 0; i < 8; i++)
        {
            temps[i] = tileObjects[i];
        }

        for (int i = 0; i < 3; i++)
        {
            tileObjects[i] = temps[i + 5];
        }

        for (int i = 0; i < 5; i++)
        {
            tileObjects[i + 3] = temps[i];
            tileObjects[i + 3].SetPosition(495.0f + 165.0f * i);
        }

    }

    // 處理最前面掉頭(不使用List)
    private void Turnround()
    {
        TileObject temp = tileObjects[0];
        for (int idx = 0; idx < tileObjects.Length ; idx++)
        {
            if (idx != tileObjects.Length - 1)
                tileObjects[idx] = tileObjects[idx + 1];
            else
            {
                //print(idx + " " + temp.name);
                tileObjects[idx] = temp;
                float pos_y = tileObjects[idx - 1].transform.localPosition.y;
                tileObjects[idx].SetPosition(pos_y + 150.0f);
            }
        }
        /*
        string str = "";
        for (int i = 0; i < tileObjects.Length; i++)
            str += tileObjects[i].name + "\n";

        print(str);
        */
    }
}
