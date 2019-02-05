public abstract class Potion : Item {
	protected Potion(string name, int weight, int value) : base(name, weight, value, "Potion") { }
}

public class HealthPotion : Potion {
	private int health;

	public HealthPotion(string name, int weight, int value, int health) : base(name, weight, value) {
		this.health = health;
		setDescription("Heals by " + health + " points.");
	}

	public override void Use(Player player) {
		player.currentHealth += health;
		//TODO Remove from players inventory
	}

	public override void Drop(Player player) {
		//TODO Remove from players inventory
	}
}