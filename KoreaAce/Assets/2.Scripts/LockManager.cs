using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance;

    public static bool isLock = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
