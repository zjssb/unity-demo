using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 游戏管理类
/// </summary>
public class GM : MonoBehaviour
{
    public static GM gm { get; private set; }

    private void Awake()
    {
        gm = this;
    }

    [Header("摄像头设置")]
    /// <summary>
    /// 主摄像机
    /// </summary>
    public Camera mainCamere;

    /// <summary>
    /// 面向角色的摄像机，用于 指向我方的动画释放
    /// </summary>
    public Camera playerCamere;

    [Header("游戏参数")]
    /// <summary>
    /// 等待标志，人物动画是否执行完毕
    /// </summary>
    public bool wait = false;

    /// <summary>
    /// 技能释放过程标志, 大招协程正在执行的标志
    /// </summary>
    public bool isExTime = false;

    /// <summary>
    /// 当前人物行动完毕
    /// </summary>
    public bool over;

    /// <summary>
    /// 当前操作角色
    /// </summary>
    public Art actArt;

    /// <summary>
    /// 选中对象
    /// </summary>
    public Art attArt;

    public int 战技点;

    public int 最大战技点;

    /// <summary>
    /// 技能释放检查 1为普攻 2为战技 3为追加技能
    /// </summary>
    [Tooltip("技能释放检查 1为普攻 2为战技 3为追加技能")]
    public int Check = 1;

    /// <summary>
    /// 当前执行的技能
    /// </summary>
    public Ex onEx;

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<Art> playerList = new List<Art>();

    /// <summary>
    /// 敌人列表
    /// </summary>
    public List<Art> monsterList = new List<Art>();

    /// <summary>
    /// 追加攻击队列
    /// </summary>
    Queue<insert> 追加队列 = new Queue<insert>();

    /// <summary>
    /// 人物大招队列
    /// </summary>
    Queue<insert> 大招队列 = new Queue<insert>();

    /// <summary>
    /// 插入队列的结构
    /// </summary>
    struct insert
    {
        public fun f;
        public Art art;
        public Ex ex;
    }

    public delegate void fun();

    /// <summary>
    /// 右侧的行动顺序
    /// </summary>
    public Speed 顺序;

    public GameObject GameOverPanel;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D) && (wait || isExTime))
        {
            var index = monsterList.IndexOf(attArt);
            if (index >= 0) // 指向敌方
            {
                index += 1;
                if (index < monsterList.Count)
                {
                    attArt = monsterList[index];
                }
            }
            else // 指向我方
            {
                index = playerList.IndexOf(attArt);
                index -= 1;
                if (index >= 0)
                {
                    attArt = playerList[index];
                }
            }

            UIGoal();
        }

        if (Input.GetKeyUp(KeyCode.A) && (wait || isExTime))
        {
            var index = monsterList.IndexOf(attArt);
            if (index >= 0) // 指向敌方
            {
                index -= 1;
                if (index >= 0)
                {
                    attArt = monsterList[index];
                }
            }
            else // 指向我方
            {
                index = playerList.IndexOf(attArt);
                index += 1;
                if (index < playerList.Count)
                {
                    attArt = playerList[index];
                }
            }
            UIGoal();
        }
    }

    public void Init()
    {
        GameOverPanel.SetActive(false);
        // 获取角色列表
        Transform art_parent = GameObject.Find("角色队列").transform;
        for (int i = 0; i < art_parent.childCount; i++)
        {
            playerList.Add(art_parent.GetChild(i).GetComponent<Player>());
        }

        // 获取敌人列表
        art_parent = GameObject.Find("敌人队列").transform;
        for (int i = 0; i < art_parent.childCount; i++)
        {
            monsterList.Add(art_parent.GetChild(i).GetComponent<Monster>());
        }

        attArt = monsterList[0] as Monster;

        // 对角色ui进行初始化
        UGUI.gui.UIStart();

        UGUI.gui.UIChange(0);

        // 开启回合循环
        StartCoroutine(next());

        StartCoroutine(nameof(追加大招));
    }

    public void 战技点改变(int index)
    {
        战技点 = Mathf.Clamp(战技点 + index, 0, 最大战技点);
        UGUI.gui.战技点UI.text = 战技点 + "/" + 最大战技点;
    }

    /// <summary>
    /// 当前人物操作完成后，执行函数
    /// </summary>
    IEnumerator next()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UGUI.gui.UpHPImage());
        while (true)
        {
            yield return new WaitUntil(() => 追加队列.Count == 0 && 大招队列.Count == 0);
            wait = false;
            over = false;
            this.actArt = 顺序.Pop();
            // 当前行动人物 为 敌人 时
            if ((this.actArt as Monster) != null)
            {
                var M = actArt as Monster;
                M.Turn_start();
                M.action.Release();
            }
            // 当前行动人物 为 角色 时
            if ((this.actArt as Player) != null)
            {
                UGUI.gui.UIChange(1);
                wait = true;

                var P = this.actArt as Player;
                Check = 1;
                onEx = P.普通攻击;
                UIGoal();
                P.Turn_start();
                //attArt = monsterList[0] as Monster;
            }

            //需要一个信号量，判读当前回合人物行动完毕
            yield return new WaitUntil(() => over);
            // 当一个回合执行完毕，摄像机 复原
            mainCamere.enabled = true;
            playerCamere.enabled = false;
        }
    }

    /// <summary>
    /// 大招的加入大招队列的函数
    /// </summary>
    public void 大招准备(fun fun, Art art, Ex ex)
    {
        insert insert1;
        insert1.f = fun;
        insert1.art = art;
        insert1.ex = ex;

        大招队列.Enqueue(insert1);
    }

    /// <summary>
    /// 追加攻击的加入追加队列的函数
    /// </summary>
    public void 追加准备(fun fun, Art art, Ex ex)
    {
        insert insert1;
        insert1.f = fun;
        insert1.art = art;
        insert1.ex = ex;

        追加队列.Enqueue(insert1);
    }

    IEnumerator 追加大招()
    {
        while (true)
        {
            yield return new WaitUntil(() => wait);
            // 在改变执行技能前记录
            var beginEx = onEx;
            while (追加队列.Count() > 0 || 大招队列.Count > 0)
            {
                wait = false;
                isExTime = true;
                insert insertEx = new();
                if (追加队列.Count() > 0)
                {
                    insertEx = 追加队列.Dequeue();
                }
                else if (大招队列.Count() > 0)
                {
                    insertEx = 大招队列.Dequeue();
                }
                if (insertEx.ex.isOn)
                {
                    UGUI.gui.大招按钮.GetComponent<Button>().onClick.RemoveAllListeners();
                    UGUI.gui.大招按钮.GetComponent<Button>().onClick.AddListener(insertEx.f.Invoke);
                    UGUI.gui.UIChange(2);
                    if (insertEx.art is Player)
                    {
                        onEx = insertEx.ex;
                        UIGoal();
                    }
                }
                else
                {
                    insertEx.ex.OnEx();
                }
                yield return new WaitUntil(() => wait);
            }
            isExTime = false;
            onEx = beginEx;
            UIGoal();
            UGUI.gui.UIChange(1);
        }
    }

    public void 普通攻击()
    {
        if ((this.actArt as Player) != null && wait)
        {
            var art = this.actArt as Player;
            if (art.普通攻击.消耗战技点 > 战技点)
            {
                print("普通攻击无法释放");
                return;
            }
            wait = false;

            if (onEx == art.普通攻击)
            {
                UGUI.gui.GoalUI(null);
                art.普通攻击.OnEx();
                art.Turn_end();
                UGUI.gui.UIChange(0);
            }
            else
            {
                Check = 1;
                UGUI.gui.UIChange(1);
                onEx = art.普通攻击;
                UIGoal();
                wait = true;
            }
        }
    }

    public void 战技()
    {
        if ((this.actArt as Player) != null && wait)
        {
            var art = this.actArt as Player;
            if (art.战技.消耗战技点 > 战技点)
            {
                print("战技无法释放");
                return;
            }
            wait = false;

            if (onEx == art.战技)
            {
                UGUI.gui.GoalUI(null);
                art.战技.OnEx();
                art.Turn_end();
                UGUI.gui.UIChange(0);
                onEx = art.普通攻击;
                Check = 1;
            }
            else
            {
                Check = 2;
                UGUI.gui.UIChange(1);
                onEx = art.战技;
                UIGoal();
                wait = true;
            }
        }
    }

    /// <summary>
    ///  显示 目标标志
    /// </summary>
    /// <param name="player">行动角色</param>
    public void UIGoal()
    {
        //if (Check == 1)
        //{
        if (onEx.isAtt)
        {
            mainCamere.enabled = true;
            playerCamere.enabled = false;
            if (attArt is Player)
                attArt = monsterList[0];
        }
        else
        {
            mainCamere.enabled = false;
            playerCamere.enabled = true;
            if (attArt is Monster)
                attArt = playerList[0];
        }
        UGUI.gui.GoalUI(onEx.GetGoal());
        //UGUI.gui.GoalUI(ex.GetGoal());
        //}
        //else if (Check == 2)
        //{
        //    if (ex.isAtt)
        //    {
        //        mainCamere.enabled = true;
        //        playerCamere.enabled = false;
        //        if (attArt is Player)
        //            attArt = monsterList[0];
        //    }
        //    else
        //    {
        //        mainCamere.enabled = false;
        //        playerCamere.enabled = true;
        //        if (attArt is Monster)
        //            attArt = playerList[0];
        //    }
        //    var Goal = ex.GetGoal();

        //    UGUI.gui.GoalUI(Goal);
        //}
        //else if (Check == 3)
        //{
        //    if (ex.isAtt)
        //    {
        //        mainCamere.enabled = true;
        //        playerCamere.enabled = false;
        //        if (attArt is Player)
        //            attArt = monsterList[0];
        //    }
        //    else
        //    {
        //        mainCamere.enabled = false;
        //        playerCamere.enabled = true;
        //        if (attArt is Monster)
        //            attArt = playerList[0];
        //    }

        //    UGUI.gui.GoalUI(ex.GetGoal());
        //}
    }

    //public void 大招()
    //{
    //    if ((this.art as PlayerA) != null)
    //    {
    //        var art = this.art as PlayerA;
    //        art.大招.OnEx(art);
    //    }
    //}

    /// <summary>
    /// 人物死亡时， 执行方法
    /// 检测防止对象为空
    /// </summary>
    /// <param name="art">死亡人物</param>
    public void ArtDeath(Art art)
    {
        // 死亡对象为角色
        if (art as Player != null)
        {
            UGUI.gui.PlayerDeathUIChange(art as Player);
            playerList.Remove(art);
            if (playerList.Count == 0)
            {
                GameOver();
            }
            else if (actArt == art)
            {
                over = true;
            }
        }

        // 死亡对象为敌人
        if (art as Monster != null)
        {
            monsterList.Remove(art);
            if (monsterList.Count == 0)
            {
                GameOver();
            }
            // 当死亡对象 为当前正在行动对象
            if (actArt == art)
            {
                over = true;
            }
            // 当死亡对象为指向对象，重置为列表首位
            if (attArt == art)
            {
                attArt = monsterList[0];
            }
        }
        顺序.RemoveHead(art);
        art.gameObject.SetActive(false);
        Destroy(art.gameObject, 1f);
    }

    public void GameOver()
    {
        attArt = null;
        actArt = null;
        StopAllCoroutines();
        GameOverPanel.SetActive(true);
        // 向结束面板的按钮添加方法
        GameOverPanel
            .transform.GetChild(1)
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        if (playerList.Count == 0)
        {
            print("玩家失败");
            GameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "玩家失败";
        }
        if (monsterList.Count == 0)
        {
            print("玩家胜利");
            GameOverPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "玩家胜利";
        }
    }
}
