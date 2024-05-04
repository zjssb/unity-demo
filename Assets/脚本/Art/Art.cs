using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物的父类
/// </summary>
public abstract class Art : MonoBehaviour
{
    public string 名字;

    public float 血量;

    public float 血量最大值;


    /// <summary>
    /// 一般情况下，护盾最大值为血量最大值
    /// </summary>
    public float 护盾;

    public float 护盾最大值;

    public int 能量;

    public int 能量最大值;

    public float 当前攻击力;

    public float 原始攻击力;

    public float 当前防御力;

    public float 原始防御力;

    public float 当前速度;

    public float 原始速度;

    /// <summary>
    /// 暴击率,单位为 %
    /// </summary>
    public float 当前暴击;

    public float 原始暴击;

    /// <summary>
    /// 暴击伤害,单位为 %
    /// </summary>
    public float 当前爆伤;

    public float 原始爆伤;

    /// <summary>
    /// 收到影响的效果命中概率 单位为 %
    /// </summary>
    public float 当前命中效率;

    public float 原始命中效率;

    /// <summary>
    /// 对收到的负面影响的抵抗概率 单位为 %
    /// </summary>
    public float 当前效果抵抗;

    public float 原始效果抵抗;

    /// <summary>
    /// buff 列表
    /// </summary>
    public List<Buff> buffList;

    /// <summary>
    /// buff 附加的对象
    /// </summary>
    public GameObject buffs;

    /// <summary>
    /// 人物资源管理
    /// </summary>
    public GameObject assets;

    /// <summary>
    /// 回合结束执行方法
    /// </summary>
    public abstract void Turn_end() ;

    /// <summary>
    /// 回合开始执行方法
    /// </summary>
    public abstract void Turn_start();

    /// <summary>
    /// 角色添加 Buff 方法
    /// </summary>
    /// <param name="buff">需要添加的 Buff 组件</param>
    /// <returns>返回角色是否抵抗成功</returns>
    public abstract bool AddBuff(Buff buff);

    public abstract void 攻击(float att_index, Art art);

    public abstract void 受击(float sou_index);

    public abstract void 状态掉血(float hp_index);

    public abstract void 加血(float hp_index);

    public abstract void 加盾(float dun_index);

    public abstract void 死亡();
}
