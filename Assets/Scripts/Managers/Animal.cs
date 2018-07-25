using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Animal", order = 1)]
public class Animal : ScriptableObject
{
    public AnimalForm AnimalForm;
    public int RequiredParts;

    public Color AnimalColor;
    public bool Claimed;

    [Header("Top left panel sprites")]
    public Sprite[] AnimalSprites;
}
