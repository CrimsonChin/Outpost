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

            AddTransition(Transition.PlayerEscaped, StateId.Alert);
            AddTransition(Transition.PlayerSighted, StateId.PersuePlayer);
        }

        public override StateId StateId
        {
            get { return StateId.Attack; }
        }

        public override void DoBeforeEnter()
        {
            _animator.SetBool("isAttacking", true);
        }

        public override void Reason(GameObject self, GameObject player)
        {
            if (!_spiderController.CanAttackPlayer())
            {
                SpiderLogger.Log("Spider Attack: Player no longer close enough to attack");
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }

            if (_spiderController.CheckIfPlayerIsVisible())
            {
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }

            if (player == null)
            {
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
