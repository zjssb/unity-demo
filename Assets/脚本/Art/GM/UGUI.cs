using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UGUI : MonoBehaviour
{
    public static UGUI gui { get; private set; }

    private void Awake()
    {
        gui = this;
    }

    public List<GameObject> 角色UI列表;

    public Dictionary<Player, GameObject> 角色UI字典 = new();

    public GameObject UP_HP_Image;

    public GameObject 普通攻击按钮;

    public GameObject 战技按钮;

    public GameObject 大招按钮;

    public TextMeshProUGUI 战技点UI;

    private void OnGUI()
    {

        // 将人物 ui 面向主摄像机
        foreach (var monster in GM.gm.monsterList)
        {
            var canvas = monster.transform.Find("body").Find("ArtCanvas").gameObject;
            var count = canvas.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var ui = canvas.transform.GetChild(i).gameObject;
                ui.transform.rotation = GM.gm.mainCamere.transform.rotation;
            }
        }
    }

    /// <summary>
    /// 游戏开始，UI 设置
    /// </summary>
    public void UIStart()
    {
        var index = GM.gm.playerList.Count;
        for (int i = 0; i < 4; i++)
        {
           
            if (i < index)
            {
                var player = GM.gm.playerList[i] as Player;
                // 开启大招UI显示
                if (角色UI列表[i].activeSelf)
                {
                    角色UI列表[i].SetActive(true);
                    角色UI字典.Add(player, 角色UI列表[i]);
                    print(角色UI列表[i]);
                    角色UI列表[i]
                        .transform.Find("大招")
                        .GetComponent<Button>()
                        .onClick.AddListener(player.大招.InsertEx);
                    PlayerImageChange(player);
                }
            }
            else
            {
                // 关闭大招UI显示
                if (角色UI列表[i].activeSelf)
                {
                    角色UI列表[i].SetActive(false);
                }
            }
        }

        // 战技点设置
        战技点UI.text = GM.gm.战技点 + "/" + GM.gm.最大战技点;
    }

    /// <summary>
    /// 根据行动状况 切换ui
    /// </summary>
    /// <param name="value">1为 普通行动阶段， 2为 大招行动阶段，0为 不是行动阶段或正在行动阶段</param>
    public void UIChange(int value)
    {
        if (value == 1)
        {
            普通攻击按钮.SetActive(true);
            if (GM.gm.Check == 1)
            {
                普通攻击按钮.transform.Find("Check").gameObject.SetActive(true);
                战技按钮.transform.Find("Check").gameObject.SetActive(false);
            }
            else if(GM.gm.Check == 2)
            {
                战技按钮.transform.Find("Check").gameObject.SetActive(true);
                普通攻击按钮.transform.Find("Check").gameObject.SetActive(false);
            }
            战技按钮.SetActive(true);


            大招按钮.SetActive(false);
        }
        if (value == 2)
        {
            普通攻击按钮.SetActive(false);
            战技按钮.SetActive(false);

            大招按钮.SetActive(true);
        }
        if (value == 0)
        {
            普通攻击按钮.SetActive(false);
            战技按钮.SetActive(false);
            大招按钮.SetActive(false);
        }
    }

    /// <summary>
    /// 目标身上的目标标志显示
    /// </summary>
    /// <param name="Arts">现在的目标对象列表</param>
    public void GoalUI(List<Art> Arts)
    {
        // 将上一次的标志 取消
        foreach (var art in GM.gm.monsterList)
        {
            if (art == null)
                continue;
            art.transform.Find("body").Find("ArtCanvas").Find("目标标志").gameObject.SetActive(false);
        }
        foreach (var art in GM.gm.playerList)
        {
            if (art == null)
                continue;
            art.transform.Find("body").Find("ArtCanvas").Find("目标标志").gameObject.SetActive(false);
        }
        if (Arts == null)
        {
            return;
        }
        // 将现在的 标志 激活
        foreach (var art in Arts)
        {
            if (art == null)
                continue;
            art.transform.Find("body").Find("ArtCanvas").Find("目标标志").gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 屏幕顶部血条变化
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpHPImage()
    {
        while (true)
        {
            if (GM.gm.attArt == null)
            {
                yield return new WaitUntil(() => GM.gm.attArt == null);
            }
            float index = (float)GM.gm.attArt.血量 / (float)GM.gm.attArt.血量最大值;
            if (index <= 0)
                UP_HP_Image.GetComponent<Image>().DOFillAmount(index, 0.1f);
            else
                UP_HP_Image.GetComponent<Image>().DOFillAmount(index, 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 角色ui图片的变化函数 血条、能量条
    /// </summary>
    /// <param name="player">改变的角色</param>
    public void PlayerImageChange(Player player)
    {
        GameObject gui;
        gui = 角色UI字典[player];
        gui.transform.Find("名字").GetComponent<Text>().text = player.名字;

        if (player == null)
        {
            print(123);
            return;
        }
        float hp = (float)player.血量 / (float)player.血量最大值;
        gui.transform.Find("血条").GetComponent<Image>().DOFillAmount(hp, 0.2f);
        float sp = (float)player.能量 / (float)player.能量最大值;
        gui.transform.Find("大招").GetComponent<Image>().DOFillAmount(sp, 0.2f);
    }

    /// <summary>
    /// 敌人 ui 图片的变化函数
    /// </summary>
    /// <param name="monster">改变的敌人</param>
    public void MonstaerHPChange(Monster monster)
    {
        var value = monster.血量 / monster.血量最大值;
        var HP = monster.transform.Find("body").Find("ArtCanvas").Find("血条");
        if (value <= 0)
        {
            HP.GetComponent<Image>().DOFillAmount(value, 0.1f);
        }
        else
        {
            HP.GetComponent<Image>().DOFillAmount(value, 0.2f);
        }
    }


    public void PlayerDeathUIChange(Player player)
    {
        var index = GM.gm.playerList.IndexOf(player);
        角色UI列表[index].SetActive(false);
    }
}
