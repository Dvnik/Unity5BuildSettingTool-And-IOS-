#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
/**
 * 2017/04 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 共通會顯示的UI資料
 */
public abstract class SDBaseUI : EditorWindow
{
	// 取得Define存檔的列表
	private string[] mFileNameArray;
	public string[] FileNameArray { get { return mFileNameArray; } }
	// 顯示中的設定
	protected SDefineSet mShowSetInfo;
	public SDefineSet ShowSetInfo {get{return mShowSetInfo;}}
	// 顯示用的圖像
	protected ShowImageGroup mUIUseImages = new ShowImageGroup();
	// 現在設定的頁面
	protected eSettingPage mNowPage;
	// UI相關狀態
	protected Vector2 mEditorScrollView;
	protected bool mInitStatus,
	mShowCommon = true,
	mShowCommonOri = true,
	mSetDefaultIcon;

	#region Unity Base
	protected virtual void OnGUI() {
		ShowUI ();
	}
	protected virtual void OnEnable() {
		SettingInit();
		if(mFileNameArray == null)
			mFileNameArray = SDDataMove.GetSaveDataNames();
	}
	protected virtual void OnDisable() {
		mInitStatus = false;
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
	/// User interfaces the common names.
	/// </summary>
	private void UICommonNames() {
		mShowSetInfo.CompanyName = EditorGUILayout.TextField(SDBaseType.cUIName006, mShowSetInfo.CompanyName);
		mShowSetInfo.ProductName = EditorGUILayout.TextField(SDBaseType.cUIName007, mShowSetInfo.ProductName);
	}
	/// <summary>
	/// User interfaces the common orientation.
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
	/// User interfaces the common identification.
	/// </summary>
	private void UICommonIdentification() {
		EditorGUILayout.PrefixLabel(SDBaseType.cUIName012, TitleFrontStyle());
		EditorGUILayout.Space();
		EditorGUI.indentLevel++;
		mShowSetInfo.BundleIDUnknow = EditorGUILayout.TextField(SDBaseType.cUIName013, mShowSetInfo.BundleIDUnknow);
		mShowSetInfo.BundleVer = EditorGUILayout.TextField(SDBaseType.cUIName014, mShowSetInfo.BundleVer);
		EditorGUI.indentLevel--;
	}
	/// <summary>
	/// User interfaces the common icon.
	/// </summary>
	private void UICommonIcon() {
		mSetDefaultIcon = EditorGUILayout.ToggleLeft(SDBaseType.cUIName015, mSetDefaultIcon, TitleFrontStyle());
		EditorGUILayout.Space();
		if(!mSetDefaultIcon)
			return;
		EditorGUI.indentLevel++;
		for (int i = 0; i < mUIUseImages.DefaultIcon.Length; i++) 
			IconObjectSet(SDBaseType.cUIName016, ref mUIUseImages.DefaultIcon[i], ref mShowSetInfo.DefIcons[i]);
		EditorGUI.indentLevel--;
	}
	#endregion
	/// <summary>
	/// Android設定檔UI
	/// </summary>
	public void UIAndoridShow()
	{
		GUILayout.Label("Android 設置", TitleFrontStyle());
		mShowSetInfo.AndroidSet.ShowActivityIndicatorOnLoading =
			(AndroidShowActivityIndicatorOnLoading)EditorGUILayout.EnumPopup("Show Loading Indicator:", mShowSetInfo.AndroidSet.ShowActivityIndicatorOnLoading);
		GUILayout.Label("Android Bundle Identifier 設置", TitleFrontStyle());
		mShowSetInfo.AndroidSet.BundleCode = EditorGUILayout.IntField("Bundle Code", mShowSetInfo.AndroidSet.BundleCode);
		mShowSetInfo.AndroidSet.SdkVersions = (AndroidSdkVersions)EditorGUILayout.EnumPopup("SDK Versions:", mShowSetInfo.AndroidSet.SdkVersions);
		GUILayout.Label("Android Configuration(其他設置)", TitleFrontStyle());
		mShowSetInfo.AndroidSet.TargetDevice = (AndroidTargetDevice)EditorGUILayout.EnumPopup("Android Target Device:", mShowSetInfo.AndroidSet.TargetDevice);
		///
//		mShowSetInfo.AndroidSet.TargetGraphice = (TargetGlesGraphics)EditorGUILayout.EnumPopup("Android Graphics Level:", mShowSetInfo.AndroidSet.TargetGraphice);// 4.6Ver
		///
		mShowSetInfo.AndroidSet.ForceInternet = EditorGUILayout.Toggle("Force Internet Permission:", mShowSetInfo.AndroidSet.ForceInternet);
		mShowSetInfo.AndroidSet.ForceSDCard = EditorGUILayout.Toggle("Force SDCard Permission:", mShowSetInfo.AndroidSet.ForceSDCard);
		GUILayout.Label("Android Key Store", TitleFrontStyle());
		mShowSetInfo.AndroidSet.KeyStorePath = EditorGUILayout.TextField("Android Key Path:", mShowSetInfo.AndroidSet.KeyStorePath);
		mShowSetInfo.AndroidSet.KeyStorePassword = EditorGUILayout.TextField("Keystore Pass:", mShowSetInfo.AndroidSet.KeyStorePassword);
		mShowSetInfo.AndroidSet.KeyAlialsName = EditorGUILayout.TextField("Keyalias Name:", mShowSetInfo.AndroidSet.KeyAlialsName);
		mShowSetInfo.AndroidSet.KeyAlialsPassword = EditorGUILayout.TextField("Keyalias Pass:", mShowSetInfo.AndroidSet.KeyAlialsPassword);
		GUILayout.Label("Manifest Xml Set", TitleFrontStyle());
		mShowSetInfo.AndroidSet.ManifestVersionCode = EditorGUILayout.TextField("Version Code:", mShowSetInfo.AndroidSet.ManifestVersionCode);
		mShowSetInfo.AndroidSet.ManifestVersionName = EditorGUILayout.TextField("Version Name:", mShowSetInfo.AndroidSet.ManifestVersionName);
		// Unity5 New
		GUILayout.Label("");
		/*
		 * 由於5.6的GraphicsAPI UI還沒想好要怎麼作修改畫面，先註解

		GUILayout.Label("Android Graphics APIs", TitleFrontStyle());
		for(int i = 0; i < mShowSetInfo.AndroidSet.GraphicesType.Length; i++) {
			mShowSetInfo.AndroidSet.GraphicsType[i] = (UnityEngine.Rendering.GraphicsDeviceType)EditorGUILayout.EnumPopup( i + " :", mShowSetInfo.AndroidSet.GraphicsType[i]);
		}
		*/
		mShowSetInfo.AndroidSet.ApiCompatibilityLevel = (ApiCompatibilityLevel)EditorGUILayout.EnumPopup("Api Compatibility Level:", mShowSetInfo.AndroidSet.ApiCompatibilityLevel);
		GUILayout.Label("");
		// Icon
		mShowSetInfo.AndroidSet.IconSetStatus = EditorGUILayout.Toggle("設定Android ICon圖:", mShowSetInfo.AndroidSet.IconSetStatus);
		if(mShowSetInfo.AndroidSet.IconSetStatus)
			UIIconAndoridShow();
		// Splash Image
		mShowSetInfo.AndroidSet.SplashSetStatus = EditorGUILayout.Toggle("設定Android Splash圖:", mShowSetInfo.AndroidSet.SplashSetStatus);
		if(mShowSetInfo.AndroidSet.SplashSetStatus)
			UISplashAndroidShow();
	}
	/// <summary>
	/// IOS設定檔UI
	/// </summary>
	public void UIIOSShow()
	{
		GUILayout.Label("IOS 設置", TitleFrontStyle());
		mShowSetInfo.IOSSet.BuildNumber = EditorGUILayout.TextField("Build Number :", mShowSetInfo.IOSSet.BuildNumber);
		mShowSetInfo.IOSSet.StatusBarStyle = (iOSStatusBarStyle)EditorGUILayout.EnumPopup("Status Bar Style:", mShowSetInfo.IOSSet.StatusBarStyle);
		mShowSetInfo.IOSSet.ShowActivityIndicatorOnLoading =
			(iOSShowActivityIndicatorOnLoading)EditorGUILayout.EnumPopup("Show Loading Indicator:", mShowSetInfo.IOSSet.ShowActivityIndicatorOnLoading);
		GUILayout.Label("IOS SDK Setting", TitleFrontStyle());
		mShowSetInfo.IOSSet.SDKVersion = (iOSSdkVersion)EditorGUILayout.EnumPopup("SDK Version:", mShowSetInfo.IOSSet.SDKVersion);
		mShowSetInfo.IOSSet.ScriptCallOptimizationLevel = (ScriptCallOptimizationLevel)EditorGUILayout.EnumPopup("Optimization Level:", mShowSetInfo.IOSSet.ScriptCallOptimizationLevel);
		GUILayout.Label("IOS Configuration", TitleFrontStyle());
		mShowSetInfo.IOSSet.TargetDevice = (iOSTargetDevice)EditorGUILayout.EnumPopup("Target Device:", mShowSetInfo.IOSSet.TargetDevice);
		GUILayout.Label("");
//		mShowSetInfo.IOSSet.OverrideIPodMusic = EditorGUILayout.Toggle("Override IPod Music:", mShowSetInfo.IOSSet.OverrideIPodMusic);
		mShowSetInfo.IOSSet.PrepareIOSForRecording = EditorGUILayout.Toggle("IOS For Recording:", mShowSetInfo.IOSSet.PrepareIOSForRecording);
		mShowSetInfo.IOSSet.RequiresPersistentWiFi = EditorGUILayout.Toggle("Requires Persistent WiFi:", mShowSetInfo.IOSSet.RequiresPersistentWiFi);
		// Unity 4.6(under) Old
//		mShowSetInfo.IOSSet.TargetOSVersion = (iOSTargetOSVersion)EditorGUILayout.EnumPopup("Target OS Version:", mShowSetInfo.IOSSet.TargetOSVersion);// 4.6Ver
//		mShowSetInfo.IOSSet.TargetResolution = (iOSTargetResolution)EditorGUILayout.EnumPopup("iOS Target Resolution:", mShowSetInfo.IOSSet.TargetResolution);// 4.6Ver
//		mShowSetInfo.IOSSet.TargetGraphics = (TargetIOSGraphics)EditorGUILayout.EnumPopup("iOS Graphics API:", mShowSetInfo.IOSSet.TargetGraphics);// 4.6Ver
//		mShowSetInfo.IOSSet.ExitOnSuspend = EditorGUILayout.Toggle("Exit On Suspend:", mShowSetInfo.IOSSet.ExitOnSuspend);// 4.6Ver
		// Unity5 New
		GUILayout.Label("");
		/*
		 * 由於5.6的GraphicsAPI UI還沒想好要怎麼作修改畫面，先註解

 		GUILayout.Label("IOS Graphics APIs", TitleFrontStyle());
		for(int i = 0; i < mShowSetInfo.IOSSet.GraphicsType.Length; i++) {
			mShowSetInfo.IOSSet.GraphicsType[i] = (UnityEngine.Rendering.GraphicsDeviceType)EditorGUILayout.EnumPopup( i + " :", mShowSetInfo.IOSSet.GraphicsType[i]);
		}
		 */
		mShowSetInfo.IOSSet.ApiCompatibilityLevel = (ApiCompatibilityLevel)EditorGUILayout.EnumPopup("Api Compatibility Level:", mShowSetInfo.IOSSet.ApiCompatibilityLevel);
		mShowSetInfo.IOSSet.TargetOSVersionString = EditorGUILayout.TextField("Target minimum iOS Version:", mShowSetInfo.IOSSet.TargetOSVersionString);
		mShowSetInfo.IOSSet.AppInBackgroundBehavior = (iOSAppInBackgroundBehavior)EditorGUILayout.EnumPopup("Behavior in Background:", mShowSetInfo.IOSSet.AppInBackgroundBehavior);
		mShowSetInfo.IOSSet.ScriptingBackend = (ScriptingImplementation)EditorGUILayout.EnumPopup("Scripting Backend:", mShowSetInfo.IOSSet.ScriptingBackend);
		if(mShowSetInfo.IOSSet.ScriptingBackend == ScriptingImplementation.IL2CPP) {
			iPhoneArchitecture aShowEnum = (iPhoneArchitecture)mShowSetInfo.IOSSet.Architecture;
			aShowEnum = (iPhoneArchitecture)EditorGUILayout.EnumPopup("Architecture:", aShowEnum);
			mShowSetInfo.IOSSet.Architecture = (int)aShowEnum;
		}
		GUILayout.Label("");
		// Icon
		mShowSetInfo.IOSSet.IconSetStatus = EditorGUILayout.Toggle("設定IOS ICon圖:", mShowSetInfo.IOSSet.IconSetStatus);
		if(mShowSetInfo.IOSSet.IconSetStatus)
			UIIconIOSShow();
		// Splash Image
		mShowSetInfo.IOSSet.SplashSetStatus = EditorGUILayout.Toggle("設定IOS Splash圖:", mShowSetInfo.IOSSet.SplashSetStatus);
		if(mShowSetInfo.IOSSet.SplashSetStatus)
			UISplashIOSShow();
	}
	/// <summary>
	/// Android的ICON設定
	/// </summary>
	private void UIIconAndoridShow()
	{
		GUILayout.Label("Android Icon 圖片", TitleFrontStyle());
		mShowSetInfo.AndroidSet.IconOverride = EditorGUILayout.Toggle("是否覆寫Icon圖:", mShowSetInfo.AndroidSet.IconOverride);
		if(!mShowSetInfo.AndroidSet.IconOverride)
			return;
		int[] aAndroidIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.Android);
		
		for (int i = 0; i < mUIUseImages.AndroidIcons.Length; i++)
		{
			string aPicSize = "";
			if(i < aAndroidIconTexts.Length)
				aPicSize = aAndroidIconTexts[i] + "x" + aAndroidIconTexts[i];
			else
				aPicSize = "Other";

			IconObjectSet(aPicSize, ref mUIUseImages.AndroidIcons[i], ref mShowSetInfo.AndroidSet.DefIcons[i]);
		}
	}
	/// <summary>
	/// IOS的ICON設定
	/// </summary>
	private void UIIconIOSShow()
	{
		GUILayout.Label("IOS Icon 圖片", TitleFrontStyle());
		mShowSetInfo.IOSSet.IconOverride = EditorGUILayout.Toggle("是否覆寫Icon圖:", mShowSetInfo.IOSSet.IconOverride);
		mShowSetInfo.IOSSet.PrerenderedIcon = EditorGUILayout.Toggle("Prerendered Icon:", mShowSetInfo.IOSSet.PrerenderedIcon);
		if(!mShowSetInfo.IOSSet.IconOverride)
			return;

		int[] aIosIconTexts = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);
		for (int i = 0; i < mUIUseImages.IosIcons.Length; i++)
		{
			string aPicSize = "";
			if(i < aIosIconTexts.Length)
				aPicSize = aIosIconTexts[i] + "x" + aIosIconTexts[i];
			else
				aPicSize = "Other";
			
			IconObjectSet(aPicSize, ref mUIUseImages.IosIcons[i], ref mShowSetInfo.IOSSet.DefIcons[i]);
		}
	}
	/// <summary>
	/// Android Splash Image設定
	/// </summary>
	public void UISplashAndroidShow()
	{
		GUILayout.Label("Android Splash 圖片", TitleFrontStyle());
		IconObjectSet(eMobileSplashScreen.iPhoneSplashScreen.ToString(),
			ref mUIUseImages.AndroidSplashImage,
			ref mShowSetInfo.AndroidSet.SplashImage);
		//SplashImage - android only
		mShowSetInfo.AndroidSet.SplashScreenScale =
			(AndroidSplashScreenScale)EditorGUILayout.EnumPopup("Splash Screen Scale:", mShowSetInfo.AndroidSet.SplashScreenScale);
	}
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
	public bool CheckFileNameRepeat()
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
		
		if(aIsSame)
			EditorUtility.DisplayDialog("警告", "檔案名稱與現有檔案重複", "確定");
		
		return aIsSame;
	}
}
#endif