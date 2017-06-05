#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Rendering;
/**
 * 2017/04 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 修改UnityPlayerSetting的基本型別宣告
 */
public class SDBaseType{}

// 參照PlayerSetting加入
public class SDefineSet
{
	public string SettingName;// 設定名稱(檔案名稱)
	public string CompanyName;// 公司名稱
	public string ProductName;// 產檔檔名
	public eSDTarget DefineTarget;// 這個Define是為哪一個版本設置
	// Define Info
	public string ScriptDefine;
	// Orientation
	public UIOrientation UIOrientation;// 畫面方向
	public bool[] OrienRoatable;// 畫面可轉動的方向(Portrait, PortraitUpSideDown, LandRight, LandLeft)
	//系統版本
	public bool StatusBarHidden;		// 
	public bool Use32BitDisplayBuffer;	// 
	// Facebook
	public string FacebookID;// 臉書appID
	//Identification
	public string BundleID;
	public string BundleVer;
//	public string ShortBundleVer;
	//optimization
//	public ApiCompatibilityLevel ApiCompatibilityLevel;// 4.6Ver
	public StrippingLevel StrippingLevel;
	// Icon
	public bool IconSetStatus; // 是否設定icon圖
	public string[] DefIcons;// 預設圖名
	// AndroidSet
	public SDAndroidSet AndroidSet;
	// IOSSet
	public SDIOSSet IOSSet;
}
// 參照PlayerSetting加入
public class SDAndroidSet
{
	public AndroidShowActivityIndicatorOnLoading ShowActivityIndicatorOnLoading;
	public AndroidSplashScreenScale SplashScreenScale; 
	public int BundleCode;
	public AndroidSdkVersions SdkVersions;
	public AndroidTargetDevice TargetDevice;
//	public TargetGlesGraphics TargetGraphice; // 4.6Ver
	public bool ForceInternet;
	public bool ForceSDCard;
	
	public string KeyStorePath;// 
	public string KeyStorePassword;
	public string KeyAlialsName;
	public string KeyAlialsPassword;
	
	public string ManifestVersionCode;
	public string ManifestVersionName;

	public bool IconSetStatus; // 是否設定icon圖
	public bool IconOverride; // 是否覆寫Icon圖
	public string[] DefIcons;// 預設圖名
	public bool SplashSetStatus; // 是否設定Splash圖
	public string SplashImage;// 啟動之前的插入圖(Android)
	// Unity 5 New
	public ApiCompatibilityLevel ApiCompatibilityLevel;
	public GraphicsDeviceType[] GraphicsType;
}
// 參照PlayerSetting加入
public class SDIOSSet
{
	public iOSStatusBarStyle StatusBarStyle;
	public iOSShowActivityIndicatorOnLoading ShowActivityIndicatorOnLoading;
	public iOSTargetDevice TargetDevice;
	public iOSSdkVersion SDKVersion;
	public ScriptCallOptimizationLevel ScriptCallOptimizationLevel;
	public ScriptingImplementation ScriptingBackend;

	public string BuildNumber;
//	public bool OverrideIPodMusic;
	public bool PrepareIOSForRecording;
	public bool RequiresPersistentWiFi;

	public bool PrerenderedIcon;
	public bool IconSetStatus; // 是否設定icon圖
	public bool IconOverride; // 是否覆寫Icon圖
	public string[] DefIcons;// 預設圖名
	public bool SplashSetStatus; // 是否設定Splash圖
	public string[] SplashImages;// 啟動之前的插入圖
	// Unity 4.6(under) Old
//	public iOSTargetResolution TargetResolution;// 4.6Ver
//	public TargetIOSGraphics TargetGraphics;// 4.6Ver
//	public bool ExitOnSuspend;// 4.6Ver
//	public iOSTargetOSVersion TargetOSVersion;// 4.6Ver
	// Unity5 New
	public ApiCompatibilityLevel ApiCompatibilityLevel;
	public GraphicsDeviceType[] GraphicsType;
	public string TargetOSVersionString;
	public iOSAppInBackgroundBehavior AppInBackgroundBehavior;
	public bool StripEngineCode;
	public StrippingLevel StripLevel;
	public int Architecture;
}
// 顯示用的圖像
public class ShowImageGroup
{
	public Texture2D[] DefaultIcon;
	public Texture2D[] AndroidIcons;
	public Texture2D[] IosIcons;
	public Texture2D[] IOSSplashImages;
	public Texture2D AndroidSplashImage;
}
//設定檔的設定目標(一次只設定一個平台)
public enum eSDTarget
{
	Android,
	IOS,
}
// EditorUI設定頁面
public enum eSettingPage
{
	common,
	android,
	ios,
}
// EditorUI 的按鈕介面設定
public enum eButtonPos
{
	none,
	left,
	mid,
	right,
}
// SplashImage的種類
public enum eMobileSplashScreen
{
	iPhoneSplashScreen, // = Mobile Splash Screen(= Android Splash Screen)
	iPhoneHighResSplashScreen,
	iPhoneTallHighResSplashScreen,
	iPhone47inSplashScreen,
	iPhone55inPortraitSplashScreen,
	iPhone55inLandscapeSplashScreen,
	iPadPortraitSplashScreen,
	iPadHighResPortraitSplashScreen,
	iPadLandscapeSplashScreen,
	iPadHighResLandscapeSplashScreen,
}
// 
public enum iPhoneArchitecture
{
	ARMv7,
	ARM64,
	Universal
}
#endif