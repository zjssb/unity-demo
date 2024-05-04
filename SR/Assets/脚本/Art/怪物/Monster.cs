using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 怪物的父类
/// </summary>
public abstract class Monster : Art
{
    /// <summary>
    /// 行动AI(逻辑)
    /// </summary>
    public Action_logic action;

}
