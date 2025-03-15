using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscenes/Cutscene Data")]
public class CutsceneData : ScriptableObject
{
    [System.Serializable]
    public class SlideData
    {
        [Tooltip("The texture (raw image) to display for this slide")]
        public Texture image; // Changed from Sprite to Texture

        [Tooltip("Text caption to show with this slide")]
        [TextArea(3, 5)]
        public string caption;

        [Tooltip("How long to display this slide (in seconds)")]
        [Range(0.5f, 30f)]
        public float duration = 3.0f;

        [Tooltip("Audio clip to play when this slide appears")]
        public AudioClip slideAudio;
    }

    [Header("Cutscene Information")]
    [Tooltip("Name of this cutscene sequence")]
    public string cutsceneName;

    [Tooltip("Description or notes about this cutscene")]
    [TextArea(2, 4)]
    public string description;

    [Header("Slides")]
    [Tooltip("List of all slides in this cutscene")]
    public List<SlideData> slides = new List<SlideData>();

    [Header("Audio")]
    [Tooltip("Background music to play during the cutscene")]
    public AudioClip backgroundMusic;

    [Header("Settings")]
    [Tooltip("Speed of fade transitions (higher = faster)")]
    [Range(0.5f, 5f)]
    public float fadeSpeed = 1.0f;

    [Tooltip("Whether the player can skip this cutscene")]
    public bool allowSkipping = true;
}