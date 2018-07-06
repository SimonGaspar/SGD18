using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFox : CharacterAttributes{

    public CharacterFox() {
        GravityModifier = 0.5f;
        MinGroundNormalY = 1f;
        JumpTakeOffSpeed = 14f;
        MaxSpeed = 14f;
    }
}
