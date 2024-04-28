using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandlerScript : MonoBehaviour {

    private static BattleHandlerScript instance;
    private Battle battle;
    private string infoTextMessage;


    // Start is called before the first frame update
    void Start() {
        instance = this;
        infoTextMessage = "Battle Started";
    }


    // Update is called once per frame
    void FixedUpdate() {
        if (battle != null) {
            BattleUIScript.updateStatusPanel(battle.party, battle.enemies, battle.currentUnit);
            BattleUIScript.setInfoText(infoTextMessage);
        }
    }


    /// <summary>
    /// Sets the info text to be displayed to the user.
    /// </summary>
    /// <param name="message"></param>
    public void setInfoText(string message) {
        infoTextMessage = message;
    }


    /// <summary>
    /// Starts a battle between the player and a group of enemies.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemies"></param>
    public static void startBattle(Player player, Enemy[] enemies) {
        instance.battle = new Battle();
        instance.battle.startBattle(player, enemies);
    }


    /// <summary>
    /// Interface for selecting a skill for the current unit to use.
    /// </summary>
    /// <param name="i"></param>
    public static void selectSkill(int i) {
        instance.battle.selectSkill(i);
    }


    /// <summary>
    /// Interface for selecting a target for the current unit to attack.
    /// </summary>
    /// <param name="i"></param>
    public static void selectTarget(int i) {
        instance.battle.selectTarget(i);
    }


    /// <summary>
    /// Opens the panel for selecting a Target to hit.
    /// </summary>
    public static void openEnemyTargetPanel() {
        BattleUIScript.openTargetPanel(instance.battle.enemies);
    }


    private class Battle {
        public Player player;
        public PartyMember[] party;
        public Enemy[] enemies;

        private int turnIndex;
        public Unit currentUnit;
        private List<Unit> turnOrder;

        private Skill chosenSkill;
        private Unit chosenTarget;


        /// <summary>
        /// Starts a battle between the player and a group of enemies.
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="Enemies"></param>
        public void startBattle(Player Player, Enemy[] Enemies) {
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
        public void restartTurnValues() {
            chosenSkill = null;
            chosenTarget = null;
            BattleUIScript.closeAllPanels();
        }


        /// <summary>
        /// Moves to the next turn in the battle.
        /// </summary>
        public void nextTurn() {
            if (isFinished()) {
                endBattle();
                return;
            }
            restartTurnValues();

            for (int i = 0; i < turnOrder.Count; i++) 
                if (!turnOrder[i].isAlive()) turnOrder.RemoveAt(i);
            
            turnOrder.Sort((a, b) => a.getAgility().CompareTo(b.getAgility()));

            turnIndex++;
            if (turnIndex >= turnOrder.Count) turnIndex = 0;
            currentUnit = turnOrder[turnIndex];
            BattleUIScript.setUnit(currentUnit);
            // BattleUIScript.updateStatusPanel(party, enemies, currentUnit);
        
            if (currentUnit is Enemy enemy) enemyTurn();
        }


        /// <summary>
        /// Checks if the battle has ended.
        /// </summary>
        /// <returns></returns>
        private bool isFinished() {
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
        public void endBattle() {
            // Display to User.
        }


        /// <summary>
        /// Selects a skill for the current unit to use.
        /// Used by Unity Buttons.
        /// </summary>
        /// <param name="i"></param>
        public void selectSkill(int i) {
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
        public void selectTarget(int i) {
            if (chosenSkill == null) {
                chosenTarget = enemies[i];
                melee();
                return;
            }

            if (chosenSkill is AttackSkill attackSkill) {
                chosenTarget = enemies[i];
                useAttackSkill(attackSkill);
            }
            else if (chosenSkill is HealSkill healSkill) {
                chosenTarget = party[i];
                heal(healSkill);
            }

            currentUnit.chargeCost(chosenSkill.getType(), chosenSkill.getCost());
            nextTurn();
        }


        /// <summary>
        /// Handles the enemy's turn in the battle.
        /// </summary>
        public void enemyTurn() {
            if (currentUnit is Enemy enemy) {
                BattleAction action = enemy.getAction(party, enemies);

                switch(action.getAction()) {
                    case Enums.BattleAction.Skill: {
                        chosenSkill = action.getSkill();
                        chosenTarget = action.getTarget();
                        if (chosenSkill is AttackSkill attackSkill) {
                            useAttackSkill(attackSkill);
                        }
                        else if (chosenSkill is HealSkill healSkill) {
                            heal(healSkill);
                        }
                        currentUnit.chargeCost(chosenSkill.getType(), chosenSkill.getCost());
                        break;
                    }

                    case Enums.BattleAction.Weapon: {
                        chosenTarget = action.getTarget();
                        melee();
                        break;
                    }
                    
                } 
            }
            
            nextTurn();
        }


        /// <summary>
        /// Attacks the chosen target with a melee attack.
        /// </summary>
        private void melee() {
            Weapon weapon = currentUnit.getWeapon();
            int damage = weapon.getDamage();
            int hitRate = weapon.getHitRate();

            // Evasion Check
            if (UnityEngine.Random.Range(0, 100) > hitRate) {
                instance.setInfoText(chosenTarget.getName() + " dodged the attack!");
            } else {
                instance.setInfoText(currentUnit.getName() + " dealt " + damage + " damage to " + chosenTarget.getName());
                chosenTarget.damage(damage);
            }
        }


        /// <summary>
        /// Heals the chosen target by the amount specified in the heal skill.
        /// </summary>
        /// <param name="healSkill"></param>
        private void heal(HealSkill healSkill) {
            int amount = healSkill.isPercentBased() ? 
                (int) Math.Floor((double) chosenTarget.getMaxHp() * healSkill.getAmount()) : healSkill.getAmount();
            chosenTarget.heal(amount);
            BattleUIScript.setInfoText(currentUnit.getName() + " healed " + chosenTarget.getName() + " for " + amount + " HP");
        }


        /// <summary>
        /// Attacks the chosen target with the chosen attack skill.
        /// </summary>
        /// <param name="attackSkill"></param>
        private void useAttackSkill(AttackSkill attackSkill) {
            // Evasion Check
            if (evasionCheck(attackSkill, currentUnit, chosenTarget)) {
                instance.setInfoText(chosenTarget.getName() + " dodged the attack!");
                return;
            }

            // Damage Calculation
            int damage = damageCalc(attackSkill, currentUnit, chosenTarget);
            chosenTarget.damage(damage);
            instance.setInfoText(currentUnit.getName() + " dealt " + damage + " damage to " + chosenTarget.getName());
        }


        // /// <summary>
        // /// Wait Command
        // /// </summary>
        // /// <param name="seconds"></param>
        // private void waitSeconds(float seconds) {
        //     StartCoroutine(wait(seconds));
        // }
        // private IEnumerator wait(float seconds) {
        //     yield return new WaitForSeconds(seconds);
        // }


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

    public Enums.BattleAction getAction() {
        return action;
    }

    public Skill getSkill() {
        return skill;
    }

    public Unit getTarget() {
        return target;
    }
}


public static class BattleSim {

    /// <summary>
    /// Simulates the Damage Calculation that would Theoretically occur in a Battle.
    /// No RNG Involved.
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
        int luckRoll = (int) Math.Floor(baseDamage * (luckStat / 198f) + 1);
        int damageRoll = (minRoll + maxRoll) / 2 + luckRoll;

        int scaleStat = (attackSkill.getType() == Enums.SkillType.Physical) ? attacker.getStrength() : attacker.getIntelligence();
        int targetDefense = targetUnit.getDefense();
        int damage = (int) Math.Floor(damageRoll + ((scaleStat - targetDefense) / 100f * damageRoll));

        return damage;
    }
}