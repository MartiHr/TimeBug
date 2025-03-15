using UnityEngine;
using UnityEngine.UI; // Required for RawImage
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene Data")]
    [SerializeField] private CutsceneData cutsceneData;

    [Header("UI References")]
    [Tooltip("RawImage component that displays the current slide")]
    [SerializeField] private RawImage slideImage; // Changed from Image to RawImage

    [Tooltip("Text component for displaying captions")]
    [SerializeField] private TextMeshProUGUI captionText;

    [Tooltip("CanvasGroup used for fading the entire screen")]
    [SerializeField] private CanvasGroup fadeOverlay;

    [Tooltip("CanvasGroup containing all cutscene elements")]
    [SerializeField] private CanvasGroup cutscenePanel;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Transition Settings")]
    [SerializeField] private float fadeSpeed = 1.5f;
    [SerializeField] private bool autoProgress = true;

    [Header("Input Controls")]
    [SerializeField] private KeyCode nextSlideKey = KeyCode.Space;
    [SerializeField] private KeyCode skipCutsceneKey = KeyCode.Escape;

    [Header("Events")]
    public UnityEvent onCutsceneStarted;
    public UnityEvent onCutsceneEnded;
    public UnityEvent onSlideChanged;

    private int currentSlideIndex = 0;
    private Coroutine slideshowCoroutine;
    private bool isPlaying = false;
    private Color originalImageColor;
    private Color originalTextColor;

    private void Awake()
    {
        if (slideImage != null)
        {
            slideImage.texture = null; // Clear the default texture
            slideImage.color = new Color(1, 1, 1, 0); // Set alpha to 0
            originalImageColor = slideImage.color;
        }

        if (captionText != null)
        {
            captionText.color = new Color(1, 1, 1, 0); // Set alpha to 0
            originalTextColor = captionText.color;
        }

        if (cutscenePanel != null)
        {
            cutscenePanel.alpha = 0; // Set alpha to 0
            cutscenePanel.gameObject.SetActive(false); // Disable the panel
        }
    }


    private void Update()
    {
        if (!isPlaying) return;

        if (Input.GetKeyDown(skipCutsceneKey))
        {
            Debug.Log("Skip Cutscene Key Pressed");
            SkipCutscene();
        }
        else if (Input.GetKeyDown(nextSlideKey))
        {
            Debug.Log("Next Slide Key Pressed");
            NextSlide();
        }
    }

    public void StartCutscene()
    {
        if (isPlaying || cutsceneData == null) return;

        Debug.Log("Cutscene Started");
        isPlaying = true;
        currentSlideIndex = 0;

        if (cutscenePanel != null)
        {
            cutscenePanel.alpha = 0; // Ensure the panel is hidden initially
            cutscenePanel.gameObject.SetActive(true); // Enable the panel
            StartCoroutine(FadeCanvasGroup(cutscenePanel, 0, 1)); // Fade in the panel
        }

        if (musicSource != null && cutsceneData.backgroundMusic != null)
        {
            musicSource.clip = cutsceneData.backgroundMusic;
            musicSource.Play();
        }

        onCutsceneStarted?.Invoke();
        slideshowCoroutine = StartCoroutine(RunSlideshow());
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float targetAlpha)
    {
        if (group == null) yield break;

        float elapsed = 0;
        float fadeDuration = 1f / fadeSpeed;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / fadeDuration);

            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);

            yield return null;
        }

        group.alpha = targetAlpha;
    }

    private IEnumerator FadeInCutscene()
    {
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(fadeOverlay, 1, 0)); // Fade from opaque to transparent
        }

        slideshowCoroutine = StartCoroutine(RunSlideshow());
    }

    public void NextSlide()
    {
        if (!isPlaying) return;

        if (currentSlideIndex >= cutsceneData.slides.Count - 1)
        {
            SkipCutscene();
            return;
        }

        StopCoroutine(slideshowCoroutine);
        currentSlideIndex++;
        slideshowCoroutine = StartCoroutine(RunSlideshow());
    }

    public void SkipCutscene()
    {
        if (!isPlaying) return;

        StopAllCoroutines();
        StartCoroutine(EndCutsceneSequence());
    }

    private IEnumerator RunSlideshow()
    {
        if (currentSlideIndex == 0)
        {
            yield return StartCoroutine(FadeCutscenePanel(1));
        }

        while (currentSlideIndex < cutsceneData.slides.Count)
        {
            CutsceneData.SlideData currentSlide = cutsceneData.slides[currentSlideIndex];
            yield return StartCoroutine(TransitionToSlide(currentSlide));
            onSlideChanged?.Invoke();

            if (autoProgress)
            {
                yield return new WaitForSeconds(currentSlide.duration);
                currentSlideIndex++;
            }
            else
            {
                break;
            }
        }

        if (currentSlideIndex >= cutsceneData.slides.Count)
        {
            yield return StartCoroutine(EndCutsceneSequence());
        }
    }

    private IEnumerator TransitionToSlide(CutsceneData.SlideData slide)
    {
        yield return StartCoroutine(FadeSlideContent(0));

        if (slideImage != null && slide.image != null)
        {
            Debug.Log("Setting Slide Image: " + slide.image.name);
            slideImage.texture = slide.image; // Assign texture instead of sprite
        }
        else
        {
            Debug.LogWarning("Slide image or texture is missing.");
        }

        if (captionText != null)
        {
            Debug.Log("Setting Caption: " + slide.caption);
            captionText.text = slide.caption;
        }

        if (sfxSource != null && slide.slideAudio != null)
        {
            Debug.Log("Playing Slide Audio: " + slide.slideAudio.name);
            sfxSource.PlayOneShot(slide.slideAudio);
        }

        yield return StartCoroutine(FadeSlideContent(1));
    }

    private IEnumerator FadeSlideContent(float targetAlpha)
    {
        if (slideImage == null && captionText == null) yield break;

        Color currentImageColor = slideImage != null ? slideImage.color : Color.clear;
        Color currentTextColor = captionText != null ? captionText.color : Color.clear;

        float startImageAlpha = currentImageColor.a;
        float startTextAlpha = currentTextColor.a;

        float elapsed = 0;
        float fadeDuration = 1f / fadeSpeed;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / fadeDuration);

            if (slideImage != null)
            {
                currentImageColor.a = Mathf.Lerp(startImageAlpha, targetAlpha, normalizedTime);
                slideImage.color = currentImageColor;
            }

            if (captionText != null)
            {
                currentTextColor.a = Mathf.Lerp(startTextAlpha, targetAlpha, normalizedTime);
                captionText.color = currentTextColor;
            }

            yield return null;
        }

        if (slideImage != null)
        {
            currentImageColor.a = targetAlpha;
            slideImage.color = currentImageColor;
        }

        if (captionText != null)
        {
            currentTextColor.a = targetAlpha;
            captionText.color = currentTextColor;
        }
    }

    private IEnumerator FadeCutscenePanel(float targetAlpha)
    {
        if (cutscenePanel == null) yield break;

        float startAlpha = cutscenePanel.alpha;
        float elapsed = 0;
        float fadeDuration = 1f / fadeSpeed;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / fadeDuration);

            cutscenePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);

            yield return null;
        }

        cutscenePanel.alpha = targetAlpha;
    }

    private IEnumerator EndCutsceneSequence()
    {
        if (fadeOverlay != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(fadeOverlay, 0, 1)); // Fade from transparent to opaque
        }

        if (cutscenePanel != null)
        {
            yield return StartCoroutine(FadeCanvasGroup(cutscenePanel, 1, 0));
            cutscenePanel.gameObject.SetActive(false);
        }

        if (musicSource != null && musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0;
            float fadeDuration = 1f / fadeSpeed;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsed / fadeDuration);

                musicSource.volume = Mathf.Lerp(startVolume, 0, normalizedTime);

                yield return null;
            }

            musicSource.Stop();
            musicSource.volume = startVolume;
        }

        isPlaying = false;
        currentSlideIndex = 0;
        onCutsceneEnded?.Invoke();
    }
}