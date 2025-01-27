using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public LevelComplete levelComplete; // Reference to your levelComplete script



    private void Awake()
    {
        levelComplete = GetComponent<LevelComplete>();  
    }
    private void Update()
    {
        if (levelComplete.Complete == true)
        {
            StartCoroutine(LoadNextSceneWithDelay());
        }
    }

    private IEnumerator LoadNextSceneWithDelay()
    {
        yield return new WaitForSeconds(2f); // Wait for 5 seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
