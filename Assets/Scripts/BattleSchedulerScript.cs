using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSchedulerScript : MonoBehaviour {

    private static Player player;
    private static PartyMember[] party;
    private static Enemy[] selectedEnemies;


    private static readonly Enemy[] enemies = new Enemy[] {
        new ("Cripple", 15, 40, 5, new int[]{1, 5, 3, 2, 1, 5, 1}, new Skill[]{
            Skill.getAttackSkill(Enums.SkillNames.Attack.Fireball),
            Skill.getAttackSkill(Enums.SkillNames.Attack.IceBlast),
        }, Enums.AlgorithemType.Sniper),
        
        new ("Scary Cube", 30, 40, 5, new int[]{7, 5, 5, 7, 1, 1, 5}, new Skill[]{
            Skill.getAttackSkill(Enums.SkillNames.Attack.Slash),
            Skill.getAttackSkill(Enums.SkillNames.Attack.PiercingHalos),
        }, Enums.AlgorithemType.Attacker)
    };
    private static readonly Enemy[][] encounters = new Enemy[][] {
        new Enemy[] {enemies[0], enemies[1]}
    };

    public static Enemy getEnemy(int index) {
        return enemies[index];
    }
    public static Enemy[] getEncounter(int index) {
        return encounters[index];
    }


    public static void startup(Player _Player, Enemy[] Enemies) {
        player = _Player;
        party = player.getParty();
        selectedEnemies = Enemies;
    }
    public static void startup(Player _Player, int encounterIndex) {
        player = _Player;
        party = player.getParty();
        selectedEnemies = getEncounter(encounterIndex);
    }


    public static void startBattle() {
        BattleHandlerScript.startBattle(player, selectedEnemies);
    }
    public static void startBattle(Player _Player, int encounterIndex) {
        player = _Player;
        selectedEnemies = getEncounter(encounterIndex);
        startBattle();
    }
}
