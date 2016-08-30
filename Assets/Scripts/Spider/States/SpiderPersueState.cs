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
            AddTransition(Transition.Attack, StateId.Attack);
        }

        public override StateId StateId
        {
            get { return StateId.PersuePlayer; }
        }

        public override void Reason(GameObject self, GameObject player)
        {
            if (Vector2.Distance(self.transform.position, _spiderController.PlayerLastSightedLocation) <= 0)
            {
                SpiderLogger.Log("Spider Persue: The Player has escaped!");
                _spiderController.PerformTransition(Transition.PlayerEscaped);
                return;
            }

            if (_spiderController.CanAttackPlayer())
            {
                SpiderLogger.Log("Spider Persue: Close enough to attack player");
                _spiderController.PerformTransition(Transition.Attack);
            }
        }

        public override void Act(GameObject self, GameObject player)
        {
            if (_spiderController.CheckIfPlayerIsVisible())
            {
                _playerLastSightedLocation = _spiderController.PlayerLastSightedLocation;
            }

            Vector2 distance = new Vector2(self.transform.position.x, self.transform.position.y) - _spiderController.PlayerLastSightedLocation;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                _spiderController.FacingDirection = distance.x > 0 ? Vector2.left : Vector2.right;
            }
            else
            {
                _spiderController.FacingDirection = distance.y > 0 ? Vector2.down : Vector2.up;
            }

            self.transform.position = Vector3.MoveTowards(self.transform.position, _spiderController.PlayerLastSightedLocation, PersueSpeed);
        }

        //public override void OnCollisionEnter2D(Collision2D other)
        //{
        //    if (other.gameObject.tag == "Player")
        //    {
        //        SpiderLogger.Log("Spider Persue: Collided with player");
        //        _spiderController.PerformTransition(Transition.CollidedWithPlayer);
        //    }
        //}
    }
}
