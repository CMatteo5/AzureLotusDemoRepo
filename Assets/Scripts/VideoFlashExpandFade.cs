using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(VideoPlayer))]
public class VideoFlashExpandFade : MonoBehaviour
{
    public float flashDuration = 0.2f;   // Time to stay bright/tinted
    public float expandDuration = 1.0f;  // Time to scale and fade out
    public float maxScale = 2.0f;        // Target expansion size
    public Color flashColor = Color.white; // Tint during the flash phase

    private RawImage rawImage;
    private VideoPlayer videoPlayer;
    private RectTransform rectTransform;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        videoPlayer = GetComponent<VideoPlayer>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void PlayVideoWithEffect()
    {
        StartCoroutine(VideoSequenceRoutine());
    }

    private IEnumerator VideoSequenceRoutine()
    {
        videoPlayer.Play();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        rawImage.texture = videoPlayer.texture;

        Vector3 initialScale = rectTransform.localScale;

        float elapsed = 0f;
        rawImage.color = flashColor; // Max alpha and tint applied

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. Expand and Fade Out Phase
        elapsed = 0f;
        while (elapsed < expandDuration)
        {
            float t = elapsed / expandDuration;

            // Interpolate scale outward
            rectTransform.localScale = Vector3.Lerp(initialScale, initialScale * maxScale, t);

            // Interpolate alpha to 0 (fade out)
            Color fadedColor = flashColor;
            fadedColor.a = Mathf.Lerp(1f, 0f, t);
            rawImage.color = fadedColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 4. Final Cleanup
        Color finalColor = flashColor;
        finalColor.a = 0f;
        rawImage.color = finalColor;
        rectTransform.localScale = initialScale; // Reset scale for next playback
        videoPlayer.Stop();                     // Stop the video engine
    }
}