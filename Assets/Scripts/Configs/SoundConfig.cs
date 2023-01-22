using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Configs/SoundConfig", order = 1)]
public class SoundConfig : ScriptableObject
{
    [SerializeField] List<Sound> soundList;

    public AudioClip GetSound(SoundType type)
    {
        return soundList.FirstOrDefault(r => r.type == type).soundEffect;
    }

    [Serializable]
    private class Sound
    {
        [SerializeField] public SoundType type;
        [SerializeField] public AudioClip soundEffect;
    }

    public enum SoundType
    {
        Rock,
        Tree,
        Building
    }
}
