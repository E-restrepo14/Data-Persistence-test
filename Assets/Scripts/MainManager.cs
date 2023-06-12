using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    ScoreManager m_scoreManager;

    public Text ScoreText;
    public Text PlayerText;

    public GameObject GameOverText;

    private bool m_Started = false;

    [SerializeField] private int m_Points = 0;
    public string currentPlayerName = "no name";
    [SerializeField] private int hihgestScore;
    [SerializeField] private string topPlayer;


    private bool m_GameOver = false;


 



    // Start is called before the first frame update
    void Start()
    {
        LoadHihgestScore();

        m_scoreManager = FindObjectOfType<ScoreManager>();
        currentPlayerName = m_scoreManager.GetName();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_GameOver = false;
                m_Started = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        CompareScores(hihgestScore);

    }

    void ShowScore(string name, int score)
    {
        PlayerText.text = ("Best Score : " + name + " : " + score);
    }

    public void GameOver()
    {
        LoadHihgestScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int dataHihgestScore;
        public string dataBestPlayer;
    }

    public void CompareScores(int scoreToCompare)
    {
        if(m_Points > scoreToCompare)
        {
            SaveHihgestScore();
        }
    }

    public void SaveHihgestScore()
    {
        print("data saved");
        SaveData data = new SaveData();
        data.dataHihgestScore = m_Points;
        data.dataBestPlayer = currentPlayerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadHihgestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            print("data loaded");
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            hihgestScore = data.dataHihgestScore;
            topPlayer = data.dataBestPlayer;
            ShowScore(topPlayer, hihgestScore);
        }
    }

}
