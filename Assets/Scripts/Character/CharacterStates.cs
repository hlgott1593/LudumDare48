namespace LD48
{
    public class CharacterStates
    {
        public enum CharacterConditions
        {
            Normal,
            Paused,
            Dead
        }
        
        public enum MovementStates 
        {
            Null,
            Idle,
            Falling,
            Running,
            Jumping,
            DoubleJumping
        }
    }
}