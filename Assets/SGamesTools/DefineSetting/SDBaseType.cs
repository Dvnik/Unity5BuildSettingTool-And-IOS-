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
public class SDBaseType {
	public const string cUIName001 = "設置檔名",
	cUIName002 = "設置平台",
	cUIName003 = "存檔列表",
	cUIName004 = "Path :",
	cUIName005 = "基本設置",
	cUIName006 = "Company Name",
	cUIName007 = "Product Name",
	cUIName008 = "旋轉方向設定",
	cUIName009 = "預設方向",
	cUIName010 = "使用動畫旋轉",
	cUIName011 = "設定自動旋轉允許方向",
	cUIName012 = "Identification",
	cUIName013 = "Bundle Identifier",
	cUIName014 = "Version*",
	cUIName015 = "設定ICon圖",
	cUIName016 = "Default Icon",
	cUIName017 = "Android 設置",
	cUIName018 = "Resolution and Presentation",
	cUIName019 = "Use 32Bit Display Buffer*",
	cUIName020 = "Disable Depth and Stencil*",
	cUIName021 = "Show Loading Indicator",
	cUIName022 = "Other Settings",
	cUIName023 = "Bundle Version Code",
	cUIName024 = "Configuration",
	cUIName025 = "Internet Access",
	cUIName026 = "Write Permission",
	cUIName027 = "Api Compatibility Level*",
	cUIName028 = "Publishing Settings",
	cUIName029 = "Keystore Path",
	cUIName030 = "Keystore Password",
	cUIName031 = "KeyAlias Name",
	cUIName032 = "KeyAlias Password",
	cUIName033 = "KeyStorePath 預設都取Assets底下的路徑",
	cUIName034 = "設定Icon圖",
	cUIName035 = "是否覆寫Icon圖",
	cUIName036 = "Other",
	cUIName037 = "設定Splash圖",
	cUIName038 = "目前只設定一般的Splash Image\n不設定VR Splash 和 Splash Sreen",
	cUIName039 = "Image",
	cUIName040 = "Scaling",
	cUIName041 = "IOS 設置",
	cUIName042 = "Multitasking Support",
	cUIName043 = "Requires Fullscreen",
	cUIName044 = "Status Bar",
	cUIName045 = "Status Bar Hidden",
	cUIName046 = "Status Bar Style",
	cUIName047 = "Show Loading Indicator",
	cUIName048 = "Debugging and crash reporting",
	cUIName049 = "Unity5增加的類別，盡量做到設定",
	cUIName050 = "Crash Reporting",
	cUIName051 = "Log Obj-C Uncaught Exception",
	cUIName052 = "Enable CrashReport API*",
	cUIName053 = "On .Net UnhandleException",
	cUIName054 = "Build",
	cUIName055 = "Automatically Sign",
	cUIName056 = "Automatic Signing Team ID",
	cUIName057 = "iOS Provisioning Profile",
	cUIName058 = "Scripting Backend",
	cUIName059 = "Architecture",
	cUIName060 = "Target Device",
	cUIName061 = "Target SDK",
	cUIName062 = "Target minimum iOS Verstion",
	cUIName063 = "Prepare iOS for Recording",
	cUIName064 = "Requires Persistent WiFi*",
	cUIName065 = "Behavior in Background",
	cUIName066 = "Optimization",
	cUIName067 = "Strip Engine Code*",
	cUIName068 = "Strpping Level*",
	cUIName069 = "Script Call Optimization"
	;

	public const string cUIInfo001 = "一個設定檔只會部屬一個平台的設定\n這是為了自動產檔時能夠快速設定用的";

	public static string[] cAllowAutoRotNames = new string[] {
		"Portrait",
		"Portrait Upside Down",
		"Landscape Right",
		"Landscape Left"
	};

	public static string[] InternetAccess = new string[]{"Auto", "Require"};
	public static string[] WritePermission = new string[]{"Internal", "External(SDCard)"};
	public static string[] ApiCompatibilityLevel = new string[]{".NET 2.0", ".NET 2.0 Subset"};
	public static string[] ScriptingBackend = new string[]{"Mono2x", "IL2CPP"};

}

// 參照PlayerSetting加入
public class SDefineSet {
	// Data Save
	public string SettingName;// 設定名稱(檔案名稱)
	public eSDTarget DefineTarget;// 這個Define是為哪一個版本設置
	// Names
	public string CompanyName;// 公司名稱
	public string ProductName;// 產檔檔名
	// Orientation
	public UIOrientation UIOrientation;// 畫面方向
	public bool UseAnimAutor;// 使用動畫旋轉
	public bool[] OrienRoatable;// 畫面可轉動的方向(Portrait, PortraitUpSideDown, LandRight, LandLeft)
	// Bundle Identification
	public string BundleIDUnknow;
	public string BundleVer;
	// Icon
	public bool IconSetStatus; // 是否設定icon圖
	public string[] DefIcons;// 預設圖名

	// AndroidSet
	public SDAndroidSet AndroidSet;
	// IOSSet
	public SDIOSSet IOSSet;
	// -------------------------------------------------------







	// Define Info
	public string ScriptDefine;

	//系統版本


	// Facebook
	public string FacebookID;// 臉書appID
//	public string ShortBundleVer;
	//optimization
//	public ApiCompatibilityLevel ApiCompatibilityLevel;// 4.6Ver
	public StrippingLevel StrippingLevel;
}
// 參照PlayerSetting加入
public class SDAndroidSet {
	// Resolution and Presentation
	public bool Use32BitDisplayBuffer;	// 
	public bool disableDepthAndStencilBuffers;
	public AndroidShowActivityIndicatorOnLoading ShowActivityIndicatorOnLoading;
	// Identification
	public string BundleIDAndroid;
	public int BundleCode;
	// Configuration
	public ApiCompatibilityLevel ApiCompatibilityLevel;
	public bool ForceInternet;
	public bool ForceSDCard;
	// Publishing Settings
	public string KeyStorePath;// 
	public string KeyStorePassword;
	public string KeyAlialsName;
	public string KeyAlialsPassword;
	// Icon
	public bool IconSetStatus; // 是否設定icon圖
	public bool IconOverride; // 是否覆寫Icon圖
	public string[] DefIcons;// 預設圖名
	// Splash Image
	public bool SplashSetStatus; // 是否設定Splash圖
	public string SplashImage;// 啟動之前的插入圖(Android)
	public AndroidSplashScreenScale SplashScreenScale; 
	// -------------------------------------------------------



	public AndroidSdkVersions SdkVersions;
	public AndroidTargetDevice TargetDevice;
	
	public string ManifestVersionCode;
	public string ManifestVersionName;

	// Unity 4.6(under) Old
//	public TargetGlesGraphics TargetGraphice; // 4.6Ver

	// Unity 5 New

	public GraphicsDeviceType[] GraphicsType;
}
// 參照PlayerSetting加入
public class SDIOSSet {
	// Resolution and Presentation
	public bool RequiresFullScreen;
	public bool StatusBarHidden;
	public iOSStatusBarStyle StatusBarStyle;
	// Debugging and crash reporting
	public bool LogObjCUncaughtExceptions;
	public bool EnableCrashReportAPI;
	public ActionOnDotNetUnhandledException ActionOnDotNetUnhandledException;
	// Identification
	public string BundleIDIOS;
	public string BuildNumber;
	public bool AppleEnableAutomaticSigning;
	public string AppleDeveloperTeamID;
	public string ProvisioningProfileID;
	// Configuration
	public ScriptingImplementation ScriptingBackend;
	public int Architecture;
	public ApiCompatibilityLevel ApiCompatibilityLevel;
	public iOSTargetDevice TargetDevice;
	public iOSSdkVersion SDKVersion;
	public string TargetOSVersionString;
	public bool PrepareIOSForRecording;
	public bool RequiresPersistentWiFi;
	public iOSAppInBackgroundBehavior AppInBackgroundBehavior;
	// Optimization
	public ScriptCallOptimizationLevel ScriptCallOptimizationLevel;
	// -------------------------------------------------------



	public iOSShowActivityIndicatorOnLoading ShowActivityIndicatorOnLoading;






//	public bool OverrideIPodMusic;



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

	public GraphicsDeviceType[] GraphicsType;


	public bool StripEngineCode;
	public StrippingLevel StripLevel;

}
// 顯示用的圖像
public class ShowImageGroup {
	public Texture2D[] DefaultIcon;
	public Texture2D[] AndroidIcons;
	public Texture2D[] IosIcons;
	public Texture2D[] IOSSplashImages;
	public Texture2D AndroidSplashImage;
}
//設定檔的設定目標(一次只設定一個平台)
public enum eSDTarget {
	Android,
	IOS,
}
// EditorUI設定頁面
public enum eSettingPage {
	common,
	android,
	ios,
}
// EditorUI 的按鈕介面設定
public enum eButtonPos {
	none,
	left,
	mid,
	right,
}
// SplashImage的種類
public enum eMobileSplashScreen {
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
public enum iPhoneArchitecture {
	ARMv7,
	ARM64,
	Universal
}
#endif