using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;

public class AdMob : MonoBehaviour {

    private void RequestBanner() {
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
            string adUnitId = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else   
            string adUnitId = "unexpected_platform";
        #endif

        // Test Ad
        AdRequest request = new AdRequest.Builder()
        .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
        .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")  // My test device.
        .Build();

        // Create a 320x50 at location
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, 0, 50);
        // Create an empty ad request.
        // AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);

    }

}
