public class Weapon : Item {
	private int damage;

	public Weapon(string name, int weight, int value, int damage) : base(name, weight, value, "Weapon") {
		this.damage = damage;
		addStat("Damage", damage.ToString());
	}

	public int getDamage() {
		return damage;
	}

	public override void Use(Player player) {
		//TODO
	}
  
	public override void Drop(Player player) {
		//TODO Remove from players inventory
	}
}