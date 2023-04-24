using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    // Start is called before the first frame update
    public string questName;
    public int[] npcId;
    
    public QuestData(string name ,int[] npc)
    {
        questName = name;
        npcId = npc;

    }
}
