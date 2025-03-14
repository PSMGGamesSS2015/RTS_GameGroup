﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AssetReferences;
using Assets.Scripts.Controllers.Characters.Imps;
using UnityEngine;

namespace Assets.Scripts.Controllers.Objects
{
    public class FragileWallController : MonoBehaviour
    {
        private List<Rigidbody2D> stones; 

        public void Awake()
        {
            stones = GetComponentsInChildren<Rigidbody2D>().ToList();
        }

        public void Detonate(ImpController detonationSource)
        {
            var collider = GetComponent<Collider2D>();
            var max = collider.bounds.max * 0.5f;
            var min = collider.bounds.min * 0.5f;
            collider.bounds.SetMinMax(min, max);

            gameObject.layer = LayerReferences.DecorationLayerBackground;
            stones.ForEach(s => s.isKinematic = false);
            stones.ForEach(s => ApplyForce(detonationSource, s));
        }

        private void ApplyForce(ImpController detonationSource, Rigidbody2D rigidbody2D)
        {
            var x = rigidbody2D.transform.position.x - detonationSource.gameObject.transform.position.x;
            var y = rigidbody2D.transform.position.y - detonationSource.gameObject.transform.position.y;
            y *= (Random.value + 0.5f);
            x *= (Random.value + 0.5f);

            var positionRelativeToImp = new Vector2(x, y);
            rigidbody2D.velocity = new Vector2(positionRelativeToImp.x * 7, positionRelativeToImp.y * 7);
        }
    }
}