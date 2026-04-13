using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public FlexibleColorPicker colorPicker;
    public Button btnOpenColorPicker;
    public Button btnSetColorToCamera;

    Camera _camera;
    GameObject theParent;

    private Color currentColor;
    private Color targetColor;

    private ColorTransitionSettings transitionSettings;
    private bool isTransitioning = false;

    private void Start()
    {
        theParent = colorPicker.transform.parent.gameObject;
        btnOpenColorPicker.onClick.AddListener(() => { OpenColorPicker(); });
        btnSetColorToCamera.onClick.AddListener(() => { SetColorToCamera(); });

        _camera = Camera.main;
        currentColor = _camera.backgroundColor;

        transitionSettings = new ColorTransitionSettings();
        SetColorTransitionSettings(0.5f, ColorTransitionType.Linear);
    }

    public void OpenColorPicker()
    {
        if (theParent.activeSelf)
        {
            theParent.SetActive(false);
        }
        else
        {
            theParent.SetActive(true);
        }
    }

    public void SetColorToCamera()
    {
        SetTargetColor(colorPicker.GetColor());
        OpenColorPicker();
    }

    // Update is called once per frame
    private void TransitionColor()
    {
        if (!isTransitioning && currentColor != targetColor)
        {
            isTransitioning = true;
            StartCoroutine(TransitionColorCoroutine());
        }
    }

    private IEnumerator TransitionColorCoroutine()
    {
        float t = 0f;
        while (t < transitionSettings.duration)
        {
            t += Time.deltaTime;
            _camera.backgroundColor = Color.Lerp(currentColor, targetColor, transitionSettings.curve.Evaluate(t / transitionSettings.duration));
            yield return null;
        }
        currentColor = targetColor;
        isTransitioning = false;
    }

    public void SetTargetColor(Color newTargetColor)
    {
        targetColor = newTargetColor;
        TransitionColor();
    }

    public void SetColorTransitionSettings(float duration, ColorTransitionType transitionType)
    {
        switch (transitionType)
        {
            case ColorTransitionType.Linear:
                transitionSettings.curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
                break;
            case ColorTransitionType.EaseIn:
                transitionSettings.curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                break;
            case ColorTransitionType.EaseOut:
                transitionSettings.curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                break;
        }
        transitionSettings.duration = duration;
    }

    private class ColorTransitionSettings
    {
        public float duration;
        public AnimationCurve curve;
    }

    public enum ColorTransitionType
    {
        Linear,
        EaseIn,
        EaseOut
    }
}
