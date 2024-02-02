using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject shopButton;
    public GameObject soundButton;
    public static bool isPaused = false;
    public static bool PauseGame = false;
    public GameObject shopMenu;
    public GameObject soundMenu;
   

    public void SoundButtonPressed()
    {
        if (soundMenu.activeSelf && !shopMenu.activeSelf)
        {
            // ������� ������ ���� �����, ���� ���� �������� �� �������
            soundMenu.SetActive(false);
            Resume();

        }
        else if (soundMenu.activeSelf && shopMenu.activeSelf)
        {
            // ������� ������ ���� �����, ���� ��� ���� �������
            soundMenu.SetActive(false);

        }
        else
        {
            // � ��������� ������� ������� ���� �����
            OpenSoundMenu();
        }
    }

    public void ShopButtonPressed()
    {
        if (shopMenu.activeSelf && !soundMenu.activeSelf)
        {
            // ������� ������ ���� ��������, ���� ���� ����� �� �������
            shopMenu.SetActive(false);
            Resume();

        }
        else if (shopMenu.activeSelf && soundMenu.activeSelf)
        {
            // ������� ������ ���� ��������, ���� ��� ���� ������� 
            shopMenu.SetActive(false);

        }
        else
        {
            // � ��������� ������� ������� ���� ��������
            OpenShopMenu();
        }
    }

    public void OpenShopMenu()
    {
        
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        isPaused = true;
    }

    public void OpenSoundMenu()
    {
       
        soundMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        isPaused = true;
    }

    public void Resume()
    {
       
        shopMenu.SetActive(false);
        soundMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        isPaused = false;
    }

    void Update()
    {
       

       // if (Input.GetKeyDown(KeyCode.Escape))
       // {
          //  ShopButtonPressed();
       // }
    }
    public void LoadMenu()

    {

        // ��������� ���� ������� ����� ������� �� �����

        StopAllCoroutines();

        // ������������� ����������� ������� �������

        Time.timeScale = 1f;

        // �������� ����� ����

        SceneManager.LoadScene("Menu");

    }

}
