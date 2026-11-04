using System.Collections;
using UnityEngine;

/// <summary>
/// "Dying bulb" flicker for a realtime Point light (Built-in Render Pipeline).
/// The bulb sits dark for long stretches, sputters a few times, then holds on
/// briefly with a faint buzz before cutting out again.
///
/// Setup:
///  - Put this on a Point light with Mode = Realtime (not Baked / Mixed).
///  - Optional: drag the fixture's mesh into Emissive Renderers so its glow
///    dims with the light. That material needs Emission enabled (Standard shader).
/// </summary>
[RequireComponent(typeof(Light))]
[DisallowMultipleComponent]
public class FlickerLight : MonoBehaviour
{
    [Header("Intensity")]
    [Tooltip("Brightness when the bulb is fully on. Captured from the Light on start if left at 0.")]
    [SerializeField] private float onIntensity = 0f;

    [Tooltip("Brightness while 'dead'. 0 = fully off; a small value reads as a dim ember.")]
    [SerializeField] private float offIntensity = 0f;

    [Header("Dark gaps (seconds)")]
    [SerializeField] private Vector2 darkDuration = new Vector2(1.5f, 6f);

    [Header("Sputter before turning on")]
    [Tooltip("How many quick blinks happen before the bulb holds on.")]
    [SerializeField] private Vector2Int sputterCount = new Vector2Int(1, 4);

    [Tooltip("Length of each blink on/off (seconds).")]
    [SerializeField] private Vector2 sputterTime = new Vector2(0.03f, 0.12f);

    [Tooltip("Brightness range of each blink, as a fraction of On Intensity.")]
    [SerializeField] private Vector2 sputterBrightness = new Vector2(0.3f, 1f);

    [Header("Holding on (seconds)")]
    [SerializeField] private Vector2 onDuration = new Vector2(0.2f, 1.5f);

    [Tooltip("How much the light wavers while on (0 = steady).")]
    [Range(0f, 1f)]
    [SerializeField] private float buzzAmount = 0.15f;

    [SerializeField] private float buzzSpeed = 25f;

    [Tooltip("Seconds to fade out when the bulb cuts. 0 = instant snap.")]
    [SerializeField] private float cutOutFade = 0.04f;

    [Header("Fixture glow (optional)")]
    [SerializeField] private Renderer[] emissiveRenderers;

    [Tooltip("Which material slot on those renderers holds the emissive material.")]
    [SerializeField] private int emissiveMaterialIndex = 0;

    [Header("Misc")]
    [Tooltip("Random delay before the first cycle, so multiple bulbs don't sync up.")]
    [SerializeField] private float maxStartDelay = 3f;

    [Tooltip("Turn the Light component off when it's fully dark (saves a per-pixel light slot).")]
    [SerializeField] private bool disableLightWhenDark = true;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private Light _light;
    private float _baseIntensity;
    private Color[] _baseEmission;
    private MaterialPropertyBlock _mpb;
    private float _noiseSeed;
    private float _level;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _baseIntensity = _light.intensity;
        if (onIntensity <= 0f) onIntensity = _light.intensity;

        _noiseSeed = Random.value * 100f;
        _mpb = new MaterialPropertyBlock();
        CacheEmission();

#if UNITY_EDITOR
        if (_light.lightmapBakeType != LightmapBakeType.Realtime)
            Debug.LogWarning($"[FlickerLight] '{name}' is not set to Realtime mode. " +
                             "Baked/Mixed lights get baked into the lightmap and won't flicker cleanly.", this);
#endif
    }

    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // Restore the light so it doesn't get stuck off when the script is disabled.
        _light.enabled = true;
        _light.intensity = _baseIntensity;
        SetEmission(1f);
    }

    private IEnumerator Run()
    {
        SetLevel(0f);
        yield return new WaitForSeconds(Random.Range(0f, maxStartDelay));

        while (true)
        {
            // 1. Dead
            SetLevel(0f);
            yield return new WaitForSeconds(RandomIn(darkDuration));

            // 2. Sputter
            int blinks = Random.Range(sputterCount.x, sputterCount.y + 1);
            for (int i = 0; i < blinks; i++)
            {
                SetLevel(RandomIn(sputterBrightness));
                yield return new WaitForSeconds(RandomIn(sputterTime));
                SetLevel(0f);
                yield return new WaitForSeconds(RandomIn(sputterTime));
            }

            // 3. Hold on with a buzz
            float hold = RandomIn(onDuration);
            float t = 0f;
            while (t < hold)
            {
                float noise = Mathf.PerlinNoise(Time.time * buzzSpeed, _noiseSeed);
                SetLevel(1f - buzzAmount * noise);
                t += Time.deltaTime;
                yield return null;
            }

            // 4. Cut out
            if (cutOutFade > 0f)
            {
                float start = _level;
                float f = 0f;
                while (f < cutOutFade)
                {
                    f += Time.deltaTime;
                    SetLevel(Mathf.Lerp(start, 0f, f / cutOutFade));
                    yield return null;
                }
            }
        }
    }

    /// <param name="level">0 = dead, 1 = fully on.</param>
    private void SetLevel(float level)
    {
        _level = level;
        float intensity = Mathf.Lerp(offIntensity, onIntensity, level);
        _light.intensity = intensity;

        if (disableLightWhenDark)
            _light.enabled = intensity > 0.001f;

        SetEmission(level);
    }

    private void CacheEmission()
    {
        if (emissiveRenderers == null) return;

        _baseEmission = new Color[emissiveRenderers.Length];
        for (int i = 0; i < emissiveRenderers.Length; i++)
        {
            Renderer r = emissiveRenderers[i];
            if (r == null) continue;

            Material[] mats = r.sharedMaterials;
            if (emissiveMaterialIndex < mats.Length &&
                mats[emissiveMaterialIndex] != null &&
                mats[emissiveMaterialIndex].HasProperty(EmissionColorId))
            {
                _baseEmission[i] = mats[emissiveMaterialIndex].GetColor(EmissionColorId);
            }
        }
    }

    private void SetEmission(float level)
    {
        if (emissiveRenderers == null || _baseEmission == null) return;

        for (int i = 0; i < emissiveRenderers.Length; i++)
        {
            Renderer r = emissiveRenderers[i];
            if (r == null) continue;

            r.GetPropertyBlock(_mpb, emissiveMaterialIndex);
            _mpb.SetColor(EmissionColorId, _baseEmission[i] * level);
            r.SetPropertyBlock(_mpb, emissiveMaterialIndex);
        }
    }

    private static float RandomIn(Vector2 range) => Random.Range(range.x, range.y);
}
