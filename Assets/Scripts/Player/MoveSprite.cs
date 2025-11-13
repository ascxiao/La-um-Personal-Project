using UnityEngine;

public class MoveAttack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

     // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 pos = PlayerMovement.instance.rb.position;
        if(!PlayerMovement.instance.spriteRenderer.flipX){
            pos.x += PlayerMovement.instance.moveSpeed * Time.deltaTime;
            PlayerMovement.instance.rb.MovePosition(pos);
        } else {
            pos.x -= PlayerMovement.instance.moveSpeed * Time.deltaTime;
            PlayerMovement.instance.rb.MovePosition(pos);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
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
