#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
#region BeginFadeGroup
//using UnityEditor.AnimatedValues;
#endregion
public class TestFile : EditorWindow
{
//	[MenuItem("SuperGame/Test/UnityLogShow")]
	private static void FilePathLog()
	{
		Debug.Log("persistentData:\n" +Application.persistentDataPath);
		Debug.Log("data\n" +Application.dataPath);
		Debug.Log("temporaryCache\n" +Application.temporaryCachePath);
		Debug.Log("streamingAssets\n" +Application.streamingAssetsPath);

		string aAssWord = "/Assets";
		int aStarIndex = Application.dataPath.Length - aAssWord.Length;
		string aPath = Application.dataPath.Remove(aStarIndex);
		Debug.Log("Remove AssetsPath\n" +aPath);

	}
//	[MenuItem("SuperGame/Test/UnityLogShow")]
	private static void ProTest()
	{
		PlayerSettings[] aAllSet = Resources.FindObjectsOfTypeAll<PlayerSettings> ();
		for(int i = 0; i < aAllSet.Length; i++)
		{
			SerializedObject aSO = new SerializedObject(aAllSet[i]);
			aSO.Update();
			Texture2D aTmp =  aSO.FindProperty ("iPhoneSplashScreen").objectReferenceValue as Texture2D;
			Debug.Log("iPhoneSplashScreen :" + aTmp.name);
		}


//		foreach (var player in Resources.FindObjectsOfTypeAll<PlayerSettings> ())
//		{
//			var so = new SerializedObject (player);
//			so.Update ();
//			Texture2D aTmp =  so.FindProperty ("iPhoneSplashScreen").objectReferenceValue as Texture2D;
//			
//			Debug.Log("iPhoneSplashScreen :" + aTmp.name);
//		}

//		Debug.Log(eMobileSplashScreen.iPhoneSplashScreen.ToString());
	}


	[MenuItem("SuperGame/Test/TestSetting")]
	private static void TestDoSetting()
	{
//		PlayerSettings.Android.forceInternetPermission = !PlayerSettings.Android.forceInternetPermission;
//		PlayerSettings.Android.forceSDCardPermission = !PlayerSettings.Android.forceSDCardPermission;
//		PlayerSettings.enableCrashReportAPI = !PlayerSettings.enableCrashReportAPI;
//		PlayerSettings.logObjCUncaughtExceptions = !PlayerSettings.logObjCUncaughtExceptions;
//		Debug.Log("PlayerSettings.Android.forceSDCardPermission = " + PlayerSettings.logObjCUncaughtExceptions);
		Debug.Log("Before Define :" + PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone));
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "abcd");
		AssetDatabase.Refresh();
	}
}
#endif