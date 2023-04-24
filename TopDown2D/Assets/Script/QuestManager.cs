using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    Dictionary<int, QuestData> questList;
    // Start is called before the first frame update
    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData(); 
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("첫 마을 방문 ", new int[] { 1000,2000 }));
        questList.Add(20, new QuestData("루도의 동전 찾아주기 ", new int[] { 5000,2000 }));
        questList.Add(30, new QuestData("퀘스트 올클리어 ", new int[] { 5000, 2000 }));
    }


    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }
    public string checkQuest()
    {
        return questList[questId].questName;
    }

    public string checkQuest(int id)
    {
        //Control Quest Object;
       

        if (id == questList[questId].npcId[questActionIndex])
        questActionIndex++;

        ControlObject();

        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }


   public void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if(questActionIndex == 2)
                {
                    questObject[0].SetActive(true);
                }

                break;
                
            case 20:
                if (questActionIndex == 0)
                {
                    questObject[0].SetActive(true);
                }
                else if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);
                }
                break;

        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
