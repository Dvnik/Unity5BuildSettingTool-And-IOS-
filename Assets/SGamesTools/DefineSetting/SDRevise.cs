﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
/**
 * 2017/04 Merge By SuperGame
 * programmer : dvnik147
 * 
 * 修改自設設定檔的內容
 */
public class SDRevise : SDBaseUI
{
	private int mFileSelectIndex;
	private int mFileShowIndex;

	private bool mSaveOther;
	/// <summary>
	/// 初始化
	/// </summary>
	protected override void SettingInit()
	{
		if(FileNameArray != null)
			SetShowSettingInfo();
	}
	/// <summary>
	/// 修改視窗
	/// </summary>
	protected override void ShowUI()
	{
		if(CheckHaveFiles())
			return;
		// Start Scroll View
		mEditorScrollView = EditorGUILayout.BeginScrollView(mEditorScrollView);
		if(mSaveOther)
			SaveOtherFile();
		else
			NormalPage();
		// End Scroll View
		EditorGUILayout.EndScrollView ();
	}
	/// <summary>
	/// 主畫面
	/// </summary>
	private void NormalPage()
	{
		mFileSelectIndex = UITopSaveFileSelect(mFileSelectIndex);
		CheckShowFile();// Check FileSelect Change
		// Top
		UITopSettingShow(false);
		// Common
		UICommonArea();
		// Android
		UIAndroidArea();
		// IOS
		UIIOSArea();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		// Button
	   if(GUILayout.Button("另存存檔", ButtonMyStyle(eButtonPos.mid)))
			OpenSaveOtherPage();
		// Button Horizontal
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("存檔", ButtonMyStyle(eButtonPos.left)))
			DoRevise();
		if (GUILayout.Button ("取消", ButtonMyStyle(eButtonPos.right)))
			Close ();
		EditorGUILayout.EndHorizontal();

	}
	/// <summary>
	/// 另存新檔畫面
	/// </summary>
	private void SaveOtherFile()
	{
		UITopSettingShow();
		// Button Horizontal
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("另存存檔", ButtonMyStyle(eButtonPos.left)))
			DoSaveOther();
		if (GUILayout.Button ("取消", ButtonMyStyle(eButtonPos.right)))
			OpenSaveOtherPage (false);
		EditorGUILayout.EndHorizontal();
	}
	/// <summary>
	/// 修改存檔
	/// </summary>
	private void DoRevise()
	{
		SDDataMove.SaveData(ShowSetInfo);
		
		Close();
	}
	/// <summary>
	/// 另存新檔頁面開關
	/// </summary>
	private void OpenSaveOtherPage(bool iOpen = true)
	{
		mSaveOther = iOpen;
	}
	/// <summary>
	/// 執行另存新檔
	/// </summary>
	private void DoSaveOther()
	{
		if(!CheckFileNameRepeat())
			DoRevise();
	}
	/// <summary>
	/// 檢查顯示的檔案
	/// </summary>
	private void CheckShowFile()
	{
		if(mFileShowIndex.Equals(mFileSelectIndex))
			return;

		mFileShowIndex = mFileSelectIndex;
		SetShowSettingInfo();
	}
	/// <summary>
	/// 顯示現有存檔的設定訊息
	/// </summary>
	private void SetShowSettingInfo()
	{
		SDefineSet aShowInfo = SDDataMove.LoadData(FileNameArray[mFileShowIndex]);

		ShowImageGroup aImageG = new ShowImageGroup();
		// Icon
		aImageG.DefaultIcon = SDDataMove.LoadIconTexture(eSettingPage.common, ref aShowInfo);
		aImageG.AndroidIcons = SDDataMove.LoadIconTexture(eSettingPage.android, ref aShowInfo);
		aImageG.IosIcons = SDDataMove.LoadIconTexture(eSettingPage.ios, ref aShowInfo);
		// Splash Image
		aImageG.IOSSplashImages = SDDataMove.LoadIOSSplashTexture(ref aShowInfo);
		aImageG.AndroidSplashImage = SDDataMove.LoadAndroidSplashTexture(ref aShowInfo);
		// SetData
		SetDefineData(aShowInfo, aImageG);
	}
}
#endif