using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEventSystem
{

    public static event Action TriggerDiceMove;
    public static void DiceMoved()
    {
        TriggerDiceMove();
    }
}
