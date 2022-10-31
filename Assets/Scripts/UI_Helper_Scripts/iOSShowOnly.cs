using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOSShowOnly : MonoBehaviour
{
#if !UNITY_IOS
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif
}
