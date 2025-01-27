using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool GameOver;

    public Canvas canvas;

    public void EndGame()
    {
        if (GameOver == false)
        {
            GameOver = true;
            canvas.gameObject.SetActive(true);
            Debug.Log("GameOver");

        }
    }



    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }


}