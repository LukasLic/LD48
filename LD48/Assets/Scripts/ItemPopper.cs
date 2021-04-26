using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPopper : MonoBehaviour
{
    [HideInInspector]
    public int maxLife;

    public int amount;
    private int remaining;

    private int popAfterTaken;
    private int previousRemaining;

    public GameObject prefabToPop;

    public void Init(int maxLife)
    {
        if(amount > 0)
        {
            popAfterTaken = maxLife / amount;
            remaining = amount;
        }
    }

    public void Pop(int count, Vector3 position, Direction direction)
    {
        if(prefabToPop == null)
        {
            return;
        }

        var takenDamage = count + previousRemaining;
        var toPop = takenDamage / popAfterTaken; // How much to pop

        previousRemaining = toPop % popAfterTaken;

        // Dont pop more then remaining
        if(toPop >remaining)
        {
            toPop = 0;
            remaining = 0;
        }
        else
        {
            remaining -= toPop;
        }

        // TODO: direction

        StartCoroutine(SpawnGemInLoop(0.1f, prefabToPop, position, DirectionToVector(direction), toPop));
    }

    public void PopAllRemaining(Vector3 position, Direction direction)
    {
        if (prefabToPop == null)
        {
            return;
        }

        // TODO: direction

        StartCoroutine(SpawnGemInLoop(0.1f, prefabToPop, position, DirectionToVector(direction), remaining));
        remaining = 0;
    }

    IEnumerator SpawnGemInLoop(float interval, GameObject prefab, Vector3 position ,Vector2 popForce, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var gem = Instantiate(prefab, position, Quaternion.identity).GetComponent<Gem>();

            if(gem != null)
            {
                gem.Init(popForce, InvetoryController.Instance);
            }            

            yield return new WaitForSeconds(interval);
        }
    }

    private Vector2 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return new Vector2(-.5f, .75f);
            case Direction.Right:
                return new Vector2(.5f, .75f);
            case Direction.Top:
                return new Vector2(-.5f + 1f * Random.value, 1f);
            case Direction.Down:
                return new Vector2(-.5f + 1f * Random.value, -1f);
            default:
                return Vector2.zero;
        }
    }
}
