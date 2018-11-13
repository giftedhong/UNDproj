using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Transform m_transform;

    Transform m_camTransform;//摄像机Transform
    Vector3 m_camRot;//摄像机旋转角度
    float m_camHeight = 1.7f;//摄像机高度(即表示主角身高)

    CharacterController m_ch;//角色控制器组件
    float m_movSpeed = 3.0f;//角色移动速度
    float m_gravity = 2.0f;//角色所受重力
    public int m_life = 5;//角色生命值


    /*射击属性*/
    Transform m_muzzlepoint;//枪口Transform
    public LayerMask m_layer;//射击，射线能射到的碰撞层
    public Transform m_fx;//击中后的粒子效果
    public AudioClip m_audio;//射击音效
    float m_shootTimer = 0;//射击间隔计时器


	// Use this for initialization
	void Start () {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();//获取角色控制器组件

        /*初始化摄像机的位置和旋转角度，并锁定鼠标*/
        m_camTransform = Camera.main.transform;//获取摄像机

        //使用TransformPoint获取Player在Y轴偏移一定高度的位置
        m_camTransform.position = m_camTransform.TransformPoint(0, m_camHeight, 0);//获取摄像机初始高度

        m_camTransform.rotation = m_transform.rotation;//摄像机旋转角度同步主角
        m_camRot = m_camTransform.eulerAngles;

        Screen.lockCursor = true;//锁定鼠标
        //Cursor.visible = true;

        m_muzzlepoint = m_camTransform.Find("M16/weapon/muzzlepoint").transform;//获取到枪口位置
	}
	
	// Update is called once per frame
	void Update () {
        //生命值归零
        if (m_life <= 0)
            return;
        Control();

        //更新射击间隔时间
        m_shootTimer -= Time.deltaTime;

        //鼠标左键射击
        if (Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1f;
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);//播放音效

            GameManager.Instance.SetAmmo(1);//消耗弹药

            RaycastHit info;//包存射线探测结果

            //从muzzlepoint的位置，向摄像机面向的正方射出一根射线
            //射线只能与m_layer所指定层碰撞
            //参数5个！！！！！！！！！！！！
            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);

            if (hit)
            {
                //击中Tag 为 enemy的游戏体
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();

                    enemy.OnDamage(1);//减少生命
                }

                //射中位置释放粒子效果
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }
	}

    //控制主角移动代码
    void Control() {

        float xm = 0, ym = 0, zm = 0;//定义三个值控制移动

        ym -= m_gravity * Time.deltaTime;//重力运动

        /*获取鼠标移动距离*/
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        /*旋转摄像机*/
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        /*使主角面向方向与摄像机一致*/
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;

        /*上下左右移动*/
        /*W A S D按键*/
        if(Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            zm -= m_movSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }

        //使用角色控制器提供的Move函数移动 （自带检测碰撞）
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));


        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);//更新摄像机位置与Player一致

    }

    //为主角显示一个图标
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;

        GameManager.Instance.SetLife(m_life);//更新UI
        

        //取消光标锁定
        if (m_life <= 0)
            //Cursor.visible = false;
            Screen.lockCursor = false;


    }
}
