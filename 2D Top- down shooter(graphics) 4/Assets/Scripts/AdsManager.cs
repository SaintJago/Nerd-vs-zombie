using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [SerializeField] GameObject rewardedAdsButton, deathPanel;
    [SerializeField] float timeBtwAd;
    float timerBtwAd;

    [SerializeField] AudioClip popSound;

    void Start()
    {
        Advertisement.Initialize("5175129", false, this);
    }

    void Update()
    {
        timerBtwAd += Time.deltaTime;

        if(timerBtwAd >= timeBtwAd)
        {
            Advertisement.Show("Interstitial_Android", this);
            timerBtwAd = 0;
        }
    }

    public void ShowAd()
    {
        Advertisement.Show("Rewarded_Android", this);

        SoundManager.instance.PlayerSound(popSound);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId == "Rewarded_Android")
        {
            rewardedAdsButton.SetActive(false);
            deathPanel.SetActive(false);
            Player.instance.gameObject.SetActive(true);
            Player.instance.AddHealth(Player.instance.maxHealth);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }
}
