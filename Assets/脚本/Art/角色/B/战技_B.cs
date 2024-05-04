using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class 战技_B : Ex
{
    private void Start()
    {
        名字 = "这是B的战技";
        描述 = "向我方单体回复 50% 攻击力的生命";
        isOn = true;
        耗能 = 20;
        消耗战技点 = 1;
        isAtt = false;
    }

    /// <summary>
    /// 单体
    /// </summary>
    /// <returns>单体</returns>
    public override List<Art> GetGoal()
    {
        var arts = new List<Art>();
        if (GM.gm.attArt is not Player)
        {
            arts.Add(GM.gm.playerList[0]);
        }
        else
        {
            arts.Add(GM.gm.attArt);
        }
        return arts;
    }

    public override void OnEx()
    {
        int index = GM.gm.战技点 - 消耗战技点;
        if (index >= 0)
        {
            var player = this.transform.parent.GetComponent<Player>();
            var Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Sphere.transform.position = gameObject.transform.position;
            Sphere.transform.SetParent(gameObject.transform);
            Sphere.transform.localScale = Sphere.transform.localScale / 2;
            Sphere
                .transform.DOJump(GM.gm.attArt.transform.position, 2f, 1, 1f)
                .OnComplete(() =>
                {
                    print("B的战技");
                    GM.gm.战技点改变(-消耗战技点);
                    var list = this.GetGoal(); // 指向对象集合

                    foreach (var item in list)
                    {
                        if (item is Player)
                        {
                            (item as Player).加血(player.当前攻击力 * 0.5f);
                        }
                        else // 测试用
                        {
                            (item as Monster).加血(player.当前攻击力 * 0.5f);
                        }
                    }

                    GM.gm.over = true;
                    GM.gm.wait = true;
                    Destroy(Sphere);
                });
            player.改变能量(耗能);
        }
        else
        {
            print("B：战技点不够，无法释放战技");
            GM.gm.wait = true;
        }
    }
}
