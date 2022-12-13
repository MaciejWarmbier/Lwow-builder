using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "Sound Config", menuName = "Configs/Sound Config", order = 1)]
public class SoundConfig : ScriptableObject
{
    [SerializeField] List<Sound> soundList;

    public AudioSource GetSound(SoundType type)
    {
        return soundList.FirstOrDefault(r => r.type == type).soundEffect;
    }

    [Serializable]
    private class Sound
    {
        [SerializeField] public SoundType type;
        [SerializeField] public AudioSource soundEffect;
    }

    public enum SoundType
    {
        Rock,
        Tree,
        Building
    }
}
