using System;
using System.Collections.Generic;

public class Skill {

    private string name;
    private int cost;
    private Enums.SkillType type;

    public Skill(string Name, int Cost, Enums.SkillType Type) {
        name = Name;
        cost = Cost;
        type = Type;
    }

    public string getName() {
        return name;
    }

    public int getCost() {
        return cost;
    }

    public Enums.SkillType getType() {
        return type;
    }


    private static readonly Dictionary<Enums.SkillNames.Attack, AttackSkill> attackSkills = new Dictionary<Enums.SkillNames.Attack, AttackSkill>() {
        {Enums.SkillNames.Attack.Slash, new AttackSkill("Slash", Enums.Element.Physical, 10)},
        {Enums.SkillNames.Attack.Fireball, new AttackSkill("Fireball", Enums.Element.Fire, 20)},
        {Enums.SkillNames.Attack.IceBlast, new AttackSkill("Ice Blast", Enums.Element.Ice, 20)},
        {Enums.SkillNames.Attack.Thunder, new AttackSkill("Thunder", Enums.Element.Elec, 20)},
        {Enums.SkillNames.Attack.Earthquake, new AttackSkill("Earthquake", Enums.Element.Earth, 20)},
        {Enums.SkillNames.Attack.WindSlash, new AttackSkill("Wind Slash", Enums.Element.Wind, 20)},
        {Enums.SkillNames.Attack.PiercingHalos, new AttackSkill("Piercing Halos", Enums.Element.Almighty, 30)}
    };

    private static readonly Dictionary<Enums.SkillNames.Heal, HealSkill> healSkills = new Dictionary<Enums.SkillNames.Heal, HealSkill>() {
        {Enums.SkillNames.Heal.Basic, new HealSkill("Basic Heal", false, 10)},
        {Enums.SkillNames.Heal.Intermediate, new HealSkill("Intermediate Heal", false, 20)},
        {Enums.SkillNames.Heal.Advanced, new HealSkill("Advanced Heal", false, 30)},
        {Enums.SkillNames.Heal.Mastered, new HealSkill("Mastered Heal", true, 25)}
    };

    public static AttackSkill getAttackSkill(Enums.SkillNames.Attack key) {
        return attackSkills[key];
    }
    
    public static HealSkill getHealSkill(Enums.SkillNames.Heal key) {
        return healSkills[key];
    }
    
}

public class AttackSkill : Skill {
    private static readonly float COST_RATIO = 0.356f;
    private int damage;
    private Enums.Element element;

    public AttackSkill(string Name, Enums.Element Element, int Damage) : base
    (
        Name, 
        (int) Math.Floor(Damage * COST_RATIO), 
        (Element == Enums.Element.Physical) ? Enums.SkillType.Physical : Enums.SkillType.Magical
    ) {
        element = Element;
        damage = Damage;
    }
    
    public int getBaseDamage() {
        return damage;
    }

    public Enums.Element getElement() {
        return element;
    }
}

public class HealSkill : Skill {

    private static readonly float PERCENT_COST_RATIO = 1.5f;
    private static readonly float COST_RATIO = 0.432f;

    private bool percent;
    private int amount;

    public HealSkill(string Name, bool Percent, int Amount) : base
    (
        Name,
        Percent ? (int) Math.Floor(Amount * PERCENT_COST_RATIO) : (int) Math.Floor(Amount * COST_RATIO),
        Enums.SkillType.Magical
    ) {
        percent = Percent;
        amount = Amount;
    }

    public bool isPercentBased() {
        return percent;
    }

    public int getAmount() {
        return amount;
    }
}

// Put on the backburner
// public class SupportSkill : Skill {

//     public SupportSkill(string Name, Ailment _Ailment, double chance)
// }

public class UniqueSkill : Skill {

    public readonly Action action;

    public UniqueSkill(string Name, int Cost, Enums.SkillType Type, Action _Action) : base(Name, Cost, Type) {
        action = _Action;
    }
}
