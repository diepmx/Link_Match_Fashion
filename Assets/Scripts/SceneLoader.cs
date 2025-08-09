using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Gọi khi muốn chuyển sang scene Main
    public void LoadMainScene()
    {
        SceneManager.LoadScene("game"); // tên scene chính xác trong Build Settings
    }

    // Gọi khi muốn chuyển sang scene Dressup
    public void LoadDressupScene()
    {
        SceneManager.LoadScene("dressup");
    }

    // Hoặc cho phép load scene theo tên bất kỳ
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
