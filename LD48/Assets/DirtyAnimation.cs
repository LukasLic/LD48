using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyAnimation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back, 60f * Time.deltaTime);
    }
}
