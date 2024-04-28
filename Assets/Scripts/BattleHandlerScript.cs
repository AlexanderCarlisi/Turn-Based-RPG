using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandlerScript : MonoBehaviour {    
    private static Player player;
    private static PartyMember[] party;
    private static Enemy[] enemies;

    private static int turnIndex;
    private static Unit currentUnit;
    private static List<Unit> turnOrder;

    private static Skill chosenSkill;
    private static Unit chosenTarget;


    /// <summary>
    /// Starts a battle between the player and a group of enemies.
    /// </summary>
    /// <param name="Player"></param>
    /// <param name="Enemies"></param>
    public static void startBattle(Player Player, Enemy[] Enemies) {
        player = Player;
        party = player.getParty();
        enemies = Enemies;

        turnIndex = -1; // incremented by nextTurn()
        turnOrder = new List<Unit>();
        
        foreach (PartyMember member in party) {
            turnOrder.Add(member);
        }

        foreach (Enemy enemy in enemies) {
            turnOrder.Add(enemy);
        }

        nextTurn();
    }


    /// <summary>
    /// Resets the values for the current turn. Closes UI panels.
    /// </summary>
    public static void restartTurnValues() {
        chosenSkill = null;
        chosenTarget = null;
        BattleUIScript.closeAllPanels();
    }


    /// <summary>
    /// Moves to the next turn in the battle.
    /// </summary>
    public static void nextTurn() {
        if (isFinished()) {
            endBattle();
            return;
        }
        restartTurnValues();

        turnOrder.ForEach(unit => {
            if (!unit.isAlive()) {
                turnOrder.Remove(unit);
            }
        });
        turnOrder.Sort((a, b) => a.getAgility().CompareTo(b.getAgility()));

        turnIndex++;
        if (turnIndex >= turnOrder.Count) turnIndex = 0;
        currentUnit = turnOrder[turnIndex];
        BattleUIScript.setUnit(currentUnit);
        BattleUIScript.updateStatusPanel(party, enemies, currentUnit);

        
        // if (currentUnit is Enemy enemy)  
    }


    /// <summary>
    /// Checks if the battle has ended.
    /// </summary>
    /// <returns></returns>
    private static bool isFinished() {
        bool partyAlive = false;
        bool enemiesAlive = false;

        foreach (PartyMember member in party) {
            if (member.isAlive()) {
                partyAlive = true;
                break;
            }
        }

        foreach (Enemy enemy in enemies) {
            if (enemy.isAlive()) {
                enemiesAlive = true;
                break;
            }
        }

        return !partyAlive || !enemiesAlive;
    }


    /// <summary>
    /// Ends the battle. Displays the results to the user.
    /// </summary>
    public static void endBattle() {
        // Display to User.
    }


    /// <summary>
    /// Selects a skill for the current unit to use.
    /// Used by Unity Buttons.
    /// </summary>
    /// <param name="i"></param>
    public static void selectSkill(int i) {
        if (currentUnit == null) {
            Debug.LogError("No unit selected");
            return;
        }
        if (i >= currentUnit.getSkills().Length) {
            Debug.LogError("Invalid skill index");
            return;
        }

        chosenSkill = currentUnit.getSkills()[i];

        // Check Skill Requirements
        if (!currentUnit.checkCost(chosenSkill.getType(), chosenSkill.getCost())) {
            // display error message
            chosenSkill = null;
            return;
        }

        // Open Target Selection
        if (chosenSkill is AttackSkill) {
            BattleUIScript.openTargetPanel(enemies);
        }
        else if (chosenSkill is HealSkill) {
            BattleUIScript.openTargetPanel(party);
        }
        
        // Perform Unique Action
        else if (chosenSkill is UniqueSkill uniqueSkill) {
            uniqueSkill.action();
        }
    }


    /// <summary>
    /// Selects a target for the current unit to attack.
    /// Used by Unity Buttons.
    /// </summary>
    /// <param name="i"></param>
    public static void selectTarget(int i) {
        if (chosenSkill == null) {
            Debug.LogError("No skill selected");
            return;
        }

        if (chosenSkill is AttackSkill attackSkill) {
            chosenTarget = enemies[i];
            attack(attackSkill);
        }
        else if (chosenSkill is HealSkill healSkill) {
            chosenTarget = party[i];
            heal(healSkill);
        }

        currentUnit.chargeCost(chosenSkill.getType(), chosenSkill.getCost());
        nextTurn();
    }


    /// <summary>
    /// Heals the chosen target by the amount specified in the heal skill.
    /// </summary>
    /// <param name="healSkill"></param>
    private static void heal(HealSkill healSkill) {
        int amount = healSkill.isPercentBased() ? 
            (int) Math.Floor((double) chosenTarget.getMaxHp() * healSkill.getAmount()) : healSkill.getAmount();
        chosenTarget.heal(amount);
        BattleUIScript.setInfoText("Healed " + chosenTarget.getName() + " for " + amount + " HP");
    }


    /// <summary>
    /// Attacks the chosen target with the chosen attack skill.
    /// </summary>
    /// <param name="attackSkill"></param>
    private static void attack(AttackSkill attackSkill) {
        // Evasion Check
        if (evasionCheck(attackSkill, currentUnit, chosenTarget)) {
            BattleUIScript.setInfoText(chosenTarget.getName() + " dodged the attack!");
            return;
        }

        // Damage Calculation
        int damage = damageCalc(attackSkill, currentUnit, chosenTarget);

        chosenTarget.damage(damage);
        BattleUIScript.setInfoText("Dealt " + damage + " damage to " + chosenTarget.getName());
    }


    /// <summary>
    /// Calculates the damage dealt by the attacker to the target unit.
    /// </summary>
    /// <param name="attackSkill"></param>
    /// <param name="attacker"></param>
    /// <param name="targetUnit"></param>
    /// <returns></returns>
    public static int damageCalc(AttackSkill attackSkill, Unit attacker, Unit targetUnit) {
        int baseDamage = attackSkill.getBaseDamage();
        int luckStat = attacker.getLuck();

        int minRoll = baseDamage - (baseDamage / 3);
        int maxRoll = baseDamage + (baseDamage / 5);
        int luckRoll = (int) UnityEngine.Random.Range(0, (baseDamage * (luckStat / 198f)) + 1);
        int damageRoll = UnityEngine.Random.Range(minRoll, maxRoll) + luckRoll;

        int scaleStat = (attackSkill.getType() == Enums.SkillType.Physical) ? attacker.getStrength() : attacker.getIntelligence();
        int targetDefense = targetUnit.getDefense();
        int damage = (int) Math.Floor(damageRoll + ((scaleStat - targetDefense) / 100f * damageRoll));

        return damage;
    }


    /// <summary>
    /// Checks if the target unit evades the attack.
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="attacker"></param>
    /// <param name="targetUnit"></param>
    /// <returns></returns>
    public static bool evasionCheck(Skill skill, Unit attacker, Unit targetUnit) {
        int unitHitRate = (skill.getType() == Enums.SkillType.Physical) ? attacker.getAgility() : attacker.getSpirit();

        int targetLuck = targetUnit.getLuck();
        int targetDodgeChance = UnityEngine.Random.Range(-targetLuck, targetLuck);
        int targetDodgeRate = targetUnit.getAgility() + targetDodgeChance;

        return UnityEngine.Random.Range(0, 100) < targetDodgeRate - unitHitRate;
    }
}


public class BattleAction {
    private Enums.BattleAction action;
    private Skill skill;
    private Unit target;

    public BattleAction(Enums.BattleAction Action, Skill Skill, Unit Target) {
        action = Action;
        skill = Skill;
        target = Target;
    }
}