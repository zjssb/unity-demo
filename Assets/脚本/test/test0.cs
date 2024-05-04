using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class test0 : MonoBehaviour
{
    public bool f = false;

    public int t;

    private void Start()
    {
        t = 9;
        StartCoroutine(test());
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(1f);
        print(t);
    }
    private void Update() { }
}
