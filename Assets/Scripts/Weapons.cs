public class Weapon : Item {

    private int damage;
    private int hitRate;
    private Enums.Element element;
    private Enums.WeaponType type;

    public Weapon(string Name, string Description, int Damage, int HitRate, Enums.Element Element, Enums.WeaponType Type) : base(Name, Description) {
        damage = Damage;
        hitRate = HitRate;
        element = Element;
        type = Type;
    }

    public int getDamage() {
        return damage;
    }

    public int getHitRate() {
        return hitRate;
    }

    public Enums.Element getElement() {
        return element;
    }

    public Enums.WeaponType getType() {
        return type;
    }
    
}
