#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;
/**
 * 2017/04 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 新建一個Setting檔
 */
public class SDCreate : SDBaseUI
{
	/// <summary>
	/// 初始化介面資料設定
	/// </summary>
	protected override void SettingInit()
	{
		if(mInitStatus)
			return;
		// Set Default Info
		DefaultSetCommon();
		DefaultSetAndroid();
		DefaultSetIOS();
		
		mInitStatus = true;
	}
	/// <summary>
	/// 顯示GUI內容
	/// </summary>
	protected override void ShowUI()
	{
		// Start Scroll View
		mEditorScrollView = EditorGUILayout.BeginScrollView(mEditorScrollView);
		// Top
		UITopSettingShow();
		// Common
		UICommonArea();
		// Android
		UIAndroidArea();
		// IOS
		UIIOSArea();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("存檔", ButtonMyStyle(eButtonPos.left)))
			DoCreate();
		if (GUILayout.Button ("取消", ButtonMyStyle(eButtonPos.right)))
			Close ();
		EditorGUILayout.EndHorizontal();
		// End Scroll View
		EditorGUILayout.EndScrollView ();
	}
	/// <summary>
	/// 執行產生一個新的檔案
	/// </summary>
	private void DoCreate()
	{
		if(string.IsNullOrEmpty(ShowSetInfo.SettingName))
		{
			EditorUtility.DisplayDialog("警告", "需要檔案名稱", "確定");
			return;
		}

		if(CheckFileNameRepeat())// 確認有沒有重複名稱
			return;

		SDDataMove.SaveData(ShowSetInfo);// 存檔

		Close();
	}
	/// <summary>
	/// 預設共通資料(取現在的PlayerSetting)
	/// </summary>
	private void DefaultSetCommon()
	{
		mShowSetInfo = new SDefineSet();
		// Names
		mShowSetInfo.CompanyName = PlayerSettings.companyName;
		mShowSetInfo.ProductName = PlayerSettings.productName;
		// Orientation
		mShowSetInfo.UIOrientation = PlayerSettings.defaultInterfaceOrientation;
		mShowSetInfo.UseAnimAutor = PlayerSettings.useAnimatedAutorotation;
		mShowSetInfo.OrienRoatable = new bool[4];
		mShowSetInfo.OrienRoatable[0] = PlayerSettings.allowedAutorotateToPortrait;
		mShowSetInfo.OrienRoatable[1] = PlayerSettings.allowedAutorotateToPortraitUpsideDown;
		mShowSetInfo.OrienRoatable[2] = PlayerSettings.allowedAutorotateToLandscapeLeft;
		mShowSetInfo.OrienRoatable[3] = PlayerSettings.allowedAutorotateToLandscapeRight;
		// Identification
		/*
		 * PlayerSettings.applicationIdentifier 是取得當下Platform的BundleIdentifier
		 * 要取得特定Platform的Identifier
		 * 要改用PlayerSettings.GetApplicationIdentifier(取得)和PlayerSettings.SetApplicationIdentifier(設置)
		*/
		mShowSetInfo.BundleIDStandalone = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone);
		mShowSetInfo.BundleVer = PlayerSettings.bundleVersion;
		// Scripting Define Symbols
		mShowSetInfo.ScriptDefineSymblos = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
		// Icon
		SDDataMove.GetIconsGroup(BuildTargetGroup.Standalone, ref mUIUseImages.DefaultIcon, ref mShowSetInfo.DefIcons);
		// ---------------------------------------------------------------
	}
	/// <summary>
	/// 預設Android資料(取現在的PlayerSetting)
	/// </summary>
	private void DefaultSetAndroid()
	{
		SDAndroidSet aTmpSet = new SDAndroidSet();
		// Resolution and Presentation
		aTmpSet.Use32BitDisplayBuffer = PlayerSettings.use32BitDisplayBuffer;
		aTmpSet.disableDepthAndStencilBuffers = PlayerSettings.Android.disableDepthAndStencilBuffers;
		aTmpSet.ShowActivityIndicatorOnLoading = PlayerSettings.Android.showActivityIndicatorOnLoading;
		// Identification
		aTmpSet.BundleIDAndroid = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
		aTmpSet.BundleCode = PlayerSettings.Android.bundleVersionCode;
		aTmpSet.SdkVersions = PlayerSettings.Android.minSdkVersion;
		// Configuration
		aTmpSet.TargetDevice = PlayerSettings.Android.targetDevice;
		aTmpSet.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Android);
		aTmpSet.ForceInternet = PlayerSettings.Android.forceInternetPermission;
		aTmpSet.ForceSDCard = PlayerSettings.Android.forceSDCardPermission;
		// Publishing Settings
		// PlayerSettings 的keystoreName是完整的Keystore路徑
		// 撰寫工具上我希望盡可能簡單的把檔案都放在Assets裡面
		// 所以在取路徑的時候檢查拿掉專案Assets之前的路徑
		string aKeyStorePath = PlayerSettings.Android.keystoreName;
		int aCheckNum = aKeyStorePath.IndexOf("Assets", System.StringComparison.OrdinalIgnoreCase);
		if(aCheckNum > 0)
			aKeyStorePath = aKeyStorePath.Remove(0, Application.dataPath.Length + 1);

		aTmpSet.KeyStorePath = aKeyStorePath;
		aTmpSet.KeyStorePassword = PlayerSettings.Android.keystorePass;
		aTmpSet.KeyAlialsName = PlayerSettings.Android.keyaliasName;
		aTmpSet.KeyAlialsPassword = PlayerSettings.Android.keyaliasPass;
		// Scripting Define Symbols
		aTmpSet.ScriptDefineSymblos = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
		// Icon
		aTmpSet.IconOverride = true;
		SDDataMove.GetIconsGroup(BuildTargetGroup.Android, ref mUIUseImages.AndroidIcons, ref aTmpSet.DefIcons);
		// Splash Images
		aTmpSet.SplashImage = SDDataMove.GetSplashScreenPath("androidSplashScreen", ref mUIUseImages.AndroidSplashImage);
		aTmpSet.SplashScreenScale = PlayerSettings.Android.splashScreenScale;
		//-------------------------------------------------
		// Unity5 New
		aTmpSet.GraphicsType = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
		// Set Down
		mShowSetInfo.AndroidSet = aTmpSet;
	}
	/// <summary>
	/// 預設IOS資料(取現在的PlayerSetting)
	/// </summary>
	private void DefaultSetIOS()
	{
		SDIOSSet aTmpSet = new SDIOSSet();
		// Resolution and Presentation
		aTmpSet.RequiresFullScreen = PlayerSettings.iOS.requiresFullScreen;
		aTmpSet.StatusBarHidden = PlayerSettings.statusBarHidden;
		aTmpSet.StatusBarStyle = PlayerSettings.iOS.statusBarStyle;
		aTmpSet.ShowActivityIndicatorOnLoading = PlayerSettings.iOS.showActivityIndicatorOnLoading;
		// Debugging and crash reporting
		aTmpSet.ActionOnDotNetUnhandledException = PlayerSettings.actionOnDotNetUnhandledException;
		aTmpSet.LogObjCUncaughtExceptions = PlayerSettings.logObjCUncaughtExceptions;
		aTmpSet.EnableCrashReportAPI = PlayerSettings.enableCrashReportAPI;
		// Identification
		aTmpSet.BundleIDIOS = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
		aTmpSet.BuildNumber = PlayerSettings.iOS.buildNumber;
		aTmpSet.AppleEnableAutomaticSigning = PlayerSettings.iOS.appleEnableAutomaticSigning;
		aTmpSet.AppleDeveloperTeamID = PlayerSettings.iOS.appleDeveloperTeamID;
		aTmpSet.ProvisioningProfileID = PlayerSettings.iOS.iOSManualProvisioningProfileID;
		// Configuration
		aTmpSet.ScriptingBackend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.iOS);
		aTmpSet.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.iOS);
		aTmpSet.TargetDevice = PlayerSettings.iOS.targetDevice;
		aTmpSet.SDKVersion = PlayerSettings.iOS.sdkVersion;
		aTmpSet.TargetOSVersionString = PlayerSettings.iOS.targetOSVersionString;
		aTmpSet.PrepareIOSForRecording = SDDataMove.GetBoolPlayerSetting("Prepare IOS For Recording");
		aTmpSet.RequiresPersistentWiFi = PlayerSettings.iOS.requiresPersistentWiFi;
		aTmpSet.AppInBackgroundBehavior = PlayerSettings.iOS.appInBackgroundBehavior;
		aTmpSet.Architecture = PlayerSettings.GetArchitecture(BuildTargetGroup.iOS);
		// Optimization
		aTmpSet.ScriptCallOptimizationLevel = PlayerSettings.iOS.scriptCallOptimization;
		aTmpSet.StripEngineCode = PlayerSettings.stripEngineCode;
		aTmpSet.StripLevel = PlayerSettings.strippingLevel;
		// Scripting Define Symbols
		aTmpSet.ScriptDefineSymblos = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
		// Icon
		aTmpSet.IconOverride = true;
		SDDataMove.GetIconsGroup(BuildTargetGroup.iOS, ref mUIUseImages.IosIcons, ref aTmpSet.DefIcons);
		aTmpSet.PrerenderedIcon = PlayerSettings.iOS.prerenderedIcon;
		// Splash Images
		int aEmumTotal = Enum.GetNames(typeof(eMobileSplashScreen)).Length;
		mUIUseImages.IOSSplashImages = new Texture2D[aEmumTotal];
		aTmpSet.SplashImages = new string[aEmumTotal];
		for(int i = 0; i < aEmumTotal; i++)
			aTmpSet.SplashImages[i] = SDDataMove.GetSplashScreenPath((eMobileSplashScreen)i, ref mUIUseImages.IOSSplashImages[i]);
		//-------------------------------------------------
		// Unity5 New
		aTmpSet.GraphicsType = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
		// Set Down
		mShowSetInfo.IOSSet = aTmpSet;
	}
}
#endif