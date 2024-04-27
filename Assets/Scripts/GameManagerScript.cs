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
            Skill.getAttackSkill(Enums.SkillNames.Attack.Slash),
            Skill.getAttackSkill(Enums.SkillNames.Attack.PiercingHalos),
        });
        party[1] = new PartyMember("Mage", 10, 20, 2, new int[]{1, 3, 4, 6, 7, 8, 9}, new Skill[]{
            Skill.getAttackSkill(Enums.SkillNames.Attack.Fireball),
            Skill.getAttackSkill(Enums.SkillNames.Attack.IceBlast),
            Skill.getAttackSkill(Enums.SkillNames.Attack.Thunder),
            Skill.getAttackSkill(Enums.SkillNames.Attack.Earthquake),
            Skill.getAttackSkill(Enums.SkillNames.Attack.WindSlash),
            Skill.getAttackSkill(Enums.SkillNames.Attack.PiercingHalos)
        });
        party[2] = new PartyMember("Healer", 10, 15, 3, new int[]{1, 2, 3, 4, 5, 6, 7}, new Skill[]{
            Skill.getHealSkill(Enums.SkillNames.Heal.Basic),
            Skill.getHealSkill(Enums.SkillNames.Heal.Intermediate),
            Skill.getHealSkill( Enums.SkillNames.Heal.Advanced),
        });
        party[3] = new PartyMember("Rogue", 10, 10, 4, new int[]{1, 2, 3, 4, 5, 6, 7}, new Skill[]{
            Skill.getAttackSkill(Enums.SkillNames.Attack.Slash),
            Skill.getAttackSkill(Enums.SkillNames.Attack.PiercingHalos)
        });

        player.setParty(party);

        BattleSchedulerScript.startup(player, 0);
    }
}
