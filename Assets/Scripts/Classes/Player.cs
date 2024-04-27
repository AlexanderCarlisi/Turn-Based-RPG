using System.Collections.Generic;
using System.Runtime.Serialization.Json;

public class Player {
    private static readonly int PARTY_SIZE = 4;

    private static Inventory inventory;
    private static PartyMember[] party;

    public Player() {
        inventory = new Inventory();
        party = new PartyMember[PARTY_SIZE];
    }

    public void setParty(PartyMember[] Party) {
        party = Party;
    }

    public PartyMember[] getParty() {
        return party;
    }

    public Inventory getInventory() {
        return inventory;
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