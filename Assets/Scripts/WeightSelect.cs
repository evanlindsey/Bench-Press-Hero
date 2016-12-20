using UnityEngine;
using UnityEngine.SceneManagement;

public class WeightSelect : MonoBehaviour
{
    public void NewLevel(int number)
    {
        PlayerPrefs.SetInt("Active", number);
        SceneManager.LoadScene("Level");
    }
}
