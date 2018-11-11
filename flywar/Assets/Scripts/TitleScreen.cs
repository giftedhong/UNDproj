using UnityEngine.SceneManagement;
using UnityEngine;

[AddComponentMenu("MyGame/TitleScreen")]
public class TitleScreen : MonoBehaviour {

    //响应游戏开始
    public void onButtonGameStart()
    {
        SceneManager.LoadScene("level1");
    }

	
}
