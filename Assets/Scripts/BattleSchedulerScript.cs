using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSchedulerScript : MonoBehaviour {

    private static Player player;
    private static PartyMember[] party;
    private static Enemy[] enemies;


    public static void startup(Player _Player, Enemy[] Enemies) {
        player = _Player;
        party = player.getParty();
        enemies = Enemies;
    }


    public static void startBattle() {
        BattleHandlerScript.startBattle(player, enemies);
    }
}
