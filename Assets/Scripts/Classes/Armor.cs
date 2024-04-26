public class Armor : Item {

    private int physicalDefense;
    private int magicDefense;
    private int evasionRate;

    public Armor(string Name, string Description, int PhysicalDefense, int MagicDefense, int EvasionRate) : base(Name, Description) {
        physicalDefense = PhysicalDefense;
        magicDefense = MagicDefense;
        evasionRate = EvasionRate;
    }

    // public Armor(string Name, string Description, int PhysicalDefense, int MagicDefense, int EvasionRate, int Amount) : base(Name, Description, Amount) {
    //     physicalDefense = PhysicalDefense;
    //     magicDefense = MagicDefense;
    //     evasionRate = EvasionRate;
    // }

    public int getPhysicalDefense() {
        return physicalDefense;
    }

    public int getMagicDefense() {
        return magicDefense;
    }

    public int getEvasionRate() {
        return evasionRate;
    }
    
}
