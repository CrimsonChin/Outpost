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

            // set externally
            AddTransition(Transition.CollidedWithPlayer, StateId.Attack);
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
            //if (_spiderController.CanSensePlayer())
            //{
            //    _spiderController.PerformTransition(Transition.CanSensePlayer);
            //}

            if (_spiderController.CheckIfPlayerIsVisible())
            {
                Debug.Log("Spider Alert: Player sighted");
                _spiderController.PerformTransition(Transition.PlayerSighted);
                return;
            }

            _totalTimeSpentLooking -= Time.deltaTime;
            if (_totalTimeSpentLooking <= 0)
            {
                Debug.Log("Spider Alert: Player escaped");
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
