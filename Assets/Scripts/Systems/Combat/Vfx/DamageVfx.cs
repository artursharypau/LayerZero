using Core.Utils;
using UnityEngine;

namespace Systems.Combat.Vfx
{
    public class DamageVfx : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private Material _material;

        private SpriteRenderer _sr;
        private IDamageable _damageable;
        private CountdownTimer _timer;
        private Material _initialMaterial;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
            _damageable = GetComponent<IDamageable>();

            _timer = new CountdownTimer();

            _initialMaterial = _sr.material;
        }

        private void OnEnable()
        {
            _damageable.Damaged += OnDamaged;
        }

        private void OnDisable()
        {
            _damageable.Damaged -= OnDamaged;
        }

        private void Update()
        {
            _timer.Tick(Time.deltaTime);
            if (_timer.IsExpired)
            {
                _sr.material = _initialMaterial;
            }
        }

        private void OnDamaged(DamageInfo obj)
        {
            _sr.material = _material;

            _timer.Start(_duration);
        }
    }
}
