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
		CheckHaveFiles();
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
		// Names
		PlayerSettings.companyName = mShowSetInfo.CompanyName;
		PlayerSettings.productName = mShowSetInfo.ProductName;
		// Orientation
		PlayerSettings.defaultInterfaceOrientation = mShowSetInfo.UIOrientation;
		PlayerSettings.useAnimatedAutorotation = mShowSetInfo.UseAnimAutor;
		PlayerSettings.allowedAutorotateToPortrait = mShowSetInfo.OrienRoatable[0];
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = mShowSetInfo.OrienRoatable[1];
		PlayerSettings.allowedAutorotateToLandscapeLeft = mShowSetInfo.OrienRoatable[2];
		PlayerSettings.allowedAutorotateToLandscapeRight = mShowSetInfo.OrienRoatable[3];
		// Identification
		/*
		 * PlayerSettings.applicationIdentifier 是取得當下Platform的BundleIdentifier
		 * 要取得特定Platform的Identifier
		 * 要改用PlayerSettings.GetApplicationIdentifier(取得)和PlayerSettings.SetApplicationIdentifier(設置)
		*/
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, mShowSetInfo.BundleIDStandalone);
		PlayerSettings.bundleVersion = mShowSetInfo.BundleVer;
		// Scripting Define Symbols
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, mShowSetInfo.ScriptDefineSymblos);
		// Icon
		if(mShowSetInfo.IconSetStatus)
			SDDataMove.SetIconsGroup(BuildTargetGroup.Unknown, mShowSetInfo.DefIcons);
		// ---------------------------------------------------------------
	}
	/// <summary>
	/// Android設定複寫
	/// </summary>
	private void OverrideSetAndroid()
	{
		SDAndroidSet aTmpSet = mShowSetInfo.AndroidSet;
		// Resolution and Presentation
		PlayerSettings.use32BitDisplayBuffer = aTmpSet.Use32BitDisplayBuffer;
		PlayerSettings.Android.disableDepthAndStencilBuffers = aTmpSet.disableDepthAndStencilBuffers;
		PlayerSettings.Android.showActivityIndicatorOnLoading = aTmpSet.ShowActivityIndicatorOnLoading;
		// Identification
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, aTmpSet.BundleIDAndroid);
		PlayerSettings.Android.bundleVersionCode = aTmpSet.BundleCode;
		PlayerSettings.Android.minSdkVersion = aTmpSet.SdkVersions;
		// Configuration
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, aTmpSet.ApiCompatibilityLevel);
		PlayerSettings.Android.forceInternetPermission = aTmpSet.ForceInternet;
		PlayerSettings.Android.forceSDCardPermission = aTmpSet.ForceSDCard;
		PlayerSettings.Android.targetDevice = aTmpSet.TargetDevice;
		// Publishing Settings
		PlayerSettings.Android.keystoreName = string.Format ("{0}/{1}", Application.dataPath, aTmpSet.KeyStorePath);
		PlayerSettings.Android.keystorePass = aTmpSet.KeyStorePassword;
		PlayerSettings.Android.keyaliasName = aTmpSet.KeyAlialsName;
		PlayerSettings.Android.keyaliasPass = aTmpSet.KeyAlialsPassword;
		// Scripting Define Symbols
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, aTmpSet.ScriptDefineSymblos);
		// Icon
		if(aTmpSet.IconSetStatus)
		{
			if(aTmpSet.IconOverride)
				SDDataMove.SetIconsGroup(BuildTargetGroup.Android, aTmpSet.DefIcons);
			else
				SDDataMove.ClearnIconsGroup(BuildTargetGroup.Android);
		}
		// Splash Image
		if(aTmpSet.SplashSetStatus) {
			SDDataMove.SetSplashScreen("androidSplashScreen", mShowSetInfo.AndroidSet.SplashImage);
			PlayerSettings.Android.splashScreenScale = aTmpSet.SplashScreenScale;
		}
		//----------------------------------------
		// Unity5 New
		PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, aTmpSet.GraphicsType);
	}
	/// <summary>
	/// IOS設定複寫
	/// </summary>
	private void OverrideSetIOS()
	{
		SDIOSSet aTmpSet = mShowSetInfo.IOSSet;
		// Resolution and Presentation
		PlayerSettings.iOS.requiresFullScreen = aTmpSet.RequiresFullScreen;
		PlayerSettings.statusBarHidden = aTmpSet.StatusBarHidden;
		PlayerSettings.iOS.statusBarStyle = aTmpSet.StatusBarStyle;
		PlayerSettings.iOS.showActivityIndicatorOnLoading = aTmpSet.ShowActivityIndicatorOnLoading;
		// Debugging and crash reporting
		PlayerSettings.actionOnDotNetUnhandledException = aTmpSet.ActionOnDotNetUnhandledException;
		PlayerSettings.logObjCUncaughtExceptions = aTmpSet.LogObjCUncaughtExceptions;
		PlayerSettings.enableCrashReportAPI = aTmpSet.EnableCrashReportAPI;
		// Identification
		PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, aTmpSet.BundleIDIOS);
		PlayerSettings.iOS.buildNumber = aTmpSet.BuildNumber;
		PlayerSettings.iOS.appleEnableAutomaticSigning = aTmpSet.AppleEnableAutomaticSigning;
		PlayerSettings.iOS.appleDeveloperTeamID = aTmpSet.AppleDeveloperTeamID;
		PlayerSettings.iOS.iOSManualProvisioningProfileID = aTmpSet.ProvisioningProfileID;
		// Configuration
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, aTmpSet.ScriptingBackend);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, aTmpSet.ApiCompatibilityLevel);
		PlayerSettings.iOS.targetDevice = aTmpSet.TargetDevice;
		PlayerSettings.iOS.sdkVersion = aTmpSet.SDKVersion;
		PlayerSettings.iOS.targetOSVersionString = aTmpSet.TargetOSVersionString;
		SDDataMove.SetBoolPalyerSetting("Prepare IOS For Recording", aTmpSet.PrepareIOSForRecording);
		PlayerSettings.iOS.requiresPersistentWiFi = aTmpSet.RequiresPersistentWiFi;
		PlayerSettings.iOS.appInBackgroundBehavior = aTmpSet.AppInBackgroundBehavior;
		if(mShowSetInfo.IOSSet.ScriptingBackend == ScriptingImplementation.IL2CPP &&
			mShowSetInfo.IOSSet.SDKVersion == iOSSdkVersion.DeviceSDK) {
			PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, aTmpSet.Architecture);
		}
		// Optimization
		PlayerSettings.iOS.scriptCallOptimization = aTmpSet.ScriptCallOptimizationLevel;
		if(aTmpSet.ScriptingBackend == ScriptingImplementation.IL2CPP) {
			PlayerSettings.stripEngineCode = aTmpSet.StripEngineCode;
		}
		else if(aTmpSet.ScriptingBackend == ScriptingImplementation.Mono2x) {			
			PlayerSettings.strippingLevel = aTmpSet.StripLevel;
		}
		// Scripting Define Symbols
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, aTmpSet.ScriptDefineSymblos);
		// Icon
		if(aTmpSet.IconSetStatus) {
			if(aTmpSet.IconOverride)
				SDDataMove.SetIconsGroup(BuildTargetGroup.iOS, aTmpSet.DefIcons);
			else
				SDDataMove.ClearnIconsGroup(BuildTargetGroup.iOS);
		}
		PlayerSettings.iOS.prerenderedIcon = aTmpSet.PrerenderedIcon;
		// Splash
		if(aTmpSet.SplashSetStatus) {
			for(int i = 0; i < mShowSetInfo.IOSSet.SplashImages.Length; i++) {
				SDDataMove.SetSplashScreen((eMobileSplashScreen)i, mShowSetInfo.IOSSet.SplashImages[i]);
			}
		}
		//----------------------------------------
		// Unity5 New
		PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, aTmpSet.GraphicsType);
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