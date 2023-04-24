using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanelAnim;
    public Animator portraitAnim;
    public GameObject talkBox;
    public Sprite prevPortrait;
    public TypeEffect talk;
    public Image portratImg;
    public Text questText;
    public GameObject scanObject;
    public GameObject menuSet;
    public GameObject player;
    public int talkIndex;

    public bool isAction;


     void Start()
    {
        GameLoad();
      questText.text= questManager.checkQuest();
    }


     void Update()
    {

        if (Input.GetButtonDown("Cancel"))
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);

            }
            else
            {
                menuSet.SetActive(true);
            }

    }


    public void Action(GameObject scObj)
    {
        scanObject = scObj;
        ObjectData objData =scObj.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
        // talkBox.SetActive(isAction);
        //
        talkPanelAnim.SetBool("isShow", isAction);
    }

    void Talk(int id, bool isNpc)
    {

        int questTalkIndex = 0;
        string talkData = "";
        //Set Talk Data
        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }
       

        //End Talk
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questManager.checkQuest(id);
            questText.text = questManager.checkQuest();
            return;
        }


        //Continue Talk
        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);
          //Show Portrait
            portratImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            
            portratImg.color = new Color(1, 1, 1, 1);
            if(prevPortrait != portratImg.sprite)
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portratImg.sprite;
            }
            
            portraitAnim.SetTrigger("doEffect");
        }
        else
        {
            //Hide Portrait
            talk.SetMsg(talkData);
            portratImg.color = new Color(1, 1, 1, 0);

        }
        isAction = true; 
        talkIndex++;
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId );
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();
        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX")){
            return;
        }
        float x=   PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId =PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position =new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
    // Update is called once per frame

}
