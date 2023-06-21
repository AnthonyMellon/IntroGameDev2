using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemy", menuName = "Enemy/New Enemy")]
public class EnemyConfig : ScriptableObject
{
    public AnimatorController animatorController;
    public float health;
    public float speed;
}
