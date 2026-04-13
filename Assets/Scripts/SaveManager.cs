using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    private const string CURRENT_VERSION = "v0.53";
        
    public static int Saving;
    public static string Version;
    public static SaveManager Instance;

    public Sprite kosong;
    private UnityEvent<string, int[]> loadFileToListData;

    #region Unity
    private void Awake()
    {
        Instance = this;
        Version = PlayerPrefs.GetString("Version");
        Saving = PlayerPrefs.GetInt("Saving");

        if (Version.Equals(CURRENT_VERSION))
        {
            return;
        }
        else
        {
            PlayerPrefs.DeleteAll();
            Saving = 0;
        }
    }
    #endregion

    #region Save Stuff
    public void SaveImageData(ref List<TypeMode> _listData)
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ModeCount", _listData.Count);

        for (int i = 0; i < _listData.Count; i++)
        {
            PlayerPrefs.SetString("Hotkey_Mode" + i, _listData[i].Hotkey);

            foreach (ImageData item in _listData[i].ImageDatas)
            {
                PlayerPrefs.SetString(item.Tag + "_importedPath_" + item.Mode, item.importedPath);
            }
        }

        PlayerPrefs.SetInt("Saving", 1);
        PlayerPrefs.SetString("Version", CURRENT_VERSION);
    }
    #endregion

    #region Load Stuff
    public void AddListenerToLoadFileToListData(UnityAction<string, int[]> _action)
    {
        if (loadFileToListData == null) loadFileToListData = new();
        loadFileToListData.AddListener(_action);
    }
    public void LoadImageData(Func<int, TypeMode> _addHotkey)
    {
        for (int j = 0; j < ModeController.ModeCount; j++)
        {
            _addHotkey.Invoke(j).Hotkey = PlayerPrefs.GetString("Hotkey_Mode" + j);

            for (int i = 0; i < 5; i++)
            {
                string filepath = PlayerPrefs.GetString("Texture_" + i + "_importedPath_" + j);
                if (filepath.Equals(""))
                {

                }
                else
                {
                    loadFileToListData.Invoke(filepath, new int[] { i, j });
                }
            }
        }

    }
    public void LoadModeCount()
    {
        if (Saving > 0)
        {
            ModeController.ModeCount = PlayerPrefs.GetInt("ModeCount");
        }
        else
        {
            ModeController.ModeCount = 4;
        }
    }
    #endregion

    public static void ClearImageData(ImageData data)
    {
        data.Sprite = Instance.kosong;
        data.Thumbnail.sprite = Instance.kosong;
        data.animSprites.Clear();
        data.importedPath = "";
    }
}
