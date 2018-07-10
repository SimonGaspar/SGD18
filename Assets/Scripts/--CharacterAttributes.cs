using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes
{

    public float GravityModifier { get; set; }
    public float MinGroundNormalY { get; set; }
    public float JumpTakeOffSpeed { get; set; }
    public float MaxSpeed { get; set; }
    public bool CanFly { get; set; }

    public CharacterAttributes()
    {
        GravityModifier = 1f;
        MinGroundNormalY = .65f;
        JumpTakeOffSpeed = 7f;
        MaxSpeed = 7f;
        CanFly = false;
    }
}
