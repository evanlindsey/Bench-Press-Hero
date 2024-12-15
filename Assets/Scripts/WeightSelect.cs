using UnityEngine;

public class WeightSelect : MonoBehaviour
{
    public void NewLevel(int number)
    {
        Scene.LoadLevel(number);
    }
}
