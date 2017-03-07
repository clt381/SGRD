using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu: MonoBehaviour {

    public string startLevel;
    public string levelSelect;

    public void NewGame()           //NOTE: button is named 'play', not 'newGame' 
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();     //must be tested by creating a build out of the editor; editor will not quit
    }
}
