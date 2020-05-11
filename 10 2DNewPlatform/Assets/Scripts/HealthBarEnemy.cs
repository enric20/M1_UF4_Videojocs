using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    //public Text numericHP;
    //private int currentHP;
    //private int maxHP;

    private void Start()
    {
        //numericHP.text = currentHP + " | " + maxHP;
        //numericHP.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        //maxHP = health;
        //currentHP = health;
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);

    }

    public void SetHealth(int health)
    {
        //currentHP = health;
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        //numericHP.text = currentHP + " | " + maxHP;
        //numericHP.color = gradient.Evaluate(slider.normalizedValue);
    }

}