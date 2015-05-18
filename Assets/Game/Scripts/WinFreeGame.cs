using UnityEngine;

public class WinFreeGame : MonoBehaviour {

    IWinFreeGame2GM IwinFreeGame2GM;

    public UIPanel uiPanel;

    public UILabel label_context;


    // Use this for initialization
    void Start()
    {
        IwinFreeGame2GM = GameManager.Instance;
    }

    public void OpenAndSetContext(string context)
    {
        label_context.text = context;
        uiPanel.alpha = 1.0f;
    }

    public void OnClick_Close()
    {
        uiPanel.alpha = 0.0f;

        IwinFreeGame2GM.OnClick_CloseWinFreeGame();
    }
}
