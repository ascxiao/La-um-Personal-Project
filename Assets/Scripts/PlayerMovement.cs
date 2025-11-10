using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> seSprites;
    public List<Sprite> sSprites;

    public float frameRate;
    float idleTime;

    private void Awake(){
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void Update(){
        PlayerInput();
    }

    private void FixedUpdate(){
        Move();
    }

    private void PlayerInput(){
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
    }

    private void Move(){
        rb.linearVelocity = movement * moveSpeed;
        if (movement == Vector2.zero)
            rb.linearVelocity = Vector2.zero;

        FlipHandling();
        List <Sprite> directionSprites = GetSpriteDirection();
        
        if (directionSprites != null){
            float playTime = Time.time - idleTime;
            int frame = (int)((playTime * frameRate)%directionSprites.Count);
            spriteRenderer.sprite = directionSprites[frame];
        }else{
            idleTime = Time.time;
        }
    }

    void FlipHandling(){
        if(!spriteRenderer.flipX&&movement.x < 0){
            spriteRenderer.flipX = true;
        } else if (spriteRenderer.flipX && movement.x > 0){
            spriteRenderer.flipX = false;
        }
    }

    List<Sprite> GetSpriteDirection(){

        List<Sprite> selectedSprites = null;
        if(movement.y > 0){ //North
            if (Mathf.Abs(movement.x)>0){
                selectedSprites = neSprites;
            } else {
                selectedSprites = nSprites;
            }

        }else if(movement.y < 0){ //South
            if (Mathf.Abs(movement.x)>0){
                selectedSprites = seSprites;
            } else {
                selectedSprites = sSprites;
            }
        }else{
            if (Mathf.Abs(movement.x)>0){
                selectedSprites = eSprites;
            }
        }

        return selectedSprites;
    }
}
