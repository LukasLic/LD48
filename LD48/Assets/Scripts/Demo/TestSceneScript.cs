using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneScript : MonoBehaviour
{
    public int EmeraldCount;
    public GameObject EmeraldPrefab;

    private bool playing = false;
    private int currentEmeraldCount = 0;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playing && Input.GetKeyDown(KeyCode.Space))
        {
            playing = true;
            GameController.Instance.StartNewGame();
            PrepareGame();
        }
    }

    private void PrepareGame()
    {
        currentEmeraldCount = 0;

        var parent = GameController.Instance.ObjectsRoots[0];

        // Spawn emeralds from -1 to +1
        currentEmeraldCount = EmeraldCount;
        var step = 2f / EmeraldCount;

        for (int i = 0; i < EmeraldCount; i++)
        {
            var position = new Vector3(0f, 0.5f, -1f + step * i);

            var pickOnClick = Instantiate(EmeraldPrefab, position, Quaternion.identity, parent).GetComponent<PickOnClick>();
            pickOnClick.OnClick += PickOnClick_OnClick;
        }

    }

    private void PickOnClick_OnClick(PickOnClick pickOnClick)
    {
        audioSource.PlayOneShot(audioSource.clip);

        currentEmeraldCount--;

        pickOnClick.OnClick -= PickOnClick_OnClick;
        Destroy(pickOnClick.gameObject);

        if (currentEmeraldCount == 0)
        {
            playing = false;
            GameController.Instance.EndGame();
        }
    }
}
