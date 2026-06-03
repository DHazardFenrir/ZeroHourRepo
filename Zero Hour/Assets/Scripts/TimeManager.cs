using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [SerializeField] float timeLimit = 90f; // 1 minuto y medio
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] public GameObject escapeMenuVictory;

    private float currentTime;
    private bool isTimerRunning = false;

    void Awake()
    {
        if (Instance == null) Instance = this; 
        else Destroy(gameObject);
    }

    void Start()
    {
        currentTime = timeLimit;
        isTimerRunning = true;
        DisplayBestTime();
    }

    void Update()
    {
        if (!isTimerRunning || !GameManager.Instance.IsPlayerAlive()) return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            currentTime = 0;
            isTimerRunning = false;
            TriggerTimeGameOver();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TriggerTimeGameOver()
    {
        Debug.Log("¡Se acabó el tiempo!");
        GameManager.Instance.GameOver(); 
    }

    public void SaveEscapeTime()
    {
        isTimerRunning = false;
        float timeEscaped = timeLimit - currentTime; 

        float currentRecord = PlayerPrefs.GetFloat("BestEscapeTime", 9999f);

        if (timeEscaped < currentRecord)
        {
            PlayerPrefs.SetFloat("BestEscapeTime", timeEscaped);
            Debug.Log("¡Nuevo Récord de tiempo!");
        }
        
        DisplayBestTime();
    }

    void DisplayBestTime()
    {
        if (PlayerPrefs.HasKey("BestEscapeTime"))
        {
            float best = PlayerPrefs.GetFloat("BestEscapeTime");
            int minutes = Mathf.FloorToInt(best / 60);
            int seconds = Mathf.FloorToInt(best % 60);
            bestTimeText.text = string.Format("Mejor Tiempo: {0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            bestTimeText.text = "Mejor Tiempo: --:--";
        }
    }
}
