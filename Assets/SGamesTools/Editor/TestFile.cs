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
	[MenuItem("SuperGame/Test/TestGUI")]
	private static void TestFunction()
	{
		EditorWindow.GetWindow(typeof(TestFile));
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

//	private void OnEnable()
//	{
//
//	}



	private void OnGUI()
	{

	}
	#region UI Text
//	bool showBtn = true;
//	/// <summary>
//	/// Examps the toggle.
//	/// </summary>
//	private void ExampToggle()
//	{
//		showBtn = EditorGUILayout.Toggle("Show Button", showBtn);
//
//		showBtn = EditorGUILayout.ToggleLeft("Left Show Btn", showBtn);
//
//
//		if (showBtn)
//		if (GUILayout.Button("Close"))
//			this.Close();
//	}
//	string ammo;
//	string ammo2;
//	/// <summary>
//	///	PerfixLabel和 LabelField的差別在視窗分配上
//	///	PerfixLabel : 只取用一小部分，其餘是輸入區域
//	///	LabelField : 和輸入區域對分一半使用區
//	/// </summary>
//	private void ExampPrefixLabel()
//	{
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Ammo");
//		ammo = EditorGUILayout.TextField(ammo);
//		EditorGUILayout.EndHorizontal();
//		EditorGUILayout.Space();// 留個空間
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.LabelField("Ammo2");
//		ammo2 = EditorGUILayout.TextField(ammo2);
//		EditorGUILayout.EndHorizontal();
//	}
	/// <summary>
	/// 在GUI裡面出現提示訊息用的
	/// </summary>
//	private void ExampHelpBox()
//	{
//		EditorGUILayout.HelpBox("1111", MessageType.Info);
//	}
//	#endregion
//	#region Foldout
//	bool mShowPosition = true;
//	string mStatus = "Select a GameObject";
//	Vector3 mField = new Vector3();
//	private void ExampFoldout()
//	{
//		mShowPosition = EditorGUILayout.Foldout(mShowPosition, mStatus);
//		if (mShowPosition) {
//			mField = EditorGUILayout.Vector3Field("Position", mField);
//		}
//	}
	#endregion

	#region BeginFadeGroup
//	AnimBool mShowExtraF;
//	string mString;
//	Color mColor = Color.white;
//	int mNum = 0;
//
//	private void ExampFadeGroupInit()
//	{
//		mShowExtraF = new AnimBool(true);
//		mShowExtraF.valueChanged.AddListener(Repaint);
//	}
//
//	/// <summary>
//	/// 用動畫的方式顯示內容物
//	/// </summary>
//	private void ExampFadeGroup()
//	{
//		mShowExtraF.target = EditorGUILayout.ToggleLeft("Show extra fields", mShowExtraF.target);
//		//Extra block that can be toggled on and off.
//		if (EditorGUILayout.BeginFadeGroup(mShowExtraF.faded))// Start
//		{
//			EditorGUI.indentLevel++;
//			EditorGUILayout.PrefixLabel("Color");
//			mColor = EditorGUILayout.ColorField(mColor);
//			EditorGUILayout.PrefixLabel("Text");
//			mString = EditorGUILayout.TextField(mString);
//			EditorGUILayout.PrefixLabel("Number");
//			mNum = EditorGUILayout.IntSlider(mNum, 0, 10);
//			EditorGUI.indentLevel--;
//		}
//		EditorGUILayout.EndFadeGroup();// End
//	}
	#endregion


}
#endif