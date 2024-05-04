using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Buff_MA_1 : Buff
{
    public Art art;

    public void Init()
    {
        名字 = "虚弱";
        描述 = "攻击力下降50%";
        持续回合 = 1;
        类型 = 0;
        isDel = true;
        isControls = false;
        if (art != null)
        {
            art.当前攻击力 = (art.当前攻击力 * 0.5f);
        }
    }

    public override void OnBuff()
    {
        //art.攻击力影响 -= index;
        //var P = art as Player;
        //P.状态掉血(index);
    }

    public void OnDestroy()
    {
        if (art != null)
        {
            art.当前攻击力 = art.当前攻击力 / 0.5f;
        }
    }
}
