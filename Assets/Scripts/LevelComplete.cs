using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public Canvas canvas;
    public bool Complete;

    private void OnTriggerEnter(Collider collider)
    {
       if(collider.gameObject.tag== "Player")
        {
            Complete = true;
            canvas.gameObject.SetActive(true);
        }
    }

}

    
