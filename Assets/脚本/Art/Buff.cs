using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    /// <summary>
    /// 当为-1时，为持续性buff，不与回合数有关
    /// </summary>
    public int 持续回合;

    /// <summary>
    /// buff的类型，增益(1)、负面(0)、特殊(-1)
    /// </summary>
    public int 类型;

    /// <summary>
    /// 是否能够移除
    /// </summary>
    public bool isDel;

    public string 名字;

    public string 描述;

    /// <summary>
    /// 是否为控制类 buff 
    /// ( 角色不能进行操作 )
    /// </summary>
    public bool isControls;

    /// <summary>
    /// 在回合开始时，buff生效
    /// </summary>
    public abstract void OnBuff();


}
