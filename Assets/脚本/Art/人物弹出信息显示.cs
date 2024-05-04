using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;

public class 人物弹出信息显示 : MonoBehaviour
{
    /// <summary>
    /// 从asset文件夹中，通过路径获取的支持中文的 黑体 字体
    /// </summary>
    public static TMP_FontAsset FontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
        "Assets/素材/Font/SIMHEISDF.asset"
    );

    public static GameObject parent = GameObject.Find("弹出文字");

    /// <summary>
    /// 每个信息的排序顺序，自增1 ，大于1000重置
    /// </summary>
    static int orderInLayer = 1;

    /// <summary>
    /// 创建人物浮动文本弹出
    /// </summary>
    /// <param name="transform">文本位置</param>
    /// <param name="text">文本信息</param>
    /// <param name="color">文本颜色</param>
    /// <param name="time">显示时间</param>
    public static void ArtTextPop(
        Transform transform,
        string text,
        Color color,
        float fontSize = 36,
        float time = 0.5f
    )
    {
        // 实例化一个游戏对象，用于搭建TextMeshPro组件
        var T = new GameObject("Text");
        // 父对象设置
        T.transform.parent = parent.transform;
        // 坐标设置
        T.transform.localPosition = transform.position;
        // 在父对象的坐标基础上，将z轴前移 0.5，之后在 x,y 轴以0.2为范围随机分布
        T.transform.position += new Vector3(
            Random.Range(-0.2f, 0.2f),
            Random.Range(-0.2f, 0.2f),
            -0.6f
        );
        // 设置缩放
        T.transform.localScale = new(0.1f, 0.1f, 0.1f);
        // 添加 TMP组件
        var TMP = T.AddComponent<TextMeshPro>();
        // 设置文字、颜色
        TMP.SetText(text);
        TMP.color = color;
        // 字体大小（默认36）
        TMP.fontSize = fontSize;
        // 文字对其方式，上下左右居中
        TMP.alignment = TextAlignmentOptions.Center;
        TMP.alignment = TextAlignmentOptions.Midline;
        // 设置排序
        TMP.sortingOrder = orderInLayer;
        orderInLayer++;
        if (orderInLayer >= 1000)
        {
            orderInLayer = 1;
        }
        // 动画由dotween完成
        TMP.transform.DOLocalMoveY(1f, time).OnComplete(() => Destroy(T));

    }
}
