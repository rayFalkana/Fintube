using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class FintubeManager : MonoBehaviour
{
    [SerializeField] private ImportFile importFile;
    [SerializeField] private KeyboardController keyboard;

    public ModeController modeController;
    public AudioInputManager micInput;
    public UIManager UserInterface;
    public DisplayAccController accController;

    [SerializeField] public List<TypeMode> ListData;

    #region Variable.ImportFile
    private const int idListData = 1;
    private const int idImageData = 0;
    #endregion

    #region Keyboard
    private void InitKeyboard()
    {
        keyboard.KeyboardEvent = new UnityEvent<string>();
        keyboard.KeyboardEvent.AddListener(GetInputFromKeyboard);
    }
    private void GetInputFromKeyboard(string _key)
    {
        if (UserInterface.PanelAssignHotkey)
        {
            UserInterface.InputNewHotkey(_key);
        }
        else
        {
            ChangeModeHotkey(_key);
        }
    }
    private void UpdateTextHotkey()
    {
        for (int i = 0; i < ModeController.ModeCount; i++)
        {
            ListData[i].UpdateText();
        }
    }
    #endregion

    #region ListData Get Set Stuff
    public ImageData ImportedImageDatas(int[] _id) { return ListData[_id[idListData]].ImageDatas[_id[idImageData]]; }
    private void SetAnimSpriteToDataList(List<AnimSprite> _animSprites, int[] _id) { ImportedImageDatas(_id).Input(_animSprites); }
    private void SetSpriteToListData(int[] _id, Sprite _sprite) { ImportedImageDatas(_id).Input(_sprite); }
    private void SetThumbnailToListData(int[] _id, Sprite _sprite) { ImportedImageDatas(_id).Thumbnail.sprite = _sprite; }
    private void InitListData()
    {
        for (int i = 0; i < ModeController.ModeCount; i++)
        {
            int nameForF = i + 1;
            ListData[i].Hotkey = "_f" + nameForF;
            //Debug.Log(ListData[i].Hotkey);

            for (int j = 0; j < ListData[i].Count(); j++)
            {
                ImageData imageData = ListData[i].ImageDatas[j];
                imageData.CreateID(j, i);
                imageData.button.onClick.AddListener(() => {
                    importFile.ImportFiles(imageData.ReturnID());
                });
            }

            ListData[i].UpdateText();
        }
    }
    #endregion

    #region ModeController
    private void InitModeController()
    {
        LoadModeCount();

        modeController.AnimInputStartPlaying();

        for (int i = 0; i < ModeController.ModeCount; i++)
        {
            TypeMode typeMode = modeController.GenerateMode(i,
                UserInterface.ParentModes(),
                UserInterface.AddToggle,
                (_typeMode) => { UserInterface.ActivateHotkeyUI(_typeMode); }
            );
            ListData.Add(typeMode);
        }

        UserInterface.EnableToggle();
        UserInterface.AddListenerToToggle((x)=> {
            modeController.ChangeMode(x);
        });
        UserInterface.ChangeToggleViaName("Mode0");

    }
    private void GetState()
    {
        AnimationInputController animation = modeController.AnimInputStopPlaying(micInput.stateVoice);
        int[] id = new int[] { (int)micInput.stateVoice, ModeController.IdMode };
        ImageData newImageData = ImportedImageDatas(id);

        if (newImageData != null)
        {
            if (newImageData.animSprites.Count > 0)
            {
                if (animation.CheckIdAndMode(id))
                {
                    UserInterface.OffDisplay();
                    modeController.AnimInputContinuePlaying(animation);
                }
                else
                {
                    UserInterface.SetImageInDisplay(newImageData.animSprites, animation);
                }
            }
            else
            {
                Sprite sprite = newImageData.Sprite;
                UserInterface.SetImageInDisplay(sprite);
            }
        }
        else
        {
            Debug.LogError($"No ImageData with ID 'Texture_{id[0]}' found and Mode {id[1]}");
            return;
        }
    }
    private void CheckState()
    {
        if (ListData.Count == 0)
        {
            return;
        }

        modeController.SetCurrentMode();

        GetState();
    }
    public void ChangeModeHotkey(string _hotkey)
    {
        foreach (TypeMode item in ListData)
        {
            if (_hotkey.Equals(item.Hotkey))
            {
                item.TurnOnThisMode();
                return;
            }
        }
    }
    #endregion

    #region ImportFile
    public int [] GetCurrentButtonID() { return importFile.GetCurrentButtonID(); }
    private void SetThumbnailToListDataFromImportFile(Sprite _sprite) { SetThumbnailToListData(importFile.GetCurrentButtonID(), _sprite); }
    private void SetSpriteToListDataFromImportFile(Sprite _sprite) { SetSpriteToListData(importFile.GetCurrentButtonID(), _sprite); }
    private void InitImportFile()
    {
        importFile.AddListenerToResizeDisplayedImage(UserInterface.ResizeImage);

        importFile.AddListenerToGetNewlyImportedImage(UserInterface.SetImageInDisplay);
        importFile.AddListenerToGetNewlyImportedImage(SetSpriteToListDataFromImportFile);

        importFile.AddListenerToGetNewlyImportedThumnail(SetThumbnailToListDataFromImportFile);

        importFile.AddListenerToDisplayAnimatedImageInUI(UserInterface.SetImageInDisplay);
        importFile.AddListenerToDisplayAnimatedImageInUI(SetAnimSpriteToDataList);

        importFile.AddListenerToTriggerGIFLimit(UserInterface.TriggerGIFLimit);

        importFile.AddListenerToFilePathIntoListData((filepath, desiredID) => {
            ListData[desiredID[1]].ImageDatas[desiredID[0]].importedPath = filepath;
        });
    }

   // public void AddImageData(ImageData _data, int _mode) { ListData[_mode].Add(_data); }
    public void LoadModeCount() { SaveManager.Instance.LoadModeCount(); }
    #endregion

    #region Acc Controller
    private void InitAcc()
    {
        accController.AddListenerToTriggerGIFLimit(UserInterface.TriggerGIFLimit);
    }
    #endregion

    #region Load and Save Stuff
    private void LoadImageData()
    {
        if (SaveManager.Saving > 0)
        {
            SaveManager.Instance.AddListenerToLoadFileToListData(importFile.AccessFiles);
            SaveManager.Instance.LoadImageData((ID) => { return ListData[ID]; });
        }
    }
    private void SaveImageData()
    {
        if (ListData.Count > 0)
        {
            try { SaveManager.Instance.SaveImageData(ref ListData); }
            catch (Exception e) { Debug.Log(e.Message); }
        }
        else
        {
            Debug.Log("Nothing Saved !!");
        }
    }
    #endregion

    #region Unity
    // Start is called before the first frame update
    private void Start()
    {
        InitKeyboard();
        UserInterface.StartUI(this);
        micInput._start();
        InitModeController();
        InitListData();
        InitImportFile();
        InitAcc();

        LoadImageData();
        UpdateTextHotkey();
    }

    // Update is called once per frame
    private void Update()
    {
        micInput._update();
        CheckState();
    }

    private void OnApplicationQuit()
    {
        SaveImageData();
    }
    #endregion
}
