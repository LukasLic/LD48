using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public AnimationClip runningClip;
    public AnimationClip miningClip;
    public AnimationClip jumpingClip;
    public AnimationClip idleClip;

    public Animation animationComponent;

    public Transform graphics;

    private float xDir = 1;
    private float speed = 0;
    private bool mining = false;
    private bool isGrounded = false;

    private void Awake()
    {
        animationComponent.AddClip(idleClip, idleClip.name);
        animationComponent.AddClip(miningClip, miningClip.name);
        animationComponent.AddClip(jumpingClip, jumpingClip.name);
        animationComponent.AddClip(runningClip, runningClip.name);

        animationComponent.Play(idleClip.name, PlayMode.StopAll);
    }

    public void SetXDirection(float x)
    {
        xDir = x;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetGround(bool state)
    {
        isGrounded = state;
    }

    public void SetMining(bool state)
    {
        mining = state;

        if(state)
        {
            animationComponent.Play(miningClip.name, PlayMode.StopAll);
        }
    }

    private void Update()
    {
        // Rotate
        if(speed > 0f)
        {
            graphics.LookAt(transform.position + xDir * Vector3.left);
        }

        if (!mining)
        {
            if(speed > 0f)
            {
                animationComponent.Play(runningClip.name/*, PlayMode.StopAll*/);
            }
            else
            {
                animationComponent.Play(idleClip.name, PlayMode.StopAll);
            }
        }
    }
}
