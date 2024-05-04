using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{
    /// <summary>
    /// UI 中人物头像的父节点
    /// </summary>
    public GameObject head;

    /// <summary>
    /// 人物头像列表
    /// </summary>
    public List<GameObject> headList = new List<GameObject>();

    public List<顺序对象> 行动顺序 = new List<顺序对象>();

    private void Start()
    {
        foreach (var s in GM.gm.playerList)
        {
            行动顺序.Add(new(s));
        }
        foreach (var s in GM.gm.monsterList)
        {
            行动顺序.Add(new(s));
        }

        // 初始化左侧头像
        foreach (var s in 行动顺序)
        {
            s.head0 = Instantiate(head.transform.GetChild(0).gameObject, head.transform);
            s.head0.name = s.art.名字;

            s.head0.GetComponent<Image>().sprite = s.head_Sprite;
            if (headList.Count != 0)
            {
                s.head0.GetComponent<RectTransform>().anchoredPosition =
                    headList[headList.Count - 1].GetComponent<RectTransform>().anchoredPosition
                    - new Vector2(
                        0,
                        headList[headList.Count - 1].GetComponent<RectTransform>().rect.height
                    );
            }
            else
            {
                s.head0.transform.position = head.transform.GetChild(0).transform.position;
            }
            headList.Add(s.head0);
        }
        head.transform.GetChild(0).gameObject.SetActive(false);
        sort();
        //GM.gm.next();
    }

    public void sort()
    {
        var before = new List<顺序对象>(行动顺序);
        // 数据层面排序
        行动顺序.Sort(
            (顺序对象 s1, 顺序对象 s2) =>
            {
                    return s1.time.CompareTo(s2.time);
            }
        );
        行动改变(before);
    }

    /// <summary>
    /// 有人物行动顺序的改变时，执行的动画
    /// </summary>
    /// <param name="before_list">改变前的顺序</param>
    public void 行动改变(List<顺序对象> before_list)
    {
        for (int i = 0; i < 行动顺序.Count; i++)
        {
            //int t = headList.IndexOf(行动顺序[i].head0);
            行动顺序[i].head0.transform.DOMove(before_list[i].head0.transform.position, 0.1f);
        }
    }

    public void RemoveHead(Art art)
    {
        for (int i = 0; i < 行动顺序.Count; i++)
        {
            if (行动顺序[i].art == art)
            {
                headList.Remove(行动顺序[i].head0);
                for (int j = 行动顺序.Count - 1; j > i; j--)
                {
                    行动顺序[j].head0.transform.position = 行动顺序[j - 1].head0.transform.position;
                }
                Destroy(行动顺序[i].head0);
                行动顺序.RemoveAt(i);
                break;
            }
        }
    }

    public Art Pop()
    {
        sort();
        var time = 行动顺序[0].time;
        var first = 行动顺序[0].art;
        foreach (var s in 行动顺序)
        {
            s.time -= time;
        }
        行动顺序[0].Restart();
        return first;
    }

    public class 顺序对象
    {
        public Art art;
        public float time;
        public Sprite head_Sprite;
        public GameObject head0;

        public 顺序对象(Art art)
        {
            this.art = art;
            time = 10000f / (float)art.当前速度;
            this.head_Sprite = art.transform.Find("body").GetComponent<Assets>().HeadImage;
        }

        public void Restart()
        {
            time = 10000f / (float)art.当前速度;
        }
    }
}
