using UnityEngine;
using System;

namespace Work._01_Scripts
{
    public class Player : Agent
    {
        [SerializeField] private InputReader _inputReader;

        [SerializeField] private Weapon _weapon;

        public Weapon Weapon { get => _weapon; }

        protected override void Awake()
        {
            Transform visualTrm = transform.Find("HeadView/Visual");
            AnimatorCompo = visualTrm.GetComponent<Animator>();
            MovementCompo = GetComponent<IMovement>();
            MovementCompo.Initialize(this);

            Stat = Instantiate(Stat);
            Stat.SetOwner(this);

            HealthCompo = GetComponent<Health>();
            HealthCompo.Initialize(this);

            _weapon.InitCaster(this);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void SetDead()
        {
            isDead = true;
        }
    }
}
