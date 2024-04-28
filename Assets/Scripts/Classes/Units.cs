using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Unit {

    private string name;
    private int hp;
    private int maxHp;
    private int sp;
    private int maxSp;
    private int level;

    private Skill[] skills;

    private int strength;
    private int intelligence;
    private int endurence;
    private int defense;
    private int agility;
    private int spirit;
    private int luck;

    public Unit(string Name, int Hp, int Sp, int Level, int[] Stats, Skill[] Skills) {
        name = Name;
        hp = Hp;
        sp = Sp;
        maxHp = hp;
        maxSp = sp;
        level = Level;

        setStatsToArray(Stats);

        skills = Skills;
    }

    public string getName() {
        return name;
    }

    public int getHp() {
        return hp;
    }

    public int getMaxHp() {
        return maxHp;
    }

    public int getSp() {
        return sp;
    }

    public int getMaxSp() {
        return maxSp;
    }

    public int getLevel() {
        return level;
    }

    public int getStrength() {
        return strength;
    }

    public int getIntelligence() {
        return intelligence;
    }

    public int getEndurence() {
        return endurence;
    }

    public int getDefense() {
        return defense;
    }

    public int getAgility() {
        return agility;
    }

    public int getSpirit() {
        return spirit;
    }

    public int getLuck() {
        return luck;
    }

    public Skill[] getSkills() {
        return skills;
    }

    /// <summary>
    /// Returns the stats of the unit as an array.
    /// Order: strength, intelligence, endurence, defense, agility, spirit, luck
    /// </summary>
    /// <returns></returns>
    public int[] getStatsAsArray() {
        return new int[] {strength, intelligence, endurence, defense, agility, spirit, luck};
    }
    
    /// <summary>
    /// Sets the stats of the unit from an array.
    /// </summary>
    /// <param name="statArray"></param>
    public void setStatsToArray(int[] statArray) {
        strength = statArray[0];
        intelligence = statArray[1];
        endurence = statArray[2];
        defense = statArray[3];
        agility = statArray[4];
        spirit = statArray[5];
        luck = statArray[6];
    }

    /// <summary>
    /// Heals the unit by the given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void heal(int amount) {
        hp += amount;
        if (hp > maxHp) hp = maxHp;
    }

    /// <summary>
    /// Damages the unit by the given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void damage(int amount) {
        hp -= amount;
        if (hp < 0) hp = 0;
    }

    /// <summary>
    /// Charges the cost of the skill.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cost"></param>
    public void chargeCost(Enums.SkillType type, int cost) {
        if (type == Enums.SkillType.Physical) {
            hp -= cost;
            if (hp < 0) hp = 0;
        }
        else if (type == Enums.SkillType.Magical) {
            sp -= cost;
            if (sp < 0) sp = 0;
        }
    }

    /// <summary>
    /// Checks if the unit has enough cost to use the skill.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool checkCost(Enums.SkillType type, int cost) {
        if (type == Enums.SkillType.Physical) {
            return hp >= cost;
        }
        else if (type == Enums.SkillType.Magical) {
            return sp >= cost;
        }
        return false;
    }

    /// <summary>
    /// Checks if the unit is alive.
    /// </summary>
    /// <returns></returns>
    public bool isAlive() {
        return hp > 0;
    }
}


public class PartyMember : Unit {
    
    private int exp;
    // private int expToNextLevel;
    
    public PartyMember(string Name, int Hp, int Sp, int Level, int[] Stats, Skill[] Skills) : base(Name, Hp, Sp, Level, Stats, Skills) {
        exp = 0;
        // expToNextLevel = 0;
    }
    
    public int getExp() {
        return exp;
    }

    // public int getExpToNextLevel() {
    //     return expToNextLevel;
    // }

    // public void addExp(int Exp) {
    //     exp += Exp;
    //     if (exp >= expToNextLevel) {
    //         levelUp();
    //     }
    // }

    // private void levelUp() {
    //     level++;
    //     exp -= expToNextLevel;
    //     expToNextLevel = (int) Math.Floor(expToNextLevel * 1.5);
    //     maxHp = (int) Math.Floor(maxHp * 1.1);
    //     maxSp = (int) Math.Floor(maxSp * 1.1);
    //     hp = maxHp;
    //     sp = maxSp;
    // }
}


public class Enemy : Unit {
    private static readonly float EXP_RATIO = 2.564f;

    private int expDrop;
    private Dictionary<Item, int> itemDrops;
    private Enums.AlgorithemType algorithem;

    public Enemy(string Name, int Hp, int Sp, int Level, int[] Stats, Skill[] Skills, Enums.AlgorithemType Type) : base(Name, Hp, Sp, Level, Stats, Skills) {
        expDrop = (int) Math.Floor(EXP_RATIO * getLevel());
        itemDrops = new Dictionary<Item, int>();
        algorithem = Type;
    }

    public Enemy(string Name, int Hp, int Sp, int Level, int[] Stats, Skill[] Skills, Dictionary<Item, int> ItemDrops) : base(Name, Hp, Sp, Level, Stats, Skills) {
        expDrop = (int) Math.Floor(EXP_RATIO * getLevel());
        itemDrops = ItemDrops;
        algorithem = Enums.AlgorithemType.TeamPlayer;
    }

    public Enemy(string Name, int Hp, int Sp, int Level, int[] Stats, Skill[] Skills, Enums.AlgorithemType Type, Dictionary<Item, int> ItemDrops) : base(Name, Hp, Sp, Level, Stats, Skills) {
        expDrop = (int) Math.Floor(EXP_RATIO * getLevel());
        itemDrops = ItemDrops;
        algorithem = Type;
    }

    public int getExpDrop() {
        return expDrop;
    }

    public Dictionary<Item, int> getItemDrops() {
        return itemDrops;
    }

    public Enums.AlgorithemType getAlgorithem() {
        return algorithem;
    }

    public BattleAction getAction(PartyMember[] party, Enemy[] enemies) {
        Unit target = party[0];
        Skill skill = null;
        Enums.BattleAction actionType = Enums.BattleAction.Weapon;


        switch (algorithem) {
            case Enums.AlgorithemType.Attacker: {
                // check for buffs and buff skill, if not buffed buff themselves
                // else attack a random party member

                // Get Random Target
                target = party[UnityEngine.Random.Range(0, party.Length)];

                // Check if it should use a Skill or it's Weapon
                bool useSkill = UnityEngine.Random.Range(0, 1) == 1;
                if (useSkill) {
                    skill = getSkills()[UnityEngine.Random.Range(0, getSkills().Length)];
                    actionType = Enums.BattleAction.Skill;

                    // Check if the cost is affordable, if not use recursion and try again
                    if (!checkCost(skill.getType(), skill.getCost())) {
                        return getAction(party, enemies);
                    }

                } else { // Use the Weapon
                    actionType = Enums.BattleAction.Weapon;
                }
                break;
            }
                
            case Enums.AlgorithemType.Support: {
                // check if any teammate is at or below 30% hp, if so heal them
                // else buff a random teammate that isn't already buffed.

                // Get first Healing Skill that is affordable
                foreach (Skill sk in getSkills()) {
                    if (sk is HealSkill && checkCost(skill.getType(), skill.getCost())) {
                        // Get for first teammate that is at or below 30% hp
                        foreach (Enemy enemy in enemies) {
                            if (enemy.getHp() <= enemy.getMaxHp() * 0.3) {
                                skill = sk;
                                actionType = Enums.BattleAction.Skill;
                                target = enemy;
                                return new BattleAction(actionType, skill, target);
                            }
                        }
                        break; // No target to heal
                    }
                    // make an array of skills that are buffs/debuffs
                }
                break;
            }
                
            case Enums.AlgorithemType.Tank: {
                // Attack opponent with the highest current Hp
                // Use the skill that does the most damage and within cost

                // Get the opponent with the highest current Hp
                foreach (PartyMember member in party) {
                    if (member.getHp() > target.getHp()) {
                        target = member;
                    }
                }

                // Get the skill that does the most damage and within cost
                foreach (Skill sk in getSkills()) {
                    if (skill == null && sk is AttackSkill atkSkil && checkCost(sk.getType(), sk.getCost())) {
                        skill = atkSkil;
                        actionType = Enums.BattleAction.Skill;
                    }
                    else if (sk is AttackSkill atkSkill && skill is AttackSkill topSkill && checkCost(sk.getType(), sk.getCost())) {
                        if (skill == null || atkSkill.getBaseDamage() > topSkill.getBaseDamage()) {
                            skill = atkSkill;
                        }
                    }
                }

                // If there is no Attack skills wihin cost
                if (skill == null) actionType = Enums.BattleAction.Weapon;
                break;
            }
                
            case Enums.AlgorithemType.Sniper: {
                // Attack opponent with the lowest current Hp
                // If the opponent is dead, get the next lowest
                foreach (PartyMember member in party) {
                    if (member.getHp() < target.getHp() && member.isAlive()) {
                        target = member;
                    }
                }

                // If the target is at 10% or less hp, check each attackSkill for one that will kill
                AttackSkill strongestSkill = null;
                if (target.getHp() <= target.getMaxHp() * 0.1) foreach (Skill sk in getSkills()) {
                    // Look for attack skill within cost
                    if (sk is AttackSkill atkSkill && checkCost(sk.getType(), sk.getCost())) {
                        // Keep track of Strongest Skill.
                        if (strongestSkill == null || atkSkill.getBaseDamage() > strongestSkill.getBaseDamage()) {
                            strongestSkill = atkSkill;
                        }

                        // Simulate damage to the target and check if it will die, DOESN'T necessarily mean they WILL die.
                        int damage = BattleHandlerScript.damageCalc(atkSkill, this, target);
                        // If multiple skills will kill, use the one with the lowest cost.
                        if (target.getHp() - damage <= 0 && atkSkill.getCost() < skill.getCost()) {
                            skill = atkSkill;
                            actionType = Enums.BattleAction.Skill;
                            break;
                        }
                    }
                }

                // If no skill will kill, use the strongest skill.
                if (skill == null && strongestSkill != null) {
                    skill = strongestSkill;
                    actionType = Enums.BattleAction.Skill;
                } else { // If there are no AttackSkills within Cost use Weapon
                    actionType = Enums.BattleAction.Weapon;
                }

                break;
            }
                
            // case Enums.AlgorithemType.TeamPlayer:
            // Requires implementation of BattleTurn history 
        }

        return new BattleAction(actionType, skill, target);
    }
}