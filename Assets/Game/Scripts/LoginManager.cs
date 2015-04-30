using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security.Cryptography;
using System.Text;

using LitJson;

public class LoginManager : MonoBehaviour
{

    string[] esball_ips = { "bm.ago545q6e5.com" ,
                            "bm.efr465dwd789.com",
                            "bm.kkmfdfv.com",
                            "bm.gjje18644cc.com",
                            "bm.tjefjjvmcw.com",
                            "bm.bo558z7cv5.com",
                            "bm.acga778qmz579.com",
                            "bm.ddhqywa.com",
                            "bm.zooe19734qq.com",
                            "bm.hgznfgqkgie.com"
                            };

    Dictionary<string, string> dic_errortable_tw;
    
    public GameObject prefab_msg;

    public GF_Window window_loginMsg;

    // button 防呆
    bool sw_allowbut_login;

    void Awake()
    {

        // 禁止休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        sw_allowbut_login = true;

        // 建立錯誤訊息表
        dic_errortable_tw = new Dictionary<string, string>();

        dic_errortable_tw.Add("90002", "會員帳號或密碼錯誤");
        dic_errortable_tw.Add("90004", "會員凍結");
        dic_errortable_tw.Add("90003", "網站維護中");
        dic_errortable_tw.Add("90005", "會員停用");
        dic_errortable_tw.Add("90006", "30秒重覆登入");
        dic_errortable_tw.Add("90007", "會員停權");
        dic_errortable_tw.Add("90015", "廳主DomainCode不存在");
        //dic_errortable_tw.Add("90018", "");
        dic_errortable_tw.Add("44001", "參數未帶齊");
        dic_errortable_tw.Add("22000", "使用者未登入");
        dic_errortable_tw.Add("22001", "使用者登出");

        window_loginMsg.Close();
    }


    public void OnClick_UserLogin(string str_acc , string str_pw)
    {

        if (sw_allowbut_login)
        {
            // 不允許重複按
            sw_allowbut_login = false;
            
            string[] infos = str_acc.Split('@');
            int rand = Random.Range(0, 9);

            // 檢查帳號格式
            if (infos.Length == 2 && str_pw.Length != 0)
            {
                MD5 md5Hash = MD5.Create();

                //string secret_key = GetMd5Hash(md5Hash, "Domain20150427");
                //print("secret_key" + secret_key);
                string url = "http://" + esball_ips[rand] + "/amfphp/json.php/Member.getDomainList/" + infos[1];
                //string url = "http://bm.efr465dwd789.com/app/WebService/view/display.php/GetDomainList?domaincode=" + infos[1] + "&secret_key=" + secret_key;

                print("url " + url);

                window_loginMsg.Open();
                window_loginMsg.SetContext("GetDomin.. ");
                StartCoroutine(GetDomain(url , str_acc , str_pw));
            }
            else
            {
                window_loginMsg.Close();
                GenerateMSG_Window("錯誤的格式");
                sw_allowbut_login = true;
                print("錯誤的格式");
            }
        }
    }

    IEnumerator GetDomain(string url , string str_acc, string str_pw)
    {

        print("GetDomain");
        print("url is " + url);
        using (WWW www = new WWW(url))
        {

            yield return www;

            if (www.error != null)
            {
                window_loginMsg.Close();
                GenerateMSG_Window("getdomain.error is " + www.error);
                sw_allowbut_login = true;
                print("getdomain.error is " + www.error);
            }
            else
            {
                print(www.text);

                if (www.text != "Durian ERROR!")
                {
                    JsonData jd = JsonMapper.ToObject(www.text);

                    ICollection i_keys = ((IDictionary)(jd["domain"])).Keys;
                    string[] strs = new string[2];
                    i_keys.CopyTo(strs, 0);

                    string domain = jd["domain"][strs[0]][0].ToString();

                    string url_esball = "https://" + domain + "/amfphp/json.php/Member.getServerIp";


                    window_loginMsg.AddContext("Success.\nGetIP.. ");
                    StartCoroutine(GetIP(url_esball,str_acc,str_pw));
                }
                else
                {
                    window_loginMsg.Close();
                    GenerateMSG_Window("Server 斷線");
                    sw_allowbut_login = true;
                    print("Server 斷線");
                }
            }
        }
    }

    IEnumerator GetIP(string url,string str_acc , string str_pw)
    {
        print("GetIP");
        using (WWW www = new WWW(url))
        {

            yield return www;

            if (www.error != null)
            {
                window_loginMsg.Close();
                GenerateMSG_Window("getdomain.error is " + www.error);
                sw_allowbut_login = true;
                print("getdomain.error is " + www.error);
            }
            else
            {

                print(www.text);

                JsonData jd = JsonMapper.ToObject(www.text);
                string ip_esball = jd["data"]["ip"].ToString();

                RtmpC2S.hallid = (jd["data"]["HallID"]).ToString();
                
                string[] infos = str_acc.Split('@');

                string user_acc = infos[0];
                string domaincode = infos[1];

                string url_getsid = "http://bm.esballgame.com" +
                "/app/WebService/view/display.php/MobileLogin?username=" + user_acc +
                "&password=" + str_pw +
                "&domaincode=" + domaincode +
                "&ip=" + ip_esball +
                "&platform=iPhone";

                window_loginMsg.AddContext("Success.\nGetSid.. ");
                StartCoroutine(GetSid(url_getsid, ip_esball));
            }
        }
    }

    IEnumerator GetSid(string url, string ip)
    {

        print("GetSid");
        using (WWW www = new WWW(url))
        {

            yield return www;

            if (www.error != null)
            {
                window_loginMsg.Close();
                GenerateMSG_Window("GetSid.error is " + www.error);
                sw_allowbut_login = true;
                print("GetSid.error is " + www.error);
            }
            else
            {
                print(www.text);

                JsonData jd = JsonMapper.ToObject(www.text);

                if ((bool)jd["result"])
                {

                    string sid = "";
                    sid = jd["data"]["session_token"].ToString();

                    RtmpC2S.userid = (jd["data"]["UserID"]).ToString();

                    if (sid != "")
                    {

                        print("sid is " + sid);

                        RtmpC2S.sid = sid;
                        RtmpC2S.ip = ip;
                        Application.LoadLevel("Loading");
                    }

                }
                else
                {
                    window_loginMsg.Close();
                    sw_allowbut_login = true;
                    ShowGetSIDError(jd);
                }
            }
        }
    }

    void ShowGetSIDError(JsonData jd)
    {


        string str_error;
        string key = (jd["data"]["Code"]).ToString();

        dic_errortable_tw.TryGetValue(key, out str_error);

        if (string.IsNullOrEmpty(str_error))
        {
            string str = jd["data"]["Code"] + " Message is " + jd["data"]["Message"];
            GenerateMSG_Window(str);
        }
        else
            GenerateMSG_Window(str_error);
    }

    void GenerateMSG_Window(string str)
    {

        GameObject ga_coverpanel = GameObject.Find("CoverPanel");
        GameObject ga = Instantiate(
            prefab_msg,
            new Vector3(0.0f, 0.0f, 0.0f),
            Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        ga.transform.parent = ga_coverpanel.transform;
        GF_Window window = ga.GetComponent("GF_Window") as GF_Window;

        window.SetContext(str);

    }

    string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}