using UnityEngine;

namespace Assets.Scripts.Spider.States
{
    class SpiderAttackState : State
    {
        private readonly SpiderController _spiderController;
        private readonly Animator _animator;
        private float _attackTime;

        public SpiderAttackState(SpiderController spiderController, Animator animator)
        {
            _spiderController = spiderController;
            _animator = animator;

            // Set externally
            AddTransition(Transition.PlayerEscaped, StateId.Alert);
        }

        public override StateId StateId
        {
            get { return StateId.Attack; }
        }

        public override void DoBeforeEnter()
        {
            // face player
            _animator.SetBool("isAttacking", true);
        }

        public override void Reason(GameObject self, GameObject player)
        {
            // Edge case, player is dead? who knows
            if (player == null)
            {
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }

            if (!_spiderController.IsPlayerWithinAttackingDistance())
            {
                Debug.Log("Spider Attack: Player escaped");
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }
        }

        public override void Act(GameObject self, GameObject player)
        {
            if (_attackTime <= 0)
            {
                DamagePlayer(player);
                _attackTime += 30;
            }
            else
            {
                _attackTime--;
            }
        }

        public override void DoBeforeExit()
        {
            _animator.SetBool("isAttacking", false);
        }

        private void DamagePlayer(GameObject player)
        {
            Destroyable destroyablePlayer = player.GetComponent<Destroyable>();
            if (destroyablePlayer != null)
            {
                destroyablePlayer.Damage(1);
            }
        }
    }
}
