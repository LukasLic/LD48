using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrettyValueUiController : MonoBehaviour
{
    public Animation anim;
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
            this.value = value;
            text.text = value.ToString();

            if (anim != null)
            {
                if(value <0)
                {
                    anim.Play(PlayMode.StopAll);
                }
                else
                {
                    // TODO: Play different animation and sound
                }
            }
        }
    }


}
