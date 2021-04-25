﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public struct Link_GemType_PrettyValueUiController
{
    public GemType gemType;
    public PrettyValueUiController uiController;
}

public class InvetoryController : GenericSingleton<InvetoryController>
{
    public Link_GemType_PrettyValueUiController[] collectedGems;

    public override void Awake()
    {
        foreach (var gem in collectedGems)
        {
            gem.uiController.Value = 0;
        }

        base.Awake();
    }

    public void Collect(Gem gem)
    {
        try
        {
            var link = collectedGems.First(a => a.gemType == gem.type);
            link.uiController.Value += gem.value;
        }
        catch
        {
            Debug.LogError($"Dictionary does not contain gem type {gem.type}!");
        }
    }

    public void Pay(GemType gemType, int amount)
    {
        try
        {
            var link = collectedGems.First(a => a.gemType == gemType);
            link.uiController.Value -= amount;

            if(link.uiController.Value < 0)
            {
                Debug.LogError($"Payed more gems that are avaliable!");
                link.uiController.Value = 0;
            }
        }
        catch
        {
            Debug.LogError($"Dictionary does not contain gem type {gemType}!");
        }
    }

    public int GetValue(GemType gemType)
    {
        try
        {
            var link = collectedGems.First(a => a.gemType == gemType);
            return link.uiController.Value;
        }
        catch
        {
            Debug.LogError($"Dictionary does not contain gem type {gemType}!");
            return 0;
        }
    }
}