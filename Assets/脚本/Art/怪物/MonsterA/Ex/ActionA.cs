using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 怪物A 的 行动逻辑
/// </summary>
public class ActionA : Action_logic
{
    private void Start()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    public override void Release()
    {
        Art art;
        art = GM.gm.playerList[Random.Range(0, GM.gm.playerList.Count)];

        var Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Sphere.transform.position = gameObject.transform.position;
        Sphere.transform.SetParent(gameObject.transform);
        Sphere.transform.localScale = Sphere.transform.localScale / 2;
        Sphere
            .transform.DOJump(art.transform.position, 2f, 1, 1f)
            .OnComplete(() =>
            {
                print(monster.名字 + " 的攻击");
                if(art != null)
                    monster.攻击(monster.当前攻击力, art);
                var index = new GameObject();
                var buff = index.AddComponent<Buff_MA_1>();
                buff.Init();
                // 100%的命中概率
                if (Random.Range(0, 100) < 100 && art.AddBuff(buff))
                {
                    if(art!=null)
                        art.buffs.AddComponent<Buff_MA_1>().art = art;
        
                    print(art.名字 + " 被施加 " + "虚弱");
                }
                Destroy(Sphere);
                Destroy(index);
                GM.gm.wait = true;
                GM.gm.over = true;
            });
    }
}
