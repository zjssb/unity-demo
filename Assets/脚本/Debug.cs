using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug : MonoBehaviour
{
    public static Debug bug { get; private set; }

    private void Awake()
    {
        bug = this;
    }

    private void Start()
    {
        
        
    }


}
