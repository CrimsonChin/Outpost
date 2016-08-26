using UnityEngine;

namespace Assets.Scripts.Spider.States
{
    class SpiderPersueState : State
    {
        private const float PersueSpeed = 0.02f; // TODO have base speed and use a multiplier
        private readonly SpiderController _spiderController;
        private Vector3 _playerLastSightedLocation;

        public SpiderPersueState(SpiderController spiderController)
        {
            _spiderController = spiderController;

            AddTransition(Transition.PlayerEscaped, StateId.Alert);
        }

        public override StateId StateId
        {
            get { return StateId.PersuePlayer; }
        }

        public override void Reason(GameObject self, GameObject player)
        {
            // Arrived at target but the player isn't there!
            if (Vector2.Distance(self.transform.position, _playerLastSightedLocation) <= 0)
            {
                Debug.Log("Spider Persue: The Player has escaped!");
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }

            // Attack
        }

        public override void Act(GameObject self, GameObject player)
        {
            if (_spiderController.CheckIfPlayerIsVisible())
            {
                _playerLastSightedLocation = player.transform.position;
            }

            Vector2 distance = self.transform.position - _playerLastSightedLocation;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                _spiderController.LookDirection = distance.x > 0 ? Vector2.left : Vector2.right;
            }
            else
            {
                _spiderController.LookDirection = distance.y > 0 ? Vector2.down : Vector2.up;
            }

            self.transform.position = Vector3.MoveTowards(self.transform.position, _playerLastSightedLocation, PersueSpeed);
        }
    }
}
