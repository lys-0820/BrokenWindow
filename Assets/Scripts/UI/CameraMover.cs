using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    public float moveDuration = 2f; // Duration of movement
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Smooth movement

    private Camera mainCamera;
    private Coroutine moveCoroutine;

    public Transform cityViewTransform;
    public Transform balconyViewTransform;

    public AudioSource cityAudioSource;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void MoveToCityView() {
        Vector3 targetPosition = cityViewTransform.position;
        MoveCamera(targetPosition);
        StartCoroutine(FadeInCityAudio(true));
    }

    public void MoveToBalconyView() {
        Vector3 targetPosition = balconyViewTransform.position;
        MoveCamera(targetPosition);
        StartCoroutine(FadeInCityAudio(false));
        // cityAudioSource.DOFade(1f, 2f).SetEase(Ease.InOutSine);
    }

    public void MoveCameraToObject(GameObject targetObject)
    {
        if (targetObject == null)
        {
            Debug.LogWarning("MoveCameraToObject: Target object is null.");
            return;
        }

        Vector3 targetPosition = targetObject.transform.position;
        MoveCamera(targetPosition);
    }

    private void MoveCamera(Vector3 targetPosition)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine); // Stop any ongoing movement

        moveCoroutine = StartCoroutine(MoveCameraCoroutine(targetPosition));
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition)
    {
        Vector3 startPosition = mainCamera.transform.position;

        // Ensure Z position stays the same to avoid zooming issues
        targetPosition.z = startPosition.z;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(t); // Smooth easing

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }

        mainCamera.transform.position = targetPosition; // Ensure final position
    }

    IEnumerator FadeInCityAudio(bool fadeIn)
    {
        float elapsedTime = 0f;
        float targetVolume = fadeIn ? 1f : 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            cityAudioSource.volume = Mathf.SmoothStep(cityAudioSource.volume, targetVolume, t); // Smooth transition

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cityAudioSource.volume = targetVolume;
    }
}