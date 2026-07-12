using UnityEngine;

namespace Systems.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;

        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }
    }
}
