using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidShowOnly : MonoBehaviour
{
#if !UNITY_ANDROID
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif
}
