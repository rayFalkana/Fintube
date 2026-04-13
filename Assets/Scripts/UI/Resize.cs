using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resize : MonoBehaviour
{
    public ImportFile importFile;

    public void Start()
    {
        importFile = GetComponent<ImportFile>();
    }
    public void ChangeValue()
    {

        GameObject inputObj = new GameObject("Resize"); 
        InputField newInput = inputObj.GetComponent<InputField>();
        newInput.characterLimit = 10; // Set the maximum number of characters allowed
        newInput.lineType = InputField.LineType.SingleLine; // Set the type of line for the input field
        if (newInput.placeholder != null && newInput.placeholder.GetComponent<Text>() != null)
        {
            newInput.placeholder.GetComponent<Text>().text = "Enter text here"; // Set the placeholder text
        }
        newInput.onValueChanged.AddListener(delegate { OnValueChanged(); });

        RectTransform rectTransform = inputObj.GetComponent<RectTransform>();
        rectTransform.SetParent(transform); // Set the parent transform
        rectTransform.anchoredPosition = Vector2.zero; // Center the input field
        rectTransform.sizeDelta = new Vector2(200f, 30f);
    }

    public void OnValueChanged()
    {
        InputField inputField = GetComponent<InputField>();
        string name = inputField.text;
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Please enter something");
        }
        /*public int maxRatio = 1000;
        public SpriteRenderer spriteRenderer;
       public void ResizeImage(int maxSize)
        {
            int width = spriteRenderer.sprite.texture.width;
            int height = spriteRenderer.sprite.texture.height;

            float aspecRatio = (float)width / height;

            int newWidth, newHeight;

            if(aspecRatio > 1f)
            {
                newWidth = Mathf.Min(width, maxSize);
                newHeight = Mathf.RoundToInt(newWidth / aspecRatio);
            }
            else
            {
                newHeight = Mathf.Min(height, maxSize);
                newWidth = Mathf.RoundToInt(newHeight * aspecRatio);
            }

            spriteRenderer.size = new Vector2(newWidth, newHeight);
        }*/
    }
}
