using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFox : CharacterAttributes
{

    public CharacterFox()
    {
        MinGroundNormalY = 1f;
        JumpTakeOffSpeed = 14f;
        MaxSpeed = 14f;
        CanFly = true;
    }
}
