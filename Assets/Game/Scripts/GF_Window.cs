using UnityEngine;

public class GF_Window : MonoBehaviour {

    public UILabel label_context;
    public UILabel label_title;

    public UIWidget uiwidth;
    public UIPanel uipanel;

    public void SetContext(string str)
    {
        label_context.text = str;
    }

    public void AddContext(string str)
    {

        label_context.text += str;
    }

    public void SetTitle(string str)
    {
        label_title.text = str;
    }
    

    public void DestroyWindow()
    {
        Destroy(gameObject);
    }

    public void Open()
    {
        if (uiwidth != null)
            uiwidth.alpha = 1.0f;

        if (uipanel != null)
            uipanel.alpha = 1.0f;
    }

    public void Close()
    {
        if (uiwidth != null)
            uiwidth.alpha = 0.0f;

        if (uipanel != null)
            uipanel.alpha = 0.0f;
    }

}
