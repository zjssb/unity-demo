using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class 大招_A : Ex
{
    private void Start()
    {
        名字 = "这是A的大招";
        描述 = "这是A的大招描述";
        isOn = true;
        耗能 = -100;
        isAtt = true;
    }

    public override List<Art> GetGoal()
    {
        var arts = new List<Art> { GM.gm.attArt };
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
                player.攻击(player.当前攻击力 * 2, GM.gm.attArt);
                Destroy(Sphere);
                // 执行完成后，需要将wait赋值为true
                GM.gm.wait = true;
            });
        UGUI.gui.UIChange(0);
    }
}
