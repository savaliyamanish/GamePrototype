using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip flipClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip mismatchClip;
    [SerializeField] private AudioClip gameOverClip;

    public static AudioManager Instance;
    private void Awake()
    {
        if(Instance==null)
            Instance=this;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlip()
    {
        PlayOneShot(flipClip);
    }

    public void PlayMatch()
    {
        PlayOneShot(matchClip);
    }

    public void PlayMismatch()
    {
        PlayOneShot(mismatchClip);
    }

    public void PlayGameOver()
    {
        PlayOneShot(gameOverClip);
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip);
    }
}
