using Core.Utils;
using Systems.Combat;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyTarget : MonoBehaviour
    {
        [SerializeField] private float _alertDuration = 5f;

        [Header("Front detection")]
        [SerializeField] private Transform _checkPoint;
        [SerializeField] private float _checkDistance = 13f;

        private float _facingDirection;
        private float _detectedLastTime;
        private Transform _behindAttackerTransform;

        public bool HasCurrent => Current != null;
        public Transform Current { get; private set; }
        public bool IsBehind => HasCurrent && !Mathf.Approximately(Direction, _facingDirection);
        public float Direction
        {
            get
            {
                if (!HasCurrent)
                {
                    return 0f;
                }

                return Current.position.x > transform.position.x ? 1 : -1;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                _checkPoint.position,
                _checkPoint.position + new Vector3(_checkDistance * _facingDirection, 0f));
        }

        public void Tick(float facingDirection)
        {
            _facingDirection = facingDirection;

            UpdateCurrent();
        }

        public void DamageAlert(DamageInfo damageInfo)
        {
            if (damageInfo.Source == DamageSource.Player)
            {
                _behindAttackerTransform = damageInfo.AttackerTransform;
            }
        }

        private void UpdateCurrent()
        {
            Transform current = null;

            RaycastHit2D targetInFront = CheckForTargetInFront();
            if (targetInFront.collider)
            {
                current = targetInFront.transform;
            }
            else if (_behindAttackerTransform)
            {
                current = _behindAttackerTransform;
                _behindAttackerTransform = null;
            }

            if (current)
            {
                _detectedLastTime = Time.time;
                Current = current;
            }

            if (_detectedLastTime + _alertDuration < Time.time)
            {
                Current = null;
            }
        }

        private RaycastHit2D CheckForTargetInFront()
        {
            RaycastHit2D raycast = Physics2D.Raycast(
                _checkPoint.position,
                Vector2.right * _facingDirection,
                _checkDistance,
                LayerMaskProvider.Player | LayerMaskProvider.Ground);

            return raycast.collider && LayerMaskProvider.Contains(raycast.collider.gameObject.layer, LayerMaskProvider.Player)
                ? raycast
                : default;
        }
    }
}
