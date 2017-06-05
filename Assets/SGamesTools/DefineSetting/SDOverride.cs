#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
/**
 * 2017/04 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 將自設儲存的設定檔複寫到PlayerSetting上
 */
public class SDOverride : SDBaseUI
{
	private static SDOverride Instance;
	public static SDOverride Self 
	{ 
		set
		{
			Instance = value;
		}
		get
		{
			if( Instance == null )
				Instance = new SDOverride();
			
			return Instance;
		}
	}

	private int mSelectSaveIndex;
	private int mTotalSaveNum;
	/// <summary>
	/// 初始化
	/// </summary>
	protected override void SettingInit()
	{
		if(mInitStatus)
			return;

		if(CheckHaveFiles())
			return;
		
		mInitStatus = true;
	}
	/// <summary>
	/// 複寫視窗
	/// </summary>
	protected override void ShowUI()
	{
		// Start Scroll View
		mEditorScrollView = EditorGUILayout.BeginScrollView(mEditorScrollView);
		mSelectSaveIndex = UITopSaveFileSelect(mSelectSaveIndex);
		GUILayout.Label("");
		GUILayout.Label("");
		// Button Horizontal
		if(GUILayout.Button("設置UnityPalyerSetting設定", ButtonMyStyle(eButtonPos.none)))
			DoOverride();
		if (GUILayout.Button ("取消", ButtonMyStyle(eButtonPos.none)))
			Close ();
		// End Scroll View
		EditorGUILayout.EndScrollView ();
	}
	/// <summary>
	/// 執行複寫
	/// </summary>
	private void DoOverride()
	{
		OverridePlayerSet(FileNameArray[mSelectSaveIndex]);
		Close ();
	}
	/// <summary>
	/// 共通設定複寫
	/// </summary>
	private void OverrideSetCommon()
	{
		// Screen Rotate
		PlayerSettings.defaultInterfaceOrientation = mShowSetInfo.UIOrientation;
		PlayerSettings.allowedAutorotateToPortrait = mShowSetInfo.OrienRoatable[0];
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = mShowSetInfo.OrienRoatable[1];
		PlayerSettings.allowedAutorotateToLandscapeLeft = mShowSetInfo.OrienRoatable[2];
		PlayerSettings.allowedAutorotateToLandscapeRight = mShowSetInfo.OrienRoatable[3];
		// Bundle
		PlayerSettings.applicationIdentifier = mShowSetInfo.BundleID;
		PlayerSettings.bundleVersion = mShowSetInfo.BundleVer;
		// Other
		PlayerSettings.statusBarHidden = mShowSetInfo.StatusBarHidden;
		PlayerSettings.use32BitDisplayBuffer = mShowSetInfo.Use32BitDisplayBuffer;
//		PlayerSettings.apiCompatibilityLevel = mShowSetInfo.ApiCompatibilityLevel;// 4.6Ver
		PlayerSettings.strippingLevel = mShowSetInfo.StrippingLevel;
		// Icon
		if(mShowSetInfo.IconSetStatus)
			SDDataMove.SetIconsGroup(BuildTargetGroup.Unknown, mShowSetInfo.DefIcons);
	}
	/// <summary>
	/// Android設定複寫
	/// </summary>
	private void OverrideSetAndroid()
	{
		SDAndroidSet aTmpSet = mShowSetInfo.AndroidSet;
		PlayerSettings.Android.showActivityIndicatorOnLoading = aTmpSet.ShowActivityIndicatorOnLoading;
		PlayerSettings.Android.splashScreenScale = aTmpSet.SplashScreenScale;
		PlayerSettings.Android.bundleVersionCode = aTmpSet.BundleCode;
		PlayerSettings.Android.minSdkVersion = aTmpSet.SdkVersions;
		PlayerSettings.Android.targetDevice = aTmpSet.TargetDevice;
//		PlayerSettings.targetGlesGraphics = aTmpSet.TargetGraphice;// 4.6Ver
		
		PlayerSettings.Android.forceInternetPermission = aTmpSet.ForceInternet;
		PlayerSettings.Android.forceSDCardPermission = aTmpSet.ForceSDCard;
		
		PlayerSettings.Android.keystoreName = string.Format ("{0}/{1}", Application.dataPath, aTmpSet.KeyStorePath);
		PlayerSettings.Android.keystorePass = aTmpSet.KeyStorePassword;
		PlayerSettings.Android.keyaliasName = aTmpSet.KeyAlialsName;
		PlayerSettings.Android.keyaliasPass = aTmpSet.KeyAlialsPassword;
		// Unity5 New
		PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, aTmpSet.GraphicsType);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, aTmpSet.ApiCompatibilityLevel);
		// Icon
		if(aTmpSet.IconSetStatus)
		{
			if(aTmpSet.IconOverride)
				SDDataMove.SetIconsGroup(BuildTargetGroup.Android, aTmpSet.DefIcons);
			else
				SDDataMove.ClearnIconsGroup(BuildTargetGroup.Android);
		}
		if(aTmpSet.SplashSetStatus)
			SDDataMove.SetSplashScreen("androidSplashScreen", mShowSetInfo.AndroidSet.SplashImage);
	}
	/// <summary>
	/// IOS設定複寫
	/// </summary>
	private void OverrideSetIOS()
	{
		SDIOSSet aTmpSet = mShowSetInfo.IOSSet;
		PlayerSettings.iOS.statusBarStyle = aTmpSet.StatusBarStyle;
		PlayerSettings.iOS.showActivityIndicatorOnLoading = aTmpSet.ShowActivityIndicatorOnLoading;
		PlayerSettings.iOS.targetDevice = aTmpSet.TargetDevice;
//		PlayerSettings.iOS.targetResolution = aTmpSet.TargetResolution;// 4.6Ver
//		PlayerSettings.targetIOSGraphics = aTmpSet.TargetGraphics;// 4.6Ver
		
		PlayerSettings.iOS.sdkVersion = aTmpSet.SDKVersion;
//		PlayerSettings.iOS.targetOSVersion = aTmpSet.TargetOSVersion;// 4.6Ver
		PlayerSettings.iOS.scriptCallOptimization = aTmpSet.ScriptCallOptimizationLevel;

//		SDDataMove.SetBoolPalyerSetting("Override IPod Music", aTmpSet.OverrideIPodMusic);
		SDDataMove.SetBoolPalyerSetting("Prepare IOS For Recording", aTmpSet.PrepareIOSForRecording);
		PlayerSettings.iOS.requiresPersistentWiFi = aTmpSet.RequiresPersistentWiFi;
//		PlayerSettings.iOS.exitOnSuspend = aTmpSet.ExitOnSuspend;// 4.6Ver

		PlayerSettings.iOS.prerenderedIcon = aTmpSet.PrerenderedIcon;
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)aTmpSet.ScriptingBackend, BuildTargetGroup.iOS);
		PlayerSettings.SetPropertyInt("Architecture", (int)aTmpSet.Architecture, BuildTargetGroup.iOS);
		// Unity5 New
		PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, aTmpSet.GraphicsType);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, aTmpSet.ApiCompatibilityLevel);
		PlayerSettings.iOS.targetOSVersionString = aTmpSet.TargetOSVersionString;
		PlayerSettings.iOS.appInBackgroundBehavior = aTmpSet.AppInBackgroundBehavior;
		// Icon
		if(aTmpSet.IconSetStatus)
		{
			if(aTmpSet.IconOverride)
				SDDataMove.SetIconsGroup(BuildTargetGroup.iOS, aTmpSet.DefIcons);
			else
				SDDataMove.ClearnIconsGroup(BuildTargetGroup.iOS);
		}
		// Splash
		if(aTmpSet.SplashSetStatus)
			for(int i = 0; i < mShowSetInfo.IOSSet.SplashImages.Length; i++)
				SDDataMove.SetSplashScreen((eMobileSplashScreen)i, mShowSetInfo.IOSSet.SplashImages[i]);
	}
	/// <summary>
	/// 更新UnityDefineSetting內容
	/// </summary>
	private void UpdatePlayerDefineSet()
	{
		switch(mShowSetInfo.DefineTarget)
		{
		case eSDTarget.Android: PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, mShowSetInfo.ScriptDefine); break;
		case eSDTarget.IOS: PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, mShowSetInfo.ScriptDefine); break;
		}
	}
	/// <summary>
	/// 讀取自設設定檔來複寫PlayerSetting
	/// </summary>
	public static void OverridePlayerSet(string iSDFileName)
	{
		Self.mShowSetInfo = SDDataMove.LoadData(iSDFileName);
		if(Self.mShowSetInfo == null)
		{
			Debug.Log("沒有找到設定檔案");
			return;
		}
		// Common
		Self.OverrideSetCommon();
		// SetDefint
		Self.UpdatePlayerDefineSet();
		// Other
		switch(Self.mShowSetInfo.DefineTarget)
		{
		case eSDTarget.Android: Self.OverrideSetAndroid(); break;
		case eSDTarget.IOS: Self.OverrideSetIOS(); break;
		}
		Debug.Log("資料設定完畢");
		AssetDatabase.Refresh();
	}
}
#endif