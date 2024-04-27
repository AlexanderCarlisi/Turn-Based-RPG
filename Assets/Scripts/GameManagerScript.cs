using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    private static Player player;

    // Start is called before the first frame update
    void Start() {
        player = new Player();

        // Game Tests
        PartyMember[] party = new PartyMember[4];

        party[0] = new PartyMember("Warrior", 10, 10, 1, new int[]{1, 2, 3, 4, 5, 6, 7}, new Skill[]{
            new AttackSkill("SlashySkill1", Enums.Element.Physical, 5),
            new AttackSkill("SlashySkill2", Enums.Element.Physical, 5),
            new AttackSkill("SlashySkill3", Enums.Element.Physical, 5)
        });
        party[1] = new PartyMember("Mage", 10, 20, 2, new int[]{1, 3, 4, 6, 7, 8, 9}, new Skill[]{
            new AttackSkill("FireSkill1", Enums.Element.Fire, 10)
        });

        player.setParty(party);

        Enemy[] enemies = new Enemy[]{
            new Enemy("Goblin", 10, 10, 1, new int[]{1, 2, 3, 4, 5, 6, 7}, new Skill[]{
                new AttackSkill("SlashySkill1", Enums.Element.Physical, 5),
                new AttackSkill("SlashySkill2", Enums.Element.Physical, 5),
                new AttackSkill("SlashySkill3", Enums.Element.Physical, 5)
            }, Enums.AlgorithemType.TeamPlayer)
        };

        BattleHandlerScript.startBattle(player, enemies);
        
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
