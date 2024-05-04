using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数值类型的包装
/// </summary>
public class AD
{
    /// <summary>
    /// 数值
    /// </summary>
    public float index;

    /// <summary>
    /// 伤害的类型集合
    /// </summary>
    public HashSet<Type> types;

    public enum Type
    {
        普通攻击,
        战技,
        大招,
        追加攻击,
        回复生命,
        回复护盾,
        回复能量
    }
}
