using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MyGame/SuperEnemy")]
public class SuperEnemy : Enemy {
    public Transform m_rocket;
    protected float m_fireTimer = 0.00002f;//子弹计时器
    protected Transform m_player;

    //protected AudioSource m_audio;//声音源
    public AudioClip m_shootClip;//声音


    private void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");//查找主角
        if (obj != null)
        {
            m_player = obj.transform;
        }
    }
    //重写
    protected override void UpdateMove()
    {
        m_fireTimer -= Time.deltaTime;
        if (m_fireTimer <= 0)
        {
            m_fireTimer = 2;
            if (m_player != null)
            {
                Vector3 relativePos = m_transform.position - m_player.position;//计算子弹初始方向，使其面向主角
                Instantiate(m_rocket, m_transform.position,Quaternion.LookRotation(relativePos));//创建子弹：第三个参数将子弹初始化朝向主角
                m_audio.PlayOneShot(m_shootClip);
            }
        }
        //前进方式
        m_transform.Translate(new Vector3(0, 0, -m_speed * Time.deltaTime));
    }
}
