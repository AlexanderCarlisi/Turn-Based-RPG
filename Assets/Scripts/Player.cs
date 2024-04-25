using System.Collections.Generic;
using System.Runtime.Serialization.Json;

public class Player {
    private static readonly int PARTY_SIZE = 4;

    private static Inventory inventory;
    private static PartyMember[] party;

    public Player() {
        inventory = new Inventory();
        party = new PartyMember[PARTY_SIZE];

        // Game Tests
        party[0] = new PartyMember("Warrior", 10, 10, 1, new int[]{1, 2, 3, 4, 5, 6, 7}, new Skill[]{
            new AttackSkill("SlashySkill1", Enums.Element.Physical, 5),
            new AttackSkill("SlashySkill2", Enums.Element.Physical, 5),
            new AttackSkill("SlashySkill3", Enums.Element.Physical, 5)
        });
        party[1] = new PartyMember("Mage", 10, 20, 2, new int[]{1, 3, 4, 6, 7, 8, 9}, new Skill[]{
            new AttackSkill("FireSkill1", Enums.Element.Fire, 10)
        });
    }
}



public class Inventory {

    private Dictionary<Item, int> accessoires;
    private Dictionary<Item, int> armor;
    private Dictionary<Item, int> consumables;
    private Dictionary<Item, int> weapons;

    public Inventory() {
        accessoires = new Dictionary<Item, int>();
        armor = new Dictionary<Item, int>();
        consumables = new Dictionary<Item, int>();
        weapons = new Dictionary<Item, int>();
    }
}