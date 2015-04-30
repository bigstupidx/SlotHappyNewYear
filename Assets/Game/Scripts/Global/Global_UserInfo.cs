using UnityEngine;
using System.Collections;

public class Global_UserInfo {

    public struct onloadInfo
    {
        public string balance ;
        public string defaultBase;
        public string loginName;
        public string Base ;

        public onloadInfo(string _balance,string _defaultbase,string _loginname,string _base)
        {
            balance = _balance; defaultBase = _defaultbase; loginName = _loginname; Base = _base;
        }
    }

    // 儲存loading 時呼叫 onloadinfo 回傳的資料
    public static onloadInfo Gl_onloadInfo;

}