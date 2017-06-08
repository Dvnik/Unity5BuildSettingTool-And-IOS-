﻿#if UNITY_EDITOR
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


	[MenuItem("SuperGame/Test/TestSetting")]
	private static void TestDoSetting()
	{
//		PlayerSettings.Android.forceInternetPermission = !PlayerSettings.Android.forceInternetPermission;
//		PlayerSettings.Android.forceSDCardPermission = !PlayerSettings.Android.forceSDCardPermission;
//		PlayerSettings.enableCrashReportAPI = !PlayerSettings.enableCrashReportAPI;
		PlayerSettings.logObjCUncaughtExceptions = !PlayerSettings.logObjCUncaughtExceptions;
		Debug.Log("PlayerSettings.Android.forceSDCardPermission = " + PlayerSettings.logObjCUncaughtExceptions);
		AssetDatabase.Refresh();
	}


	private void OnEnable() {
		FTUIInit();
		FTUIAndoridInit();
		FTUIIOSInit();
	}

	private void OnGUI() {
		FTUIShow();
	}

	Vector2 mScrollView;


	string mUnKnowBID;

	private void FTUIInit() {
		mUnKnowBID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Unknown);
	}


	private void FTUIShow() {
		mScrollView = EditorGUILayout.BeginScrollView(mScrollView);
		// Title
		FTUITitle();
		// 1
		FTUICommonArea();
		// 2
		FTUIAndroidArea();
		// 3
		FTUIIOSArea();
		EditorGUILayout.EndScrollView();
	}
#region Test CommonUI Set
	string mFFileName;
	eSDTarget mFTarget;


	private void FTUITitle() {
		mFFileName = EditorGUILayout.TextField("設置檔名:", mFFileName);
		EditorGUILayout.HelpBox("一個設定檔只會部屬一個平台的設定\n這是為了自動產檔時能夠快速設定用的", MessageType.Info);
		mFTarget = (eSDTarget)EditorGUILayout.EnumPopup("設置平台:", mFTarget);
		EditorGUILayout.Space();
	}

	string mCompanyName, mProductName, mVersion;
	bool mShowCommon = true;

	private void FTUICommonArea() {
		mShowCommon = EditorGUILayout.Foldout(mShowCommon, "基本設置");
		if(!mShowCommon)
			return;
		
		EditorGUI.indentLevel++;
		mCompanyName = EditorGUILayout.TextField("Company Name:", mCompanyName);
		mProductName = EditorGUILayout.TextField("Product Name:", mProductName);
		EditorGUI.indentLevel--;
		FTUIOrientation();
		EditorGUI.indentLevel++;
		EditorGUILayout.PrefixLabel("Identification");
		EditorGUI.indentLevel++;
		mUnKnowBID = EditorGUILayout.TextField("Bundle Identifier :", mUnKnowBID);
		mVersion = EditorGUILayout.TextField("Version :", mVersion);
		EditorGUI.indentLevel--;
		FTUIIcon();
		EditorGUI.indentLevel--;
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
		EditorGUI.indentLevel++;
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
		EditorGUI.indentLevel--;
	}
	bool mSetDefaultIcon;
	Texture2D mTempIcon;

	private void FTUIIcon() {
		mSetDefaultIcon = EditorGUILayout.ToggleLeft("設定ICon圖", mSetDefaultIcon);
		if(!mSetDefaultIcon)
			return;
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mTempIcon = EditorGUILayout.ObjectField("Default Icon(Temp)", mTempIcon, typeof(Texture2D), false) as Texture2D;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PrefixLabel("Path :");
		EditorGUILayout.LabelField("Tmp/Path");
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel--;
	}
	#endregion
#region Test Android Set
	bool mShowAndroid;
	AndroidShowActivityIndicatorOnLoading mShowLoadingIndicator;

	private void FTUIAndoridInit() {
		mAndroidIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
		mAndroidIcon = new Texture2D[mAndroidIconTexts.Length];
		mAndroidBID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
	}

	private void FTUIAndroidArea() {
		mShowAndroid = EditorGUILayout.Foldout(mShowAndroid, "Android 設置");
		if(!mShowAndroid)
			return;
		EditorGUI.indentLevel++;
		FTUIAndroidResolution();
		FTUIAndroidOtherSetting();
		FTUIAndroidPublishingSetting();
		FTUIAndroidIcon();
		FTUIAndroidSplashImage();

		EditorGUI.indentLevel--;
	}

	bool mDepthandStencil, mUse32BitDisplay;
	private void FTUIAndroidResolution() {
		EditorGUILayout.LabelField("Resolution and Presentation");
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Use 32Bit Display Buffer:");
		mUse32BitDisplay = EditorGUILayout.Toggle(mUse32BitDisplay);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Disable Depth and Stencil:");
		mDepthandStencil = EditorGUILayout.Toggle(mDepthandStencil);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show Loading Indicator:");
		mShowLoadingIndicator = 
			(AndroidShowActivityIndicatorOnLoading)EditorGUILayout.EnumPopup(mShowLoadingIndicator);
		EditorGUILayout.EndHorizontal();
		// End
		EditorGUI.indentLevel--;
	}

	string mAndroidBID, mAndroidVersion, mAndroidVersionCode;
	string[] mInternetAccess = new string[]{"Auto", "Require"};
	string[] mWritePermission = new string[]{"Internal", "External(SDCard)"};
	string[] mApiCompatibilityLevel = new string[]{".NET 2.0", ".NET 2.0 Subset"};
	int mINterNetSelect, WritePerSelect, mACBSelect;
	bool mTempSetData;
	ApiCompatibilityLevel mACBLevel;

	private void FTUIAndroidOtherSetting() {
		EditorGUILayout.LabelField("Other Settings");
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Bundle Identifier :");
		mAndroidBID = EditorGUILayout.TextField(mAndroidBID);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Version :");
		mAndroidVersion = EditorGUILayout.TextField(mAndroidVersion);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Bundle Version Code :");
		mAndroidVersionCode = EditorGUILayout.TextField(mAndroidVersionCode);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Internet Access");
		mINterNetSelect = EditorGUILayout.Popup(mINterNetSelect, mInternetAccess);
		mTempSetData = mINterNetSelect == 1;// Require = true; Auto = false;
//		Debug.Log("PlayerSettings.Android.forceInternetPermission = " + mTempSetData);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Write Permission");
		WritePerSelect = EditorGUILayout.Popup(WritePerSelect, mWritePermission);
		mTempSetData = mINterNetSelect == 1;// External(SDCard) = true; Internal = false;
//		Debug.Log("PlayerSettings.Android.forceSDCardPermission = " + mTempSetData);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Configuration");
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Api Compatibility Level*");
		mACBSelect = EditorGUILayout.Popup(mACBSelect, mApiCompatibilityLevel);
		switch(mACBSelect) {
		case 0: mACBLevel = ApiCompatibilityLevel.NET_2_0; break;
		case 1: mACBLevel = ApiCompatibilityLevel.NET_2_0_Subset; break;
		}
		EditorGUILayout.EndHorizontal();
		// End
		EditorGUI.indentLevel--;
	}
	string mKeyStorePath, mKeyStorePassword, mKeyAliasName, mKeyAliasPassword;

	private void FTUIAndroidPublishingSetting() {
		EditorGUILayout.LabelField("Publishing Settings");
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.HelpBox("KeyStorePath 預設都取Assets底下的路徑", MessageType.Info);
		mKeyStorePath = EditorGUILayout.TextField("Keystore Path:", mKeyStorePath);
		mKeyStorePassword = EditorGUILayout.TextField("Keystore Pass:", mKeyStorePassword);
		mKeyAliasName = EditorGUILayout.TextField("Keyalias Name:", mKeyAliasName);
		mKeyAliasPassword = EditorGUILayout.TextField("Keyalias Pass:", mKeyAliasPassword);
		// End
		EditorGUI.indentLevel--;
	}


	bool mSetAndroidIcon, mOverrideAndroidIcon, mEnableAndroidBanner;
	int[] mAndroidIconTexts;
	Texture2D[] mAndroidIcon;
	Texture2D mEAndroidBanner;

	private void FTUIAndroidIcon() {
		mSetAndroidIcon = EditorGUILayout.ToggleLeft("設定Icon圖", mSetAndroidIcon);
		if(!mSetAndroidIcon)
			return;
		
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		mOverrideAndroidIcon = EditorGUILayout.ToggleLeft("是否覆寫Icon圖", mOverrideAndroidIcon);
		if(mOverrideAndroidIcon) {
			EditorGUI.indentLevel++;
			for(int i = 0; i < mAndroidIconTexts.Length; i++) {
				string aPicSize = mAndroidIconTexts[i] + "x" + mAndroidIconTexts[i];

				mAndroidIcon[i] = EditorGUILayout.ObjectField(aPicSize, mAndroidIcon[i], typeof(Texture2D), false) as Texture2D;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.PrefixLabel("Path :");
				EditorGUILayout.LabelField("Tmp/Path");
				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
		}
		mEnableAndroidBanner = EditorGUILayout.ToggleLeft("Enable Android Banner", mEnableAndroidBanner);
		if(mEnableAndroidBanner) {
			EditorGUI.indentLevel++;
			mEAndroidBanner = EditorGUILayout.ObjectField("320x180", mEAndroidBanner, typeof(Texture2D), false) as Texture2D;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.PrefixLabel("Path :");
			EditorGUILayout.LabelField("Tmp/Path");
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
	}

	bool mSetAndroidSplash;
	AndroidSplashScreenScale mAndroidSplashScale;
	Texture2D mAndroidSplashImage;

	private void FTUIAndroidSplashImage() {
		mSetAndroidSplash = EditorGUILayout.ToggleLeft("設定Splash圖", mSetAndroidSplash);

		if(!mSetAndroidSplash)
			return;
		EditorGUILayout.HelpBox("目前只設定一般的Splash Image\n不設定VR Splash 和 Splash Sreen", MessageType.Info);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		mAndroidSplashImage = EditorGUILayout.ObjectField("Image", mAndroidSplashImage, typeof(Texture2D), false) as Texture2D;
		if(mAndroidSplashImage != null) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Splash Screen Scale:");
			mAndroidSplashScale = (AndroidSplashScreenScale)EditorGUILayout.EnumPopup(mAndroidSplashScale);
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
		}
	}
	#endregion
	#region IOS Text
	int[] mIOSIconTexts;
	Texture2D[] mIOSIcon;
	string mIOSBID;

	private void FTUIIOSInit() {
		mIOSIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
		mIOSIcon = new Texture2D[mIOSIconTexts.Length];
		mIOSBID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
		mIOSSplashImage = new Texture2D[mIOSLunchImages.Length];
	}

	bool mShowIOS;

	private void FTUIIOSArea() {
		mShowIOS = EditorGUILayout.Foldout(mShowIOS, "IOS 設置");
		if(!mShowIOS)
			return;
		EditorGUI.indentLevel++;
		FTUIIOSResolution();
		FTUUIIOSDebugging();
		FTUIIOSOtherSetting();
		FTUIIOSIcon();
		FTUIIOSSplashImage();
		EditorGUI.indentLevel--;
	}

	bool mIOSRequiresFullscreen, mIOSStatusBarHidden;
	iOSStatusBarStyle mIOSSBS;
	iOSShowActivityIndicatorOnLoading mIOSShowOnLoading;
	private void FTUIIOSResolution() {
		EditorGUILayout.LabelField("Resolution and Presentation");
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.LabelField("Multitasking Support", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Requires Fullscreen");
		mIOSRequiresFullscreen = EditorGUILayout.Toggle(mIOSRequiresFullscreen);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.LabelField("Status Bar", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Status Bar Hidden");
		mIOSStatusBarHidden = EditorGUILayout.Toggle(mIOSStatusBarHidden);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Status Bar Style");
		mIOSSBS = (iOSStatusBarStyle)EditorGUILayout.EnumPopup(mIOSSBS);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show Loading Indicator");
		mIOSShowOnLoading = (iOSShowActivityIndicatorOnLoading)EditorGUILayout.EnumPopup(mIOSShowOnLoading);
		EditorGUILayout.EndHorizontal();

		// End
		EditorGUI.indentLevel--;
	}

	string mIOSVersion, mIOSBuild, mAutoSignTeamID, mTargetMinIOSVer;
	bool mAutoSign, mPreapareIOSReco, mRequiresWifi;
	iOSSdkVersion mSDKVersion;
	ScriptCallOptimizationLevel mScrCallOptimizLV;
	iOSTargetDevice mIOSTargetDevice;
	ApiCompatibilityLevel mIACBLevel;
	int mIACBSelect, mScriptBackSelect;
	iOSAppInBackgroundBehavior mIOSBackgroundBehavior;
	string[] mScriptingBackend = new string[]{
		"Mono2x",
		"IL2CPP"
	};

	ScriptingImplementation mIOSBackGround;
	iPhoneArchitecture mIOSArchite;

	private void FTUIIOSOtherSetting() {
		EditorGUILayout.LabelField("Other Settings");
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.LabelField("Identification", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Bundle Identifier :");
		mIOSBID = EditorGUILayout.TextField(mIOSBID);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Version*");
		mIOSVersion = EditorGUILayout.TextField(mIOSVersion);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Build");
		mIOSBuild = EditorGUILayout.TextField(mIOSBuild);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Automatically Sign");
		mAutoSign = EditorGUILayout.Toggle(mAutoSign);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Automatic Signing Team ID");
		mAutoSignTeamID = EditorGUILayout.TextField(mAutoSignTeamID);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Scripting Backend");
		mScriptBackSelect = EditorGUILayout.Popup(mScriptBackSelect, mScriptingBackend);
		switch(mScriptBackSelect) {
		case 0: mIOSBackGround = ScriptingImplementation.Mono2x; break;
		case 1: mIOSBackGround = ScriptingImplementation.IL2CPP; break;
		}
		EditorGUILayout.EndHorizontal();
		if(mIOSBackGround == ScriptingImplementation.IL2CPP) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Architecture");
			mIOSArchite = (iPhoneArchitecture)EditorGUILayout.EnumPopup(mIOSArchite);
			EditorGUILayout.EndHorizontal();
		}


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Api Compatibility Level*");
		mIACBSelect = EditorGUILayout.Popup(mIACBSelect, mApiCompatibilityLevel);
		switch(mIACBSelect) {
		case 0: mIACBLevel = ApiCompatibilityLevel.NET_2_0; break;
		case 1: mIACBLevel = ApiCompatibilityLevel.NET_2_0_Subset; break;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Device");
		mIOSTargetDevice = (iOSTargetDevice)EditorGUILayout.EnumPopup(mIOSTargetDevice);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target SDK");
		mSDKVersion = (iOSSdkVersion)EditorGUILayout.EnumPopup(mSDKVersion);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target minimum iOS Verstion");
		mTargetMinIOSVer = EditorGUILayout.TextField(mTargetMinIOSVer);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Prepare iOS for Recording");
		mPreapareIOSReco = EditorGUILayout.Toggle(mPreapareIOSReco);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Requires Persistent WiFi*");
		mRequiresWifi = EditorGUILayout.Toggle(mRequiresWifi);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Behavior in Background");
		mIOSBackgroundBehavior = (iOSAppInBackgroundBehavior)EditorGUILayout.EnumPopup(mIOSBackgroundBehavior);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Optimization", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Script Call Optimization");
		mScrCallOptimizLV = (ScriptCallOptimizationLevel)EditorGUILayout.EnumPopup(mScrCallOptimizLV);
		EditorGUILayout.EndHorizontal();
		// End
		EditorGUI.indentLevel--;
	}

	bool mObjCUncaughtException, mEnableCrashReport;

	private void FTUUIIOSDebugging() {
		EditorGUILayout.LabelField("Debugging and crash reporting", EditorStyles.boldLabel);
		EditorGUILayout.HelpBox("Unity5增加的類別，盡量做到設定", MessageType.Info);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.LabelField("Crash Reporting");
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Log Obj-C Uncaught Exception");
		mObjCUncaughtException = EditorGUILayout.Toggle(mObjCUncaughtException);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Enable CrashReport API*");
		mEnableCrashReport = EditorGUILayout.Toggle(mEnableCrashReport);
		EditorGUILayout.EndHorizontal();
		// End
		EditorGUI.indentLevel--;
	}



	bool mSetIOSIcon, mOverrideIOSIcon;

	private void FTUIIOSIcon() {
		mSetIOSIcon = EditorGUILayout.ToggleLeft("設定Icon圖", mSetIOSIcon);
		if(!mSetIOSIcon)
			return;

		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		mOverrideIOSIcon = EditorGUILayout.ToggleLeft("是否覆寫Icon圖", mOverrideIOSIcon);
		if(mOverrideIOSIcon) {
			EditorGUI.indentLevel++;
			for(int i = 0; i < mIOSIconTexts.Length; i++) {
				string aPicSize = mIOSIconTexts[i] + "x" + mIOSIconTexts[i];

				mIOSIcon[i] = EditorGUILayout.ObjectField(aPicSize, mIOSIcon[i], typeof(Texture2D), false) as Texture2D;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.PrefixLabel("Path :");
				EditorGUILayout.LabelField("Tmp/Path");
				EditorGUILayout.EndHorizontal();
			}
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;
	}

	string[] mIOSLunchImages = new string[]{
		"Mobile Splash Screen*",
		"iPhone 3.5\"/Retina",
		"iPhone 4\"/Retina",
		"iPhone 4.7\"/Retina",
		"iPhone 5.5\"/Retina",
		"iPhone 5.5\"Landscape/Retina",
		"iPad Portrait",
		"iPad Landscape",
		"iPad Portrait/Retina",
		"iPad Landscape/Retina"
	};

	Texture2D[] mIOSSplashImage;

	private void FTUIIOSSplashImage() {
		mSetAndroidSplash = EditorGUILayout.ToggleLeft("設定Splash圖", mSetAndroidSplash);

		if(!mSetAndroidSplash)
			return;
		EditorGUILayout.HelpBox("目前只設定一般的Splash Image\n不設定VR Splash 和 Splash Sreen", MessageType.Info);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		for(int i = 0; i < mIOSLunchImages.Length; i++) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(mIOSLunchImages[i]);
			mIOSSplashImage[i] = EditorGUILayout.ObjectField(mIOSSplashImage[i], typeof(Texture2D), false) as Texture2D;	
			EditorGUILayout.EndHorizontal();
		}
	}
	#endregion

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