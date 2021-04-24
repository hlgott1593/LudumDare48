namespace LD48
{
    public class CharacterOrientation : CharacterAbility
    {
        public enum Direction
        {
            Left,
            Right
        };

        protected Direction _direction;
        protected Direction _directionLastFrame;

        protected override void Initialize()
        {
            base.Initialize();
            _direction = Direction.Right;
            _directionLastFrame = Direction.Right;
        }
        
        public override void ProcessAbility()
        {
            base.ProcessAbility();

            if (_character.Condition != CharacterStates.CharacterConditions.Normal)
            {
                return;
            }

            _directionLastFrame = _direction;
            DetermineFacingDirection();
            FlipAbilities();

        }

        private void DetermineFacingDirection()
        {
            if (_controller.Velocity.sqrMagnitude <= 0.1f) return;
            _direction = _controller.Velocity.x >= 0 ? Direction.Right : Direction.Left;
        }

        public override void Flip()
        {
            _character.Model.flipX = !_character.Model.flipX;
        }

        protected virtual void FlipAbilities()
        {
            if ((_directionLastFrame != _direction))
            {
                _character.FlipAllAbilities();
            }
        }
    }
}