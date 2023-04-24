using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{

    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {

        //Talk Data
        //루나 :1000 루도 :2000
        //책상 :100 상자 :200
        talkData.Add(100, new string[] { "그냥 책상이다. " });
        talkData.Add(200, new string[] { "무언가가 들어있는 상자이다. " });
        talkData.Add(1000, new string[] { "안녕!! 내이름은 루나야 :0 ", "여기에 처음왔구나? :1","저쪽으로 가봐~:2" });
        talkData.Add(2000, new string[] { "반갑다! 내이름은 루도 :0 ", "상자를 눌러보렴 :1" });

        //Quest Data
        talkData.Add(10+1000, new string[] { "어서와 :0 ", "우리 마을의 전설에 대해 알아? :1", "루도한테 물어봐:2" });

        talkData.Add(11 + 2000, new string[] { "너 우리마을의 전설 들었니? :0 ", "맞아 내가 루나한테 말해줬어 :1", "그런데 그전에 내 동전좀 찾아줘 :2" });

       
        talkData.Add(20 + 2000, new string[] { "동전좀 찾아줘 부탁할게 :1" });

        talkData.Add(20 + 5000, new string[] { "동전을 찾았다" });
        talkData.Add(21 + 2000, new string[] { "찾아줘서 고마워 :2" });



        //Portrait Data
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);

        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);
        
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if(!talkData.ContainsKey(id-id%10))
               return GetTalk(id-id %100, talkIndex);
            else
               return GetTalk(id-id%10, talkIndex);

      
        }

        if(talkIndex == talkData[id].Length)
        {
            return null;
        }else
            return talkData[id][(int)talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }

}
