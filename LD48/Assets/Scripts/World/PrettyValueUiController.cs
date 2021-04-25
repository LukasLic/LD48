using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrettyValueUiController : MonoBehaviour
{
    public Animation anim;
    public AnimationClip increment;
    public AnimationClip decrement;

    public Text text;

    private int value = 0;
    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            var previousValue = this.value;

            this.value = value;
            text.text = value.ToString();

            if(anim!= null)
            {
                if (value > previousValue && increment != null)
                {
                    anim.clip = increment;
                    anim.Play(PlayMode.StopAll);
                }
                else if (value < previousValue && decrement != null)
                {
                    anim.clip = decrement;
                    anim.Play(PlayMode.StopAll);
                }
            }
        }
    }


}
