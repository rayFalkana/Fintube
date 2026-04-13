using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MovingTextController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Text text;

    [Space(5)]
    [Header("Point")]
    [SerializeField] private RectTransform pointText;
    [SerializeField] private RectTransform pointLeft;
    [SerializeField] private RectTransform pointRight;
    [SerializeField] private RectTransform pointTail;

    private const float movingSpeed = 2f;
    private const float minDistance = 1.0f;

    private float GetPositionX(RectTransform _rectTransform) => _rectTransform.position.x;

    private void MoveToPosition(RectTransform _source, RectTransform _target)
    {
        _source.SetPositionAndRotation(_target.position, Quaternion.identity);
    }

    private void MoveText()
    {
        pointText.Translate(Vector2.left * movingSpeed);
    }

    private bool IsDistanceNear(RectTransform value, RectTransform target, float minDistance)
    {
        float distance = Mathf.Abs(GetPositionX(value) - GetPositionX(target));
        return distance < minDistance;
    }

    public void SetText(string _text)
    {
        StopCoroutine(TextMovementCoroutine());
        MoveToPosition(pointText, pointLeft);
        text.text = _text;
        StartCoroutine(TextMovementCoroutine());
    }

    public void PauseWalk()=> StopCoroutine(TextMovementCoroutine());
    public void ContinueWalk() => StartCoroutine(TextMovementCoroutine());

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        MoveToPosition(pointText, pointLeft);
    }

    private IEnumerator TextMovementCoroutine()
    {
        while (true)
        {
            if (GetPositionX(pointTail) < GetPositionX(pointLeft))
            {
                MoveToPosition(pointText, pointRight);
            }
            else if (IsDistanceNear(pointText, pointLeft, minDistance))
            {
                MoveToPosition(pointText, pointLeft);
                yield return new WaitForSecondsRealtime(2);
            }

            MoveText();

            yield return null; // Wait for the next frame
        }
    }
}
