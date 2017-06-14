#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
/**
 * 2017/06 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 共通會顯示的UI資料
 */
public abstract class SDBaseUI : EditorWindow
{
	// 取得Define存檔的列表
	private string[] mFileNameArray;
	public string[] FileNameArray {
		get {
			if(mFileNameArray == null)
				mFileNameArray = SDDataMove.GetSaveDataNames();
			return mFileNameArray;
		}
	}
	// 顯示中的設定
	protected SDefineSet mShowSetInfo;
	public SDefineSet ShowSetInfo {get{return mShowSetInfo;}}
	// 顯示用的圖像
	protected ShowImageGroup mUIUseImages = new ShowImageGroup();
	// 現在設定的頁面
	protected eSettingPage mNowPage;
	// UI相關狀態
	protected Vector2 mEditorScrollView;
	protected bool mShowCommon = true,// 共通設定頁
	mShowCommonOri = true,// 設定選轉方向的畫面顯示
	mShowAndroid,// Android設定頁
	mShowIOS;// IOS設定頁
	private int mAndInternetAccess,// Internet Access設定(用數字辨別)
	mAndWritePermission,// Write Permission設定(用數字辨別)
	mAndApiComLevel,// Androdi Api Compatibility Level設定(用數字辨別)
	mScriptBackIndex,// Scripting Backend設定(用數字辨別)
	mIOSApiComLevel;// IOS Api Compatibility Level設定(用數字辨別)
	#region Unity Base
	/// <summary>
	/// OnGUI>顯示介面
	/// OnEnable >啟動時預先處理的內容
	/// OnDisable >介面關閉後處理的內容
	/// </summary>
	protected virtual void OnGUI() {
		ShowUI ();
	}
	protected virtual void OnEnable() {
		SettingInit();
	}
	protected virtual void OnDisable() {
		mFileNameArray = null;
	}
	#endregion
	#region Base Method
	// 抽象化功能，OnGUI中先初始化再顯示UI
	protected abstract void SettingInit();
	protected abstract void ShowUI();
	/// <summary>
	/// 設定Define資料
	/// </summary>
	public void SetDefineData(SDefineSet iInfoData, ShowImageGroup iImageG)
	{
		mShowSetInfo = iInfoData;
		mUIUseImages = iImageG;
	}
	/// <summary>
	/// 設定ICon時使用的UI
	/// </summary>
	/// <param name="iTitle">標題</param>
	/// <param name="iImage">圖片</param>
	/// <param name="iIconName">圖片名稱</param>
	private void IconObjectSet(string iTitle, ref Texture2D iImage, ref string iIconName)
	{
		iImage = EditorGUILayout.ObjectField(iTitle, iImage, typeof(Texture2D), false) as Texture2D;

		if(iImage != null)
			iIconName = AssetDatabase.GetAssetPath(iImage);
		else
			iIconName = string.Empty;

		GUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PrefixLabel(SDBaseType.cUIName004);
		EditorGUILayout.LabelField(iIconName);
		GUILayout.EndHorizontal();
	}
	/// <summary>
	/// 設定鉤鉤介面(標題文字平均分佈)
	/// </summary>
	private bool SetToggleHor(string iInfo, bool iValue) {
		bool aResult = iValue;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iInfo);
		aResult = EditorGUILayout.Toggle(aResult);
		EditorGUILayout.EndHorizontal();
		return aResult;
	}
	/// <summary>
	/// 設定Enum屬性的選單(標題文字平均分佈)
	/// </summary>
	private System.Enum SetEnumPopupHor(string iInfo, System.Enum iValue) {
		System.Enum aResult = iValue;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iInfo);
		aResult = (System.Enum)EditorGUILayout.EnumPopup(aResult);
		EditorGUILayout.EndHorizontal();
		return aResult;
	}
	/// <summary>
	/// 設定依照文字陣列顯示的選單(標題文字平均分佈)
	/// </summary>
	private int SetPopupHor(string iInfo, int iValue, string[] iOption) {
		int aResult = iValue;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iInfo);
		aResult = EditorGUILayout.Popup(aResult, iOption);
		EditorGUILayout.EndHorizontal();
		return aResult;
	}
	/// <summary>
	/// 文字輸入介面(標題文字平均分佈)
	/// </summary>
	private string SetTextHor(string iInfo, string iValue) {
		string aResult = iValue;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iInfo);
		aResult = EditorGUILayout.TextField(aResult);
		EditorGUILayout.EndHorizontal();
		return aResult;
	}
	/// <summary>
	/// 設定整數輸入介面(標題文字平均分佈)
	/// </summary>
	private int SetIntHor(string iInfo, int iValue) {
		int aResult = iValue;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iInfo);
		aResult = EditorGUILayout.IntField(aResult);
		EditorGUILayout.EndHorizontal();
		return aResult;
	}
	#endregion
	#region Top UI
	/// <summary>
	/// 設置檔名和這個設定是為Android/IOS哪一個平台設定
	/// </summary>
	public void UITopSettingShow(bool iSetName = true) {
		if(iSetName)
			mShowSetInfo.SettingName = EditorGUILayout.TextField(SDBaseType.cUIName001, mShowSetInfo.SettingName);
		else
			EditorGUILayout.LabelField(SDBaseType.cUIName001, mShowSetInfo.SettingName);
		
		EditorGUILayout.HelpBox(SDBaseType.cUIInfo001, MessageType.Info);
		mShowSetInfo.DefineTarget = (eSDTarget)EditorGUILayout.EnumPopup(SDBaseType.cUIName002, mShowSetInfo.DefineTarget);
		EditorGUILayout.Space();
	}
	/// <summary>
	/// 列出已經擁有存檔的名稱
	/// </summary>
	public int UITopSaveFileSelect(int iSelectIndex)
	{
		int aResult = -1;
		if(FileNameArray != null) {
			aResult = EditorGUILayout.Popup(SDBaseType.cUIName003, iSelectIndex, FileNameArray);
			EditorGUILayout.Space();
		}
		return aResult;
	}
	#endregion
	#region Common UI
	/// <summary>
	/// 共通介面區域
	/// </summary>
	public void UICommonArea() {
		mShowCommon = EditorGUILayout.Foldout(mShowCommon, SDBaseType.cUIName005);
		if(!mShowCommon)
			return;
		EditorGUI.indentLevel++;
		UICommonNames();
		EditorGUILayout.Space();
		UICommonOrientation();
		EditorGUILayout.Space();
		UICommonIdentification();
		EditorGUILayout.Space();
		UICommonIcon();
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// App名稱
	/// </summary>
	private void UICommonNames() {
		mShowSetInfo.CompanyName = EditorGUILayout.TextField(SDBaseType.cUIName006, mShowSetInfo.CompanyName);
		mShowSetInfo.ProductName = EditorGUILayout.TextField(SDBaseType.cUIName007, mShowSetInfo.ProductName);
	}
	/// <summary>
	/// 螢幕轉向
	/// </summary>
	private void UICommonOrientation() {
		mShowCommonOri = EditorGUILayout.Foldout(mShowCommonOri, SDBaseType.cUIName008);
		if (mShowCommonOri) {
			EditorGUI.indentLevel++;
			// Orientaion Popup
			mShowSetInfo.UIOrientation = (UIOrientation)EditorGUILayout.EnumPopup(SDBaseType.cUIName009, mShowSetInfo.UIOrientation);
			if(mShowSetInfo.UIOrientation == UIOrientation.AutoRotation) {// If Auto Rotation, Show AutoRotation Set;
				if(mShowSetInfo.DefineTarget == eSDTarget.IOS) {// if SetTarget is IOS, Show UseAnimAutor Set
					mShowSetInfo.UseAnimAutor = EditorGUILayout.Toggle(SDBaseType.cUIName010, mShowSetInfo.UseAnimAutor);
				}
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(SDBaseType.cUIName011);
				EditorGUI.indentLevel++;
				for(int i = 0; i < SDBaseType.cAllowAutoRotNames.Length; i++) {// Custom Name Show
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(SDBaseType.cAllowAutoRotNames[i]);
					mShowSetInfo.OrienRoatable[i] = EditorGUILayout.Toggle(mShowSetInfo.OrienRoatable[i]);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}
	}
	/// <summary>
	/// BundleID與Define設定
	/// </summary>
	private void UICommonIdentification() {
		EditorGUILayout.PrefixLabel(SDBaseType.cUIName012, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mShowSetInfo.BundleIDStandalone = EditorGUILayout.TextField(SDBaseType.cUIName013, mShowSetInfo.BundleIDStandalone);
		mShowSetInfo.BundleVer = EditorGUILayout.TextField(SDBaseType.cUIName014, mShowSetInfo.BundleVer);
		// Define Info
		mShowSetInfo.ScriptDefineSymblos = SetTextHor(SDBaseType.cUIName073, mShowSetInfo.ScriptDefineSymblos);
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// 預設Icon
	/// </summary>
	private void UICommonIcon() {
		mShowSetInfo.IconSetStatus = EditorGUILayout.ToggleLeft(SDBaseType.cUIName015, mShowSetInfo.IconSetStatus, TitleFrontStyle());
		EditorGUILayout.Space();
		if(!mShowSetInfo.IconSetStatus)
			return;
		EditorGUI.indentLevel++;
		for (int i = 0; i < mUIUseImages.DefaultIcon.Length; i++) 
			IconObjectSet(SDBaseType.cUIName016, ref mUIUseImages.DefaultIcon[i], ref mShowSetInfo.DefIcons[i]);
		EditorGUI.indentLevel--;
	}
	#endregion
	#region Android UI
	/// <summary>
	/// Android介面區域
	/// </summary>
	public void UIAndroidArea() {
		mShowAndroid = EditorGUILayout.Foldout(mShowAndroid, SDBaseType.cUIName017);
		if(!mShowAndroid)
			return;
		EditorGUI.indentLevel++;
		UIAndroidResolution();
		EditorGUILayout.Space();
		UIAndroidOtherSetting();
		EditorGUILayout.Space();
		UIAndroidPublishingSetting();
		EditorGUILayout.Space();
		UIAndroidIcon();
		EditorGUILayout.Space();
		UIAndroidSplashImage();
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// 畫面設定
	/// </summary>
	private void UIAndroidResolution() {
		EditorGUILayout.LabelField(SDBaseType.cUIName018, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		mShowSetInfo.AndroidSet.Use32BitDisplayBuffer = SetToggleHor(SDBaseType.cUIName019, mShowSetInfo.AndroidSet.Use32BitDisplayBuffer);
		mShowSetInfo.AndroidSet.disableDepthAndStencilBuffers = SetToggleHor(SDBaseType.cUIName020, mShowSetInfo.AndroidSet.disableDepthAndStencilBuffers);
		mShowSetInfo.AndroidSet.ShowActivityIndicatorOnLoading =
			(AndroidShowActivityIndicatorOnLoading)SetEnumPopupHor(SDBaseType.cUIName021, mShowSetInfo.AndroidSet.ShowActivityIndicatorOnLoading);
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// 其他設定
	/// </summary>
	private void UIAndroidOtherSetting() {
		EditorGUILayout.LabelField(SDBaseType.cUIName022, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		// Identification
		EditorGUILayout.LabelField(SDBaseType.cUIName012, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mShowSetInfo.AndroidSet.BundleIDAndroid = SetTextHor(SDBaseType.cUIName013, mShowSetInfo.AndroidSet.BundleIDAndroid);
		mShowSetInfo.AndroidSet.BundleCode = SetIntHor(SDBaseType.cUIName023, mShowSetInfo.AndroidSet.BundleCode);
		mShowSetInfo.AndroidSet.SdkVersions = (AndroidSdkVersions)SetEnumPopupHor("Minimum API Level", mShowSetInfo.AndroidSet.SdkVersions);
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		// Configuration
		EditorGUILayout.LabelField(SDBaseType.cUIName024, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mShowSetInfo.AndroidSet.TargetDevice = (AndroidTargetDevice)SetEnumPopupHor("Device Filter", mShowSetInfo.AndroidSet.TargetDevice);
		if(mShowSetInfo.AndroidSet.ForceInternet)
			mAndInternetAccess = 1;
		if(mShowSetInfo.AndroidSet.ForceSDCard)
			mAndWritePermission = 1;
		if(mShowSetInfo.AndroidSet.ApiCompatibilityLevel == ApiCompatibilityLevel.NET_2_0_Subset)
			mAndApiComLevel = 1;

		mAndApiComLevel = SetPopupHor(SDBaseType.cUIName027, mAndApiComLevel, SDBaseType.ApiCompatibilityLevel);
		mAndInternetAccess = SetPopupHor(SDBaseType.cUIName025, mAndInternetAccess, SDBaseType.InternetAccess);
		mAndWritePermission = SetPopupHor(SDBaseType.cUIName026, mAndWritePermission, SDBaseType.WritePermission);

		switch(mAndApiComLevel) {
		case 0: mShowSetInfo.AndroidSet.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0; break;
		case 1: mShowSetInfo.AndroidSet.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset; break;
		}

		mShowSetInfo.AndroidSet.ForceInternet = mAndInternetAccess == 1;// Require = true; Auto = false;
		mShowSetInfo.AndroidSet.ForceSDCard = mAndWritePermission == 1;// External(SDCard) = true; Internal = false;
		// Define Info
		mShowSetInfo.AndroidSet.ScriptDefineSymblos = SetTextHor(SDBaseType.cUIName073, mShowSetInfo.AndroidSet.ScriptDefineSymblos);
		EditorGUILayout.Space();
		EditorGUI.indentLevel--;
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// Keystore設定
	/// KeyStore檔案的路徑為至預設在Unity專案底下，方便快速設定用
	/// </summary>
	private void UIAndroidPublishingSetting() {
		EditorGUILayout.LabelField(SDBaseType.cUIName028, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.HelpBox(SDBaseType.cUIName033, MessageType.Info);
		mShowSetInfo.AndroidSet.KeyStorePath = EditorGUILayout.TextField(SDBaseType.cUIName029, mShowSetInfo.AndroidSet.KeyStorePath);
		mShowSetInfo.AndroidSet.KeyStorePassword = EditorGUILayout.TextField(SDBaseType.cUIName030, mShowSetInfo.AndroidSet.KeyStorePassword);
		mShowSetInfo.AndroidSet.KeyAlialsName = EditorGUILayout.TextField(SDBaseType.cUIName031, mShowSetInfo.AndroidSet.KeyAlialsName);
		mShowSetInfo.AndroidSet.KeyAlialsPassword = EditorGUILayout.TextField(SDBaseType.cUIName032, mShowSetInfo.AndroidSet.KeyAlialsPassword);
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// Icon設定
	/// </summary>
	private void UIAndroidIcon() {
		mShowSetInfo.AndroidSet.IconSetStatus = EditorGUILayout.ToggleLeft(SDBaseType.cUIName034, mShowSetInfo.AndroidSet.IconSetStatus);
		if(!mShowSetInfo.AndroidSet.IconSetStatus)
			return;

		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		mShowSetInfo.AndroidSet.IconOverride = EditorGUILayout.ToggleLeft(SDBaseType.cUIName035, mShowSetInfo.AndroidSet.IconOverride);
		if(mShowSetInfo.AndroidSet.IconOverride) {
			EditorGUI.indentLevel++;

			int[] aAndroidIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
			for (int i = 0; i < mUIUseImages.AndroidIcons.Length; i++) {
				string aPicSize = "";
				if(i < aAndroidIconTexts.Length)
					aPicSize = aAndroidIconTexts[i] + "x" + aAndroidIconTexts[i];
				else
					aPicSize = SDBaseType.cUIName036;
				if(i < mShowSetInfo.AndroidSet.DefIcons.Length)
					IconObjectSet(aPicSize, ref mUIUseImages.AndroidIcons[i], ref mShowSetInfo.AndroidSet.DefIcons[i]);
			}
			EditorGUI.indentLevel--;
		}
		// Icon設定中有個Enable Android Banner區域，暫時不做處理
//		mEnableAndroidBanner = EditorGUILayout.ToggleLeft("Enable Android Banner", mEnableAndroidBanner);
//		if(mEnableAndroidBanner) {
//			EditorGUI.indentLevel++;
//			mEAndroidBanner = EditorGUILayout.ObjectField("320x180", mEAndroidBanner, typeof(Texture2D), false) as Texture2D;
//
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.Space();
//			EditorGUILayout.Space();
//			EditorGUILayout.PrefixLabel("Path :");
//			EditorGUILayout.LabelField("Tmp/Path");
//			EditorGUILayout.EndHorizontal();
//			EditorGUI.indentLevel--;
//		}
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// SplashImage設定
	/// </summary>
	private void UIAndroidSplashImage() {
		mShowSetInfo.AndroidSet.SplashSetStatus = EditorGUILayout.ToggleLeft(SDBaseType.cUIName037, mShowSetInfo.AndroidSet.SplashSetStatus);

		if(!mShowSetInfo.AndroidSet.SplashSetStatus)
			return;
		EditorGUILayout.HelpBox(SDBaseType.cUIName038, MessageType.Info);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		IconObjectSet(SDBaseType.cUIName039, ref mUIUseImages.AndroidSplashImage, ref mShowSetInfo.AndroidSet.SplashImage);

		if(mUIUseImages.AndroidSplashImage != null) {
			mShowSetInfo.AndroidSet.SplashScreenScale = (AndroidSplashScreenScale)SetEnumPopupHor(SDBaseType.cUIName040, mShowSetInfo.AndroidSet.SplashScreenScale);
		}
		EditorGUI.indentLevel--;
	}
	#endregion
	#region IOS UI
	/// <summary>
	/// IOS介面區域
	/// </summary>
	public void UIIOSArea() {
		mShowIOS = EditorGUILayout.Foldout(mShowIOS, SDBaseType.cUIName041);
		if(!mShowIOS)
			return;
		EditorGUI.indentLevel++;
		UIIOSResolution();
		EditorGUILayout.Space();
		FTUUIIOSDebugging();
		EditorGUILayout.Space();
		UIIOSOtherSetting();
		EditorGUILayout.Space();
		UIIOSIcon();
		EditorGUILayout.Space();
		UIIOSSplashImage();
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// 畫面設定
	/// </summary>
	private void UIIOSResolution() {
		EditorGUILayout.LabelField(SDBaseType.cUIName018, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.LabelField(SDBaseType.cUIName042, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		mShowSetInfo.IOSSet.RequiresFullScreen = SetToggleHor(SDBaseType.cUIName043, mShowSetInfo.IOSSet.RequiresFullScreen);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(SDBaseType.cUIName044, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		mShowSetInfo.IOSSet.StatusBarHidden = SetToggleHor(SDBaseType.cUIName045, mShowSetInfo.IOSSet.StatusBarHidden);
		mShowSetInfo.IOSSet.StatusBarStyle =
			(iOSStatusBarStyle)SetEnumPopupHor(SDBaseType.cUIName046, mShowSetInfo.IOSSet.StatusBarStyle);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		mShowSetInfo.IOSSet.ShowActivityIndicatorOnLoading =
			(iOSShowActivityIndicatorOnLoading)SetEnumPopupHor(SDBaseType.cUIName047, mShowSetInfo.IOSSet.ShowActivityIndicatorOnLoading);
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// IOS的Debug偵錯設定
	/// </summary>
	private void FTUUIIOSDebugging() {
		EditorGUILayout.LabelField(SDBaseType.cUIName048, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox(SDBaseType.cUIName049, MessageType.Info);
		EditorGUILayout.Space();

		EditorGUI.indentLevel++;
		// Start
		EditorGUILayout.LabelField(SDBaseType.cUIName050, EditorStyles.boldLabel);
		mShowSetInfo.IOSSet.ActionOnDotNetUnhandledException =
			(ActionOnDotNetUnhandledException)SetEnumPopupHor(SDBaseType.cUIName053, mShowSetInfo.IOSSet.ActionOnDotNetUnhandledException);
		mShowSetInfo.IOSSet.LogObjCUncaughtExceptions = SetToggleHor(SDBaseType.cUIName051, mShowSetInfo.IOSSet.LogObjCUncaughtExceptions);
		mShowSetInfo.IOSSet.EnableCrashReportAPI = SetToggleHor(SDBaseType.cUIName052, mShowSetInfo.IOSSet.EnableCrashReportAPI);
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// 其他設定
	/// </summary>
	private void UIIOSOtherSetting() {
		EditorGUILayout.LabelField(SDBaseType.cUIName022, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		// Start
		// Identification
		EditorGUILayout.LabelField(SDBaseType.cUIName012, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		mShowSetInfo.IOSSet.BundleIDIOS = SetTextHor(SDBaseType.cUIName013, mShowSetInfo.IOSSet.BundleIDIOS);
		mShowSetInfo.IOSSet.BuildNumber = SetTextHor(SDBaseType.cUIName054, mShowSetInfo.IOSSet.BuildNumber);
		mShowSetInfo.IOSSet.AppleEnableAutomaticSigning = SetToggleHor(SDBaseType.cUIName055, mShowSetInfo.IOSSet.AppleEnableAutomaticSigning);

		if(mShowSetInfo.IOSSet.AppleEnableAutomaticSigning) {
			mShowSetInfo.IOSSet.AppleDeveloperTeamID = SetTextHor(SDBaseType.cUIName056, mShowSetInfo.IOSSet.AppleDeveloperTeamID);
		}
		else {
			mShowSetInfo.IOSSet.ProvisioningProfileID = SetTextHor(SDBaseType.cUIName057, mShowSetInfo.IOSSet.ProvisioningProfileID);
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(SDBaseType.cUIName024, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		mScriptBackIndex = (int)mShowSetInfo.IOSSet.ScriptingBackend;
		mScriptBackIndex = SetPopupHor(SDBaseType.cUIName058, mScriptBackIndex, SDBaseType.ScriptingBackend);
		switch(mScriptBackIndex) {
		case 0: mShowSetInfo.IOSSet.ScriptingBackend = ScriptingImplementation.Mono2x; break;
		case 1: mShowSetInfo.IOSSet.ScriptingBackend = ScriptingImplementation.IL2CPP; break;
		}
		// API Compatibility Level
		if(mShowSetInfo.IOSSet.ApiCompatibilityLevel == ApiCompatibilityLevel.NET_2_0_Subset)
			mIOSApiComLevel = 1;
		mIOSApiComLevel = SetPopupHor(SDBaseType.cUIName027, mIOSApiComLevel, SDBaseType.ApiCompatibilityLevel);
		switch(mIOSApiComLevel) {
		case 0: mShowSetInfo.IOSSet.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0; break;
		case 1: mShowSetInfo.IOSSet.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_2_0_Subset; break;
		}
		mShowSetInfo.IOSSet.TargetDevice = (iOSTargetDevice)SetEnumPopupHor(SDBaseType.cUIName060, mShowSetInfo.IOSSet.TargetDevice);
		mShowSetInfo.IOSSet.SDKVersion = (iOSSdkVersion)SetEnumPopupHor(SDBaseType.cUIName061, mShowSetInfo.IOSSet.SDKVersion);
		// Architecture
		if(mShowSetInfo.IOSSet.ScriptingBackend == ScriptingImplementation.IL2CPP &&
			mShowSetInfo.IOSSet.SDKVersion == iOSSdkVersion.DeviceSDK) {
			iPhoneArchitecture aTmpArch = (iPhoneArchitecture)mShowSetInfo.IOSSet.Architecture;
			aTmpArch = (iPhoneArchitecture)SetEnumPopupHor(SDBaseType.cUIName059, aTmpArch);
			mShowSetInfo.IOSSet.Architecture = (int)aTmpArch;
		}
		mShowSetInfo.IOSSet.TargetOSVersionString = SetTextHor(SDBaseType.cUIName062, mShowSetInfo.IOSSet.TargetOSVersionString);
		mShowSetInfo.IOSSet.PrepareIOSForRecording = SetToggleHor(SDBaseType.cUIName063, mShowSetInfo.IOSSet.PrepareIOSForRecording);
		mShowSetInfo.IOSSet.RequiresPersistentWiFi = SetToggleHor(SDBaseType.cUIName064, mShowSetInfo.IOSSet.RequiresPersistentWiFi);
		mShowSetInfo.IOSSet.AppInBackgroundBehavior =
			(iOSAppInBackgroundBehavior)SetEnumPopupHor(SDBaseType.cUIName065, mShowSetInfo.IOSSet.AppInBackgroundBehavior);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(SDBaseType.cUIName066, EditorStyles.boldLabel);
		EditorGUILayout.Space();
		if(mShowSetInfo.IOSSet.ScriptingBackend == ScriptingImplementation.IL2CPP) {
			mShowSetInfo.IOSSet.StripEngineCode =
				SetToggleHor(SDBaseType.cUIName067, mShowSetInfo.IOSSet.StripEngineCode);
		}
		else if(mShowSetInfo.IOSSet.ScriptingBackend == ScriptingImplementation.Mono2x) {			
			mShowSetInfo.IOSSet.StripLevel =
				(StrippingLevel)SetEnumPopupHor(SDBaseType.cUIName068, mShowSetInfo.IOSSet.StripLevel);
		}
		mShowSetInfo.IOSSet.ScriptCallOptimizationLevel =
			(ScriptCallOptimizationLevel)SetEnumPopupHor(SDBaseType.cUIName069, mShowSetInfo.IOSSet.ScriptCallOptimizationLevel);
		// Define Info
		mShowSetInfo.IOSSet.ScriptDefineSymblos = SetTextHor(SDBaseType.cUIName073, mShowSetInfo.IOSSet.ScriptDefineSymblos);
		// End
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// ICON設定
	/// </summary>
	private void UIIOSIcon() {
		mShowSetInfo.IOSSet.IconSetStatus = EditorGUILayout.ToggleLeft(SDBaseType.cUIName034, mShowSetInfo.IOSSet.IconSetStatus);
		if(!mShowSetInfo.IOSSet.IconSetStatus)
			return;

		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mShowSetInfo.IOSSet.IconOverride = EditorGUILayout.ToggleLeft(SDBaseType.cUIName035, mShowSetInfo.IOSSet.IconOverride);
		if(mShowSetInfo.IOSSet.IconOverride) {
			EditorGUI.indentLevel++;

			int[] aIosIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
			for (int i = 0; i < mUIUseImages.IosIcons.Length; i++) {
				string aPicSize = "";
				if(i < aIosIconTexts.Length)
					aPicSize = aIosIconTexts[i] + "x" + aIosIconTexts[i];
				else
					aPicSize = SDBaseType.cUIName036;
				if(i == 9) {
					EditorGUILayout.LabelField(SDBaseType.cUIName070, EditorStyles.boldLabel);
					EditorGUILayout.Space();
				}
				else if (i == 12) {
					EditorGUILayout.LabelField(SDBaseType.cUIName071, EditorStyles.boldLabel);
					EditorGUILayout.Space();
				}
				if(i < mShowSetInfo.IOSSet.DefIcons.Length)
					IconObjectSet(aPicSize, ref mUIUseImages.IosIcons[i], ref mShowSetInfo.IOSSet.DefIcons[i]);
			}
			EditorGUI.indentLevel--;
		}
		mShowSetInfo.IOSSet.PrerenderedIcon = SetToggleHor(SDBaseType.cUIName072, mShowSetInfo.IOSSet.PrerenderedIcon);
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// SplashImage設定
	/// </summary>
	private void UIIOSSplashImage() {
		mShowSetInfo.IOSSet.SplashSetStatus = EditorGUILayout.ToggleLeft(SDBaseType.cUIName037, mShowSetInfo.IOSSet.SplashSetStatus);

		if(!mShowSetInfo.IOSSet.SplashSetStatus)
			return;
		EditorGUILayout.HelpBox(SDBaseType.cUIName038, MessageType.Info);
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;

		for(int i = 0; i < mShowSetInfo.IOSSet.SplashImages.Length; i++) {
			if( i < SDBaseType.IOSLunchImages.Length) {
				IconObjectSet(SDBaseType.IOSLunchImages[i],
					ref mUIUseImages.IOSSplashImages[i],
					ref mShowSetInfo.IOSSet.SplashImages[i]);
			}
		}
	}
	#endregion
	/// <summary>
	/// IOS Splash Image設定
	/// </summary>
	public void UISplashIOSShow()
	{
		GUILayout.Label("IOS Splash 圖片", TitleFrontStyle());
		
		for(int i = 0; i < mShowSetInfo.IOSSet.SplashImages.Length; i++)
		{
			eMobileSplashScreen aShowScreen = (eMobileSplashScreen)i;
			IconObjectSet(aShowScreen.ToString(),
				ref mUIUseImages.IOSSplashImages[i],
				ref mShowSetInfo.IOSSet.SplashImages[i]);
		}
	}
	/// <summary>
	/// 設定樣式(Title)
	/// </summary>
	public GUIStyle TitleFrontStyle()
	{
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = 20;
		myStyle.normal.textColor = Color.white;
		
		return myStyle;
	}
	/// <summary>
	/// 設定樣式(Text)
	/// </summary>
	public GUIStyle TextFrontMyStyle()
	{
		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = 16;
		myStyle.normal.textColor = Color.white;
		
		return myStyle;
	}
	/// <summary>
	/// 設定樣式(Button)
	/// </summary>
	public GUIStyle ButtonMyStyle(eButtonPos iPos)
	{
		GUIStyle myStyle = EditorStyles.miniButtonLeft;
		
		switch(iPos)
		{
		case eButtonPos.left:  myStyle = EditorStyles.miniButtonLeft; break;
		case eButtonPos.mid:   myStyle = EditorStyles.miniButtonMid; break;
		case eButtonPos.right: myStyle = EditorStyles.miniButtonRight; break;
		default: myStyle = EditorStyles.miniButton; break;
		}
		myStyle.fontSize = 20;
		
		return myStyle;
	}
	/// <summary>
	/// 設定樣式(Popup)
	/// </summary>
	public GUIStyle EditorPopMyStyle()
	{
		GUIStyle myStyle = EditorStyles.popup;
		myStyle.alignment = TextAnchor.MiddleCenter;
		myStyle.fontSize = 16;
		myStyle.fixedHeight = 20;
		
		return myStyle;
	}
	/// <summary>
	/// 設定樣式(確認擁有檔案)
	/// </summary>
	public bool CheckHaveFiles()
	{
		if(FileNameArray == null)
		{
			NoAnyFilesEW();
			return true;
		}
		
		if(FileNameArray.Length <= 0)
		{
			NoAnyFilesEW();
			return true;
		}
		
		return false;
	}
	/// <summary>
	/// 沒有存檔警告
	/// </summary>
	private void NoAnyFilesEW()
	{
		EditorUtility.DisplayDialog("警告", "沒有任何存檔", "確定");
		Close();
	}
	/// <summary>
	/// 確認是否有重複檔案
	/// </summary>
	public bool CheckFileNameRepeat(bool iShowAlert = true)
	{
		bool aIsSame = false;
		if(FileNameArray == null)
			return aIsSame;

		for(int i = 0; i < FileNameArray.Length; i++)
		{
			if(ShowSetInfo.SettingName.Equals(FileNameArray[i]))
			{
				aIsSame = true;
				break;
			}
		}
		
		if(aIsSame && iShowAlert)
			EditorUtility.DisplayDialog("警告", "檔案名稱與現有檔案重複", "確定");
		
		return aIsSame;
	}
}
#endif