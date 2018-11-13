using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI名称域
using UnityEngine.SceneManagement;//关卡名称域

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public int m_score = 0;//游戏得分
    public int m_hiscore = 0;//游戏最高得分
    public int m_ammo = 100;//子弹数量

    Player m_player;//游戏主角

    /*UI*/
    Text txt_ammo;
    Text txt_hiscore;
    Text txt_score;
    Text txt_life;

    Button button_restart;

	// Use this for initialization
	void Start () {
        Instance = this;

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获取主角

        GameObject uicanvas = GameObject.Find("Canvas");//获取UI>Canvas

        //循环 所有Transform（uicanvas）里的 所有子类
        //通过名称进行分别处理
        foreach (Transform t in uicanvas.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("txt_ammo") == 0)
            {
                txt_ammo = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("txt_hiscore") == 0)
            {
                txt_hiscore = t.GetComponent<Text>();
                txt_hiscore.text = "High Score " + m_hiscore;//设置文本

            }
            else if (t.name.CompareTo("txt_score") == 0)
            {
                txt_score = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("txt_life") == 0)
            {
                txt_life = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Restart Button") == 0)
            {
                button_restart = t.GetComponent<Button>();
                button_restart.onClick.AddListener(delegate ()
                {
                    //设置重新开始按钮事件
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);//读取当前关卡
                });
                button_restart.gameObject.SetActive(false);//游戏初期隐藏重新开始按钮
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //更新分数
    public void SetScore(int score) {
        m_score += score;

        if (m_score > m_hiscore)
            m_hiscore = m_score;

        txt_score.text = "Score" + m_score;
        txt_hiscore.text = "High Score" + m_score;
    }
    //更新弹药
    public void SetAmmo(int ammo) {
        m_ammo -= ammo;

        //装填弹药（不足时）
        if (m_ammo <= 0)
            m_ammo = 100 - m_ammo;

        txt_ammo.text = m_ammo.ToString() + "/100";
    }
    //更新生命
    public void SetLife(int life) {
        Debug.Log("LIFE:" + life);
        txt_life.text = life.ToString();

        if (life <= 0)
            //生命值为0 重新开始
            button_restart.gameObject.SetActive(true);
    }
}
