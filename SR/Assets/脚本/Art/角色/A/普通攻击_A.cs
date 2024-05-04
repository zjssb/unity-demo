using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class 普通攻击_A : Ex
{
    private void Start()
    {
        名字 = "这是A的普通攻击";
        描述 = "这是A的普通攻击描述";
        isOn = true;
        耗能 = 10;
        isAtt = true;
    }

    public override List<Art> GetGoal()
    {
        var arts = new List<Art>() { GM.gm.attArt };
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
                print("A的普通攻击");
                player.攻击(player.当前攻击力, GM.gm.attArt);
                GM.gm.战技点改变(1);
                GM.gm.over = true;
                GM.gm.wait = true;
                Destroy(Sphere);
            });
        player.改变能量(耗能);
    }
}
