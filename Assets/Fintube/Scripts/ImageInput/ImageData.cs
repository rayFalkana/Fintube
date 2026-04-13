using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageData : MonoBehaviour
{
    public int ID;
    public int Mode;
    public string Tag;
    public string importedPath;
    public Sprite Sprite;
    public Image Thumbnail;
    public Button button;
    public Button btnResetData;
    [SerializeField] public List<AnimSprite> animSprites; 

    public void CreateID(int id, int mode)
    {
        ID = id;
        Mode = mode;
        Tag = $"Texture_{id}";
    }

    public void Input(int ID, int Mode, string Tag, string importedPath, Sprite Sprite, Image Thumbnail, Button button, List<AnimSprite> animSprites = null)
    {
        this.ID = ID;
        this.Mode = Mode;
        this.Tag = Tag;
        this.importedPath = importedPath;
        this.Sprite = Sprite;
        this.Thumbnail = Thumbnail;
        this.button =button;
        this.animSprites = animSprites;
    }

    public void Input(Sprite sprite)
    {
        this.Sprite = sprite;
        animSprites.Clear();
    }

    public void Input(List<AnimSprite> animSprites)
    {
        this.animSprites.Clear();
        this.animSprites = animSprites;
        Sprite = null;
    }

    public int[] ReturnID() { return new int[] { ID, Mode }; }

    public void ResetData()
    {
        SaveManager.ClearImageData(this);
    }

    public void Start()
    {
        btnResetData.onClick.AddListener(() => ResetData());
    }
}

[Serializable]
public class AnimSprite
{
    public Sprite sprite;
    public float delayTime;

    public AnimSprite(Sprite sprite, float delayTime)
    {
        this.sprite = sprite;
        this.delayTime = delayTime;
    }

    public AnimSprite(Texture2D tex, string name, float delayTime)
    {
        sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        sprite.name = name;
        this.delayTime = delayTime;
    }
}
