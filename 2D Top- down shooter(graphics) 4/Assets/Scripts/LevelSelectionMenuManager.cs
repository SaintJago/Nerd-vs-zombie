using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectionMenuManager : MonoBehaviour
{
  // Start is called before the first frame update
  public static int currLevel;
  public void onClickLevel(int levelNum) 
    {
    currLevel = levelNum;
    SceneManager.LoadScene("Level" + (levelNum + 1));
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
