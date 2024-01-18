using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScript : MonoBehaviour
{
  public void OnLevelComplete()
  {
      if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
      {
      LevelSelectionMenuManager.unlockedLevels++;
      PlayerPrefs.SetInt("unlockedLevels", LevelSelectionMenuManager.unlockedLevels);
      }
    SceneManager.LoadScene("menu");
  }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
