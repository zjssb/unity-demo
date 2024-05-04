using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ex : MonoBehaviour
{
    public string 名字;

    /// <summary>
    /// 是否手动控制
    /// </summary>
    public bool isOn;

    public string 描述;

    /// <summary>
    /// 负数为消耗能量 正数为增加能量
    /// </summary>
    public int 耗能 = 0;

    /// <summary>
    /// 释放战技消耗的战技点 （正数）
    /// </summary>
    public int 消耗战技点 = 0;

    /// <summary>
    /// 是否为攻击技能，即指向为我方还是地方
    /// </summary>
    public bool isAtt;

    public abstract void OnEx();

    public abstract List<Art> GetGoal();

    public void InsertEx()
    {
        var player = this.transform.parent.GetComponent<Player>();
        if (player.能量 + 耗能 >= 0)
        {
            player.改变能量(耗能);
            GM.gm.大招准备(OnEx, player, this);
        }
        else
        {
            print("能量不够，无法释放大招");
        }
    }
}
