using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public string storyModeScene;
    public string battleModeScene;

    public void changeStoryMode()
    {
        Invoke("StoryChange", 1f);
    }

    public void changeBattleMode()
    {
        Invoke("BattleChange", 1f);
    }

    void StoryChange()
    {
        SceneManager.LoadScene(storyModeScene);
    }

    void BattleChange()
    {
        SceneManager.LoadScene(battleModeScene);
    }
}
