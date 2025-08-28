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

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;

    [Header("Crouch")]
    public float crouchHeight = 0.5f;
    public float standingHeight = 1f;
    public float crouchSpeed = 5f;
    private bool isCrouching = false;

    [Header("Sprint (Double-Tap W)")]
    public float sprintDoubleClickTime = 0.5f;
    private float lastForwardPressTime = 0f;
    private bool doubleClickStarted = false;
    private bool isSprinting = false;

    private float playerHeight;
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

    private PlayerData playerData;
    private GameState gameState;
    private ToastManager toastManager;
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

        mainCamera = player.GetChild(0).GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerHeight = standingHeight;

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
        playerData.totalTime += Time.deltaTime;
        gameTime += Time.deltaTime;

        gameScore = Mathf.Max(gameScore, player.position.z - startPosition) - scoreDecrement;

        scoreTxt.text = "Score: " + Mathf.RoundToInt(gameScore);

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

        HandleMouseLook();
        HandleInput();
        HandleCrouch();
        HandleSprint();
        HandleJump();
        HandleDoorToggle();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleMouseLook()
    {
        float sensitivity = (playerData.sensitivity * 500) + 200;

        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        player.transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 forward = player.transform.forward * z;
        Vector3 right = player.transform.right * x;
        moveDir = (forward + right).normalized;
    }

    private void MovePlayer()
    {
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetPos = rb.position + speed * Time.fixedDeltaTime * moveDir;
        rb.MovePosition(targetPos);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isCrouching = true;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            isCrouching = false;

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        playerHeight = Mathf.Lerp(playerHeight, targetHeight, Time.deltaTime * crouchSpeed);

        // Smoothly lower/raise camera instead of scaling player
        Vector3 camLocalPos = mainCamera.transform.localPosition;
        camLocalPos.y = playerHeight;
        mainCamera.transform.localPosition = camLocalPos;
    }

    private void HandleSprint()
    {
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
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCrouching && GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerData.totalJumps++;
            gameJumps++;
            jumpSfx.Play();
        }
    }

    private void HandleDoorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f, doorSwitchLayerMask))
            {
                Collider hitObject = hitInfo.collider;
                Transform wall = hitObject.transform.parent;

                DoorController leftDoor = hitObject.transform.GetChild(0).GetComponent<DoorController>();
                DoorController rightDoor = hitObject.transform.GetChild(1).GetComponent<DoorController>();
                DoorLightController lights = wall.GetChild(0).GetComponent<DoorLightController>();

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
        lifeTxt.text = "Life: " + gameLife;
        deductionTxt.text = "SCORE DEDUCTED";
        deductionTimeSfx.Play();

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
        Time.timeScale = 1f;
        buttonClickSfx.Play();

        if (gameState == GameState.PAUSE)
        {
            UpdateDataWhenLose();
        }

        // todo: add fade in-out transition effect
        SceneManager.LoadScene("Play");
    }

    // retry and next level buttons
    public void RetryButton()
    {
        Time.timeScale = 1f;
        buttonClickSfx.Play();

        if (gameState == GameState.PAUSE)
        {
            UpdateDataWhenLose();
        }

        // todo: add fade in-out transition effect
        SceneManager.LoadScene("Infinite");
    }

    public void ShopButton()
    {
        Time.timeScale = 1f;
        buttonClickSfx.Play();
        playerData.SaveData();
        // todo: add fade in-out transition effect
        SceneManager.LoadScene("Shop");
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
        playerData.highScore = Mathf.Max(playerData.highScore, Mathf.RoundToInt(gameScore));
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

        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.GetChild(1).GetComponent<Animator>().SetBool("isOpen", true);
        loseSfx.Play();
        StopAllAudio();

        UpdateDataWhenLose();

        loseCoinsTxt.text = gameCoinsCollected.ToString();
        loseJumpsTxt.text = gameJumps.ToString();
        loseTimeTxt.text = Mathf.RoundToInt(gameTime).ToString();
        loseScoreTxt.text = "Score: " + Mathf.RoundToInt(gameScore).ToString();

        Time.timeScale = 0f;
    }

    public void OnWaterHit()
    {
        loseTxt.text = "WATER STUCK";
        Lose();
    }

    public void OnSpikeHit()
    {
        loseTxt.text = "SPIKE HIT";
        Lose();
    }

    public void OnSurrender()
    {
        loseTxt.text = "SURRENDER";
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
}
