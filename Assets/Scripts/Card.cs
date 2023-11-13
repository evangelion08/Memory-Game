using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public enum CardStatus
{
    show_back,
    show_front,
    rotating_to_back,
    rotating_to_front
}


public class Card : MonoBehaviour
{
    [SerializeField] private CardStatus status;
    [SerializeField] private float turnTargetTime;
    [SerializeField] private float turnTimer;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private SpriteRenderer frontRenderer;
    [SerializeField] private SpriteRenderer backRenderer;
    [SerializeField] private Game game;

    void Start()
    {

    }

    void Update()
    {
        if (status == CardStatus.rotating_to_back || status == CardStatus.rotating_to_front)
        {

            turnTimer += Time.deltaTime;
            float percentage = turnTimer / turnTargetTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percentage);

            if (percentage >= 1f)
            {
                if (status == CardStatus.rotating_to_back)
                {
                    status = CardStatus.show_back;
                }
                else if (status == CardStatus.rotating_to_front)
                {
                    status = CardStatus.show_front;

                    game.SelectCard(this);
                }
            }
        }

    }

    private void Awake()
    {
        status = CardStatus.show_back;

        FindObjectOfType<Game>();
    }

    private void OnMouseUp()
    {
        if (game.CanSelectCard() == true)
        {
            if (status == CardStatus.show_back)
            {
                game.startSelectingCard(this);
                TurnToFront();
            }
            else if (status == CardStatus.show_front)
            {
                TurnToBack();
            }
        }

    }

    private void TurnToFront()
    {
        status = CardStatus.rotating_to_front;
        turnTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 180, 0);
    }

    public void TurnToBack()
    {
        status = CardStatus.rotating_to_back;
        turnTimer = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetFront(Sprite frontSprite)
    {
        frontRenderer.sprite = frontSprite;
    }

    public Sprite GetFront()
    {
        return frontRenderer.sprite;
    }

    public void SetBack(Sprite backSprite)
    {
        backRenderer.sprite = backSprite;
    }

}