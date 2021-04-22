﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : GenericSingleton<GameController>
{
    public Vector3 WorldCenter = Vector3.zero;
    public Transform[] ObjectsRoots;

    public RawImage OverlayScreen;

    public void EndGame()
    {
        StopGame();

        StopAllCoroutines();
        StartCoroutine(FadeIn(OverlayScreen.material));
    }

    public override void Awake()
    {
        base.Awake();

        StopGame();

        var c = OverlayScreen.material.color;
        c.a = 1f;
        OverlayScreen.material.color = c;
    }

    private void StopGame()
    {
        CleanWorld();
    }

    public void StartNewGame()
    {
        CleanWorld();

        // Clean victory/gameover UI
        StopAllCoroutines();
        StartCoroutine(FadeOut(OverlayScreen.material));
    }

    private void CleanWorld()
    {
        for (int i = 0; i < ObjectsRoots.Length; i++)
        {
            for (int j = 0; j < ObjectsRoots[i].childCount; j++)
            {
                Destroy(ObjectsRoots[i].GetChild(j).gameObject);
            }
        }
    }

    IEnumerator FadeIn(Material m)
    {
        var ic = m.color;
        ic.a = 0f;
        m.color = ic;

        var stepDelta = 0.25f;
        var frequency = 24f;

        var alpha = 0f;
        while (alpha < 1f)
        {
            alpha += stepDelta;

            Color c = m.color;
            c.a = Mathf.Min(alpha, 1f);
            m.color = c;

            yield return new WaitForSeconds(1f / frequency);
        }

        var lc = m.color;
        lc.a = 1f;
        m.color = lc;
    }

    IEnumerator FadeOut(Material m)
    {
        var ic = m.color;
        ic.a = 1f;
        m.color = ic;

        var stepDelta = 0.25f;
        var frequency = 24f;

        var alpha = 1f;
        while(alpha > 0f)
        {
            alpha -= stepDelta;

            Color c = m.color;
            c.a = Mathf.Max(alpha, 0f);
            m.color = c;

            yield return new WaitForSeconds(1f/frequency);
        }

        var lc = m.color;
        lc.a = 0f;
        m.color = lc;
    }
}
