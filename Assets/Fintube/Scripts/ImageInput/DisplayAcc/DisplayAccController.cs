using System.IO;
using SFB;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisplayAccController : MonoBehaviour
{
    [SerializeField] private Button btnAcc;
    [SerializeField] private Image displayImage;
    [SerializeField] private AnimationInputController displayAnim;

    [SerializeField] private GameObject miniMenu;
    [SerializeField] private ImageData imageData;
    [SerializeField] private Button btnSetting;

    [SerializeField] private GameObject settingMenu;

    private ExtensionFilter[] filters;
    private UnityEvent triggerGIFLimit;

    public void AddListenerToTriggerGIFLimit(UnityAction _action)
    {
        if (triggerGIFLimit == null) triggerGIFLimit = new();
        triggerGIFLimit.AddListener(_action);
    }

    private void ShowMenu(GameObject _object)
    {
        if (_object.activeSelf)
        {
            _object.SetActive(false);
        }
        else{
            _object.SetActive(true);
        }
    }
    private void ResizeImage(int _value)
    {
        int currentWidth = displayImage.sprite.texture.width;
        int currentHeight = displayImage.sprite.texture.height;

        float aspecRatio = (float)currentWidth / currentHeight;

        int newWidth, newHeight;

        if (aspecRatio > 1f)
        {
            newWidth = Mathf.Min(currentWidth, _value);
            newHeight = Mathf.RoundToInt(newWidth / aspecRatio);
        }
        else
        {
            newHeight = Mathf.Min(currentHeight, _value);
            newWidth = Mathf.RoundToInt(newHeight * aspecRatio);
        }

        displayImage.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
    }
    private void SetImageInDisplay(Sprite _sprite)
    {
        imageData.Input(_sprite);
        displayImage.gameObject.SetActive(true);
        displayImage.sprite = _sprite;
    }
    private void SetImageInDisplay(List<AnimSprite> _sprite)
    {
        imageData.Input(_sprite);
        displayImage.gameObject.SetActive(false);
        displayAnim.PlayAnimation(_sprite);
    }
    private void SetThumbnail(Sprite _sprite)
    {
        imageData.Thumbnail.sprite = _sprite;
    }

    private void ImportFile()
    {
        string[] selectedFilepaths = StandaloneFileBrowser.OpenFilePanel("Select Image Files", "", filters, false);

        if (selectedFilepaths.Length == 0)
        {
            Debug.Log("No files selected");
            return;
        }

        string filepath = selectedFilepaths[0];
        AccessFiles(filepath);
    }
    private void AccessFiles(string _filepath)
    {
        FileInfo fileInfo = new FileInfo(_filepath);

        // Check if the file exists
        if (fileInfo.Exists)
        {
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
                    imageData.importedPath = _filepath;
                    CreateImages(_filepath);
                    ResizeImage(1000);
                }
                else if (extension == ".gif")
                {
                    imageData.importedPath = _filepath;
                    PlayGIF(_filepath);
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
    private void CreateImages(string _filepath)
    {
        byte[] fileData = File.ReadAllBytes(_filepath);

        Texture2D texture = new(2, 2);
        texture.LoadImage(fileData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        sprite.name = "_acc";

        SetImageInDisplay(sprite);

        Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        SetThumbnail(thumbnail);
    }
    private void PlayGIF(string _filename)
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
                sprite.name = "_acc" + index;

                AnimSprite anim = new(sprite, (img.Delay / 1000.0f));
                animSprites.Add(anim);
                index++;

                img = decoder.NextImage();

            } while (img != null);
        }

        SetImageInDisplay(animSprites);
        SetThumbnail(animSprites[0].sprite);
    }


    // Start is called before the first frame update
    void Start()
    {
        filters = new[]
{
            new ExtensionFilter("Image Files", "jpg", "jpeg", "png", "gif")
        };

        miniMenu.SetActive(false);
        btnAcc.onClick.AddListener(() => { ShowMenu(miniMenu); });
        imageData.button.onClick.AddListener(() => { ImportFile(); });
        btnSetting.onClick.AddListener(() => { ShowMenu(settingMenu); });

        displayAnim.StartAnimationInputController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
