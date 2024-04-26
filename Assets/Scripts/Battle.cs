using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Battle {    
    private static Player player;
    private static PartyMember[] party;
    private static Enemy[] enemies;

    private static int turnIndex;
    private static Unit currentUnit;
    private static List<Unit> turnOrder;

    private static Skill chosenSkill;
    private static Unit chosenTarget;

    public static void startBattle(Player player, Enemy[] enemies) {
        Battle.player = player;
        party = player.getParty();
        Battle.enemies = enemies;

        turnIndex = 0;
        turnOrder = new List<Unit>();
        
        foreach (PartyMember member in party) {
            turnOrder.Add(member);
        }

        foreach (Enemy enemy in enemies) {
            turnOrder.Add(enemy);
        }

        turnOrder.Sort((a, b) => a.getAgility().CompareTo(b.getAgility()));

        currentUnit = turnOrder[turnIndex];

    }


    public static void restartTurnValues() {
        chosenSkill = null;
        chosenTarget = null;
    }

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

        // if enemy
    }

    private static bool isFinished() {
        bool partyAlive = false;
        bool enemiesAlive = false;

        foreach (PartyMember member in party) {
            if (member.getHp() > 0) {
                partyAlive = true;
                break;
            }
        }

        foreach (Enemy enemy in enemies) {
            if (enemy.getHp() > 0) {
                enemiesAlive = true;
                break;
            }
        }

        return !partyAlive || !enemiesAlive;
    }

    public static void endBattle() {
        // Display to User.
    }

    public static void selectSkill(int i) {
        if (currentUnit == null) {
            Debug.LogError("No unit selected");
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
            // Open Enemy Target Panel
        }
        else if (chosenSkill is HealSkill) {
            // Open Party Target Panel
        }
        
        // Perform Unique Action
        else if (chosenSkill is UniqueSkill uniqueSkill) {
            uniqueSkill.action();
        }
    }

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
    }

    private static void heal(HealSkill healSkill) {
        int amount = healSkill.isPercentBased() ? 
            (int) Math.Floor((double) chosenTarget.getMaxHp() * healSkill.getAmount()) : healSkill.getAmount();
        chosenTarget.heal(amount);
        // display amount
    }

    private static void attack(AttackSkill attackSkill) {
        // Evasion Check
        int unitHitRate = (attackSkill.getType() == Enums.SkillType.Physical) ? currentUnit.getAgility() : currentUnit.getSpirit();

        int targetLuck = chosenTarget.getLuck();
        int targetDodgeChance = UnityEngine.Random.Range(-targetLuck, targetLuck);
        int targetDodgeRate = chosenTarget.getAgility() + targetDodgeChance;

        // int dodgeZone = targetAgility - unitAgility;
        // if (targetEvasion - unitEvasion > dodgeZone) {
        //     // display miss
        //     nextTurn();
        //     return;
        // }

        // Damage Calculation
        int baseDamage = attackSkill.getBaseDamage();
        int luckStat = currentUnit.getLuck();

        int minRoll = baseDamage - (baseDamage / 3);
        int maxRoll = baseDamage + (baseDamage / 5);
        int luckRoll = (int) UnityEngine.Random.Range(0, (baseDamage * (luckStat / 198f)) + 1);
        int damageRoll = UnityEngine.Random.Range(minRoll, maxRoll) + luckRoll;

        int scaleStat = (attackSkill.getType() == Enums.SkillType.Physical) ? currentUnit.getStrength() : currentUnit.getIntelligence();
        int targetDefense = chosenTarget.getDefense();
        int damage = (int) Math.Floor(damageRoll + ((scaleStat - targetDefense) / 100f * damageRoll));

        chosenTarget.damage(damage);
        // display damage
    }
}
