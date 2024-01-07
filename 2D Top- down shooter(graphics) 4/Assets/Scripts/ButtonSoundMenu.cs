using UnityEngine;

public class ButtonSoundMenu : MonoBehaviour
{
    public void OpenSoundMenu()
    {
        // Вызовите SoundMenu, если он существует
        if (SoundMenu.Instance != null)
        {
            SoundMenu.Instance.gameObject.SetActive(true);
        }
    }
}
