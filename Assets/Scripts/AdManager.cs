using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    BannerView banner;
    InterstitialAd interstitial;

    void Start() {
        RequestBanner();
        Debug.Log(interstitial);
        //RequestInterstitial();
    }
    
    public void playAd() {
        if (interstitial.IsLoaded())
            interstitial.Show();
    }

    private void RequestBanner() {
        #if UNITY_EDITOR
            string adUnitId = "ca-app-pub-2593585099025338/6902847482";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-2593585099025338/6902847482";
#elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 at location
        AdSize adSize = new AdSize(360, 50);
        banner = new BannerView(adUnitId, adSize, AdPosition.Bottom);

        // Test Ad
        /*
        AdRequest request = new AdRequest.Builder()
        .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
        .AddTestDevice("CE5277B4BAF1C68D19BFF15805ADC5D3")  // My test device.
        .Build();
        */

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        banner.LoadAd(request);
    }

    private void RequestInterstitial() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-2593585099025338/8847141081";
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.

        // Test Ad
        
        AdRequest request = new AdRequest.Builder()
        .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
        .AddTestDevice("CE5277B4BAF1C68D19BFF15805ADC5D3")  // My test device.
        .Build();
        
        //AdRequest request = new AdRequest.Builder().Build();

        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

}
