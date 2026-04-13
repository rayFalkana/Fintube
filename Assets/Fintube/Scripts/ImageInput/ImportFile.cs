using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImportFile : MonoBehaviour
{
    public int CurrentTextureIndex = 0;
    public bool IsMultiGifPlayer = false;

    private ExtensionFilter[] filters;
    private int[] currentButtonID;

    private UnityEvent triggerGIFLimit;
    private UnityEvent resizeDisplayedImage;
    private UnityEvent<Sprite> newImportedImage;
    private UnityEvent<Sprite> newImportedThumbnail;
    private UnityEvent<List<AnimSprite>, int []> displayAnimatedImageInUI;
    private UnityEvent<string, int[]> filePathIntoListData;

    public void AddListenerToTriggerGIFLimit(UnityAction _action) 
    {
        if (triggerGIFLimit == null) triggerGIFLimit = new();
        triggerGIFLimit.AddListener(_action);
    }
    public void AddListenerToGetNewlyImportedThumnail(UnityAction<Sprite> _action)
    {
        if (newImportedThumbnail == null) newImportedThumbnail = new();
        newImportedThumbnail.AddListener(_action);
    }
    public void AddListenerToGetNewlyImportedImage(UnityAction<Sprite> _action)
    {
        if (newImportedImage == null) newImportedImage = new();
        newImportedImage.AddListener(_action);
    }
    public void AddListenerToDisplayAnimatedImageInUI(UnityAction<List<AnimSprite>, int[]> _action)
    {
        if (displayAnimatedImageInUI == null) displayAnimatedImageInUI = new();
        displayAnimatedImageInUI.AddListener(_action);
    }
    public void AddListenerToResizeDisplayedImage(UnityAction _action)
    {
        if (resizeDisplayedImage == null) resizeDisplayedImage = new();
        resizeDisplayedImage.AddListener(_action);
    }
    public void AddListenerToFilePathIntoListData(UnityAction<string, int []> _action)
    {
        if (filePathIntoListData == null) filePathIntoListData = new();
        filePathIntoListData.AddListener(_action);
    }

    public int [] GetCurrentButtonID() { return currentButtonID; }
    public void ImportFiles(int [] _id)
    {
        string[] selectedFilepaths = StandaloneFileBrowser.OpenFilePanel("Select Image Files", "", filters,false);

        if (selectedFilepaths.Length == 0)
        {
            Debug.Log("No files selected");
            return;
        }

        string filepath = selectedFilepaths[0];
        AccessFiles(filepath, _id);
    }
    public void AccessFiles(string _filepath, int [] _id)
    {
        FileInfo fileInfo = new FileInfo(_filepath);

        // Check if the file exists
        if (fileInfo.Exists)
        {
            currentButtonID = _id;

            // Get the size of the file in bytes
            long fileSizeInBytes = fileInfo.Length;

            string extension = Path.GetExtension(_filepath);
            extension = extension.ToLower();

            // You can convert the file size to kilobytes, megabytes, etc. if needed
            //   double fileSizeInKB = fileSizeInBytes / 1024.0; // Convert to kilobytes

            // Check if the file size is within a certain limit before opening it
            if (fileSizeInBytes < (1024 * 1024 * 4)) // Example: If the file size is less than 1 MB
            {
                // Open the file or perform other operations
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    filePathIntoListData.Invoke(_filepath, _id);
                    CreateImages(_filepath, _id);
                    resizeDisplayedImage.Invoke();
                }
                else if (extension == ".gif")
                {
                    filePathIntoListData.Invoke(_filepath, _id);
                    PlayGIF(_filepath, _id);
                }
                //else if (extension == ".json")
                //{
                //    string jsonData = File.ReadAllText(filepaths);
                //    Debug.Log(jsonData);

                //    // Add the new object to the list of imported JSON paths
                //    //importedJsonPaths.Add(filepaths);
                //}
                else
                {
                    Debug.Log("Unsupported file format");
                }
            }
            else
            {
                triggerGIFLimit.Invoke();
            }
        }
        else
        {
            Debug.Log("File does not exist.");
        }
    }
    private void CreateImages(string _filepath, int [] _id)
    { 
        byte[] fileData = File.ReadAllBytes(_filepath);

        Texture2D texture = new(2, 2);
        texture.LoadImage(fileData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        
        sprite.name = _id[0] +"_"+ _id[1];
        
        newImportedImage.Invoke(sprite);

        Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        newImportedThumbnail.Invoke(thumbnail);
    }
    private void PlayGIF(string _filename, int [] _id)
    {
        if (string.IsNullOrWhiteSpace(_filename))
        {
            return;
        }

        //var path = Path.Combine(Application.streamingAssetsPath, filename);
        List<AnimSprite> animSprites = new();

        using (var decoder = new MG.GIF.Decoder(File.ReadAllBytes(_filename)))
        {
            MG.GIF.Image img = decoder.NextImage();
            int index = 0;
            do
            {
                Texture2D item = img.CreateTexture();

                Sprite sprite = Sprite.Create(item, new Rect(0, 0, item.width, item.height), Vector2.zero);
                sprite.name = _id[0] + "_" + _id[1] + "_" + index;

                AnimSprite anim = new(sprite, (img.Delay / 1000.0f));
                animSprites.Add(anim);
                index++;

                img = decoder.NextImage();

            } while (img != null);
        }

        displayAnimatedImageInUI.Invoke(animSprites, _id);
        newImportedThumbnail.Invoke(animSprites[0].sprite);
    }

    #region Unity
    private void Awake()
    {
        filters = new[]
        {
            new ExtensionFilter("Image Files", "jpg", "jpeg", "png", "gif")
        };
    }
    #endregion
}









