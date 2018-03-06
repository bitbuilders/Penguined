using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyIntroData", menuName = "Intro/EnemyIntro", order = 1)]
public class EnemyIntroData : ScriptableObject
{
    public string title;
    [TextArea] public string shortDescription;
    public float duration;
    public Color titleColor;
    public Color descriptionColor;
    public GameObject[] enemies;
}
