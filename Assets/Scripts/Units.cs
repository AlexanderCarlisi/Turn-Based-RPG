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

        strength = Stats[0];
        intelligence = Stats[1];
        endurence = Stats[2];
        defense = Stats[3];
        agility = Stats[4];
        spirit = Stats[5];
        luck = Stats[6];

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
    
    public void setStatsToArray(int[] statArray) {
        strength = statArray[0];
        intelligence = statArray[1];
        endurence = statArray[2];
        defense = statArray[3];
        agility = statArray[4];
        spirit = statArray[5];
        luck = statArray[6];
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
}