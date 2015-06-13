﻿using UnityEngine;

namespace Assets.Scripts.Controllers.Characters.Imps
{
    public class ImpSchwarzeneggerService : ImpProfessionService
    {
        public bool IsAtThrowingPosition { get; private set; }

        public void Awake()
        {
            InitAttributes();
        }

        private void InitAttributes()
        {
            IsAtThrowingPosition = false;
        }

        public void ThrowImp(ImpController projectile)
        {
            // TODO 
        }
    }
}