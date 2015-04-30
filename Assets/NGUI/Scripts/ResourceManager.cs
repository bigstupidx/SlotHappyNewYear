using UnityEngine;
using System.Collections;

public class ResourceManager : MonoBehaviour {

    public TextAsset text_csv;

    public GameObject[] gas_resource;

    public Texture2D[] texs_resource;

    static ResourceManager _instance = null;

    void Awake()
    {
        Localization.language = "TW";
    }

    static public ResourceManager Instance()
    {

        if (_instance == null)
        {
            _instance = GameObject.Find("Manager").GetComponent<ResourceManager>();
            return _instance;
        }
        else
            return _instance;

    }

    public GameObject Instantiate(GameObject ga_parent ,int idx,Vector3 pos)
    {

        GameObject ga = Instantiate(gas_resource[idx], Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        ga.transform.parent = ga_parent.transform;
        ga.transform.localPosition = pos;
        ga.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        return ga;
    }

    // 回傳多國語的 CSV
    public TextAsset GetTextAsset()
    {
        return text_csv;
    }

}
