using UnityEngine;
using System.Collections;

public class myPlugin3 : MonoBehaviour {

	#if UNITY_ANDROID
	public static void CallAndroidRtmpDo(string SetSid, string SetRTMPlink){
		
		
		using (AndroidJavaClass javaClass = new AndroidJavaClass("bbin.mobile.i0001.Main"))
		{
			using (AndroidJavaObject activity = javaClass.GetStatic<AndroidJavaObject>("mContext"))
			{
				activity.Call("callRTMPdo",SetSid, SetRTMPlink);
			}
		}
		
	}

	public static void CallAndroidStatic4(params object[] API){
		
		
		using (AndroidJavaClass javaClass = new AndroidJavaClass("bbin.mobile.i0001.Main"))
		{
			javaClass.CallStatic("testCall",API);
		}
		
	}
	#endif
}
