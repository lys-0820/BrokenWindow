using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stamp : MonoBehaviour
{
    public float duration = 0.3f;
    public Vector3 startScale = Vector3.one * 2.5f;
    public Vector3 endScale = Vector3.one;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private RectTransform rectTransform;
    public AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        transform.gameObject.SetActive(false);
    }

    public void Play()
    {
        
        StopAllCoroutines();
        transform.gameObject.SetActive(true);
        rectTransform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(AnimateStamp());
    }

    private IEnumerator AnimateStamp()
    {
        yield return new WaitForSeconds(3f);
        audioSource.Play();
        
        rectTransform.localScale = startScale;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float scaleValue = scaleCurve.Evaluate(t);
            rectTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, scaleValue);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = endScale;

        yield return new WaitForSeconds(3f);
        transform.gameObject.SetActive(false);
    }
}
