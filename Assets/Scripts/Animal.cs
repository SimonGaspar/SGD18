using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Animal", order = 1)]
public class Animal : ScriptableObject
{
    public string Name;
    public float JumpTakeOffSpeed;
    public float MaxSpeed;
    public bool CanFly;

    public Color Color;
    public Animator Animator;
}
