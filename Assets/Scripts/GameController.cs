using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject powerup;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite blanket;
    [SerializeField] GameObject player;
    [SerializeField] int totalPresents;
    [SerializeField] TextMeshProUGUI giftCounter;
    [SerializeField] TextMeshProUGUI youDied;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scarfText;
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject scarfCounter;
    [SerializeField] string[] healthNames = { "frozen", "cold", "chilly", "lukewarm", "warm", "hot", "cozy" };
    [SerializeField] Color[] healthColors;
    private const int HOT_CHOCO = 0;
    private const int BLANKET = 1;
    private bool startDeathScreen = false;
    public bool canEnterHouse = false;
    private float deathTimer;
    private float deathScreenTime;
    private int numPresentsGained;
    private int numScarfs = 0;
    void Start()
    {
        numPresentsGained = 0;
        GameObject obj = Instantiate(powerup);
        obj.GetComponent<Powerup>().InitializePowerup(player, sprite, 0.5f, 10, 0);
        transform.position = new Vector2(-3, 0);
    }

    private void Update()
    {
        giftCounter.text = numPresentsGained + "/" + totalPresents;
        scarfText.text = "x" + numScarfs;
        if (numScarfs > 0)
        {
            scarfCounter.SetActive(true);
        }
        if (startDeathScreen)
        {
            deathTimer -= Time.deltaTime;
            float opacity = (deathScreenTime - deathTimer) / deathScreenTime;
            if (deathTimer <= 0)
            {
                startDeathScreen = false;
                opacity = 1;
                Time.timeScale = 0.1f;
            }
            youDied.color = new Color(youDied.color.r, youDied.color.g, youDied.color.b, opacity);
        }
        if (!player.GetComponent<Health>().isDead)
        {
            Health healthScript = player.GetComponent<Health>();
            int num = (int)((healthNames.Length - 1) * (float)(healthScript.health) / healthScript.maxHealth);
            healthText.text = healthNames[num];
            healthText.color = healthColors[num];
        }
    }

    public void ShowDeathScreen(float time)
    {
        player.GetComponent<PlayerController>().canMove = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.GetComponent<Rigidbody2D>().Sleep();
        player.GetComponent<Animator>().SetBool("IsDead", true);
        
        deathScreenTime = time;
        deathTimer = time;
        startDeathScreen = true;
        deathPanel.SetActive(true);
        StartCoroutine(restartButtonApear());
    }

    public void SpawnPowerup(int type, Vector2 pos)
    {
        if (type == HOT_CHOCO)
        {
            GameObject obj = Instantiate(powerup);
            obj.GetComponent<Powerup>().InitializePowerup(player, sprite, 0.5f, 10, 0);
            obj.transform.position = pos;
        }
        else if (type == BLANKET)
        {
            GameObject obj = Instantiate(powerup);
            obj.GetComponent<Powerup>().InitializePowerup(player, blanket, 0.5f, 0, 1);
            obj.transform.position = pos;
            numScarfs++;
        }
    }
    public int incrementPresents(int x)
    {
        numPresentsGained += x;
        if (numPresentsGained == totalPresents) canEnterHouse = true;
        return numPresentsGained;
    }


    private IEnumerator restartButtonApear()
    {
        yield return new WaitForSeconds(0.15f);
        restartButton.SetActive(true);
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
