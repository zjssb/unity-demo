using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class 大招_B : Ex
{
    private void Start()
    {
        名字 = "这是B的大招";
        描述 = "对我方全体回复 攻击力 100% 的生命";
        isOn = true;
        耗能 = -100;
        isAtt = false;
    }

    /// <summary>
    /// 我方全体
    /// </summary>
    /// <returns></returns>
    public override List<Art> GetGoal()
    {
        var arts = new List<Art>(GM.gm.playerList);
        arts.Remove(GM.gm.attArt);
        arts.Insert(0, GM.gm.attArt);

        return arts;
    }

    public override void OnEx()
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
                print("A的大招");
                var list = this.GetGoal();
                foreach (var item in list)
                {
                    if (item is Player)
                    {
                        (item as Player).加血(player.当前攻击力);
                    }
                }

                Destroy(Sphere);
                // 执行完成后，需要将wait赋值为true
                GM.gm.wait = true;
            });
        UGUI.gui.UIChange(0);
    }
}
