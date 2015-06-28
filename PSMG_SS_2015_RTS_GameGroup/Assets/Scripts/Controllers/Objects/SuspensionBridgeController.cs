﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AssetReferences;
using Assets.Scripts.Controllers.Characters.Imps;
using Assets.Scripts.Controllers.Characters.Imps.SubServices;
using Assets.Scripts.Types;
using UnityEngine;

namespace Assets.Scripts.Controllers.Objects
{
    public class SuspensionBridgeController : MonoBehaviour, TriggerCollider2D.ITriggerCollider2DListener
    {

        private List<ImpController> cowardsOnBridge;
        private TriggerCollider2D suspensionBridgeArea;
        private List<BreakableLink> breakableLinks; 

        public void Awake()
        {
            cowardsOnBridge = new List<ImpController>();
            suspensionBridgeArea = GetComponent<TriggerCollider2D>();
            suspensionBridgeArea.RegisterListener(this);
            breakableLinks = GetComponentsInChildren<BreakableLink>().ToList();
        }

        void TriggerCollider2D.ITriggerCollider2DListener.OnTriggerEnter2D(TriggerCollider2D self, Collider2D collider)
        {
            if (self.GetInstanceID() != suspensionBridgeArea.GetInstanceID()) return;
            this.AddCowardToList(collider);
        }

        private void AddCowardToList(Collider2D collider)
        {
            if (collider.gameObject.tag != TagReferences.Imp) return;
            var imp = collider.gameObject.GetComponent<ImpController>();
            if (imp.GetComponent<ImpTrainingService>().Type != ImpType.Coward) return;
            if (cowardsOnBridge.Contains(imp)) return;
            cowardsOnBridge.Add(imp);
            CheckIfBridgeBreaks();
        }

        private void CheckIfBridgeBreaks()
        {
            if (cowardsOnBridge.Count >= 4)
            {
                Break();
            }
        }

        private void Break()
        {
            Debug.Log("Bridge is breaking");
            breakableLinks.ForEach(breakableLink => breakableLink.Break());
        }

        void TriggerCollider2D.ITriggerCollider2DListener.OnTriggerExit2D(TriggerCollider2D self, Collider2D collider)
        {
            if (self.GetInstanceID() != suspensionBridgeArea.GetInstanceID()) return;
            RemoveCowardFromList(collider);
        }

        void TriggerCollider2D.ITriggerCollider2DListener.OnTriggerStay2D(TriggerCollider2D self, Collider2D collider)
        {
            if (self.GetInstanceID() != suspensionBridgeArea.GetInstanceID()) return;
            AddCowardToList(collider);
        }

        private void RemoveCowardFromList(Collider2D collider)
        {
            if (collider.gameObject.tag != TagReferences.Imp) return;
            var imp = collider.gameObject.GetComponent<ImpController>();
            if (imp.GetComponent<ImpTrainingService>().Type != ImpType.Coward) return;
            cowardsOnBridge.Remove(imp);
        }
    }
}
