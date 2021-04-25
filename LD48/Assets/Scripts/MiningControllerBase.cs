﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiningControllerBase : MonoBehaviour
{
    public abstract bool IsDiggedOut { get; set; }

    public abstract void Init(TilesManager tilesManager, (int x, int y) coordinates);

    public abstract void Mine(Collider2D collider);

    public abstract void UpdateState();

    public abstract void DigInto();
}
