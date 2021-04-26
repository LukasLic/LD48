using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[Serializable]
public struct ConversionLink
{
    public GemType gemType;
    public Sprite sprite;
}

public class GemTypeToSpriteConversion : MonoBehaviour
{
    public ConversionLink[] conversionList;

    public void SetImage(GemType gemType, Image image)
    {
        //try
        //{
        //    image.sprite = conversionList.First(l => l.gemType == gemType).sprite;
        //}
        //catch
        //{
        //    image.sprite = null;
        //    Debug.LogError($"Gem type {gemType} does not have sprite conversion!");
        //}
    }
}
