using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DOTweenEffects : MonoBehaviour
{
    #region DO Tween Constants
    private const float SCALE_GROW_MULTIPLIER = 2F;
    private const float SCALE_SMALL_MULTIPLIER = 2F;
    private const float TIME_TO_GROW = 0.2f;
    private const float TIME_TO_GET_SMALL = 0.2f;
    private const float TIME_TO_COLOR = 0.1F;
    private const float TIME_TO_SHAKE = 1F;
    #region Four Direction
    private const float TRANSFORM_RIGHT_AND_LEFT = 100F;
    private const float TIME_RIGHT_AND_LEFT = 5F;
    private const float TRANSFORM_UP_AND_DOWN = 100F;
    private const float TIME_UP_AND_DOWN = 5F;
    #endregion

    #region Marker
    private const float TIME_TO_MOVE = 2F;
    private const float TIME_TO_SCALE_MARKER = 2F;
    private const float TIME_TO_MOVE_ARC = 0.3F;

    private const float TIME_TO_RISK = 0.25F;
    #endregion
    #endregion
    public static DOTweenEffects Instance;
    private void Start()
    {
        Instance = this;
    }

    #region DOTween
    #region BlinkScaleToLow
    public void BlinkScaleSmall(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;

        transformToChange.DOScale(startLocalScale, TIME_TO_GET_SMALL).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_SMALL_MULTIPLIER,
                transformToChange.localScale.y * SCALE_SMALL_MULTIPLIER,
                transformToChange.localScale.z * SCALE_SMALL_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GET_SMALL))
        );
    }

    public void HoverScaleSmall(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;

        transformToChange.DOScale(startLocalScale, TIME_TO_GET_SMALL).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_SMALL_MULTIPLIER,
                transformToChange.localScale.y * SCALE_SMALL_MULTIPLIER,
                transformToChange.localScale.z * SCALE_SMALL_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GET_SMALL))
        );
    }
    #endregion

    #region BlinkScaleNormal
    public void BlinkScaleGrow(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;

        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }
    public void HoverScaleGrow(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;

        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }
    #endregion


    #region BlinkAndShake
    public void BlinkScaleAndShake(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;
        //
        Shake(transformToChange);
        //
        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }
    public void Shake(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        transformToChange.DOShakeRotation(TIME_TO_SHAKE, 10);

    }
    #endregion

    #region BlinkShakeAndPosition

    public void ScaleToZero(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        transformToChange.DOScale(Vector3.zero, TIME_TO_GROW);
    }
    #endregion
    #region MoveFourDirectionsAndDesactive
    public void DesactiveLeft(Transform transformToChange)
    {
        Color startColor = transformToChange.GetComponent<Image>().color;
        Vector3 startLocalPosition = transformToChange.localPosition;
        Vector3 localPositionDestination =
            new Vector3(transformToChange.localPosition.x + TRANSFORM_RIGHT_AND_LEFT,
            transformToChange.localPosition.y,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(localPositionDestination, TIME_RIGHT_AND_LEFT).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.GetComponent<Image>().DOColor(Color.white, TIME_TO_COLOR).
            OnComplete(() => transformToChange.GetComponent<Image>().DOColor(startColor, TIME_TO_COLOR)));
        transformToChange.localPosition = startLocalPosition;
        transformToChange.gameObject.SetActive(false);

    }
    public void DesactiveCenterToLeft(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        Vector3 startLocalPosition = transformToChange.localPosition;
        Vector3 localPositionDestination =
            new Vector3(transformToChange.localPosition.x - TRANSFORM_RIGHT_AND_LEFT,
            transformToChange.localPosition.y,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(localPositionDestination, TIME_RIGHT_AND_LEFT).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.DOScale(Vector3.zero, TIME_RIGHT_AND_LEFT).OnComplete(() =>
            transformToChange.DOScale(startLocalScale,
            TIME_UP_AND_DOWN))).
        OnComplete(() => {
            transformToChange.localPosition = startLocalPosition;
            transformToChange.gameObject.SetActive(false);
        });
    }
    public void DesactiveRightToCenter(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        Vector3 startLocalPosition = transformToChange.localPosition;
        Vector3 localPositionDestination =
            new Vector3(transformToChange.localPosition.x + TRANSFORM_RIGHT_AND_LEFT,
            transformToChange.localPosition.y,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(localPositionDestination, TIME_RIGHT_AND_LEFT).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.DOScale(Vector3.zero, TIME_RIGHT_AND_LEFT).OnComplete(() =>
            transformToChange.DOScale(startLocalScale,
            TIME_UP_AND_DOWN))).
        OnComplete(() => {
            transformToChange.localPosition = startLocalPosition;
            transformToChange.gameObject.SetActive(false);
        });
    }

    public void DesactiveCenterToDown(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        Vector3 startLocalPosition = transformToChange.localPosition;
        Vector3 localPositionDestination =
            new Vector3(transformToChange.localPosition.x,
            transformToChange.localPosition.y - TRANSFORM_UP_AND_DOWN,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(localPositionDestination, TIME_UP_AND_DOWN).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.DOScale(Vector3.zero, TIME_UP_AND_DOWN).OnComplete(() =>
            transformToChange.DOScale(startLocalScale,
            TIME_UP_AND_DOWN))).
        OnComplete(() => {
            transformToChange.localPosition = startLocalPosition;
            transformToChange.gameObject.SetActive(false);
        });
    }
    public void DesactiveCenterToUp(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        Vector3 startLocalPosition = transformToChange.localPosition;
        Vector3 localPositionDestination =
            new Vector3(transformToChange.localPosition.x,
            transformToChange.localPosition.y + TRANSFORM_UP_AND_DOWN,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(localPositionDestination, TIME_UP_AND_DOWN).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.DOScale(Vector3.zero, TIME_UP_AND_DOWN).OnComplete(() =>
            transformToChange.DOScale(startLocalScale,
            TIME_UP_AND_DOWN))).
        OnComplete(() => {
            transformToChange.localPosition = startLocalPosition;
            transformToChange.gameObject.SetActive(false);
        });
    }

    #region MoveFourDirections
    private void MoveLeft(Transform transformToChange)
    {
        Color startColor = transformToChange.GetComponent<Image>().color;
        Vector3 startLocalPosition = transformToChange.localPosition;
        transformToChange.localPosition =
            new Vector3(transformToChange.localPosition.x + TRANSFORM_RIGHT_AND_LEFT,
            transformToChange.localPosition.y,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(startLocalPosition, TIME_RIGHT_AND_LEFT).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.GetComponent<Image>().DOColor(Color.white, TIME_TO_COLOR).
            OnComplete(() => transformToChange.GetComponent<Image>().DOColor(startColor, TIME_TO_COLOR)));

    }
    public void LeftToCenter(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        transformToChange.DOScale(Vector3.zero, TIME_TO_GROW);
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;
        //
        MoveLeft(transformToChange);
        //
        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }
    private void MoveRight(Transform transformToChange)
    {
        Color startColor = transformToChange.GetComponent<Image>().color;
        Vector3 startLocalPosition = transformToChange.localPosition;
        transformToChange.localPosition =
            new Vector3(transformToChange.localPosition.x - TRANSFORM_RIGHT_AND_LEFT,
            transformToChange.localPosition.y,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(startLocalPosition, TIME_RIGHT_AND_LEFT).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.GetComponent<Image>().DOColor(Color.white, TIME_TO_COLOR).
            OnComplete(() => transformToChange.GetComponent<Image>().DOColor(startColor, TIME_TO_COLOR)));

    }
    public void RightToCenter(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        transformToChange.DOScale(Vector3.zero, TIME_TO_GROW);
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;
        //
        MoveRight(transformToChange);
        //
        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }

    public void DownToCenter(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        transformToChange.localScale = Vector3.zero;
        //
        MoveDown(transformToChange);
        //
        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
    }

    private void MoveDown(Transform transformToChange)
    {
        Color startColor = transformToChange.GetComponent<Image>().color;
        Vector3 startLocalPosition = transformToChange.localPosition;
        transformToChange.localPosition =
            new Vector3(transformToChange.localPosition.x,
            transformToChange.localPosition.y - TRANSFORM_UP_AND_DOWN,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(startLocalPosition, TIME_UP_AND_DOWN).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.GetComponent<Image>().DOColor(Color.white, TIME_TO_COLOR).
            OnComplete(() => transformToChange.GetComponent<Image>().DOColor(startColor, TIME_TO_COLOR)));

    }

    public void UpToCenter(Transform transformToChange)
    {
        if (DOTween.IsTweening(transformToChange))
            return;
        Vector3 startLocalScale = transformToChange.localScale;
        //

        //
        transformToChange.DOScale(startLocalScale, TIME_TO_GROW).OnComplete(() =>
            transformToChange.DOScale(new Vector3
                (transformToChange.localScale.x * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.y * SCALE_GROW_MULTIPLIER,
                transformToChange.localScale.z * SCALE_GROW_MULTIPLIER),
                TIME_TO_GROW).
                OnStepComplete(() => transformToChange.DOScale(startLocalScale, TIME_TO_GROW))
        );
        MoveUp(transformToChange);
    }

    private void MoveUp(Transform transformToChange)
    {
        Color startColor = transformToChange.GetComponent<Image>().color;
        Vector3 startLocalPosition = transformToChange.localPosition;
        transformToChange.localPosition =
            new Vector3(transformToChange.localPosition.x,
            transformToChange.localPosition.y - TRANSFORM_UP_AND_DOWN,
            transformToChange.localPosition.z);

        transformToChange.DOLocalMove(startLocalPosition, TIME_UP_AND_DOWN).
            SetEase(Ease.InOutElastic).
            OnStart(() => transformToChange.GetComponent<Image>().DOColor(Color.white, TIME_TO_COLOR).
            OnComplete(() => transformToChange.GetComponent<Image>().DOColor(startColor, TIME_TO_COLOR)));

    }
    #endregion
    #endregion

    #region Markers
    public void MarkerMovemment(Transform markerParent, Transform markerObject, Transform arcObject, Vector3 objectInitialPosition)
    {
        //Marker Parent
        Vector3 markerParentLocalScale = markerParent.localScale;
        Vector3 markerParentLocalPosition = markerParent.localPosition;
        Vector3 markerLocalPositionNavigation =
            new Vector3(markerParent.localPosition.x,
            markerParent.localPosition.y,
            markerParent.localPosition.z);
        Vector3 markerLocalInitialPosition = objectInitialPosition;
        markerParent.localPosition = markerLocalInitialPosition;
        //Marker Object
        Vector3 markerObjectLocalScale = markerObject.localScale;
        Vector3 markerObjectLocalPosition = markerObject.localPosition;
        Vector3 markerObjectPositionDestination =
            new Vector3(markerObject.localPosition.x,
            markerObject.localPosition.y,
            markerObject.localPosition.z);
        markerObject.localScale = Vector3.zero;
        //Arc
        Vector3 arcLocalScale = arcObject.localScale;
        Vector3 arcLocalPosition = arcObject.localPosition;
        Vector3 arcPositionDestination =
            new Vector3(arcObject.localPosition.x,
            arcObject.localPosition.y,
            arcObject.localPosition.z);
        arcObject.localScale = Vector3.zero;
        markerObject.DOScale(markerObjectLocalScale, TIME_TO_SCALE_MARKER);
        markerParent.DOLocalMove(markerLocalPositionNavigation, TIME_TO_MOVE).
            SetEase(Ease.InOutElastic).
            OnComplete(() => {
                arcObject.DOScale(arcLocalScale, TIME_TO_MOVE_ARC);
            });
    }
    public void RiskArc(Transform risk, Transform marker, Transform arc, Color color)
    {
        risk.DOLocalMove(marker.localPosition, TIME_TO_RISK).
            OnComplete(() =>
            {
                risk.DOScale(Vector3.zero, TIME_TO_RISK).
                OnComplete(() => {
                    marker.GetComponent<Image>().DOColor(color, TIME_TO_RISK).
                    OnComplete(() => CloseArc(arc, marker, false));
                });
            });
    }
    public void CloseArc(Transform arc, Transform marker, bool isDelete)
    {
        arc.DOScale(Vector3.zero, TIME_TO_RISK).
            OnComplete(() =>
            {
                if (isDelete)
                    marker.DOScale(Vector3.zero, TIME_TO_RISK);
            });
    }


    #endregion
    #endregion
}
