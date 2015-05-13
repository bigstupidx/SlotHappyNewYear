using UnityEngine;
using System.Collections;

public class Debug_SlotMachineControl : MonoBehaviour {

    public SlotMachine slotmachine;

	// Use this for initialization
	void Start () {
	
	}
	
    void OnGUI()
    {
        if(GUILayout.Button("Spin",GUILayout.Width(100.0f),GUILayout.Height(50.0f)))
        {
            slotmachine.StartSpin();
            string[] info = new string[] {"001", "001", "001", "001", "001",
            "001","001","001","001","001",
            "001","001","001","001","001",
            "001","001","001","001","001",
            "001","001","001","001","001"};
            slotmachine.SetTileSpriteInfo(info);
        }

        if (GUILayout.Button("Start stop Spin", GUILayout.Width(100.0f), GUILayout.Height(50.0f)))
        {
            slotmachine.OnClick_StartStop();
        }

        if (GUILayout.Button("stop Spin", GUILayout.Width(100.0f), GUILayout.Height(50.0f)))
        {
            slotmachine.OnClick_Stop();
        }
    }
}
