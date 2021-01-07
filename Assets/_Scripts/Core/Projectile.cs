using UnityEngine;

public enum ProjectileType
{
    rock, arrow, fireball
};

public enum EffectAttack
{
    ArmorP, ArrmorIgnor, Slowdown, Freezing
};

public class Projectile : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    private int attackDmage;
    [SerializeField]
    ProjectileType pType;
    [SerializeField]
    EffectAttack effect;
    [SerializeField]
    int effectValue;

    Unit unitAttacker;

    public Unit UnitAttacker
    {
        get
        {
            return unitAttacker;
        }
        set
        {
            unitAttacker = value;
        }
    }

    public int AttackDamage
    {
        get
        {
            return attackDmage;
        }
    }

    public ProjectileType PType
    {
        get
        {
            return pType;
        }
    }

    public EffectAttack Effect
    {
        get
        {
            return effect;
        }
    }

    public int EffectValue
    {
        get
        {
            return effectValue;
        }
    }
}
