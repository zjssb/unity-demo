using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行动逻辑的父类
/// </summary>
public class Action_logic : MonoBehaviour
{
    public Monster monster;

    public virtual void Release() { }
}
