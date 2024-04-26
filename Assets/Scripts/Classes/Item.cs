public class Item {
    private string name;
    private string description;

    public Item(string Name, string Description) {
        name = Name;
        description = Description;
    }

    public string getName() {
        return name;
    }

    public string getDescription() {
        return description;
    }

    // public void setName(string Name) {
    //     name = Name;
    // }
}