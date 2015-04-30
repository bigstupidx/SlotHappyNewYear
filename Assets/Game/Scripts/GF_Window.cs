using UnityEngine;
using System.Collections;

public class GF_Window : MonoBehaviour {

    public UILabel label_context;
    public UIWidget uiwidth;

    public void SetContext(string str)
    {
        label_context.text = str;
    }

    public void AddContext(string str)
    {

        label_context.text += str;
    }

    public void DestroyWindow()
    {
        Destroy(gameObject);
    }

    public void Open()
    {
        uiwidth.alpha = 1.0f;
    }

    public void Close()
    {
        uiwidth.alpha = 0.0f;
    }

}
