using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverChoice : MonoBehaviour
{
    sfxPlayer sfx;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }

    public void StartAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        sfx.PlayYes();
    }

    public void GiveUp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
        sfx.PlayNo();
    }

    
}
