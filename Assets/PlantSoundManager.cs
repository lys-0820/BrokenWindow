using UnityEngine;

public class PlantSoundManager : MonoBehaviour
{
    [Header("PlantSounds")]
    private AudioSource audioSource;
    public AudioClip plantPlacedClip;
    public AudioClip plantDiscardedClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDiscardedClip()
    {
        audioSource.PlayOneShot(plantDiscardedClip);
    }

    public void PlayPlacedClip()
    {
        audioSource.PlayOneShot(plantPlacedClip);
    }
}
