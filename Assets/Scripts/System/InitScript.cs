using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;
using JuiceFresh.Scripts.System;
#if UNITY_ADS
using JuiceFresh.Scripts.Integrations;
using UnityEngine.Advertisements;
#endif

#if CHARTBOOST_ADS
using ChartboostSDK;
#endif
#if  GOOGLE_MOBILE_ADS
using GoogleMobileAds.Api;
#endif
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Target
{
    SCORE,
    COLLECT,
    ITEMS,
    BLOCKS,
    CAGES,
    BOMBS,
}

public enum LIMIT
{
    MOVES,
    TIME
}

public enum Ingredients
{
    None = 0,
    Ingredient1,
    Ingredient2,
    Ingredient3,
    Ingredient4

}

public enum CollectItems
{
    None = 0,
    Item1,
    Item2,
    Item3,
    Item4,
    Item5,
    Item6
}

public enum CollectStars
{
    STAR_1 = 1,
    STARS_2 = 2,
    STARS_3 = 3
}


public enum RewardedAdsType
{
    GetLifes,
    GetGems,
    GetGoOn
}

public class InitScript : MonoBehaviour
{
    public static InitScript Instance;
    public static int openLevel;


    public static float RestLifeTimer;
    public static string DateOfExit;
    public static DateTime today;
    public static DateTime DateOfRestLife;
    public static string timeForReps;
    private static int Lifes;

    public List<CollectedIngredients> collectedIngredients = new List<CollectedIngredients>();

    public RewardedAdsType currentReward;

    public static int lifes
    {
        get
        {
            return InitScript.Lifes;
        }
        set
        {
            InitScript.Lifes = value;
        }
    }

    public int CapOfLife = 5;
    // Energy system (replaces lives)
    public int EnergyMax = 5;
    public int EnergyCostPerPlay = 1;
    public static int Energy;
    public static event Action<int> OnEnergyChanged;
    public float TotalTimeForRestLifeHours = 0;
    public float TotalTimeForRestLifeMin = 15;
    public float TotalTimeForRestLifeSec = 60;
    public int FirstGems = 20;
    public static int Gems;
    public static event Action<int> OnGemsChanged;
    // Currency: Coins for purchasing fashion items
    public static int Coins;
    public static event Action<int> OnCoinsChanged;
    // Stars currency for tasks/story progression
    public static event Action<int> OnStarsChanged;
    public static int waitedPurchaseGems;
    private int BoostExtraMoves;
    private int BoostPackages;
    private int BoostStripes;
    private int BoostExtraTime;
    private int BoostBomb;
    private int BoostColorful_bomb;
    private int BoostHand;
    private int BoostRandom_color;
    public List<AdEvents> adsEvents = new List<AdEvents>();

    public static bool sound = false;
    public static bool music = false;
    private bool adsReady;
    public bool enableUnityAds;
    public bool enableGoogleMobileAds;
    public bool enableChartboostAds;
    public string rewardedVideoZone;
    public string nonRewardedVideoZone;
    public int ShowChartboostAdsEveryLevel;
    public int ShowAdmobAdsEveryLevel;
    private bool leftControl;
#if GOOGLE_MOBILE_ADS
	private InterstitialAd interstitial;
	private AdRequest requestAdmob;
#endif
    public string admobUIDAndroid;
    public string admobUIDIOS;

    public int ShowRateEvery;
    public string RateURL;
    public string RateURLIOS;
    private GameObject rate;
    public int rewardedGems = 5;
    public bool losingLifeEveryGame;
    public static Sprite profilePic;
    public GameObject facebookButton;
    //1.3.3
    public string admobRewardedUIDAndroid;
    public string admobRewardedUIDIOS;

    // Use this for initialization
    void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        RestLifeTimer = PlayerPrefs.GetFloat("RestLifeTimer");
        //		if (Application.isEditor)//TODO comment it
        //			PlayerPrefs.DeleteAll ();

        DateOfExit = PlayerPrefs.GetString("DateOfExit", "");
        Gems = PlayerPrefs.GetInt("Gems");
        Coins = PlayerPrefs.GetInt("Coins");
        lifes = PlayerPrefs.GetInt("Lifes");
        // Energy initialization and migration from legacy lives
        EnergyMax = CapOfLife > 0 ? CapOfLife : 5;
        Energy = PlayerPrefs.GetInt("Energy", -1);
        if (Energy < 0)
        {
            // Migrate from legacy lives or start full
            int legacyLives = PlayerPrefs.GetInt("Lifes", -1);
            Energy = legacyLives >= 0 ? Mathf.Clamp(legacyLives, 0, EnergyMax) : EnergyMax;
            PlayerPrefs.SetInt("Energy", Energy);
        }
        if (PlayerPrefs.GetInt("Lauched") == 0)
        {    //First lauching
            lifes = CapOfLife;
            PlayerPrefs.SetInt("Lifes", lifes);
            Gems = FirstGems;
            PlayerPrefs.SetInt("Gems", Gems);
            // Initialize Energy to full
            Energy = EnergyMax;
            PlayerPrefs.SetInt("Energy", Energy);
            // Initialize Coins on first launch (default 0)
            Coins = 0;
            PlayerPrefs.SetInt("Coins", Coins);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Sound", 1);

            PlayerPrefs.SetInt("Lauched", 1);
            PlayerPrefs.Save();
        }
        var canvasGlobal = GameObject.Find("CanvasGlobal");
        if (canvasGlobal != null)
        {
            var rateTr = canvasGlobal.transform.Find("Rate");
            if (rateTr != null)
            {
                rate = rateTr.gameObject;
                rate.SetActive(false);
            }
        }
        //rate.transform.SetParent(GameObject.Find("CanvasGlobal").transform);
        //rate.transform.localPosition = Vector3.zero;
        //rate.GetComponent<RectTransform>().anchoredPosition = (Resources.Load("Prefabs/Rate") as GameObject).GetComponent<RectTransform>().anchoredPosition;
        //rate.transform.localScale = Vector3.one;
        gameObject.AddComponent<InternetChecker>();
        var musicObj = GameObject.Find("Music");
        if (musicObj != null)
        {
            var musicAudio = musicObj.GetComponent<AudioSource>();
            if (musicAudio != null) musicAudio.volume = PlayerPrefs.GetInt("Music");
        }
        if (SoundBase.Instance != null)
        {
            var snd = SoundBase.Instance.GetComponent<AudioSource>();
            if (snd != null) snd.volume = PlayerPrefs.GetInt("Sound");
        }
#if UNITY_ADS//1.3
        enableUnityAds = false; // Tắt Unity Ads
        //var unityAds = Resources.Load<UnityAdsID>("UnityAdsID");
        //#if UNITY_ANDROID
        //    Advertisement.Initialize(unityAds.androidID,false);
        //#elif UNITY_IOS
        //    Advertisement.Initialize(unityAds.iOSID,false);
        //#endif
#else
        enableUnityAds = false;
#endif
#if CHARTBOOST_ADS//1.4.1
        enableChartboostAds = false; // Tắt Chartboost Ads
#else
        enableChartboostAds = false;
#endif


#if FACEBOOK
		FacebookManager fbManager = gameObject.AddComponent<FacebookManager> ();//1.3.3
		fbManager.facebookButton = facebookButton;//1.3.3
#endif

#if GOOGLE_MOBILE_ADS
        enableGoogleMobileAds = false; // Tắt Google Mobile Ads
#if UNITY_ANDROID
        MobileAds.Initialize(admobUIDAndroid);
        interstitial = new InterstitialAd(admobUIDAndroid);
#elif UNITY_IOS
        MobileAds.Initialize(admobUIDIOS);
        interstitial = new InterstitialAd(admobUIDIOS);
#else
        MobileAds.Initialize(admobUIDAndroid);
		interstitial = new InterstitialAd (admobUIDAndroid);
#endif

		// Create an empty ad request.
		requestAdmob = new AdRequest.Builder ().Build ();
		// Load the interstitial with the request.
		interstitial.LoadAd (requestAdmob);
		interstitial.OnAdLoaded += HandleInterstitialLoaded;
		interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
#else
        enableGoogleMobileAds = false; //1.3
#endif
        if (canvasGlobal != null)
        {
            Transform canvas = canvasGlobal.transform;
            foreach (Transform item in canvas)
            {
                item.gameObject.SetActive(false);
            }
        }

        // Notify HUDs on first scene regardless of load order
        OnCoinsChanged?.Invoke(Coins);
        OnGemsChanged?.Invoke(Gems);
        OnEnergyChanged?.Invoke(Energy);
        OnStarsChanged?.Invoke(PlayerPrefs.GetInt("Stars", 0));
    }
#if GOOGLE_MOBILE_ADS
	
	public void HandleInterstitialLoaded (object sender, EventArgs args) {
		print ("HandleInterstitialLoaded event received.");
	}

	public void HandleInterstitialFailedToLoad (object sender, AdFailedToLoadEventArgs args) {
		print ("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	}
#endif
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            leftControl = true;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            leftControl = false;

        if (Input.GetKeyUp(KeyCode.U))
        {
            for (int i = 1; i < GameObject.Find("Levels").transform.childCount; i++)
            {
                SaveLevelStarsCount(i, 1);
            }

        }

        // Debug: press 'C' to add 1 coin for quick testing
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddCoins(10);
        }
    }

    public void SaveLevelStarsCount(int level, int starsCount)
    {
        Debug.Log(string.Format("Stars count {0} of level {1} saved.", starsCount, level));
        PlayerPrefs.SetInt(GetLevelKey(level), starsCount);

    }

    private string GetLevelKey(int number)
    {
        return string.Format("Level.{0:000}.StarsCount", number);
    }

    public bool GetRewardedUnityAdsReady()
    {
        // Tắt Unity Ads
        return false;
    }

    public void ShowRewardedAds()
    {
        // Tắt tất cả quảng cáo
        CheckRewardedAds(); // Trả thưởng trực tiếp
    }

    public void CheckAdsEvents(GameState state)
    {

        foreach (AdEvents item in adsEvents)
        {
            if (item.gameEvent == state)
            {
                if ((LevelManager.THIS.gameStatus == GameState.GameOver || LevelManager.THIS.gameStatus == GameState.Pause ||
                    LevelManager.THIS.gameStatus == GameState.Playing || LevelManager.THIS.gameStatus == GameState.PrepareGame || LevelManager.THIS.gameStatus == GameState.PreWinAnimations ||
                    LevelManager.THIS.gameStatus == GameState.RegenLevel || LevelManager.THIS.gameStatus == GameState.Win))
                {
                    item.calls++;
                    if (item.calls % item.everyLevel == 0)
                        ShowAdByType(item.adType);
                    // } else {
                    // ShowAdByType (item.adType);

                }
            }
        }
    }

    void ShowAdByType(AdType adType)
    {
        if (adType == AdType.AdmobInterstitial)
            ShowAds(false);
        else if (adType == AdType.UnityAdsVideo)
            ShowVideo();
        else if (adType == AdType.ChartboostInterstitial)
            ShowAds(true);

    }

    public void ShowVideo()
    {
        Debug.Log("show Unity ads video on " + LevelManager.THIS.gameStatus);
#if UNITY_ADS
        // Tắt Unity Ads
        //if (Advertisement.IsReady ("video")) {
        //    Advertisement.Show ("video");
        //} else {
        //    if (Advertisement.IsReady ("defaultZone")) {
        //        Advertisement.Show ("defaultZone");
        //    }
        //}
#endif
    }

    public void ShowAds(bool chartboost = true)
    {
        if (chartboost)
        {
            Debug.Log("show Chartboost Interstitial on " + LevelManager.THIS.gameStatus);
#if CHARTBOOST_ADS
			Chartboost.showInterstitial (CBLocation.Default);
			Chartboost.cacheInterstitial (CBLocation.Default);
#endif
        }
        else
        {
            Debug.Log("show Google mobile ads Interstitial on " + LevelManager.THIS.gameStatus);
#if GOOGLE_MOBILE_ADS
			if (interstitial.IsLoaded ()) {
				interstitial.Show ();
#if UNITY_ANDROID
				interstitial = new InterstitialAd (admobUIDAndroid);
#elif UNITY_IOS
                interstitial = new InterstitialAd(admobUIDIOS);
#else
				interstitial = new InterstitialAd (admobUIDAndroid);
#endif

				// Create an empty ad request.
				requestAdmob = new AdRequest.Builder ().Build ();
				// Load the interstitial with the request.
				interstitial.LoadAd (requestAdmob);
			}
#endif
        }
    }

    public void ShowRate()
    {
        //InternetChecker.THIS.CheckInternet(true, (isConnected) =>
        //{
        //    if (isConnected) rate.SetActive(true);
        //});
    }


    public void CheckRewardedAds()
    {
        RewardIcon reward = GameObject.Find("CanvasGlobal").transform.Find("Reward").GetComponent<RewardIcon>();
        if (currentReward == RewardedAdsType.GetGems)
        {
            reward.SetIconSprite(0);

            reward.gameObject.SetActive(true);
            AddGems(rewardedGems);
            GameObject.Find("CanvasGlobal").transform.Find("GemsShop").GetComponent<AnimationManager>().CloseMenu();
        }
        else if (currentReward == RewardedAdsType.GetLifes)
        {
            reward.SetIconSprite(1);
            reward.gameObject.SetActive(true);
            RestoreLifes();
            GameObject.Find("CanvasGlobal").transform.Find("LiveShop").GetComponent<AnimationManager>().CloseMenu();
        }
        else if (currentReward == RewardedAdsType.GetGoOn)
        {
            GameObject.Find("CanvasGlobal").transform.Find("MenuFailed").GetComponent<AnimationManager>().GoOnFailed();
        }

    }

    public void SetGems(int count)
    {//1.3.3
        Gems = count;
        PlayerPrefs.SetInt("Gems", Gems);
        PlayerPrefs.Save();
        OnGemsChanged?.Invoke(Gems);
    }

    public void AddGems(int count)
    {
        Gems += count;
        PlayerPrefs.SetInt("Gems", Gems);
        PlayerPrefs.Save();
        OnGemsChanged?.Invoke(Gems);
#if PLAYFAB || GAMESPARKS
		NetworkManager.currencyManager.IncBalance (count);
#endif
    }

    public void SpendGems(int count)
    {
        SoundBase.Instance.PlaySound(SoundBase.Instance.cash);
        Gems -= count;
        PlayerPrefs.SetInt("Gems", Gems);
        PlayerPrefs.Save();
        OnGemsChanged?.Invoke(Gems);
#if PLAYFAB || GAMESPARKS
		NetworkManager.currencyManager.DecBalance (count);
#endif
    }

    // Energy API
    public void SetEnergy(int count)
    {
        Energy = Mathf.Clamp(count, 0, EnergyMax);
        PlayerPrefs.SetInt("Energy", Energy);
        PlayerPrefs.Save();
        OnEnergyChanged?.Invoke(Energy);
    }

    public void AddEnergy(int count)
    {
        if (count <= 0) return;
        SetEnergy(Energy + count);
    }

    public bool SpendEnergy(int count)
    {
        if (count <= 0) return true;
        if (Energy < count) return false;
        SoundBase.Instance.PlaySound(SoundBase.Instance.cash);
        SetEnergy(Energy - count);
        return true;
    }

    public bool CanStart(int cost)
    {
        return Energy >= Mathf.Max(0, cost);
    }

    public void RestoreEnergy()
    {
        SetEnergy(EnergyMax);
    }

    public int GetEnergy()
    {
        if (Energy > EnergyMax)
        {
            SetEnergy(EnergyMax);
        }
        return Energy;
    }

    // Coins API
    public void SetCoins(int count)
    {
        Coins = Mathf.Max(0, count);
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(Coins);
    }

    // Stars API
    public int GetStars()
    {
        return PlayerPrefs.GetInt("Stars", 0);
    }

    public void AddStars(int amount)
    {
        if (amount <= 0) return;
        int stars = GetStars() + amount;
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        OnStarsChanged?.Invoke(stars);
    }

    public bool SpendStars(int amount)
    {
        if (amount <= 0) return true;
        int stars = GetStars();
        if (stars < amount) return false;
        stars -= amount;
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        OnStarsChanged?.Invoke(stars);
        return true;
    }

    public void AddCoins(int count)
    {
        Coins += Mathf.Max(0, count);
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(Coins);
    }

    public bool SpendCoins(int count)
    {
        if (count <= 0) return true;
        if (Coins < count) return false;
        SoundBase.Instance.PlaySound(SoundBase.Instance.cash);
        Coins -= count;
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }


    public void RestoreLifes()
    {
        // Legacy wrapper → Energy
        RestoreEnergy();
        lifes = Energy;
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.Save();
    }

    public void AddLife(int count)
    {
        // Legacy wrapper → Energy
        AddEnergy(count);
        lifes = Energy;
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.Save();
    }

    public int GetLife()
    {
        // Legacy wrapper → Energy
        lifes = GetEnergy();
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.Save();
        return lifes;
    }

    public void PurchaseSucceded()
    {
        AddGems(waitedPurchaseGems);
        waitedPurchaseGems = 0;
    }

    public void SpendLife(int count)
    {
        // Legacy wrapper → Energy
        if (SpendEnergy(count))
        {
            lifes = Energy;
            PlayerPrefs.SetInt("Lifes", lifes);
            PlayerPrefs.Save();
        }
    }

    public void BuyBoost(BoostType boostType, int price, int count)
    {
        PlayerPrefs.SetInt("" + boostType, count);
        PlayerPrefs.Save();
#if PLAYFAB || GAMESPARKS
		NetworkManager.dataManager.SetBoosterData ();
#endif

        //   ReloadBoosts();
    }

    public void SpendBoost(BoostType boostType)
    {
        PlayerPrefs.SetInt("" + boostType, PlayerPrefs.GetInt("" + boostType) - 1);
        PlayerPrefs.Save();
#if PLAYFAB || GAMESPARKS
		NetworkManager.dataManager.SetBoosterData ();
#endif

    }

    // Exposed for wiring to a temporary UI Button in the editor
    public void DebugAddOneCoin()
    {
        AddCoins(1);
    }
    //void ReloadBoosts()
    //{
    //    BoostExtraMoves = PlayerPrefs.GetInt("" + BoostType.ExtraMoves);
    //    BoostPackages = PlayerPrefs.GetInt("" + BoostType.Packages);
    //    BoostStripes = PlayerPrefs.GetInt("" + BoostType.Stripes);
    //    BoostExtraTime = PlayerPrefs.GetInt("" + BoostType.ExtraTime);
    //    BoostBomb = PlayerPrefs.GetInt("" + BoostType.Bomb);
    //    BoostColorful_bomb = PlayerPrefs.GetInt("" + BoostType.Colorful_bomb);
    //    BoostHand = PlayerPrefs.GetInt("" + BoostType.Hand);
    //    BoostRandom_color = PlayerPrefs.GetInt("" + BoostType.Random_color);

    //}
    //public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    //{
    //    PurchaseSucceded();
    //}

    void OnApplicationFocus(bool focusStatus)
    {//1.3.3
        if (MusicBase.Instance)
        {
            MusicBase.Instance.GetComponent<AudioSource>().Play();
        }
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            if (RestLifeTimer > 0)
            {
                PlayerPrefs.SetFloat("RestLifeTimer", RestLifeTimer);
            }
            PlayerPrefs.SetInt("Lifes", lifes);
            PlayerPrefs.SetInt("Energy", Energy);
            PlayerPrefs.SetString("DateOfExit", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }

    void OnApplicationQuit()
    {   //1.4  added 
        if (RestLifeTimer > 0)
        {
            PlayerPrefs.SetFloat("RestLifeTimer", RestLifeTimer);
        }
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.SetInt("Energy", Energy);
        PlayerPrefs.SetString("DateOfExit", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    public void OnLevelClicked(object sender, LevelReachedEventArgs args)
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
            return;
        if (!GameObject.Find("CanvasGlobal").transform.Find("MenuPlay").gameObject.activeSelf && !GameObject.Find("CanvasGlobal").transform.Find("GemsShop").gameObject.activeSelf && !GameObject.Find("CanvasGlobal").transform.Find("LiveShop").gameObject.activeSelf)
        {
            // Block replaying older levels: only allow starting the next unopened level
            int maxLevelReached = PlayerPrefs.GetInt("MaxLevelReached", 1);
            int target = args.Number;
            if (target < maxLevelReached)
            {
                // Ignore clicks on completed levels
                Debug.Log("Level already completed. Replay disabled.");
                return;
            }
            PlayerPrefs.SetInt("OpenLevel", target);
            PlayerPrefs.Save();
            LevelManager.THIS.MenuPlayEvent();
            LevelManager.THIS.LoadLevel();
            openLevel = args.Number;
            //  currentTarget = targets[args.Number];
            GameObject.Find("CanvasGlobal").transform.Find("MenuPlay").gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        LevelsMap.LevelSelected += OnLevelClicked;
    }

    void OnDisable()
    {
        LevelsMap.LevelSelected -= OnLevelClicked;

        //		if(RestLifeTimer>0){
        PlayerPrefs.SetFloat("RestLifeTimer", RestLifeTimer);
        //		}
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.SetInt("Energy", Energy);
        PlayerPrefs.SetString("DateOfExit", DateTime.Now.ToString());
        PlayerPrefs.Save();
#if GOOGLE_MOBILE_ADS
		interstitial.OnAdLoaded -= HandleInterstitialLoaded;
		interstitial.OnAdFailedToLoad -= HandleInterstitialFailedToLoad;
#endif

    }


}
