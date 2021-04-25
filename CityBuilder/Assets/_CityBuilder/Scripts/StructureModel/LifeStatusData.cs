using System;
using UnityEngine;

namespace _CityBuilder.Scripts.StructureModel
{
    [Serializable]
    public class LifeStatusData
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        public int MAXHealth => maxHealth;
        public int CurrentHealth => currentHealth;
        
        public void Initialize(int cHealth, int mHealth)
        {
            maxHealth = mHealth;
            currentHealth = cHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
    }
}