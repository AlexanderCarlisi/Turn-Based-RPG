using System;

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

    private static readonly float PERCENT_COST_RATIO = 0.5f;
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
