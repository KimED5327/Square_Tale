using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlay : MonoBehaviour
{
    public void PlayPopUpSound()
    {
        SoundManager.instance.PlayEffectSound("PopUp");
    }

    public void PlayPopDownSound()
    {
        SoundManager.instance.PlayEffectSound("PopDown");
    }

    public void PlayClickSound()
    {
        SoundManager.instance.PlayEffectSound("Click");
    }

    public void PlayNpcClickSound()
    {
        SoundManager.instance.PlayEffectSound("NPC_Click");
    }

    public void PlayMenuClickSound()
    {
        SoundManager.instance.PlayEffectSound("Menu_Click");
    }

}
