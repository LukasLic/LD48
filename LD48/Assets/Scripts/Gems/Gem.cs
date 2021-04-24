using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GemType type;
    public int value;

    public float minPopForce;
    public float maxPopForce;
    public float pickUpTime;
    private float pickUpTimer;
    public Animation pickupAnimation;

    public float rotateSpeed;
    public Rigidbody2D rb2D;

    private InvetoryController invetory;

    public void Init(Vector2 popDirection, InvetoryController invetory)
    {
        this.invetory = invetory;

        var popForce = minPopForce + Random.value * (maxPopForce - minPopForce);
        rb2D.AddForce(popForce * popDirection.normalized, ForceMode2D.Impulse);
        pickUpTimer = pickUpTime;
    }

    public void Init(Vector2 popDirection)
    {
        this.invetory = GameObject.FindGameObjectWithTag("Player").GetComponent<InvetoryController>();

        var popForce = minPopForce + Random.value * (maxPopForce - minPopForce);
        rb2D.AddForce(popForce * popDirection.normalized, ForceMode2D.Impulse);
        pickUpTimer = pickUpTime;
    }

    /// <summary>
    /// Set from animation!
    /// </summary>
    public void Despawn()
    {
        invetory.Collect(this);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        if (pickUpTimer > 0f)
        {
            pickUpTimer -= Time.deltaTime;

            if (pickUpTimer <= 0f)
            {
                pickupAnimation.Play();
            }
        }
    }
}
