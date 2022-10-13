using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void LoadScene(UnityEditor.SceneAsset scene)
    {
        SceneManager.LoadScene(scene.name);      
    }
}
