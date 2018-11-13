using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour {
    Transform m_transform;

    Animator m_ani;//动画组件

    Player m_player;//主角
    NavMeshAgent m_agent;//寻路组件[来自UnityEngine.AI]
    /*敌人基本属性*/
    float m_movSpeed = 2.5f;//移动速度
    float m_rotSpeed = 5.0f;//旋转速度：用于转向主角
    float m_timer = 10;//计时器
    float m_life = 5;//生命值

    protected EnemySpawn m_spawn;//敌人出生点


    // Use this for initialization
    void Start () {
        m_transform = this.transform;

        m_ani = this.GetComponent<Animator>();//获取敌人动画播放器

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();//获取主角

        m_agent = GetComponent<NavMeshAgent>();//获取寻路组件
        m_agent.speed = m_movSpeed;//设置寻路器行走速度
        m_agent.SetDestination(m_player.m_transform.position);//设置目的地
	}
	
	// Update is called once per frame
	void Update () {
        /*主角生命值归0 TO DO NOTHING*/
        if (m_player.m_life <= 0)
            return;

        m_timer -= Time.deltaTime;//更新计时器

        /*动画状态：idle， run，death， attack*/
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);//获取当前动画状态

        /*动画状态：待机 且 不处于过渡状态*/
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);

            //待机时间完成 return
            if (m_timer > 0)
            {
                return;
            }

            //与主角距离小于1.5m 进入攻击状态
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5)
            {
                m_agent.ResetPath();//停止寻路
                m_ani.SetBool("attack", true);//进入攻击状态
            }
            else
            {
                m_timer = 1;//重置定时器

                m_agent.SetDestination(m_player.m_transform.position);//设置寻路目标点

                m_ani.SetBool("run", true);//进入跑步动画状态
            }
        }

        /*动画状态：跑步 且 不处于过渡状态*/
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("run", false);

            //每隔一秒重新定位主角位置
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);//设定新位置

                m_timer = 1;
            }

            //与主角距离小于1.5m 进入攻击状态
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 1.5)
            {
                m_agent.ResetPath();//停止寻路

                m_ani.SetBool("attack", true);//进入攻击状态
            }

        }

        /*动画状态：攻击 且 不处于过渡状态*/
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack") && !m_ani.IsInTransition(0))
        {
            RotateTo();//面向主角

            m_ani.SetBool("attack", false);

            //动画播放完成进入待机状态
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);

                m_timer = 2;//重置计时器 待机2秒

                m_player.OnDamage(1);//添加对主角的伤害功能
            }
        }

        /*动画状态：死亡 且 不处于过渡状态*/
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false);
            //死亡动画结束
            if (stateInfo.normalizedTime >= 1.0f)
            {
                GameManager.Instance.SetScore(100);//加分100

                Destroy(this.gameObject);//销毁敌人游戏体
            }
        }


        //更新出生点计数
        m_spawn.m_enemyCount--;
    }

    //转向目标函数
    void RotateTo() {
        Vector3 targetdir = m_player.m_transform.position - m_transform.position;//获取方向向量

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);//计算出旋转方向

        m_transform.rotation = Quaternion.LookRotation(newDir);//旋转至新方向
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;

        //生命值为0时 当场去世
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
            m_agent.ResetPath();//敌人停止寻路
        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }
}
