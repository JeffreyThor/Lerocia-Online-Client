public class Apparel : Item {
	private int armor;

	public Apparel(int id, string name, int weight, int value, int armor) : base(id, name, weight, value, "Apparel") {
		this.armor = armor;
		addStat("Armor", armor.ToString());
	}

	public int getArmor() {
		return armor;
	}
  
	public override void Use(Player player) {
		//TODO
	}
}