public class Weapon : Item {
	private int damage;

	public Weapon(int id, string name, int weight, int value, int damage) : base(id, name, weight, value, "Weapon") {
		this.damage = damage;
		addStat("Damage", damage.ToString());
	}

	public int getDamage() {
		return damage;
	}

	public override void Use(Player player) {
		//TODO
	}
}