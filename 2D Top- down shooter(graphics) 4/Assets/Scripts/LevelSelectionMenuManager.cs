using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectionMenuManager : MonoBehaviour
{
  public LevelObject[] levelObjects;
  // Start is called before the first frame update
  public static int currLevel;
  public static int unlockedLevels;

  public void onClickLevel(int levelNum) 
    {
    currLevel = levelNum;
    SceneManager.LoadScene("Level" + (levelNum + 1));
    }
    void Start()
    {
    unlockedLevels = PlayerPrefs.GetInt("unlockedLevels",0);
    for (int i = 0; i < levelObjects.Length; i++)
    {
      if(unlockedLevels >= i)
      {
        levelObjects[i].levelButton.interactable = true;
      }
    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
