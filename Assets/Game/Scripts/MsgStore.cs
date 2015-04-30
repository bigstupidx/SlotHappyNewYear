using UnityEngine;
using System.Collections;

using System.IO;

public class MsgStore : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Store(string msg)
    {

        string path = Application.persistentDataPath + "/MsgStore";

        if (!System.IO.Directory.Exists(path))
        {

            System.IO.Directory.CreateDirectory(path);

            if (!System.IO.Directory.Exists(path))
                Debug.Log("CreateDirectory Failed !");
            else
            {
                Debug.Log("CreateDirectory Success !");


                FileInfo file = new FileInfo(path + "/MsgStore.txt");

                if (file.Exists)
                {

                    file.Create();
                    Debug.Log("Create file Success !");
                }
            }
        }

        using (FileStream fs = new FileStream(path + "/MsgStore.txt", FileMode.Append))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {

                sw.WriteLine(msg);
            }
        }
    }
    public void DeleteStroe()
    {
        string path = Application.persistentDataPath + "/MsgStore";

        DirectoryInfo di = new DirectoryInfo(path);

        if (di.Exists)
        {
            di.Delete(true);
            Debug.Log("Delete ALL");
        }
    }      
        
}
