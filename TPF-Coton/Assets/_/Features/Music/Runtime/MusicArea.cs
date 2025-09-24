using UnityEngine;

public class ZoneMusic : MonoBehaviour
{
    [Header("MusicList")]
    [SerializeField] private AudioSource[] musicSources;

    [Header("Parameter")]
    [SerializeField] private float fadeOutDuration = 1.5f;

    private bool isPlayerInside = false;
    private bool fadingOut = false;
    private float fadeTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;
        fadingOut = false;
        fadeTimer = 0f;

        foreach (var source in musicSources)
        {
            if (source == null) continue;

            source.volume = 1f;
            source.loop = true;
            if (!source.isPlaying)
                source.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;
        fadingOut = true;
        fadeTimer = 0f;
    }

    private void Update()
    {
        if (!fadingOut) return;

        fadeTimer += Time.deltaTime;
        float progress = fadeTimer / fadeOutDuration;
        float newVolume = Mathf.Lerp(1f, 0f, progress);

        foreach (var source in musicSources)
        {
            if (source != null)
                source.volume = newVolume;
        }

        if (progress >= 1f)
        {
            foreach (var source in musicSources)
            {
                if (source != null)
                {
                    source.Stop();
                    source.volume = 1f; 
                }
            }
            fadingOut = false;
        }
    }
}