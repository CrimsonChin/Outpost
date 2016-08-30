using UnityEngine;

namespace Assets.Scripts.Spider.States
{
    class SpiderAlertState : State
    {
        private readonly float TimeLooking = 6f;
        private float _totalTimeSpentLooking;

        private readonly float LookTimeInDirection = 1.5f;
        private float _remainingTimeLookingInDirection;

        private readonly SpiderController _spiderController;

        public SpiderAlertState(SpiderController spiderController)
        {
            _spiderController = spiderController;

            AddTransition(Transition.CanSensePlayer, StateId.PersuePlayer);
            AddTransition(Transition.PlayerSighted, StateId.PersuePlayer);
            AddTransition(Transition.PlayerEscaped, StateId.Roam);
            AddTransition(Transition.Attack, StateId.Attack);
        }

        public override StateId StateId
        {
            get { return StateId.Alert; }
        }

        public override void DoBeforeEnter()
        {
            _totalTimeSpentLooking = TimeLooking;
        }

        public override void Reason(GameObject self, GameObject player)
        {
            if (_spiderController.CanAttackPlayer())
            {
                SpiderLogger.Log("Spider Alert: CAN ATTACK PLAYER !!!!!!!!");
                _spiderController.PerformTransition(Transition.Attack);
            }

            if (_spiderController.CheckIfPlayerIsVisible())
            {
                SpiderLogger.Log("Spider Alert: Player sighted");
                _spiderController.PerformTransition(Transition.PlayerSighted);
                return;
            }

            if (_spiderController.CanSensePlayer())
            {
                // we can sense the player, stay in this state as long as the player can be sensed
                //_totalTimeSpentLooking = TimeLooking;
                SpiderLogger.Log("Spider Alert: can sense the player");
                _spiderController.PlayerLastSightedLocation = player.transform.position;
                _spiderController.PerformTransition(Transition.CanSensePlayer);
                return;
            }

            _totalTimeSpentLooking -= Time.deltaTime;
            if (_totalTimeSpentLooking <= 0)
            {
                SpiderLogger.Log("Spider Alert: Player escaped");
                _spiderController.PerformTransition(Transition.PlayerEscaped);
            }
        }

        public override void Act(GameObject self, GameObject player)
        {
            if (_remainingTimeLookingInDirection > 0)
            {
                _remainingTimeLookingInDirection -= Time.deltaTime;
            }
            else
            {
                _spiderController.ChangeLookDirection();
                _remainingTimeLookingInDirection = LookTimeInDirection;
            }
        }
    }
}
