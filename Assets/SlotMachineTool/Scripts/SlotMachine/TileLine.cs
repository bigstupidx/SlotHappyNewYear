using UnityEngine;
using System.Collections;

public class TileLine : MonoBehaviour
{

    private float fspeed;

    public TileObject[] tileObjects;

    private bool sw_Move_all;

    private bool sw_Move;
    private bool sw_Break;
    private bool sw_Rebound;

    private float timer_rebound;

    private int cnt_stop = 0;

    public delegate void TL_FinishSpin();

    TL_FinishSpin tilefinishspin;

    // Use this for initialization
    void Start()
    {

        this.sw_Move_all = false;
        this.sw_Break = false;

    }
    
    public void Update_Sync()
    {
        if (sw_Move_all)
        {
            MoveSymbol();
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

    public void MoveSymbol()
    {
        float dis = this.fspeed * Time.deltaTime;

        // 在此判斷為反彈不執行位置更改
        if (!sw_Rebound)
        {
            float pos_new_y;
            for (int idx = 0; idx < 8; idx++)
            {
                pos_new_y = tileObjects[idx].transform.localPosition.y - dis;

                tileObjects[idx].SetPosition(pos_new_y);
            }
        }
        else
            timer_rebound += Time.deltaTime;

        if (!sw_Break)
        {
            if ((tileObjects[0].transform.localPosition.y - dis) < -165.0f)
                Turnround();
        }
        else
        {
            if (!sw_Rebound)
            {
                if (tileObjects[5].transform.localPosition.y <= -50.0f)
                {
                    sw_Rebound = true;
                    timer_rebound = 0.0f;
                }
            }
            else
            {
                if (timer_rebound >= 0.05f)
                {
                    float _pos_new_y;
                    for (int idx = 0; idx < 8; idx++)
                    {
                        _pos_new_y = -825.0f + idx * 165.0f; ;

                        tileObjects[idx].SetPosition(_pos_new_y);
                    }

                    sw_Move_all = false;

                    tilefinishspin();
                }
                else
                {
                    // 執行取得反彈對應位置
                    float pos_y_5 = SlotMachine.moveMethod.GetOutput(timer_rebound);

                    float pos_new_y;
                    for (int idx = 0; idx < 8; idx++)
                    {
                        pos_new_y = pos_y_5 + ((idx - 5) * 165);

                        tileObjects[idx].SetPosition(pos_new_y);
                    }
                }

            }
        }
    }
    /*
    public void Move(int idx)
    {
        float dis = this.fspeed * Time.deltaTime;

        float pos_new , limit;
        if (dir_down[idx])
        {
            pos_new = tileObjects[idx].transform.localPosition.y - dis;
            limit = (-825.0f - 50.0f) + idx * 165.0f;
        }
        else
        {
            pos_new = tileObjects[idx].transform.localPosition.y + dis;
            limit = -825.0f + idx * 165.0f;
        }

        if (sw_Break)
        {

            tileObjects[idx].SetPosition(pos_new);

            if (dir_down[idx])
            {
                if (pos_new <= limit)
                {
                    dir_down[idx] = false;
                }
            }
            else
            {
                if (pos_new >= limit)
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
    */

    public void StartRun()
    {
        sw_Move_all = true;

        sw_Move = true;
        sw_Break = false;
        sw_Rebound = false;

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
        for (int idx = 0; idx < tileObjects.Length; idx++)
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
