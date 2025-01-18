using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;


public class MenuSelect : MonoBehaviour {

	public GameObject mainUI;
	public GameObject gameplayUI;
	public GameObject pauseUI;
	public GameObject pauseButton;
	public GameObject gameOverUI;
	private AudioSource buttonClick;

	private PlayerMovement playerMovement;
	private SpawnObstacles spawnObstacles;

    public string clickid;
    private StarkAdManager starkAdManager;
    void Start() {
		playerMovement = GameObject.Find ("square").GetComponent<PlayerMovement> ();
		spawnObstacles = GameObject.Find ("Canvas").GetComponent<SpawnObstacles> ();

		buttonClick = GameObject.Find ("buttonClick").GetComponent<AudioSource> ();
		mainUI.transform.Find ("bestScore").GetComponent<TextMeshProUGUI> ().text = "最高分: " + PlayerPrefs.GetInt ("bestScore", 0);
	}

	public void play() {
		playerMovement.enabled = true;
		spawnObstacles.enabled = true;
		mainUI.SetActive (false);
		gameplayUI.SetActive (true);
		pauseButton.SetActive (true);
		buttonClick.Play ();
	}
	public void pause() {
		playerMovement.enabled = false;
		Time.timeScale = 0;
		pauseUI.SetActive (true);
		pauseButton.SetActive (false);
		buttonClick.Play ();
	}
	public void resume() {
		Time.timeScale = 1;
		pauseUI.SetActive (false);
		pauseButton.SetActive (true);
		playerMovement.enabled = true;
		buttonClick.Play ();
	}
	public void gameOver() {
		GameObject.Find ("gameOver").GetComponent<AudioSource> ().Play ();
		if (playerMovement.score > PlayerPrefs.GetInt ("bestScore", 0)) {
			PlayerPrefs.SetInt ("bestScore", playerMovement.score);
		}
		gameOverUI.SetActive (true);
		pauseButton.SetActive (false);
        ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
        Time.timeScale = 0;
	}
	public void ContinueGame()
	{
        gameOverUI.SetActive(false);
        pauseButton.SetActive(true);
		playerMovement.Respawn();
        Time.timeScale = 1;
    }
	public void restart() {
		Time.timeScale = 1;
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		for (int i = 0; i < obstacles.Length; i++) {
			Destroy (obstacles [i]);
		}
		GameObject.Find ("square").transform.position = new Vector2 (-3, 0.6f);
		playerMovement.score = 0;
		playerMovement.up = true;
		playerMovement.positionX = -6;
		playerMovement.positionY = 0.6f;
		playerMovement.speedUp = 0.01f;
		playerMovement.timer = 0;

		GameObject.Find("score").GetComponent<TextMeshProUGUI> ().text = "得分: 0";
		GameObject.Find ("Canvas").GetComponent<SpawnObstacles> ().deletedObstacleLevel = 0;
		GameObject.Find ("Canvas").GetComponent<SpawnObstacles> ().obstacleLevel = 1;
		GameObject.Find ("Canvas").GetComponent<SpawnObstacles> ().lastObstaclePosition = 20.48f;

		playerMovement.enabled = false;
		spawnObstacles.enabled = false;
		mainUI.SetActive (true);
		gameplayUI.SetActive (false);
		mainUI.transform.Find ("bestScore").GetComponent<TextMeshProUGUI> ().text = "最高分: " + PlayerPrefs.GetInt ("bestScore", 0);
		gameOverUI.SetActive (false);
		buttonClick.Play ();
	}
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }

    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
}
