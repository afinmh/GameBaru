using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip shootingClip;
    [SerializeField] private AudioClip trailClip; 
    [SerializeField] private AudioClip hitClip;       
    [SerializeField] private AudioClip emptyClip;     
    [SerializeField] private AudioClip reloadClip; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayShootingSound()
    {
        if (shootingClip != null)
            audioSource.PlayOneShot(shootingClip);
    }

    public void PlayTrailSound()
    {
        if (trailClip != null)
            audioSource.PlayOneShot(trailClip);
    }

    public void PlayHitSound()
    {
        if (hitClip != null)
            audioSource.PlayOneShot(hitClip);
    }

    public void PlayBulletEmpty()
    {
        if (hitClip != null)
            audioSource.PlayOneShot(emptyClip);
    }

    public void PlayReloadSound()
    {
        if (hitClip != null)
            audioSource.PlayOneShot(reloadClip);
    }
}
