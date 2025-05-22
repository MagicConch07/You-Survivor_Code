using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "SO/WeaponStat")]
public class WeaponStat : ScriptableObject
{
    [FormerlySerializedAs("magazine")] public Stat maxMagazine;
    public Stat firerate;
    public Stat range;
    public Stat bulletSpeed;
    public Stat knockBackPower;
    public Stat reloading;
    
    protected Weapon _owner;
    protected Dictionary<WeaponStatType, Stat> _statDictionary;

    public virtual void SetWeapon(Weapon owner)
    {
        _owner = owner;
    }
    
    public virtual void IncreaseStatFor(int value, float duration, Stat targetStat)
    {
        _owner.StartCoroutine(StatModifyCoroutine(value, duration, targetStat));
    }

    protected IEnumerator StatModifyCoroutine(int value, float duration, Stat targetStat)
    {
        targetStat.AddModifier(value);
        yield return new WaitForSeconds(duration);
        targetStat.RemoveModifier(value);
    }

    protected virtual void OnEnable()
    {
        _statDictionary = new Dictionary<WeaponStatType, Stat>();

        Type agentStatType = typeof(WeaponStat);

        foreach(WeaponStatType typeEnum in Enum.GetValues(typeof(WeaponStatType)))
        {
            try
            {
                string fieldName = LowerFirstChar(typeEnum.ToString());
                FieldInfo statField = agentStatType.GetField(fieldName);
                _statDictionary.Add(typeEnum, statField.GetValue(this) as Stat);
            }catch(Exception ex)
            {
                Debug.LogError($"There are no stat - {typeEnum.ToString()} {ex.Message}");
            }
        }
    }

    private string LowerFirstChar(string input) 
        => $"{char.ToLower(input[0])}{input.Substring(1)}";
    

    public void AddModifier(WeaponStatType type, int value)
    {
        _statDictionary[type].AddModifier(value);
    }

    public void RemoveModifier(WeaponStatType type, int value)
    {
        _statDictionary[type].RemoveModifier(value);
    }
}
