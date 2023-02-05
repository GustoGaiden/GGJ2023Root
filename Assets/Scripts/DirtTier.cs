using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DirtTier : MonoBehaviour
    {
        public float JuiceCostPerSecond;
        public BoxCollider2D Collider;
        public int TierLevel;
        public SpriteRenderer sprite;

        private PlayerController player;
        private void Awake()
        {
            // player = GlobalVars.GameManager.PlayerController;
        }

        public bool isPlayerTouchingThisTier()
        {
            return Collider.IsTouchingLayers(LayerMask.NameToLayer("Player"));
        }
    }
}