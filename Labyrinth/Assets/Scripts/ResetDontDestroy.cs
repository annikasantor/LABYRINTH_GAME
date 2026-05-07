using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetDontDestroy : MonoBehaviour
{ 
    void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Destroy(gameObject);
            }
            else 
            {DontDestroyOnLoad(gameObject);}
        }
}
