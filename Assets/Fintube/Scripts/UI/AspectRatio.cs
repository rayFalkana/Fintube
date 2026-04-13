using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatio : MonoBehaviour
{
    public Image _displayImage;
    public static Image PlaceImageWithAspectRatio(Image imageComponent)
    {
        imageComponent.preserveAspect = true;
        
        AspectRatioFitter aspectRatioFitter = imageComponent.GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspectRatioFitter.aspectRatio = imageComponent.sprite.rect.width / imageComponent.sprite.rect.height;

        return imageComponent;
    }

    private void Update()
    {
        try
        {
            if (_displayImage.sprite.texture != null)
            {
                _displayImage.preserveAspect = true;

                AspectRatioFitter aspectRatioFitter = _displayImage.GetComponent<AspectRatioFitter>();
                aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectRatioFitter.aspectRatio = _displayImage.sprite.rect.width / _displayImage.sprite.rect.height;
            }
        }
        catch { }

    }
}
