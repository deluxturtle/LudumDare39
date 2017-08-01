using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStuff : MonoBehaviour {

	public void _Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        
    }
    public void _Quit()
    {
        Application.Quit();
    }
}
