using UnityEngine;
using System.Collections;

public class lineNumberRotate : MonoBehaviour {
	// 半徑
	float R = 220.0f;
	
	// 角度
	public float theta = 0.0f;
	private float rotate_speed = 250;
	public LineWheel m_linewheel ;
	
	private UILabel m_Label_01;
	private UILabel m_Label_02;
	private UILabel m_Label_03;
	private UILabel m_Label_04;
	private UILabel m_Label_05;
	private UILabel m_Label_06;
	private UILabel m_Label_07;
	private UILabel m_Label_08;

	public UIFont m_font_R ;
	public UIFont m_font_O ;
	// Use this for initialization
	void Start () {
		switch(this.gameObject.name){
		case "line_label_01":
			theta = 45;
			break;
/*		case "bet_label_02":
			theta = 180;
			break;
		case "bet_label_03":
			theta = 252;
			break;
		case "bet_label_04":
			theta = 324;
			break;
		case "bet_label_05":
			theta = 36;
			break;*/
			
		}

		m_linewheel = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel").GetComponent<LineWheel>();
		m_Label_01 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_01").GetComponent<UILabel>();
		m_Label_02 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_02").GetComponent<UILabel>();
		m_Label_03 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_03").GetComponent<UILabel>();
		m_Label_04 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_04").GetComponent<UILabel>();
		m_Label_05 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_05").GetComponent<UILabel>();
		m_Label_06 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_06").GetComponent<UILabel>();
		m_Label_07 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_07").GetComponent<UILabel>();
		m_Label_08 = GameObject.Find ("UI Root/Streatch/UserPanel/SelectWheel/SelectLine/Line_Selection/line_label_08").GetComponent<UILabel>();
		m_font_R = m_Label_02.bitmapFont;
		m_font_O = m_Label_05.bitmapFont;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Rotate_Up(float tmp_speed){
		float x = this.R * Mathf.Cos(Mathf.Deg2Rad * theta);
		float y = this.R * Mathf.Sin(Mathf.Deg2Rad * theta);
		
		this.gameObject.transform.localPosition = new Vector3(x, y, 0.0f);
		
		this.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, theta));

		numberUp (m_Label_02,315f);
		numberUp (m_Label_03,270f);
		numberUp (m_Label_04,225f);
		numberUp (m_Label_05,180f);
		numberUp (m_Label_06,135f);
		numberUp (m_Label_07,90f);
		numberUp (m_Label_08,45f);

		theta += tmp_speed * Time.deltaTime;
	
		if (theta > 360.0f)
			theta = 0;

		if (theta < 292.5f && theta > 247.5f)
			m_linewheel.Change_Number ("line_label_01");
        else if (theta < 337.5f && theta > 292.5f)
			m_linewheel.Change_Number ("line_label_02");
        else if (theta < 22.5f && theta > 0f || theta < 360f && theta > 337.5f)
			m_linewheel.Change_Number ("line_label_03");
        else if (theta < 67.5f && theta > 22.5f )
			m_linewheel.Change_Number ("line_label_04");
        else if (theta < 112.5f && theta > 67.5f )
			m_linewheel.Change_Number ("line_label_05");
        else if (theta < 157.5f && theta > 112.5f)
			m_linewheel.Change_Number ("line_label_06");
        else if (theta < 202.5f && theta > 157.5f )
			m_linewheel.Change_Number ("line_label_07");
        else if (theta < 247.5f && theta > 202.5f)
			m_linewheel.Change_Number ("line_label_08");

		//---Color

		if (theta < 22.5f && theta > 0f || theta < 360f && theta > 337.5f)
			Change_Color_R("line_label_01");
		else if (theta < 67.5f && theta > 22.5f)
			Change_Color_R("line_label_02");
        else if (theta < 112.5f && theta > 67.5f)
			Change_Color_R("line_label_03");
        else if (theta < 157.5f && theta > 112.5f)
			Change_Color_R("line_label_04");
        else if (theta < 202.5f && theta > 157.5f )
			Change_Color_R("line_label_05");
        else if (theta < 247.5f && theta > 202.5f)
			Change_Color_R("line_label_06");
        else if (theta < 292.5f && theta > 247.5f)
			Change_Color_R("line_label_07");
        else if (theta < 337.5f && theta > 292.5f)
			Change_Color_R("line_label_08");
	
	}

	void numberUp(UILabel m_UIlabel,float addtheta){
		float x = this.R * Mathf.Cos(Mathf.Deg2Rad * (theta+addtheta));
		float y = this.R * Mathf.Sin(Mathf.Deg2Rad * (theta+addtheta));
		m_UIlabel.gameObject.transform.localPosition = new Vector3 (x,y,0.0f);
		m_UIlabel.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, theta+addtheta));
	}

	public void Rotate_Down(float tmp_speed){
		float x = this.R * Mathf.Cos(Mathf.Deg2Rad * theta);
		float y = this.R * Mathf.Sin(Mathf.Deg2Rad * theta);
		
		this.gameObject.transform.localPosition = new Vector3(x, y, 0.0f);
		
		this.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, theta));
		theta -= tmp_speed * Time.deltaTime;

		numberUp (m_Label_02,315f);
		numberUp (m_Label_03,270f);
		numberUp (m_Label_04,225f);
		numberUp (m_Label_05,180f);
		numberUp (m_Label_06,135f);
		numberUp (m_Label_07,90f);
		numberUp (m_Label_08,45f);

		if (theta < 0f)
			theta = 360.0f;

		if (theta < 112.5f && theta > 67.5f)
			m_linewheel.Change_Number_Down ("line_label_01");
		if (theta < 157.5f && theta > 112.5f)
			m_linewheel.Change_Number_Down ("line_label_02");
		if (theta < 202.5f && theta > 157.5f )
			m_linewheel.Change_Number_Down ("line_label_03");
		if (theta < 247.5f && theta > 202.5f )
			m_linewheel.Change_Number_Down ("line_label_04");
		if (theta < 292.5f && theta > 247.5f )
			m_linewheel.Change_Number_Down ("line_label_05");
		if (theta < 337.5f && theta > 292.5f)
			m_linewheel.Change_Number_Down ("line_label_06");
		if (theta < 22.5f && theta > 0f || theta < 360f && theta > 337.5f)
			m_linewheel.Change_Number_Down ("line_label_07");
		if (theta < 67.5f && theta > 22.5f)
			m_linewheel.Change_Number_Down ("line_label_08");

		//---Color
		if (theta < 22.5f && theta > 0f || theta < 360f && theta > 337.5f)
			Change_Color_R("line_label_01");
		if (theta < 67.5f && theta > 22.5f)
			Change_Color_R("line_label_02");
		if (theta < 112.5f && theta > 67.5f)
			Change_Color_R("line_label_03");
		if (theta < 157.5f && theta > 112.5f)
			Change_Color_R("line_label_04");
		if (theta < 202.5f && theta > 157.5f )
			Change_Color_R("line_label_05");
		if (theta < 247.5f && theta > 202.5f)
			Change_Color_R("line_label_06");
		if (theta < 292.5f && theta > 247.5f)
			Change_Color_R("line_label_07");
		if (theta < 337.5f && theta > 292.5f)
			Change_Color_R("line_label_08");
	}

	//顏色變紅
	public void Change_Color_R(string str_tmp){

		switch(str_tmp){
		case "line_label_01":
			m_Label_01.bitmapFont = m_font_R;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_02":
			m_Label_02.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_03":
			m_Label_03.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_04":
			m_Label_04.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_05":
			m_Label_05.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_06":
			m_Label_06.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_07":
			m_Label_07.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			m_Label_08.bitmapFont = m_font_O;
			break;
		case "line_label_08":
			m_Label_08.bitmapFont = m_font_R;
			m_Label_01.bitmapFont = m_font_O;
			m_Label_02.bitmapFont = m_font_O;
			m_Label_03.bitmapFont = m_font_O;
			m_Label_04.bitmapFont = m_font_O;
			m_Label_06.bitmapFont = m_font_O;
			m_Label_07.bitmapFont = m_font_O;
			m_Label_05.bitmapFont = m_font_O;
			break;
		}
	}
}
