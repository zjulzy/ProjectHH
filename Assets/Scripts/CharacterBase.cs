using System;
using System.Collections.Generic;
using ProjectHH.GameEffect;
using ProjectHH.UI;
using UnityEngine;

namespace ProjectHH
{
    public class CharacterBase : MonoBehaviour
    {
        protected List<GEBase> _effects = new List<GEBase>();

        [SerializeField] protected WorldSpaceHealthBar _healthBar;

        [SerializeField] protected int _maxHealth;
        [SerializeField] protected int _currentHealth;
        public int CurrentHealth => _currentHealth;

        private void SetHealthBar(int currentHealth)
        {
            _healthBar.UpdateHealth(currentHealth / (float)_maxHealth);
        }

        public void SetHealth(int health)
        {
            _currentHealth = health;
            SetHealthBar(health);
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.UpdateHealth(1);
        }

        private void Update()
        {
        }
    }
}