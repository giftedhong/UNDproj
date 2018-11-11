using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/Enemy")]
public class Enemy : MonoBehaviour {

    public int m_point = 10;//敌人分数

    public float m_speed = 1;//速度
    public float m_life = 10;//生命
    protected float m_rotSpeed = 30;//旋转速度
    protected Transform m_transform;


    internal Renderer m_renderer;//模型渲染组件
    internal bool m_isActiv = false;//是否激活

    //音效
    protected AudioSource m_audio;//声音源
    public Transform m_explosionFX;//爆炸音效 

    // Use this for initialization
    void Start () {
    
        m_transform = this.transform;
        m_renderer = this.GetComponent<Renderer>();//获取模型渲染组件
        m_audio = this.GetComponent<AudioSource>();//获取AudioSource音效组件
    }
    //模型进入屏幕
    private void OnBecameVisible()
    {
        m_isActiv = true;
    }
    // Update is called once per frame
    void Update () {
        UpdateMove();
        //已经移动到屏幕之外
        if (m_isActiv && !this.m_renderer.isVisible)
        {
            Destroy(this.gameObject);
        }
	}
    //可用于扩展功能 - 函数  所以使用UpdateMove是一个虚函数
    protected virtual void UpdateMove()
    {
        //敌机左右移动
        float rx = Mathf.Sin(Time.time) * Time.deltaTime;
        //前进
        m_transform.Translate(new Vector3(rx, 0, -m_speed * Time.deltaTime));
    }
    //回调 - 碰撞函数
    private void OnTriggerEnter(Collider other)
    {
        Rocket rocket = other.GetComponent<Rocket>();//获取子弹C#脚本组件
        //碰撞子弹
        if (other.tag.CompareTo("PlayerRocket") == 0)
        {
            m_life -= rocket.m_power;
            if (m_life <= 0)
            {
                //敌机死亡得分
                GameManager.Instance.AddScore(m_point);//使用GameManager中的方法
                Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);//爆炸音效
                Destroy(this.gameObject);
            }
            
        }
        //碰撞玩家
        else if (other.tag.CompareTo("Player") == 0)
        {
            m_life = 0;
            Instantiate(m_explosionFX, m_transform.position, Quaternion.identity);//爆炸音效
            Destroy(this.gameObject);

        }

   
    }
}
