using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour 
{
    public float transformScaleSpeed;

    void Start()
    {
        Destroy(this.gameObject, 4.25f);
    }

    void Update()
    {
        transform.localScale += Vector3.one * transformScaleSpeed * Time.deltaTime;
    }
}
