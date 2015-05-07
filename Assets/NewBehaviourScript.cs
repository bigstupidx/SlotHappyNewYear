using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    public SlotMachine sm;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GUILayout.Button("aaaaaaaaaaaaaaab"))
        {
            //sm.OnClick_StartRun();
        }
        if (GUILayout.Button("aaaaaaaaaaaaaaab"))
        {
            string[] info = {"001","001","001","001","001",
            "001","001","001","001","001",
            "001","001","001","001","001",
            "001","001","001","001","001",
            "001","001","001","001","001",};


            sm.SetTileSpriteInfo(info);
            sm.OnClick_StartStop();
        }
    }
}
