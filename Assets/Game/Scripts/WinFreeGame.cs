using UnityEngine;

public class WinFreeGame : MonoBehaviour {

    IWinFreeGame2GM IwinFreeGame2GM;

    public UIPanel uiPanel;

    public UILabel label_context;

    public UIButton uibutton;

    // Use this for initialization
    void Start()
    {
        IwinFreeGame2GM = GameManager.Instance;
    }

    public void OpenAndSetContext(string context)
    {
        uibutton.enabled = true;
        uibutton.SetState(UIButtonColor.State.Normal, false);
        label_context.text = context;
        uiPanel.alpha = 1.0f;
    }

    public void OnClick_Close()
    {
        uibutton.enabled = false;
        uibutton.SetState(UIButtonColor.State.Disabled, false);
        uiPanel.alpha = 0.0f;

        IwinFreeGame2GM.OnClick_CloseWinFreeGame();
    }
}
