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
//	}

	private void OnGUI()
	{
		FTUIShow();
	}

	Vector2 mScrollView;
	bool mShowCommon = true;

	private void FTUIShow() {
		mScrollView = EditorGUILayout.BeginScrollView(mScrollView);
		// Title
		FTUITitle();
		// 1
		mShowCommon = EditorGUILayout.Foldout(mShowCommon, "基本設置");
		if(mShowCommon) {
			FTUICommonArea();
		}

		EditorGUILayout.EndScrollView();
	}

	string mFFileName;
	eSDTarget mFTarget;


	private void FTUITitle() {
		mFFileName = EditorGUILayout.TextField("設置檔名:", mFFileName);
		EditorGUILayout.Space();
		mFTarget = (eSDTarget)EditorGUILayout.EnumPopup("設置平台:", mFTarget);
		EditorGUILayout.Space();
	}

	string mCompanyName, mProductName;

	private void FTUICommonArea() {
		EditorGUI.indentLevel++;
		mCompanyName = EditorGUILayout.TextField("Company Name:", mCompanyName);
		mProductName = EditorGUILayout.TextField("Product Name:", mProductName);
		EditorGUI.indentLevel--;
		FTUIOrientation();
	}

	bool mFOrientation = true, mFAnimRotate;
	UIOrientation mFDefOri;
	bool[] mFAutoRotatSet = new bool[4];

	string [] mFAutoRotName = new string[] {
		"Portrait",
		"Portrait Upside Down",
		"Landscape Right",
		"Landscape Left"
	};


	private void FTUIOrientation() {
		mFOrientation = EditorGUILayout.Foldout(mFOrientation, "旋轉方向設定");
		if (mFOrientation) {
			EditorGUI.indentLevel++;
			mFDefOri = (UIOrientation)EditorGUILayout.EnumPopup("預設方向:", mFDefOri);
			if(mFDefOri == UIOrientation.AutoRotation) {
				if(mFTarget == eSDTarget.IOS) {
					mFAnimRotate = EditorGUILayout.Toggle("使用動畫旋轉:", mFAnimRotate);
				}
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("設定自動旋轉允許方向");
				EditorGUI.indentLevel++;
				for(int i = 0; i < mFAutoRotatSet.Length; i++) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(mFAutoRotName[i]);
					mFAutoRotatSet[i] = EditorGUILayout.Toggle(mFAutoRotatSet[i]);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}
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
//	#endregion
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