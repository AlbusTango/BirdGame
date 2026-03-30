using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackgroundSequence : MonoBehaviour
{
    [Header("UI Background Images")]
    public Image bgA;
    public Image bgB;

    [Header("Background Sprites")]
    public Sprite endingBackground;
    public List<Sprite> randomBackgroundsInbetween;

    [Header("Timing")]
    public float transitionTime = 1.5f;
    public float interval = 30f;

    private bool showingA = true;
    private int lastStarterIndex = -1;
    private int lastRandomIndex = -1;
    private bool endingShown = false;

    public int randomCyclesBeforeEnding = 5;
    private int cyclesPassed = 0;

    void Start()
    {
        lastStarterIndex = PlayerPrefs.GetInt("LastStarterBackground", -1);

        // Picks random starter background
        int starterIndex;
        do
        {
            starterIndex = Random.Range(0, randomBackgroundsInbetween.Count);
        }
        while (starterIndex == lastStarterIndex && randomBackgroundsInbetween.Count > 1);

        // Save this starter index for future restarts
        PlayerPrefs.SetInt("LastStarterBackground", starterIndex);

        Sprite starterBackground = randomBackgroundsInbetween[starterIndex];

        //Sets starter background
        bgA.sprite = starterBackground;
        bgA.canvasRenderer.SetAlpha(1f);
        bgB.canvasRenderer.SetAlpha(0f);
        bgB.gameObject.SetActive(false);

        lastRandomIndex = starterIndex;

        StartCoroutine(BackgroundRoutine());
    }

    IEnumerator BackgroundRoutine()
    {
        yield return new WaitForSeconds(interval);

        while (cyclesPassed < randomCyclesBeforeEnding && !endingShown)
        {
            ChangeToRandomBackground();
            cyclesPassed++;

            yield return new WaitForSeconds(interval);
        }

        ChangeToEndingBackground();
    }

    // Randomises Backgrounds
    void ChangeToRandomBackground()
    {
        if (randomBackgroundsInbetween.Count == 0) return;

        int rand;
        do
        {
            rand = Random.Range(0, randomBackgroundsInbetween.Count);
        }
        while (rand == lastRandomIndex && randomBackgroundsInbetween.Count > 1);

        lastRandomIndex = rand;
        PlayerPrefs.SetInt("LastRandomBackground", rand);

        Sprite next = randomBackgroundsInbetween[rand];
        CrossFadeTo(next);
    }

    // Ending Bg
    public void TriggerEnding()
    {
        endingShown = true;
    }

    void ChangeToEndingBackground()
    {
        CrossFadeTo(endingBackground);
    }

    // Transition: Crossfade
    void CrossFadeTo(Sprite nextSprite)
    {
        if (showingA)
        {
            bgB.sprite = nextSprite;
            StartCoroutine(CrossFade(bgA, bgB));
        }
        else
        {
            bgA.sprite = nextSprite;
            StartCoroutine(CrossFade(bgB, bgA));
        }

        showingA = !showingA;
    }

    IEnumerator CrossFade(Image from, Image to)
    {
        float t = 0f;

        to.canvasRenderer.SetAlpha(0f);
        to.gameObject.SetActive(true);

        while (t < transitionTime)
        {
            t += Time.deltaTime;
            float a = t / transitionTime;

            from.canvasRenderer.SetAlpha(1f - a);
            to.canvasRenderer.SetAlpha(a);

            yield return null;
        }

        from.gameObject.SetActive(false);
    }
}