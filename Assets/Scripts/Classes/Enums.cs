public static class Enums {

    /// <summary>
    /// Elements a Damaging Skill can be, other skills will default to None.
    /// </summary>
    public enum Element {
        None, // Buffs/Debuffs/Heal
        Physical,
        Fire,
        Water,
        Wind,
        Elec,
        Earth,
        Ice,
        Almighty // Skills that don't fall into the previous catagories
    }


    /// <summary>
    /// A physical or a magical skill. Used to dictate stats and wheither to charge Hp or Sp the cost.
    /// </summary>
    public enum SkillType {
        Physical,
        Magical
    }


    // Making these different Subclasses instead

    // /// <summary>
    // /// The Action the skill will perform. Dictates targets.
    // /// </summary>
    // public enum SkillType {
    //     Attack,
    //     Healing,
    //     Buff,
    //     Debuff,
    //     Passive,
    //     Unqiue // Scripted
    // }


    /// <summary>
    /// Status Effects that are going to have a direct effect on the battle.
    /// 
    /// ie: Sleep, you lose a turn.
    ///
    /// Statuses that deal damage are excluded since they're handled in their class.
    /// These Statuses are handled in the Battle class.
    /// </summary>
    public enum Status {
        None,
        Asleep,
        Dead,

    }


    /// <summary>
    /// Algorithems the Enemies will follow.
    /// </summary>
    public enum AlgorithemType {
        Attacker, // Buffs themselves, attacks at random
        Support, // Buffs/Debuffs, Heals those in need
        Tank, // Damage Highest health opponents
        Sniper, // Damage Lowest Health opponents
        TeamPlayer, // Balanced: heals, and attacks with the team
    }
    

    /// <summary>
    /// The type of weapon the unit is using.
    /// </summary>
    public enum WeaponType {
        Melee,
        Ranged
    }


    /// <summary>
    /// Optimized for the Skill class. Use Enums instead of Strings also AutoFill.
    /// </summary>
    public static class SkillNames {
        public enum Attack {
            Slash,
            Fireball,
            IceBlast,
            Thunder,
            Earthquake,
            WindSlash,
            PiercingHalos
        }

        public enum Heal {
            Basic,
            Intermediate,
            Advanced,
            Mastered
        }
    }


    /// <summary>
    /// The type of action the player and enemy can take in battle.
    /// </summary>
    public enum BattleAction {
        Weapon,
        Skill,
        Item,
        Guard
    }
}
