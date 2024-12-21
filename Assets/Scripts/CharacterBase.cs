using System;
using System.Collections.Generic;
using ProjectHH.GameEffect;
using ProjectHH.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHH
{
    public class CharacterBase : MonoBehaviour
    {
        protected List<GEBase> _effects = new List<GEBase>();

        [SerializeField] protected WorldSpaceHealthBar _healthBar;

        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _currentHealth;
        [SerializeField, LabelText("受伤闪烁时间")] protected float _hurtBlinkTime = 0.1f;
        private float _hurtBlinkTimer = 0;
        public int CurrentHealth => _currentHealth;

        private void SetHealthBar(int currentHealth)
        {
            _healthBar.UpdateHealth(currentHealth / (float)_maxHealth);
        }

        public void SetHealth(int health)
        {
            SetHealthBar(health);
            if (health < _currentHealth)
            {
                TempSetEmssion(true);
            }

            if (health <= 0)
            {
                Destroy(gameObject);
            }

            _currentHealth = health;
        }

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.UpdateHealth(1);
        }

        protected virtual void Update()
        {
            if (_hurtBlinkTimer > 0)
            {
                _hurtBlinkTimer = Mathf.Max(0, _hurtBlinkTimer - Time.deltaTime);
                if (_hurtBlinkTimer <= 0)
                {
                    TempSetEmssion(false);
                }
            }
        }

        private void TempSetEmssion(bool isEnable)
        {
            var renderer = transform.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                if (isEnable)
                {
                    renderer.material.EnableKeyword("_EMISSION");
                    _hurtBlinkTimer = _hurtBlinkTime;
                }
                else
                {
                    renderer.material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}