using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class 战技_A : Ex
{
    private void Start()
    {
        名字 = "这是A的战技";
        描述 = "这是A的战技描述";
        isOn = true;
        耗能 = 20;
        消耗战技点 = 1;
        isAtt = true;
    }

    public override List<Art> GetGoal()
    {
        var arts = new List<Art>();
        var index = GM.gm.monsterList.IndexOf(GM.gm.attArt);
        arts.Add(GM.gm.attArt);
        if (index != 0)
        {
            arts.Add(GM.gm.monsterList[index - 1]);
        }
        if (index + 1 != GM.gm.monsterList.Count)
        {
            arts.Add(GM.gm.monsterList[index + 1]);
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
                    
                    print("A的战技");
                    GM.gm.战技点改变(-消耗战技点);
                    var list = this.GetGoal();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] == GM.gm.attArt)
                        {
                            player.攻击(player.当前攻击力 + 8, list[i]);
                        }
                        else
                        {
                            player.攻击(player.当前攻击力 + 5, list[i]);
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
            print("A：战技点不够，无法释放战技");
            GM.gm.wait = true;
        }
    }
}
