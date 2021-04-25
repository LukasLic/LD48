using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickOnClick : MonoBehaviour
{
    public event Action<PickOnClick> OnClick;

    private void OnMouseDown()
    {
        //rend.material
        OnClick?.Invoke(this);
    }
}
