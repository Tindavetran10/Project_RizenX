using Character;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    private CharacterManager _characterManager;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_characterManager == null)
           _characterManager = animator.GetComponent<CharacterManager>();
       
       // this is called when an action ends, and the state resets to empty
       _characterManager.isPerformingAction = false;
       _characterManager.applyRootMotion = false;
       _characterManager.canRotate = true;
       _characterManager.canMove = true;
       
       if(_characterManager.IsOwner) _characterManager.characterNetworkManager.isJumping.Value = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
