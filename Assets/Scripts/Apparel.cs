public class Apparel : Item {
	private int armor;

	public Apparel(string name, int weight, int value, int armor) : base(name, weight, value, "Apparel") {
		this.armor = armor;
		addStat("Armor", armor.ToString());
	}

	public int getArmor() {
		return armor;
	}
  
	public override void Use(Player player) {
		//TODO
	}
  
	public override void Drop(Player player) {
		//TODO Remove from players inventory
	}
}