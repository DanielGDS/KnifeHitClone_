using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnotherChanceUI : MonoBehaviour
{
    [Header("Images template")]
    public Text appleTemplate;
    public Image timerFillTemplate;
    public Image Knife;
    [Header("Timer Settings")]
    public Text timerText;
    public int timer;
    public float delayTimer = 1f;
    [Header("Template int")]
    public int tempApplePrice;

    public GameController _gameController;
    [Header("Apple Button")]
    public Button appleButton;
    public Button adsButton;

    private void Awake()
    {
        _gameController = GameObject
       .FindGameObjectWithTag("GameController")
       .GetComponent<GameController>();
    }

    private void Start()
    {
        appleTemplate.text = tempApplePrice.ToString();
    }

    // Цена в яблоках за еще один шанс, и время на решение
    public void AnotherChance(int applePrice, int timer)
    {     
        // Если у игрока установлен не стандартный скин то установить его в изображение ножа на экране шанса.
        if (_gameController.profile.PlayerKnife != null) { Knife.sprite = _gameController.profile.PlayerKnife; }
        timerFillTemplate.fillAmount = 1f;
        if (_gameController.appleCount < applePrice) // Сравнивает общее количество яблок с ценой шанса, если не хватает то выключить кнопку
        {
            appleButton.interactable = false;
        }
        else if (_gameController.appleCount == applePrice) { appleButton.interactable = true; } // Если количество яблок игрока равно цене, то включает кнопку
        else { appleButton.interactable = true; } // В любом другом случае включить кнопку

        if (_gameController.timeToADS == true) { adsButton.interactable = true; } else { adsButton.interactable = false; }

        _gameController.m_counterValue = _gameController.appleCount;

        tempApplePrice = applePrice;
        appleTemplate.text = applePrice.ToString();
        StartCoroutine(timerCount(timer)); 
    }

    public void OnButtonClick()
    {
        // Если кнопка активна из-за бага, дополнительное сравнение количества яблок игрока и ценой шанса
        if (_gameController.appleCount < tempApplePrice) { return; } // Если не хватает, вернуть без выполнения дальнейших действий
        _gameController.gameIsOver = false;
        _gameController.knifeCount++;
        _gameController.appleCount = _gameController.appleCount - tempApplePrice;
        PlayerPrefs.SetInt("AppleMax", _gameController.appleCount);

        // Запускает корутину с отсчетом потраченых яблок на полученое значение
        _gameController.increaseScore();

        _gameController.GetSaveInfo(); //Сохраняет прогресс 
        _gameController.AnotherChance(); // Активирует еще один шанс и прибавляет один нож
        gameObject.SetActive(false); // Выключить данный интерфейс по завершению кода

    }

    public void OnAdsButtonClick()
    {
        // Запускает таймер с отчетом времени, через которое снова будет активна кнопка
        // После чего выключает Геймовер прибавляя нож и возращает игрока обратно в игру
        _gameController.StartTimerADS();
        _gameController.gameIsOver = false;
        _gameController.knifeCount++;
        _gameController.AnotherChance();
        gameObject.SetActive(false);
    }

    IEnumerator timerCount(int timer)
    {
        timerText.text = timer.ToString();

        yield return new WaitForSeconds(delayTimer);
        int tempTime = timer;
        for (int i = 0; timer > i; i++)
        {
            tempTime--;

            float fill = (float)tempTime / timer;
            timerFillTemplate.fillAmount = fill;
            timerText.text = tempTime.ToString();
            yield return new WaitForSeconds(delayTimer);
        }

        // Дальнейший код выполняется если время истекло

        _gameController.chanceToAnotherChance = 0; // Установить значение на еще один шанс.
        _gameController.GameOverScreen(); // Запустить экран с подсчетом очков
        gameObject.SetActive(false); // Отключить данный интерфейс
    }
}
