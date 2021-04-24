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
            Idle,
            Falling,
            Running,
            Jumping,
            DoubleJumping
        }
    }
}