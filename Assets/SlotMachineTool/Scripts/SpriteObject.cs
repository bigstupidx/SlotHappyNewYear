using UnityEngine;
using System.Collections;

public class SpriteObject : MonoBehaviour
{

    private UIBasicSprite uisprite;

    void Awake()
    {
        uisprite = gameObject.GetComponent<UIBasicSprite>();
        if (uisprite == null)
            Debug.Log("Null !");
    }

	// Use this for initialization
	void Start () 
    {

        Close();
	}
	
    public void Open()
    {
        uisprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void Close()
    {
        uisprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

}
