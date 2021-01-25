using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlay : MonoBehaviour
{
    public void PlayBubblePopSound()
    {
        SoundManager.instance.PlayEffectSound("NPC_Click", 0.5f);
    }

    public void PlayClickSound()
    {
        SoundManager.instance.PlayEffectSound("Click");
    }
}
