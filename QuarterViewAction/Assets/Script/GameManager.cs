using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreText;
    public Text scoreText;
    public Text stageText;
    public Text playTimeText;
    public Text playerHealth;
    public Text playerAmmoText;
    public Text playerCoinText;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image grenadeImg;

    public Text enemyAText;
    public Text enemyBText;
    public Text enemyCText;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;


    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", 5000);
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
       
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    private void Update()
    {
        if (isBattle)
        {
            playTime += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        //상단
        scoreText.text = string.Format("{0:n0}", player.score);
        stageText.text = "STAGE" + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, second);
        playerHealth.text = player.health + "/" + player.maxHealth;
        playerCoinText.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null)
            playerAmmoText.text = "-/" + player.ammo;
        else if( player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoText.text = "-/" + player.ammo;
        else
            playerAmmoText.text = player.equipWeapon.curAmmo +"/" + player.ammo;

        //무기
        weapon1Img.color = new Color(1, 1, 1, player.hasweapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasweapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasweapons[2] ? 1 : 0);
        grenadeImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        //몬스터 숫자
        enemyAText.text = enemyCntA.ToString();
        enemyBText.text = enemyCntB.ToString();
        enemyCText.text = enemyCntC.ToString();

        //보스 체력
        bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth,1,1);

    }
}
