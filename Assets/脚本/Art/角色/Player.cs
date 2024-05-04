using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有角色的父类
/// </summary>
public abstract class Player : Art
{
    public Ex 普通攻击;

    public Ex 战技;

    public Ex 大招;

    public abstract void 改变能量(float sp_index);
}
