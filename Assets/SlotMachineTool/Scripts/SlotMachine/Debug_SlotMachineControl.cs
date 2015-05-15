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
            string[] info = new string[15];

            for(int i = 0; i < 15; i++)
            {
                int temp = Random.Range(1, 15);

                info[i] = temp.ToString("000");
            }

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

        if (GUILayout.Button("stop immediately ", GUILayout.Width(100.0f), GUILayout.Height(50.0f)))
        {
            slotmachine.OnClick_StartStop_Immediate();
        }
    }
}
