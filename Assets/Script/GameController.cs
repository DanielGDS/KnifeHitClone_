using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("Scripts")]
    public KnifeScript woodScript;
    public KnifeSpawner knifeSpawner;
    public Profile profile;
    public AnotherChanceUI anotherChance;

    [Header("GameUIs")]
    public GameObject gameUI;
    public GameObject gameTextUI;
    public GameObject[] otherGameUI;
    public GameObject gameUIStartScreen;
    public GameObject KnifeList;

    [Header("Game text UI")]
    public Text levelText;
    public Text appleCountText;
    public Text knifesText;
    public Text AllKnifeCountText;
    public Text StageText;
    public Text ChallengeTEXT;

    public Text lastStageText;
    public Text lastAppleText;
    public Text lastKnifeMaxText;

    [Header("Prefabs")]
    public GameObject knifeUIPrefab;
    public GameObject knifePanel;

    [Space(10)]
    public GameObject applePrefab;
    public GameObject knifePrefab;
    public GameObject destroyWoodPrefab;
    public Transform wood;

    [Header("Positions")]
    public GameObject applePos;
    public Transform applePos1;
    public GameObject knifePos;

    [Header("Light Destroy Knife OBJ")]
    public GameObject destroyAllKnife;

    [Header("Level int")]
    public int LevelInt = 1;

    [Header("Game object count")]
    public int knifeUICount;
    public int knifeCount;
    public int allKnifeCount;
    public int multiplierScore = 1;
    public int multiplierTempScore;
    public int appleCount;
    public int currentStage;
    public int chanceToAnotherChance;

    [Header("Start object")]
    public int knifeInWood;
    public int appleInWood;

    [Header("Game bools")]
    public bool gameIsOver;
    public bool isInvoke;
    public bool isAppleСhallenge;

    [Header("Animation speed")]
    public float speedAnimation;
    public float speedKnifeFall;

    Transform m_rectXform;

    [Space(10)]
    public UI _UI;

    [Header("Settings Knife Spawner Position")]
    public Vector3 startPostion;
    public Vector3 onscreenPosition;
    public Vector3 endPosition;
    public float timeToMove = 1f;
    [Space(10)]
    

    [Header("GameOver Settings")]
    public GameObject gameOverSpriteKnife;
    public bool timeToADS = true;


    int tempApplePrice = 10;
    int tempAppleInWood;


    public void GetSaveInfo()
    {
        // Загрузить максимальный уровень
        if (PlayerPrefs.GetInt("StageMax") != LevelInt 
            && LevelInt > PlayerPrefs.GetInt("StageMax"))
        {
            PlayerPrefs.SetInt("StageMax", LevelInt);
        }
        // Загрузить рекорд ножей
        if (PlayerPrefs.GetInt("KnifeMax") != allKnifeCount 
            && allKnifeCount > PlayerPrefs.GetInt("KnifeMax"))
        {
            PlayerPrefs.SetInt("KnifeMax", allKnifeCount);
        }
        // Загрузить количество яблок
        if (PlayerPrefs.GetInt("AppleMax") != appleCount 
            && appleCount > PlayerPrefs.GetInt("AppleMax"))
        {
            PlayerPrefs.SetInt("AppleMax", appleCount);
        }
    }


    public void SetGameUIList(bool active)
    {
        // Выключает или включает отдельный интерфейс
        if (active)
        {
            for (int x = 0; x < otherGameUI.Length; ++x)
            {
                otherGameUI[x].SetActive(false);
            }
        }
        else
        {
            for (int x = 0; x < otherGameUI.Length; ++x)
            {
                otherGameUI[x].SetActive(true);
            }
        }


    }

    public void GameOverScreen()
    {
        SetGameUIList(true);
        GetSaveInfo();

        chanceToAnotherChance++;
        tempApplePrice += 5;
        if (chanceToAnotherChance >= 5)
        {
            anotherChance.gameObject.SetActive(true);
            anotherChance.AnotherChance(tempApplePrice + 15+(appleCount/100+tempApplePrice)*multiplierScore, 10);
            chanceToAnotherChance++;
        }
        else
        {

            knifesText.text = "0";

            GameOverScore();
            multiplierScore = 1;
            multiplierTempScore = 0;
            appleCountText.transform.GetChild(0).GetComponent<Text>().text = "x" + multiplierScore;

            if (profile.PlayerKnife != null) { gameOverSpriteKnife.transform.GetComponent<Image>().sprite = profile.PlayerKnife; gameOverSpriteKnife.transform.GetChild(0).GetComponent<Image>().sprite = profile.PlayerKnife; }
            
            tempApplePrice = 0;
            lastStageText.text = "УРОВЕНЬ " + $"{PlayerPrefs.GetInt("StageMax")}";
            lastAppleText.text = "CЧЕТ " + $"{PlayerPrefs.GetInt("KnifeMax")}";
            lastKnifeMaxText.text = "" + $"{PlayerPrefs.GetInt("KnifeMax")}";


            gameTextUI.SetActive(true);
            gameUI.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SetGameUIList(false);
        Destroy(knifeSpawner.knife);

        isAppleСhallenge = false;
        currentStage = 0;

        gameTextUI.SetActive(false);
        gameIsOver = false;
        gameUI.SetActive(false);
        LevelInt = 0;
        levelText.text = "STAGE " + LevelInt;
        knifeCount = 0;
        allKnifeCount = 0;
        knifesText.text = "" + allKnifeCount;


        // Получить сохраненные значения яблок
        appleCount = PlayerPrefs.GetInt("AppleMax");
        appleCountText.text = "" + appleCount;

        NewStage();
    }

    private void OnEnable()
    {
        lastStageText.text = "УРОВЕНЬ " + $"{PlayerPrefs.GetInt("StageMax")}";
        lastAppleText.text = "CЧЕТ " + $"{PlayerPrefs.GetInt("KnifeMax")}";
        appleCountText.text = "" + $"{ PlayerPrefs.GetInt("AppleMax")}";
    }

    private void Start()
    {
        SetGameUIList(true);
        lastStageText.text = "УРОВЕНЬ " + $"{PlayerPrefs.GetInt("StageMax")}";
        lastAppleText.text = "CЧЕТ " + $"{PlayerPrefs.GetInt("KnifeMax")}";
        appleCountText.text = "" + $"{ PlayerPrefs.GetInt("AppleMax")}";
    }

    private void FixedUpdate()
    {
        if (knifeUICount != 0) 
        {
            Instantiate(knifeUIPrefab, knifePanel.transform);
            knifeUICount --;
        }
    }

    public void StartScreen()
    {
        gameUIStartScreen.SetActive(false);
    }

    public void ShopScreen()
    {
        KnifeList.SetActive(true);
        gameUIStartScreen.SetActive(false);
    }

    public void OffShopScreen()
    {
        KnifeList.SetActive(false);
        gameUIStartScreen.SetActive(true);
        if (profile.PlayerKnife != null) { profile.TempKnifeSprite.sprite = profile.PlayerKnife; }
    }

    public void MainMenu()
    {
        gameTextUI.SetActive(false);
        gameIsOver = false;
        gameUI.SetActive(false);
        gameUIStartScreen.SetActive(true);
        LevelInt = 0;
        allKnifeCount = 0;
    }

    public void NewStage()
    {

        if (knifeSpawner.knife != null) { Destroy(knifeSpawner.knife.gameObject); }
        if (chanceToAnotherChance == 5) { chanceToAnotherChance = 0; }
        chanceToAnotherChance++;

        if (LevelInt <= 10) // Если стадия меньше или равна 10 установить данную сложность
        {

            woodScript.speed = Random.Range(120, 160);
            appleInWood = Random.Range(1, 5);
            knifeCount = Random.Range(5, 12);
            knifeInWood = Random.Range(0, 5);

        }
        if (LevelInt >= 10) // Если стадия больше или равна 10 установить данную сложность
        {

            woodScript.speed = Random.Range(90, 220);
            appleInWood = Random.Range(0, 9);
            knifeCount = Random.Range(5, 9);
            knifeInWood = Random.Range(1, 7);

        }
        if (LevelInt >= 15) // Если стадия меньше или равна 15 установить данную сложность
        {
            woodScript.speed = Random.Range(90, 320);
            appleInWood = Random.Range(0, 9);
            knifeCount = Random.Range(5, 12);
            knifeInWood = Random.Range(3, 7);

        }
        if (LevelInt >= 20) // Если стадия меньше или равна 20 установить данную сложность
        {
            woodScript.speed = Random.Range(90, 400);
            appleInWood = Random.Range(0, 9);
            knifeCount = Random.Range(3, 9);
            knifeInWood = Random.Range(2, 9);

        }
        tempAppleInWood = appleInWood;


        isInvoke = true;
        LevelInt++;
        Invoke("DestroyKnife", 0.2f);
        Invoke("NewLevel", 0.001f);

    }

    public void StageControl()
    {
        currentStage++;
        if (currentStage == 6 || currentStage == 0) { currentStage = 1; isAppleСhallenge = false; ChallengeTEXT.text = ""; }
        _UI.UpdateStage(currentStage);
        if (currentStage == 5) { isAppleСhallenge = true; /*ChallengeTEXT.text = "яблочный переполох";*/ }

    }

    public void AnotherChance()
    {
        SetGameUIList(false);
    }

    public void NewLevel()
    {
        SetGameUIList(false);
        SetZeroPosition();


        SetKnifeUI();

        knifeUICount = knifeCount;
        levelText.text = "УРОВЕНЬ " + LevelInt;
        Invoke("DeActiveDestroy", 0.3f);
        Invoke("SetNewKnife", 1f);
    }

    void SetNewKnife()
    {
        if (knifeSpawner.knife == null)
        {
            knifeSpawner.NewKnife();
            StartCoroutine(MoveRoutine(startPostion, onscreenPosition, timeToMove));
        }
        else
        {
            Invoke("SetNewKnife", 5f);
        }


    }

    void SetKnifeUI()
    {
        _UI.UpdateHealth(knifeCount);
        _UI.UpdateKnifeUI(knifeCount);
    }

    void DeActiveDestroy()
    {
        destroyAllKnife.SetActive(false);
        SpawnKnife();
    }

    void DestroyKnife()
    {
        destroyAllKnife.SetActive(true);
    }

    public void SpawnApple()
    {
        
        if (tempAppleInWood > 0)
        {
            appleCountText.text = "" + PlayerPrefs.GetInt("AppleMax");
            applePrefab.GetComponent<Rigidbody2D>().gravityScale = 0f;
            applePos1.transform.Rotate(0, 0, Random.Range(-360, 360));
            GameObject apple = Instantiate(applePrefab, applePos.transform);
            apple.transform.parent = wood.transform;
            tempAppleInWood -= 1;
            Invoke("SpawnApple", 0.01f);
        }
        else
        {
            isInvoke = false;
        }


    }
    public void SpawnKnife()
    {
        if (knifeInWood > 0)
        {
            knifePrefab.GetComponent<Rigidbody2D>().gravityScale = 0f;
            knifePos.transform.Rotate(0, 0, Random.Range(0, 180));
            Instantiate(knifePrefab, knifePos.transform);
            knifeInWood -= 1;
            Invoke("SpawnKnife", 0.1f);
        }
        else
        {
            SpawnApple();
  
        }
    }

    void SetZeroPosition()
    {
        wood.position = new Vector3(0, 1, 1000);
    }

    public void InvokeDestroyWood()
    {
        Instantiate(destroyWoodPrefab);
    }

    IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        m_rectXform = knifeSpawner.transform;
        if (m_rectXform != null)
        {
            m_rectXform.position = startPos;
        }
        bool reachedDestination = false;
        float elapsedTime = 0f;

        while (!reachedDestination)
        {
            if (Vector3.Distance(m_rectXform.position, endPos) < 0.01f)
            {
                reachedDestination = true;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            t = t * t * t * (t * (t * 6 - 15) + 10);

            if (m_rectXform != null)
            {
                m_rectXform.position = Vector3.Lerp(startPos, endPos, t);
            }
            yield return null;
        }
    }









    [Header("DownCount Score Settings")]
    public float countTime = 1f;
    public int m_counterValue = 0;
    public int m_increment = 1;

    public void increaseScore()
    {
        StartCoroutine(CountScoreRoutine());

    }

    IEnumerator CountScoreRoutine()
    {
        int iterations = 0;
        
        while (m_counterValue > appleCount && iterations < 10000)
        {
            yield return new WaitForSeconds(countTime * Time.deltaTime);
            m_counterValue -= m_increment;
            appleCountText.text = m_counterValue.ToString();
            iterations++;
            yield return new WaitForSeconds(countTime * Time.deltaTime);
            yield return null;


        }

        m_counterValue = appleCount;
        appleCountText.text = m_counterValue.ToString();

    }

    public void GameOverScore()
    {
        StartCoroutine(GameOverCountScore());
        StartCoroutine(GameOverStageScore());
    }

    IEnumerator GameOverCountScore()
    {
        int iterations = 0;
        m_counterValue = 0;
        AllKnifeCountText.text = m_counterValue.ToString();
        yield return new WaitForSeconds(1f);
        while (m_counterValue < allKnifeCount && iterations < 10000)
        {
            yield return new WaitForSeconds(0.005f);
            m_counterValue += m_increment;
            AllKnifeCountText.text = m_counterValue.ToString();
            iterations++;
            yield return new WaitForSeconds(0.005f);
            yield return null;


        }

        m_counterValue = allKnifeCount;
        AllKnifeCountText.text = m_counterValue.ToString();

    }

    IEnumerator GameOverStageScore()
    {
        int iterations = 0;
        int m_StageValue = 0;
        StageText.text = "УРОВЕНЬ " + m_StageValue;
        yield return new WaitForSeconds(1f);
        while (m_counterValue < LevelInt && iterations < 10000)
        {
            yield return new WaitForSeconds(0.005f);
            m_StageValue += m_increment;
            StageText.text = "УРОВЕНЬ " + m_StageValue;
            iterations++;
            yield return new WaitForSeconds(0.005f);
            yield return null;


        }

        m_StageValue = LevelInt;
        StageText.text = "УРОВЕНЬ " + m_StageValue;

    }


    public void StartTimerADS()
    {
        StartCoroutine(timerADS());
    }

    IEnumerator timerADS()
    {
        timeToADS = false;
        anotherChance.adsButton.interactable = false;
        yield return new WaitForSeconds(15);
        timeToADS = true;
    }
}
