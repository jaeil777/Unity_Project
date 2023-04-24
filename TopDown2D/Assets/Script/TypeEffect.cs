using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public GameObject endCursor;
    public int charPerSecond;
    public bool isAnim;

    AudioSource audioSource;
    Text msgText;
    string targetMsg;
    int index;
    float interval;
    

    private void Awake()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }
    public void SetMsg(string msg)
    {
        if(isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();

        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }

  
    }


    void EffectStart()
    {
        msgText.text = "";
        index = 0;
         endCursor.SetActive(false);

        interval = 1.0f / charPerSecond;
        isAnim = true;
        Invoke("Effecting" , interval);
    }

    void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        if (index < targetMsg.Length)
        {
            msgText.text += targetMsg[index];
            //Sound
            if (targetMsg[index] != ' ' && targetMsg[index] != '.')
            {
                audioSource.Play();
            }
            index++;
            Invoke("Effecting", interval);
        }
    }

    /*
void Effecting()
{
    if (msgText.text == targetMsg)
    {
        EffectEnd();
        return;
    }

    if (index < targetMsg.Length)
    {
        StringBuilder sb = new StringBuilder(msgText.text);
        sb.Append(targetMsg[index]);
        msgText.text = sb.ToString();
        // Sound
        if (targetMsg[index] != ' ' && targetMsg[index] != '.')
        {
            audioSource.Play();
        }
        index++;
        Invoke("Effecting", interval);
    }
}

*/


    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }

}
