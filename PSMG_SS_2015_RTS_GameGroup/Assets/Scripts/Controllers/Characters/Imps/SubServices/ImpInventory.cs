﻿using Assets.Scripts.AssetReferences;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Controllers.Characters.Imps.SubServices
{
    /// <summary>
    /// The imp inventory contains the sprite renderers of the tools
    /// used by an imp. 
    /// </summary>
    public class ImpInventory : Inventory
    {
        protected override void InitTagNames()
        {
            TagNames = new[]
            {
                TagReferences.ImpInventorySpear,
                TagReferences.ImpInventoryShield,
                TagReferences.ImpInventoryBomb,
                TagReferences.ImpInventoryLadder
            };
        }

        public override void HideItems()
        {
            base.HideItems();

            Explosion.Hide();

            TorchController.Hide();
        }

        protected override void RetrieveItems()
        {
            base.RetrieveItems();

            Explosion = GetComponentInChildren<Explosion>();
            TorchController = GetComponentInChildren<TorchController>();
        }

        public Explosion Explosion { get; private set; }

        public TorchController TorchController { get; private set; }

        public void DisplayExplosion()
        {
            Explosion.Display();
        }
    }
}