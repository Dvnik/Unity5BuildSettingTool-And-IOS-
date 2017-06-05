#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Text;
using System.Linq;

public class SManifestSet
{
	public const string DeepLinkingActivityName = "com.facebook.unity.FBUnityDeepLinkingActivity";
	public const string LoginActivityName = "com.facebook.LoginActivity";
	public const string UnityLoginActivityName = "com.facebook.unity.FBUnityLoginActivity";
	public const string UnityDialogsActivityName = "com.facebook.unity.FBUnityDialogsActivity";
	public const string ApplicationIdMetaDataName = "com.facebook.sdk.ApplicationId";
	
	public const string ProjectManifestPath = "Plugins/Android/AndroidManifest.xml";
	public const string UnityManifestPath = "PlaybackEngines/androidplayer/AndroidManifest.xml";
	
	private static string mFacebookAppID { get; set;}
	
	public static bool IsValidAppId
	{
		get
		{
			return mFacebookAppID != null 
				&& mFacebookAppID.Length > 0 
					&& !mFacebookAppID.Equals("0");
		}
	}
	/// <summary>
	/// 更換Facebook AppID方法
	/// </summary>
	/// <param name="iAppID">I app I.</param>
	public static void ChangeFacebookID(string iAppID)
	{
		mFacebookAppID = iAppID;
		var outputFile = Path.Combine(Application.dataPath, ProjectManifestPath);
		
		// only copy over a fresh copy of the AndroidManifest if one does not exist
		if (!File.Exists(outputFile))
		{
			var inputFile = Path.Combine(EditorApplication.applicationContentsPath, UnityManifestPath);
			File.Copy(inputFile, outputFile);
		}
		UpdateFBIDManifest(outputFile);
	}
	
	public static bool CheckManifest()
	{
		bool result = true;
		var outputFile = Path.Combine(Application.dataPath, ProjectManifestPath);
		if (!File.Exists(outputFile))
		{
			Debug.LogError("An android manifest must be generated for the Facebook SDK to work.  Go to Facebook->Edit Settings and press \"Regenerate Android Manifest\"");
			return false;
		}
		
		XmlDocument doc = new XmlDocument();
		doc.Load(outputFile);
		
		if (doc == null)
		{
			Debug.LogError("Couldn't load " + outputFile);
			return false;
		}
		
		XmlNode manNode = FindChildNode(doc, "manifest");
		XmlNode dict = FindChildNode(manNode, "application");
		
		if (dict == null)
		{
			Debug.LogError("Error parsing " + outputFile);
			return false;
		}
		
		string ns = dict.GetNamespaceOfPrefix("android");
		
		XmlElement loginElement = FindElementWithAndroidName("activity", "name", ns, UnityLoginActivityName, dict);
		if (loginElement == null)
		{
			Debug.LogError(string.Format("{0} is missing from your android manifest.  Go to Facebook->Edit Settings and press \"Regenerate Android Manifest\"", LoginActivityName));
			result = false;
		}
		
		var deprecatedMainActivityName = "com.facebook.unity.FBUnityPlayerActivity";
		XmlElement deprecatedElement = FindElementWithAndroidName("activity", "name", ns, deprecatedMainActivityName, dict);
		if (deprecatedElement != null)
		{
			Debug.LogWarning(string.Format("{0} is deprecated and no longer needed for the Facebook SDK.  Feel free to use your own main activity or use the default \"com.unity3d.player.UnityPlayerNativeActivity\"", deprecatedMainActivityName));
		}
		
		return result;
	}
	/// <summary>
	/// 更換下列xml參數的versionCode與versionName兩個值
	/// <manifest xmlns:android="http://schemas.android.com/apk/res/android" package="com.unity3d.player" android:installLocation="preferExternal" android:versionCode="1" android:versionName="1.0">
	/// </summary>
	/// <param name="iVCode">Version code.</param>
	/// <param name="iVName">Version name.</param>
	public static void UpdataVersionManifest (string iVCode, string iVName)
	{
		string outputFile = Path.Combine(Application.dataPath, ProjectManifestPath);
		// only copy over a fresh copy of the AndroidManifest if one does not exist
		if (!File.Exists(outputFile))
		{
			var inputFile = Path.Combine(EditorApplication.applicationContentsPath, UnityManifestPath);
			File.Copy(inputFile, outputFile);
		}
		XmlDocument doc = new XmlDocument();
		doc.Load(outputFile);
		
		if (doc == null)
		{
			Debug.LogError("Couldn't load " + outputFile);
			return;
		}
		// -----------------------------------------------------------------
//		XmlNode manNode = FindChildNode(doc, "manifest");// but its value is never used
		
		XmlElement aFirstElement = doc.DocumentElement;
		string ns = aFirstElement.GetNamespaceOfPrefix ("android");
		
		aFirstElement.SetAttribute("versionCode", ns, iVCode);
		aFirstElement.SetAttribute("versionName", ns, iVName);
		
		doc.Save(outputFile);
	}
	/// <summary>
	/// 拷貝FB官方撰寫的更換AppID的程式
	/// </summary>
	/// <param name="fullPath">Full path.</param>
	private static void UpdateFBIDManifest(string fullPath)
	{
		string appId = mFacebookAppID;
		
		if (!IsValidAppId)
		{
			Debug.LogError("You didn't specify a Facebook app ID.  Please add one using the Facebook menu in the main Unity editor.");
			return;
		}
		
		XmlDocument doc = new XmlDocument();
		doc.Load(fullPath);
		
		if (doc == null)
		{
			Debug.LogError("Couldn't load " + fullPath);
			return;
		}
		
		XmlNode manNode = FindChildNode(doc, "manifest");
		XmlNode dict = FindChildNode(manNode, "application");
		
		if (dict == null)
		{
			Debug.LogError("Error parsing " + fullPath);
			return;
		}
		
		string ns = dict.GetNamespaceOfPrefix("android");
		
		//add the unity login activity
		XmlElement unityLoginElement = FindElementWithAndroidName("activity", "name", ns, UnityLoginActivityName, dict);
		if (unityLoginElement == null)
		{
			unityLoginElement = CreateUnityOverlayElement(doc, ns, UnityLoginActivityName);
			dict.AppendChild(unityLoginElement);
		}
		
		//add the unity dialogs activity
		XmlElement unityDialogsElement = FindElementWithAndroidName("activity", "name", ns, UnityDialogsActivityName, dict);
		if (unityDialogsElement == null)
		{
			unityDialogsElement = CreateUnityOverlayElement(doc, ns, UnityDialogsActivityName);
			dict.AppendChild(unityDialogsElement);
		}
		
		//add the login activity
		XmlElement loginElement = FindElementWithAndroidName("activity", "name", ns, LoginActivityName, dict);
		if (loginElement == null)
		{
			loginElement = CreateLoginElement(doc, ns);
			dict.AppendChild(loginElement);
		}
		
		//add deep linking activity
		XmlElement deepLinkingElement = FindElementWithAndroidName("activity", "name", ns, DeepLinkingActivityName, dict);
		if (deepLinkingElement == null)
		{
			deepLinkingElement = CreateDeepLinkingElement(doc, ns);
			dict.AppendChild(deepLinkingElement);
		}
		
		//add the app id
		//<meta-data android:name="com.facebook.sdk.ApplicationId" android:value="\ 409682555812308" />
		XmlElement appIdElement = FindElementWithAndroidName("meta-data", "name", ns, ApplicationIdMetaDataName, dict);
		if (appIdElement == null)
		{
			appIdElement = doc.CreateElement("meta-data");
			appIdElement.SetAttribute("name", ns, ApplicationIdMetaDataName);
			dict.AppendChild(appIdElement);
		}
		appIdElement.SetAttribute("value", ns, "\\ " + appId); //stupid hack so that the id comes out as a string
		
		doc.Save(fullPath);
	}
	#region Other Check XmlNode
	/// <summary>
	/// Finds the child node.
	/// </summary>
	/// <returns>The child node.</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="name">Name.</param>
	private static XmlNode FindChildNode(XmlNode parent, string name)
	{
		XmlNode curr = parent.FirstChild;
		while (curr != null)
		{
			if (curr.Name.Equals(name))
			{
				return curr;
			}
			curr = curr.NextSibling;
		}
		return null;
	}
	/// <summary>
	/// Finds the name of the element with android.
	/// </summary>
	/// <returns>The element with android name.</returns>
	/// <param name="name">Name.</param>
	/// <param name="androidName">Android name.</param>
	/// <param name="ns">Ns.</param>
	/// <param name="value">Value.</param>
	/// <param name="parent">Parent.</param>
	private static XmlElement FindElementWithAndroidName(string name, string androidName, string ns, string value, XmlNode parent)
	{
		var curr = parent.FirstChild;
		while (curr != null)
		{
			if (curr.Name.Equals(name) && curr is XmlElement && ((XmlElement)curr).GetAttribute(androidName, ns) == value)
			{
				return curr as XmlElement;
			}
			curr = curr.NextSibling;
		}
		return null;
	}
	/// <summary>
	/// Creates the login element.
	/// </summary>
	/// <returns>The login element.</returns>
	/// <param name="doc">Document.</param>
	/// <param name="ns">Ns.</param>
	private static XmlElement CreateLoginElement(XmlDocument doc, string ns)
	{
		//<activity android:name="com.facebook.LoginActivity" android:configChanges="keyboardHidden|orientation" android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen">
		//</activity>
		XmlElement activityElement = doc.CreateElement("activity");
		activityElement.SetAttribute("name", ns, LoginActivityName);
		activityElement.SetAttribute("configChanges", ns, "keyboardHidden|orientation");
		activityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
		activityElement.InnerText = "\n    ";  //be extremely anal to make diff tools happy
		return activityElement;
	}
	/// <summary>
	/// Creates the deep linking element.
	/// </summary>
	/// <returns>The deep linking element.</returns>
	/// <param name="doc">Document.</param>
	/// <param name="ns">Ns.</param>
	private static XmlElement CreateDeepLinkingElement(XmlDocument doc, string ns)
	{
		//<activity android:name="com.facebook.unity.FBDeepLinkingActivity" android:exported="true">
		//</activity>
		XmlElement activityElement = doc.CreateElement("activity");
		activityElement.SetAttribute("name", ns, DeepLinkingActivityName);
		activityElement.SetAttribute("exported", ns, "true");
		activityElement.InnerText = "\n    ";  //be extremely anal to make diff tools happy
		return activityElement;
	}
	/// <summary>
	/// Creates the unity overlay element.
	/// </summary>
	/// <returns>The unity overlay element.</returns>
	/// <param name="doc">Document.</param>
	/// <param name="ns">Ns.</param>
	/// <param name="activityName">Activity name.</param>
	private static XmlElement CreateUnityOverlayElement(XmlDocument doc, string ns, string activityName)
	{
		//<activity android:name="activityName" android:configChanges="all|of|them" android:theme="@android:style/Theme.Translucent.NoTitleBar.Fullscreen">
		//</activity>
		XmlElement activityElement = doc.CreateElement("activity");
		activityElement.SetAttribute("name", ns, activityName);
		activityElement.SetAttribute("configChanges", ns, "fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen");
		activityElement.SetAttribute("theme", ns, "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
		activityElement.InnerText = "\n    ";  //be extremely anal to make diff tools happy
		return activityElement;
	}
	#endregion
}
#endif