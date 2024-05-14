using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown
{
    [SerializeField] private float cooldownTime;
    private float _nextThrowTime;

    public bool isCoolingDown => Time.time > _nextThrowTime;
    public void startCooldown() => _nextThrowTime = Time.time + cooldownTime;
}
