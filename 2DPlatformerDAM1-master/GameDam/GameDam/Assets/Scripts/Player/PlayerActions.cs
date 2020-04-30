using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public static int physicalAttack(int playerDamage, int enemyDefense)
    {
        float attackFloat = playerDamage;
       

        if (enemyDefense > 0 && enemyDefense < 100)
        {
            float defense = (float) enemyDefense / 100;
            attackFloat = (int) playerDamage*defense;
        }

        else if (enemyDefense >= 100)
        {
            attackFloat = 0;
        }

        int attackInt = (int) attackFloat;
        return attackInt;
    }
}