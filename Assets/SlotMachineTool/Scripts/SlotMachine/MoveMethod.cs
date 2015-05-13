
using System;
using UnityEngine;

public abstract class MoveMethod
{

    public abstract void Init(params object[] param);

    public abstract float GetOutput(float t);     
}

// y = m * x^2
public class XplusXMethod:MoveMethod
{
    float y_start;
    float y_end;
    float delta_y;
    float slope;
    float t_ratio;

    public override void Init(params object[] param)
    {
        y_start = (float)param[0];
        y_end = (float)param[1];
        delta_y = y_end - y_start;
        float costtime = (float)param[2];

        slope = (y_end - y_start) / costtime;

        t_ratio = 1 / costtime;
    }

    public override float GetOutput(float t)
    {
        t = t_ratio * t;
        return y_start + (delta_y  * t * t);
    }
}



