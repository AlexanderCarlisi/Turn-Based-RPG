public class Consumable : Item {

    private int durability;
    private int maxDurability;
    private Skill effect;

    public Consumable(string Name, string Description, Skill _Skill, int MaxDurability) : base(Name, Description) {
        effect = _Skill;
        maxDurability = MaxDurability;
        durability = MaxDurability;
    }

    public int getDurability() {
        return durability;
    }

    public int getMaxDurability() {
        return maxDurability;
    }

    public Skill getEffect() {
        return effect;
    }
}