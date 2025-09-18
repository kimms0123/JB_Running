using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kimchi
{
    public interface IItem
    {
        void ApplyEffect(Player player);
    }


    public class FoodItem : MonoBehaviour, IItem
    {
        public int healAmount = 20;
        public void ApplyEffect(Player player)
        {
            player.hp += healAmount;
        }
    }

    public class SlowItem : MonoBehaviour, IItem
    {
        public float duration = 5f;
        public float multiplier = 0.5f;

        public void ApplyEffect(Player player)
        {
            player.StartCoroutine(player.SpeedBoost(duration, multiplier));
        }
    }

    public class ShieldItem : MonoBehaviour, IItem
    {
        public float duration = 5f;

        public void ApplyEffect(Player player)
        {
            player.StartCoroutine(player.Invincible(duration));
        }
    }
}