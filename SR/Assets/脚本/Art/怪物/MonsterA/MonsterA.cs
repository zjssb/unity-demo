using System;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 怪物A 的 脚本
/// </summary>
public class MonsterA : Monster
{
    /// <summary>
    /// 通用委托,无参无返回值
    /// </summary>
    public delegate void fun();

    public event fun 护盾增加事件;

    public event fun 血量增加事件;

    public event fun 受击事件;

    public event fun 状态掉血事件;

    public event fun 攻击事件;

    public event fun 死亡事件;

    private void Start()
    {
        buffs = transform.Find("buff").gameObject;

        action = transform.Find("Action").GetComponent<Action_logic>();

        assets = transform.Find("body").gameObject;
    }

    public override void Turn_end()
    {
        buffList = buffs.GetComponents<Buff>().ToList();
        foreach (Buff buff in buffList)
        {
            if (buff.持续回合 == -1)
            {
                continue;
            }
            else
            {
                buff.持续回合 -= 1;
                if (buff.持续回合 == 0)
                {
                    Destroy(buff);
                }
            }
        }
    }

    public override void Turn_start()
    {
        buffList = buffs.GetComponents<Buff>().ToList();
        foreach (Buff buff in buffList)
        {
            buff.OnBuff();
        }
    }

    public override void 加盾(float dun_index)
    {
        if (dun_index >= 0)
        {
            var index = dun_index + 护盾;
            if (index >= 护盾最大值)
            {
                护盾 = 护盾最大值;
            }
            else
            {
                护盾 = (int)index;
            }
            护盾增加事件?.Invoke();
        }
    }

    public override void 加血(float hp_index)
    {
        if (hp_index >= 0)
        {
            var index = hp_index + 血量;
            if (index >= 血量最大值)
            {
                血量 = 血量最大值;
            }
            else
            {
                血量 = (int)index;
            }
            血量增加事件?.Invoke();
        }
        UGUI.gui.MonstaerHPChange(this);
    }

    public override void 受击(float sou_index)
    {
        if (sou_index >= 0)
        {
            var index = sou_index - 当前防御力;
            index = Mathf.Max((int)index, 1);
            if (index <= 护盾)
            {
                护盾 -= index;
                人物弹出信息显示.ArtTextPop(transform, index.ToString(), Color.yellow);
                print(名字 + " 的护盾格挡了" + index + "的伤害");
            }
            else
            {
                if (护盾 > 0)
                {
                    index -= 护盾;
                    人物弹出信息显示.ArtTextPop(transform, index.ToString(), Color.yellow);
                    print(名字 + " 的护盾格挡了" + 护盾 + "的伤害");
                }
                护盾 = 0;
                血量 -= index;

                人物弹出信息显示.ArtTextPop(transform, index.ToString(), Color.red);
                print(名字 + " 受到" + index + "伤害");
            }
            
            if (血量 <= 0)
            {
                血量 = 0;

                死亡();
            }
            else
            {
                UGUI.gui.MonstaerHPChange(this);
                Color color = assets.GetComponent<MeshRenderer>().material.color;
                assets
                    .GetComponent<MeshRenderer>()
                    .material.DOColor(Color.clear, 0.2f)
                    .OnComplete(() =>
                    {
                        assets.GetComponent<MeshRenderer>().material.DOColor(color, 0.3f);
                    });
            }
            受击事件?.Invoke();
        }
    }

    public override void 状态掉血(float hp_index)
    {
        if (hp_index >= 0)
        {
            人物弹出信息显示.ArtTextPop(transform, hp_index.ToString(), Color.red);

            var index = 血量 - (int)hp_index;
            if (index <= 0)
            {
                血量 = 0;
                死亡();
            }
            else
            {
                UGUI.gui.MonstaerHPChange(this);
                血量 = index;
            }
            状态掉血事件?.Invoke();
        }
    }

    public override void 攻击(float att_index, Art art)
    {
        if (att_index >= 0)
        {
            var index = att_index;
            var a = art as Player;
            a.受击((int)index);
            攻击事件?.Invoke();
        }
    }

    public override bool AddBuff(Buff buff)
    {
        if (UnityEngine.Random.Range(0, 100) < 当前效果抵抗)
        {
            人物弹出信息显示.ArtTextPop(transform, "负面效果抵抗", Color.white);

            print(名字 + "效果抵抗");
            return false;
        }
        人物弹出信息显示.ArtTextPop(transform, buff.名字, Color.white);

        return true;
    }

    public override void 死亡()
    {
        GM.gm.ArtDeath(this);
        死亡事件?.Invoke();
    }
}
