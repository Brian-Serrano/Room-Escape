using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfiniteManager : MonoBehaviour
{
    [Header("References")]
    public Transform obstaclesContainer;
    public Transform player;
    public LayerMask doorSwitchLayerMask;
    public ConfigHandler configHandler;
    public GameObject mobileUIController;
    public DragArea dragArea;
    public Animator crossfade;

    private Rigidbody rb;
    private Camera mainCamera;

    [Header("UI Elements")]
    public Transform pausePanel;
    public Transform gameOverPanel;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;
    public TMP_Text deductionTxt;

    public TMP_Text loseCoinsTxt;
    public TMP_Text loseJumpsTxt;
    public TMP_Text loseTimeTxt;
    public TMP_Text loseTxt;

    public TMP_Text scoreTxt;
    public TMP_Text lifeTxt;
    public TMP_Text pauseScoreTxt;
    public TMP_Text loseScoreTxt;

    public Button doubleCoinsButton;
    public Transform noAdsPanel;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource backgroundMusic;
    public AudioSource buttonClickSfx;
    public AudioSource coinCollectSfx;
    public AudioSource jumpSfx;
    public AudioSource loseSfx;
    public AudioSource doorToggleSfx;
    public AudioSource deductionTimeSfx;

    [Header("Revive UI")]
    public Slider reviveSlider;
    public Transform reviveMenu;
    public Button buyRevive;
    public Button watchAdRevive;
    private bool isReviveActive = false;
    private bool isRevivedPaused = false;
    private int reviveChances = 2;
    private float reviveTimer = 0f;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;

    [Header("Sprint (Double-Tap W)")]
    public float sprintDoubleClickTime = 0.3f;
#if UNITY_ANDROID || UNITY_IOS
#else
    private float lastForwardPressTime = 0f;
    private bool doubleClickStarted = false;
#endif
    private bool isSprinting = false;

    [Header("Door Toggle Input")]
    public float maxTapDuration = 0.3f;
    public float maxTapMovement = 50f;
    private float tapStartTime;
    private Vector2 tapStartPos;

    private float verticalRotation = 0f;
    private Vector3 moveDir;
    private int gameJumps = 0;
    private int gameCoinsCollected = 0;
    private int obstacleOffset = 0;
    private float gameScore = 0f;
    private int gameLife = 3;
    private float startPosition;
    private int scoreDecrement = 0;
    private float gameTime = 0f;
    private int questsCompletedInOneGame = 0;
    private Vector3 safePosition;

    private PlayerData playerData;
    private GameState gameState;
    private DeathType deathType;
    private ToastManager toastManager;
    private RewardedAdManager rewardedAdManager;
    private InterstitialAdManager interstitialAdManager;
    private List<int> nums1;
    private List<int> nums2;
    private List<List<float>> obstaclesToCreate;

    private List<int> levelsQuest = new List<int>() { 2, 4, 6 };
    private List<int> attemptsQuest = new List<int>() { 5, 10, 15 };
    private List<int> coinsQuest = new List<int>() { 20, 40, 60 };

    private string[] levelsQuestTitles = new string[] { "Level Beater", "Level Grinder", "Level Master" };
    private string[] attemptsQuestTitles = new string[] { "Nice Try", "Another Try", "Always Trying" };
    private string[] coinsQuestTitles = new string[] { "Coin Finder", "Coin Collector", "Coin Master" };

    private void Awake()
    {
        playerData = PlayerData.LoadData();
        toastManager = GetComponent<ToastManager>();
        rb = player.GetComponent<Rigidbody>();
        gameState = GameState.PLAYING;
        rewardedAdManager = RewardedAdManager.GetInstance();
        interstitialAdManager = InterstitialAdManager.GetInstance();

        mainCamera = player.GetChild(0).GetComponent<Camera>();

#if UNITY_ANDROID || UNITY_IOS
        mobileUIController.SetActive(true);
#else
        mobileUIController.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

        nums1 = new List<int>() { 5, 8, 11 };
        nums2 = new List<int>() { 14, 18, 22, 26 };

        obstaclesToCreate = RoomEscapeUtils.GetObstacleSpawnPoints();

        GameObject obstacleGroup = new GameObject("Obstacle Group");

        obstacleGroup.transform.parent = obstaclesContainer;

        obstacleGroup.transform.position = Vector3.zero;

        GameObject roomStart = Instantiate(configHandler.room, new Vector3(0, 6, -16), Quaternion.identity, obstacleGroup.transform);
        SetMaterials(roomStart.GetComponentsInChildren<IMaterialController>());

        startPosition = GameObject.FindGameObjectWithTag("Finish Line").transform.position.z;

        while (obstacleOffset * 20 <= player.position.z + 200)
        {
            Vector3 spawnOffset = obstacleOffset * 20 * Vector3.forward;

            GenerateObstacle(spawnOffset);

            obstacleOffset++;
        }

        lifeTxt.text = "Life: " + gameLife;
        scoreTxt.text = "Score: " + gameScore;
    }

    private void GenerateObstacle(Vector3 spawnOffset)
    {
        GameObject obstacleGroup = new GameObject("Obstacle Group");

        obstacleGroup.transform.parent = obstaclesContainer;

        obstacleGroup.transform.position = spawnOffset;

        List<List<int>> obstacles = RoomEscapeUtils.CreateObstacleSpawns(nums1, nums2);

        GameObject roomSlice = Instantiate(configHandler.structures[7], new Vector3(0, 6, 0) + spawnOffset, Quaternion.identity, obstacleGroup.transform);
        SetMaterials(roomSlice.GetComponentsInChildren<IMaterialController>());

        List<int> randomObstacles = obstacles[Random.Range(0, obstacles.Count)];

        foreach (int obstacle in randomObstacles)
        {
            List<float> idx = obstaclesToCreate[obstacle - 1];

            for (int j = 0; j < idx.Count; j += 4)
            {
                GameObject structure = Instantiate(configHandler.structures[(int)idx[j]], new Vector3(idx[j + 1], idx[j + 2], idx[j + 3]) + spawnOffset, Quaternion.identity, obstacleGroup.transform);
                SetMaterials(structure.GetComponentsInChildren<IMaterialController>());
            }
        }
    }

    private void SetMaterials(IMaterialController[] matControllers)
    {
        foreach (IMaterialController matController in matControllers)
        {
            matController.SetMaterial(configHandler.materials, playerData.materialsSelected);
        }
    }

    private void Start()
    {
        UpdateMusicVolume();
        UpdateSfxVolume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (noAdsPanel.gameObject.activeSelf)
            {
                CloseNoAdsPanel();
            }
            else
            {
                switch (gameState)
                {
                    case GameState.PLAYING:
                        PauseButton();
                        break;
                    case GameState.PAUSE:
                        HomeButton();
                        break;
                    case GameState.LOSE:
                        HomeButton();
                        break;
                    case GameState.WIN:
                        HomeButton();
                        break;
                    case GameState.REVIVE:
                        reviveTimer = 0f;
                        break;
                }
            }
        }

        playerData.totalTime += Time.deltaTime;
        gameTime += Time.deltaTime;

        gameScore = Mathf.Max(gameScore, player.position.z - startPosition) - scoreDecrement;

        scoreTxt.text = "Score: " + Mathf.Max(Mathf.RoundToInt(gameScore), 0);

        if (obstacleOffset * 20 <= player.position.z + 200)
        {
            Vector3 spawnOffset = obstacleOffset * 20 * Vector3.forward;

            GenerateObstacle(spawnOffset);

            obstacleOffset++;
        }

        for (int i = 0; i < obstaclesContainer.childCount; i++)
        {
            if (player.position.z - 200 >= obstaclesContainer.GetChild(i).position.z)
            {
                Destroy(obstaclesContainer.GetChild(i).gameObject);
            }
        }

        if (isReviveActive && !isRevivedPaused)
        {
            reviveTimer -= Time.unscaledDeltaTime;

            reviveSlider.value = reviveTimer / 5f;

            if (reviveTimer <= 0f)
            {
                reviveMenu.gameObject.SetActive(false);

                isReviveActive = false;

                SetGameOver();
            }
        }

        CheckSafePosition();

#if UNITY_ANDROID || UNITY_IOS
        float sensitivity = (playerData.sensitivity * 100) + 20;

        float mouseX = 0f;
        float mouseY = 0f;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                continue;

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                mouseX = delta.x * sensitivity * Time.deltaTime;
                mouseY = delta.y * sensitivity * Time.deltaTime;

                break;
            }
        }
#else
        float sensitivity = (playerData.sensitivity * 500) + 200;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
#endif

        player.transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

#if UNITY_ANDROID || UNITY_IOS
        float x = dragArea.Horizontal();
        float z = dragArea.Vertical();

        Vector3 forward = player.transform.forward * z;
        Vector3 right = player.transform.right * x;
        moveDir = forward + right;
#else
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 forward = player.transform.forward * z;
        Vector3 right = player.transform.right * x;
        moveDir = (forward + right).normalized;
#endif

#if UNITY_ANDROID || UNITY_IOS
#else
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastForwardPressTime < sprintDoubleClickTime)
                doubleClickStarted = true;

            lastForwardPressTime = Time.time;
        }

        if (doubleClickStarted && Input.GetKey(KeyCode.W))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // Reset double click if too much time has passed
        if (Time.time - lastForwardPressTime > sprintDoubleClickTime)
            doubleClickStarted = false;


        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerData.totalJumps++;
            gameJumps++;
            jumpSfx.Play();
        };
#endif
        if (gameState == GameState.PLAYING)
        {
#if UNITY_ANDROID || UNITY_IOS
            // Touch version (mobile)
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        tapStartTime = Time.time;
                        tapStartPos = touch.position;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (Time.time - tapStartTime <= maxTapDuration &&
                            Vector2.Distance(touch.position, tapStartPos) <= maxTapMovement)
                        {
                            ToggleDoor();
                        }
                    }
                }
            }
#else
            // Mouse version (for testing in editor)
            if (Input.GetMouseButtonDown(0))
            {
                tapStartTime = Time.time;
                tapStartPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - tapStartTime <= maxTapDuration &&
                    Vector2.Distance((Vector2)Input.mousePosition, tapStartPos) <= maxTapMovement)
                {
                    ToggleDoor();
                }
            }
#endif
        }
    }

    void FixedUpdate()
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetPos = rb.position + speed * Time.fixedDeltaTime * moveDir;
        rb.MovePosition(targetPos);
    }

#if UNITY_ANDROID || UNITY_IOS
    public void OnSprintDown() => isSprinting = true;

    public void OnSprintUp() => isSprinting = false;

    public void Jump()
    {
        if (GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerData.totalJumps++;
            gameJumps++;
            jumpSfx.Play();
        }
    }
#endif

    private void ToggleDoor()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f, doorSwitchLayerMask))
        {
            Collider hitObject = hitInfo.collider;
            Transform wall = hitObject.transform.parent;

            DoorController leftDoor = hitObject.transform.GetChild(0).GetComponent<DoorController>();
            DoorController rightDoor = hitObject.transform.GetChild(1).GetComponent<DoorController>();
            DoorLightController lights = wall.GetChild(0).GetComponent<DoorLightController>();

            if (!leftDoor.isOpen && !rightDoor.isOpen)
            {
                if (lights.toggle)
                {
                    if (hitObject.gameObject.name == wall.GetChild(1).gameObject.name)
                    {
                        StartCoroutine(Deduct());
                    }
                }
                else
                {
                    if (hitObject.gameObject.name == wall.GetChild(2).gameObject.name)
                    {
                        StartCoroutine(Deduct());
                    }
                }

                leftDoor.Toggle();
                rightDoor.Toggle();
                doorToggleSfx.Play();
            }
        }
    }

    private IEnumerator Deduct()
    {
        scoreDecrement += 20;
        gameLife--;
        lifeTxt.text = "Life: " + Mathf.Max(gameLife, 0);
        deductionTxt.text = "SCORE DEDUCTED";
        deductionTimeSfx.Play();

        if (gameLife <= 0 && gameState == GameState.PLAYING)
        {
            CheckRevive(DeathType.ZERO_LIVES);
        }

        yield return new WaitForSeconds(2);

        deductionTxt.text = "";
    }

    private bool GroundCheck()
    {
        // Simple raycast ground check (adjust distance to fit player height)
        return Physics.Raycast(player.position, Vector3.down, 1.1f);
    }

    // pause button
    public void PauseButton()
    {
        pausePanel.gameObject.SetActive(true);
        pausePanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
        buttonClickSfx.Play();
        PauseAllAudio();
        gameState = GameState.PAUSE;

        musicSlider.value = playerData.musicVolume;
        sfxSlider.value = playerData.sfxVolume;
        sensitivitySlider.value = playerData.sensitivity;
        pauseScoreTxt.text = "Score: " + Mathf.RoundToInt(gameScore);

        Time.timeScale = 0f;
    }

    // continue button
    public void ContinueButton()
    {
        pausePanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        playerData.SaveData();
        buttonClickSfx.Play();
        StartCoroutine(DelayedUnpause());
    }

    private IEnumerator DelayedUnpause()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        pausePanel.gameObject.SetActive(false);
        UnpauseAllAudio();
        gameState = GameState.PLAYING;
        Time.timeScale = 1f;
    }

    // home button
    public void HomeButton()
    {
        buttonClickSfx.Play();

        if (gameState == GameState.PAUSE)
        {
            UpdateDataWhenLose();
        }

        StartCoroutine(SwitchScene("Play"));
    }

    // retry and next level buttons
    public void RetryButton()
    {
        buttonClickSfx.Play();

        if (gameState == GameState.PAUSE)
        {
            UpdateDataWhenLose();
        }

        StartCoroutine(SwitchScene("Infinite"));
    }

    public void ShopButton()
    {
        buttonClickSfx.Play();
        playerData.SaveData();
        StartCoroutine(SwitchScene("Shop"));
    }

    private void UpdateMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(playerData.musicVolume) * 20);
    }

    private void UpdateSfxVolume()
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(playerData.sfxVolume) * 20);
    }

    public void OnMusicVolumeChange(float value)
    {
        playerData.musicVolume = value;
        UpdateMusicVolume();
    }

    public void OnSfxVolumeChange(float value)
    {
        playerData.sfxVolume = value;
        UpdateSfxVolume();
    }

    public void OnSensitivityChange(float value)
    {
        playerData.sensitivity = value;
    }

    private void UpdateAttemptsQuest()
    {
        if (playerData.attemptsQuestTotal > 0 && playerData.attemptsQuestProgress < playerData.attemptsQuestTotal)
        {
            playerData.attemptsQuestProgress++;

            if (playerData.attemptsQuestProgress >= playerData.attemptsQuestTotal)
            {
                int index = attemptsQuest.IndexOf(Mathf.Abs(playerData.attemptsQuestTotal));
                toastManager.ShowToast("Quest Completed: " + attemptsQuestTitles[index]);

                int coinsReceived = (index + 1) * 15;

                playerData.coins += coinsReceived;
                playerData.totalCoins += coinsReceived;
                playerData.totalQuestsCompleted++;
                playerData.attemptsQuestTotal = -playerData.attemptsQuestTotal;
                playerData.attemptsQuestProgress = 0;
                questsCompletedInOneGame++;
            }
        }
    }

    private void UpdateCoinsQuest(int coins)
    {
        if (playerData.coinsCollectedQuestTotal > 0 && playerData.coinsCollectedQuestProgress < playerData.coinsCollectedQuestTotal)
        {
            playerData.coinsCollectedQuestProgress += coins;

            if (playerData.coinsCollectedQuestProgress >= playerData.coinsCollectedQuestTotal)
            {
                int index = coinsQuest.IndexOf(Mathf.Abs(playerData.coinsCollectedQuestTotal));
                toastManager.ShowToast("Quest Completed: " + coinsQuestTitles[index]);

                int coinsReceived = (index + 1) * 15;

                playerData.coins += coinsReceived;
                playerData.totalCoins += coinsReceived;
                playerData.totalQuestsCompleted++;
                playerData.coinsCollectedQuestTotal = -playerData.coinsCollectedQuestTotal;
                playerData.coinsCollectedQuestProgress = 0;
                questsCompletedInOneGame++;
            }
        }
    }

    private void UpdateDataWhenLose()
    {
        playerData.totalAttempts++;
        playerData.highScore = Mathf.Max(playerData.highScore, Mathf.Max(Mathf.RoundToInt(gameScore), 0));
        playerData.coins += gameCoinsCollected;
        playerData.totalCoins += gameCoinsCollected;

        UpdateAttemptsQuest();
        UpdateCoinsQuest(gameCoinsCollected);

        playerData.totalQuestsCompletedOneGame = Mathf.Max(playerData.totalQuestsCompletedOneGame, questsCompletedInOneGame);

        AchievementManager.CheckAchievements(AchievementData.LoadData(), playerData, toastManager);

        playerData.SaveData();
    }

    private void Lose()
    {
        gameState = GameState.LOSE;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StopAllAudio();

        UpdateDataWhenLose();

        loseCoinsTxt.text = gameCoinsCollected.ToString();
        loseJumpsTxt.text = gameJumps.ToString();
        loseTimeTxt.text = Mathf.RoundToInt(gameTime).ToString();
        loseScoreTxt.text = "Score: " + Mathf.Max(Mathf.RoundToInt(gameScore), 0).ToString();

        doubleCoinsButton.onClick.RemoveAllListeners();

        if (gameCoinsCollected > 0)
        {
            doubleCoinsButton.onClick.AddListener(() =>
            {
                doubleCoinsButton.interactable = false;

                toastManager.PauseToasts();
                buttonClickSfx.Play();

                rewardedAdManager.ShowRewardedAd(() => { }, () =>
                {
                    playerData.coins += gameCoinsCollected;
                    playerData.totalCoins += gameCoinsCollected;

                    loseCoinsTxt.text = (gameCoinsCollected * 2).ToString();

                    playerData.SaveData();

                    doubleCoinsButton.interactable = false;
                    doubleCoinsButton.GetComponentInChildren<TMP_Text>().text = "Claimed";

                    toastManager.ResumeToasts();

                    StartCoroutine(SetPauseAfterAd());
                }, () =>
                {
                    doubleCoinsButton.interactable = true;

                    OpenNoAdsPanel();

                    toastManager.ResumeToasts();

                    StartCoroutine(SetPauseAfterAd());
                });
            });
        }
        else
        {
            doubleCoinsButton.interactable = false;
        }
        
        Time.timeScale = 0f;

        if (Mathf.RoundToInt(gameTime) > 20)
        {
            toastManager.PauseToasts();

            interstitialAdManager.ShowInterstitial(() =>
            {
                gameOverPanel.gameObject.SetActive(true);
                gameOverPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
                loseSfx.Play();
                StartCoroutine(SetPauseAfterAd());
                toastManager.ResumeToasts();
            });
        }
        else
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
            loseSfx.Play();
        }
    }

    private void CheckSafePosition()
    {
        if (Physics.Raycast(player.position, Vector3.down, out RaycastHit hit, 1.1f))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                safePosition = player.position;
            }
        }
    }

    public void CheckRevive(DeathType type)
    {
        if (reviveChances > 0)
        {
            Time.timeScale = 0f;

            isReviveActive = true;
            reviveTimer = 5f;

            reviveMenu.gameObject.SetActive(true);

            gameState = GameState.REVIVE;
            deathType = type;

            buyRevive.interactable = playerData.coins >= 20;

            PauseAllAudio();
        }
        else
        {
            Time.timeScale = 0f;
            deathType = type;

            SetGameOver();
        }
    }

    private void Revive()
    {
        buttonClickSfx.Play();

        switch (deathType)
        {
            case DeathType.WATER_STUCK:
                player.position = safePosition;
                break;
            case DeathType.SPIKE_HIT:
                player.position = safePosition;
                break;
            case DeathType.ZERO_LIVES:
                gameLife++;
                break;
        }

        reviveChances--;
        isReviveActive = false;
        deathType = DeathType.NONE;
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        gameState = GameState.PLAYING;

        reviveMenu.gameObject.SetActive(false);

        UnpauseAllAudio();

        Time.timeScale = 1f;
    }

    public void BuyRevive()
    {
        playerData.coins -= 20;

        playerData.SaveData();

        Revive();
    }

    public void WatchAdRevive()
    {
        watchAdRevive.interactable = false;

        isRevivedPaused = true;
        toastManager.PauseToasts();

        rewardedAdManager.ShowRewardedAd(() => { }, () =>
        {
            watchAdRevive.interactable = true;

            isRevivedPaused = false;

            Revive();

            toastManager.ResumeToasts();
        }, () =>
        {
            watchAdRevive.interactable = true;

            isRevivedPaused = false;

            toastManager.ResumeToasts();

            OpenNoAdsPanel();
        });
    }

    private void SetGameOver()
    {
        switch (deathType)
        {
            case DeathType.WATER_STUCK:
                OnWaterHit();
                break;
            case DeathType.SPIKE_HIT:
                OnSpikeHit();
                break;
            case DeathType.ZERO_LIVES:
                OnLivesZero();
                break;
        }
    }

    private void OnWaterHit()
    {
        loseTxt.text = "WATER STUCK";
        Lose();
    }

    private void OnSpikeHit()
    {
        loseTxt.text = "SPIKE HIT";
        Lose();
    }

    public void OnSurrender()
    {
        loseTxt.text = "SURRENDER";
        Lose();
    }

    private void OnLivesZero()
    {
        loseTxt.text = "OUT OF LIVES";
        Lose();
    }

    public void OnCoinHit()
    {
        coinCollectSfx.Play();
        gameCoinsCollected++;
    }

    private void StopAllAudio()
    {
        backgroundMusic.Stop();

        if (jumpSfx.isPlaying)
        {
            jumpSfx.Stop();
        }

        if (coinCollectSfx.isPlaying)
        {
            coinCollectSfx.Stop();
        }

        if (doorToggleSfx.isPlaying)
        {
            doorToggleSfx.Stop();
        }

        if (deductionTimeSfx.isPlaying)
        {
            deductionTimeSfx.Stop();
        }
    }

    private void PauseAllAudio()
    {
        backgroundMusic.Pause();

        if (jumpSfx.isPlaying)
        {
            jumpSfx.Pause();
        }

        if (coinCollectSfx.isPlaying)
        {
            coinCollectSfx.Pause();
        }

        if (doorToggleSfx.isPlaying)
        {
            doorToggleSfx.Pause();
        }

        if (deductionTimeSfx.isPlaying)
        {
            deductionTimeSfx.Pause();
        }
    }

    private void UnpauseAllAudio()
    {
        backgroundMusic.UnPause();

        if (!jumpSfx.isPlaying && jumpSfx.time > 0f && jumpSfx.time < jumpSfx.clip.length)
        {
            jumpSfx.UnPause();
        }

        if (!coinCollectSfx.isPlaying && coinCollectSfx.time > 0f && coinCollectSfx.time < coinCollectSfx.clip.length)
        {
            coinCollectSfx.UnPause();
        }

        if (!doorToggleSfx.isPlaying && doorToggleSfx.time > 0f && doorToggleSfx.time < doorToggleSfx.clip.length)
        {
            doorToggleSfx.UnPause();
        }

        if (!deductionTimeSfx.isPlaying && deductionTimeSfx.time > 0f && deductionTimeSfx.time < deductionTimeSfx.clip.length)
        {
            deductionTimeSfx.UnPause();
        }
    }

    private IEnumerator SetPauseAfterAd()
    {
        yield return null; // Wait 1 frame so SDK finishes its reset
        Time.timeScale = 0f;
    }

    private void OpenNoAdsPanel()
    {
        noAdsPanel.gameObject.SetActive(true);
        noAdsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void CloseNoAdsPanel()
    {
        noAdsPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", false);
        StartCoroutine(DelayedPanelClose(noAdsPanel));
    }

    private IEnumerator DelayedPanelClose(Transform panel)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.gameObject.SetActive(false);
    }

    private IEnumerator SwitchScene(string name)
    {
        crossfade.SetBool("isOpen", true);
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
}
