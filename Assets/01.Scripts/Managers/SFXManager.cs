using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXData
{
    public string name;
    public AudioClip clip;
}

public class SFXManager : MonoBehaviour
{
    public AudioSource audioSource;

    public List<SFXData> SFXList;

    private Dictionary<string,AudioClip> sfxDic = new Dictionary<string, AudioClip>();
    public void Init()
    {

        foreach (var sfx in SFXList)
        {
            sfxDic.Add(sfx.name, sfx.clip);
        }
    }

    public void PlaySFX(string name, float vol)
    {
        if (sfxDic.ContainsKey(name))
        {
            audioSource.PlayOneShot(sfxDic[name], vol);
        }
    }

    public void OnClickBtn()
    {
        PlaySFX("OnClickBtn", 1);
    }

    public void OnClickBtnFail()
    {
        PlaySFX("OnClickBtnFail", 1);
    }
    
}
