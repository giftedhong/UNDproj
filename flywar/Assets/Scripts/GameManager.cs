using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;//UI控件所需：Text……

[AddComponentMenu("MyGame/GameManager")]
public class GameManager : MonoBehaviour {
    public static GameManager Instance;//静态实例

    public Transform m_canvas_main;//显示分数的UI界面
    public Transform m_canvas_gameover;//游戏失败UI界面
    public Text m_text_score;//得分UI文字
    public Text m_text_best;//最高分UI文字
    public Text m_text_life;//生命UI文字

    protected int m_score = 0;//成绩
    public static int m_hiscore = 0;//最高分
    protected Player m_player;//主角

    public AudioClip m_musicClip;//设置背景音乐
    protected AudioSource m_Audio;//声音源

	// Use this for initialization
	void Start () {
        Instance = this;
        m_Audio = this.gameObject.AddComponent<AudioSource>();//使用代码添加音效
        m_Audio.clip = m_musicClip;//通过unity指定音效
        m_Audio.loop = true;//循环播放
        m_Audio.Play();//播放音乐

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获取到主角的GameObject

        /*获取UI控件*/
        m_text_score = m_canvas_main.transform.Find("Text_score").GetComponent<Text>();//成绩
        m_text_best = m_canvas_main.transform.Find("Text_best").GetComponent<Text>();//最高成绩
        m_text_life = m_canvas_main.transform.Find("Text_life").GetComponent<Text>();//生命
        /*初始化 UI控件*/
        m_text_score.text = string.Format("分数  {0}", m_score);//成绩
        m_text_best.text = string.Format("最高分  {0}", m_hiscore);//最高成绩
        m_text_life.text = string.Format("生命  {0}", m_player.m_life);//生命值

        var restart_button = m_canvas_gameover.transform.Find("Button_restart").GetComponent<Button>();//获取重新开始按钮

        /*按钮事件回调*/
        restart_button.onClick.AddListener(delegate()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);//重新开始关卡
        });

        m_canvas_gameover.gameObject.SetActive(false);//默认隐藏游戏失败UI
	}

    //增加成绩
    public void AddScore(int point)
    {
        m_score += point;
        //更新高分记录
        if (m_hiscore < m_score)
            m_hiscore = m_score;
        m_text_score.text = string.Format("分数  {0}", m_score);//更新分数
        m_text_best.text = string.Format("最高分  {0}", m_hiscore);//更新最高分

    }

    //改变生命值UI显示
    public void ChangeLife(float life)
    {
        m_text_life.text = string.Format("生命  {0}", life);//更新生命

        if (life <= 0)
        {
            m_canvas_gameover.gameObject.SetActive(true);//主角GG 游戏失败界面
        }
    }

}
