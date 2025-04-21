using UnityEngine;
using UnityEngine.InputSystem;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour
    {
        public Animator animator;
        public string walkForwardAnimation = "walk_forward";
        public string walkBackwardAnimation = "walk_backwards";
        public string runForwardAnimation = "run_forward";
        public string turn90LAnimation = "turn_90_L";
        public string turn90RAnimation = "turn_90_R";
        public string trotAnimation = "trot_forward";
        public string sittostandAnimation = "sit_to_stand";
        public string standtositAnimation = "stand_to_sit";

        private FarmerGameController controls;

        void Awake()
        {
            controls = new FarmerGameController();

            // Bind input actions to animation methods
            controls.Gameplay.MoveFarmer.performed += ctx => PlayWalkForwardAnimation(ctx);
            //controls.Gameplay.MoveFarmer.canceled += ctx => StopAnimation();

            controls.Gameplay.MoveCamera.performed += ctx => PlayTurnAnimation(ctx);
        }

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void PlayWalkForwardAnimation(InputAction.CallbackContext ctx)
        {
            animator.Play(walkForwardAnimation);
        }

        private void PlayTurnAnimation(InputAction.CallbackContext ctx)
        {
            animator.Play(turn90LAnimation);
        }
    }
}