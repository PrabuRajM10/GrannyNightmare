namespace Gameplay
{
    public interface IDamageable
    {
        public int GetHealth();
        public void TakeDamage(int damage);
    }
}