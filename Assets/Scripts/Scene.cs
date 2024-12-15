using UnityEngine.SceneManagement;

public class Scene
{
    enum SceneName
    {
        Menu,
        WeightSelect,
        Level
    }

    static void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static void LoadMenu()
    {
        LoadScene((int)SceneName.Menu);
    }

    public static void LoadWeightSelect()
    {
        LoadScene((int)SceneName.WeightSelect);
    }

    public static void LoadLevel(int number)
    {
        Storage.ActiveLevel = number;
        LoadScene((int)SceneName.Level);
    }
}
