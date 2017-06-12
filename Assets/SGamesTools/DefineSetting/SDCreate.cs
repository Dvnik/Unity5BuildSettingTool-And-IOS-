﻿#if UNITY_EDITOR
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
		//-----
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
		mShowSetInfo.BundleIDUnknow = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Unknown);
		mShowSetInfo.BundleVer = PlayerSettings.bundleVersion;
		// Icon
		SDDataMove.GetIconsGroup(BuildTargetGroup.Unknown, ref mUIUseImages.DefaultIcon, ref mShowSetInfo.DefIcons);

		// ---------------------------------------------------------------

//		mShowSetInfo.ShortBundleVer = PlayerSettings.shortBundleVersion;
		// Other
		mShowSetInfo.StatusBarHidden = PlayerSettings.statusBarHidden;

		/*
		 * PlayerSettings.apiCompatibilityLevel
		 * Deprecated. Use PlayerSettings.GetApiCompatibilityLevel and PlayerSettings.SetApiCompatibilityLevel instead.
		*/
//		mShowSetInfo.ApiCompatibilityLevel = PlayerSettings.apiCompatibilityLevel;// 4.6Ver


		mShowSetInfo.StrippingLevel = PlayerSettings.strippingLevel;

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
		// Configuration
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
		// Icon
		aTmpSet.IconOverride = true;
		SDDataMove.GetIconsGroup(BuildTargetGroup.Android, ref mUIUseImages.AndroidIcons, ref aTmpSet.DefIcons);
		// Splash Images
		aTmpSet.SplashImage = SDDataMove.GetSplashScreenPath("androidSplashScreen", ref mUIUseImages.AndroidSplashImage);
		aTmpSet.SplashScreenScale = PlayerSettings.Android.splashScreenScale;
		//-------------------------------------------------

		aTmpSet.SdkVersions = PlayerSettings.Android.minSdkVersion;
		aTmpSet.TargetDevice = PlayerSettings.Android.targetDevice;
//		aTmpSet.TargetGraphice = PlayerSettings.targetGlesGraphics;// 4.6Ver
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



		aTmpSet.BuildNumber = PlayerSettings.iOS.buildNumber;
		aTmpSet.StatusBarStyle = PlayerSettings.iOS.statusBarStyle;
		aTmpSet.ShowActivityIndicatorOnLoading = PlayerSettings.iOS.showActivityIndicatorOnLoading;
		aTmpSet.TargetDevice = PlayerSettings.iOS.targetDevice;
		aTmpSet.SDKVersion = PlayerSettings.iOS.sdkVersion;
		aTmpSet.ScriptCallOptimizationLevel = PlayerSettings.iOS.scriptCallOptimization;
//		aTmpSet.OverrideIPodMusic = SDDataMove.GetBoolPlayerSetting("Override IPod Music");
		aTmpSet.PrepareIOSForRecording = SDDataMove.GetBoolPlayerSetting("Prepare IOS For Recording");
		aTmpSet.RequiresPersistentWiFi = PlayerSettings.iOS.requiresPersistentWiFi;
		aTmpSet.PrerenderedIcon = PlayerSettings.iOS.prerenderedIcon;
		// Unity 4.6(under) Old
//		aTmpSet.TargetResolution = PlayerSettings.iOS.targetResolution;// 4.6Ver
//		aTmpSet.TargetGraphics = PlayerSettings.targetIOSGraphics;// 4.6Ver
//		aTmpSet.TargetOSVersion = PlayerSettings.iOS.targetOSVersion;// 4.6Ver
//		aTmpSet.ExitOnSuspend = PlayerSettings.iOS.exitOnSuspend;// 4.6Ver
//		aTmpSet.ScriptingBackend = (ScriptingImplementation)PlayerSettings.GetPropertyInt("ScriptingBackend", BuildTargetGroup.iOS);
//		if(aTmpSet.ScriptingBackend == ScriptingImplementation.IL2CPP)
//			aTmpSet.Architecture = (iPhoneArchitecture)PlayerSettings.GetPropertyInt("Architecture", BuildTargetGroup.iOS);
		// Unity5 New
		aTmpSet.GraphicsType = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
		aTmpSet.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.iOS);
		aTmpSet.TargetOSVersionString = PlayerSettings.iOS.targetOSVersionString;
		aTmpSet.AppInBackgroundBehavior = PlayerSettings.iOS.appInBackgroundBehavior;
		aTmpSet.ScriptingBackend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.iOS);
		if(aTmpSet.ScriptingBackend == ScriptingImplementation.IL2CPP) {
			aTmpSet.StripEngineCode = PlayerSettings.stripEngineCode;
			aTmpSet.Architecture = PlayerSettings.GetArchitecture(BuildTargetGroup.iOS);
		}
		else if(aTmpSet.ScriptingBackend == ScriptingImplementation.Mono2x) {			
			aTmpSet.StripLevel = PlayerSettings.strippingLevel;
		}
		// Icon
		aTmpSet.IconOverride = true;
		SDDataMove.GetIconsGroup(BuildTargetGroup.iOS, ref mUIUseImages.IosIcons, ref aTmpSet.DefIcons);
		// Set Down
		mShowSetInfo.IOSSet = aTmpSet;
		// Splash Images
		int aEmumTotal = Enum.GetNames(typeof(eMobileSplashScreen)).Length;
		mUIUseImages.IOSSplashImages = new Texture2D[aEmumTotal];
		mShowSetInfo.IOSSet.SplashImages = new string[aEmumTotal];
		for(int i = 0; i < aEmumTotal; i++)
			mShowSetInfo.IOSSet.SplashImages[i] = SDDataMove.GetSplashScreenPath((eMobileSplashScreen)i, ref mUIUseImages.IOSSplashImages[i]);
	}
}
#endif