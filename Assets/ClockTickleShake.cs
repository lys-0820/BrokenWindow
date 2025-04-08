using DG.Tweening;
using UnityEngine;

public class PlantShaker : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public float shakeDistance = 0.2f;    // How far it moves left/right
    public float shakeAngle = 10f;         // How much it tilts (in degrees)
    public float shakeDuration = 0.1f;    // How long one side movement takes

    private Tween moveTween;
    private Tween rotateTween;

    void Start()
    {
        ClockController.OnClockTickleStart += StartShaking;
        ClockController.OnClockTickleEnd += StopShaking;
    }

    void OnDestroy()
    {
        ClockController.OnClockTickleStart -= StartShaking;
        ClockController.OnClockTickleEnd -= StopShaking;
    }

    public void StartShaking()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        // Stop any previous shakes
        StopShaking();

        // Side to side movement (X axis)
        moveTween = transform.DOLocalMoveX(originalPosition.x + shakeDistance, shakeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        // Slight rotation left/right (Z axis, like leaning side to side)
        rotateTween = transform.DOLocalRotate(new Vector3(0, 0, shakeAngle), shakeDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopShaking()
    {
        // Kill tweens if active
        moveTween?.Kill();
        rotateTween?.Kill();

        // Return to original state
        transform.DOLocalMove(originalPosition, 0.2f);
        transform.DOLocalRotateQuaternion(originalRotation, 0.2f);
    }
}