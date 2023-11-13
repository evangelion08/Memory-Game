using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private List<Card> cardList;
    [SerializeField] private List<Sprite> pictures;
    [SerializeField] private Card[] selectedCards;
    [SerializeField] private GameStatus status;
    [SerializeField] private float turnBackTime;
    private float turnBackTimer = 0;
    private float removeCardTimer = 0;
    private int currentCardsRotating = 0;

    public enum GameStatus
    {
        waiting_on_first_card,
        waiting_on_second_card,
        match_found,
        no_match_found
    }

    void Start()
    {
        selectedCards = new Card[2];
        MakeCards();

        status = GameStatus.waiting_on_first_card;
    }

    void Update()
    {
        if (status == GameStatus.no_match_found)
        {
            turnBackTimer += Time.deltaTime;

            if (turnBackTimer >= turnBackTime)
            {
                selectedCards[0].TurnToBack();
                selectedCards[1].TurnToBack();
                turnBackTimer = 0;
                status = GameStatus.waiting_on_first_card;
                selectedCards[0] = null;
                selectedCards[1] = null;

                currentCardsRotating = 0;
            }
        }
        else if (status == GameStatus.match_found)
        {
            removeCardTimer += Time.deltaTime;

            if (removeCardTimer >= 1)
            {
                RotateBackOrRemovePair();
                turnBackTimer = 0;

                currentCardsRotating = 0;
            }
        }
    }

    private void MakeCards()
    {
        for (int i = 0; i < 6; i++)
        {
            //choose a random picture
            int randomPictureID = Random.Range(0, pictures.Count);

            Sprite randomPicture = pictures[randomPictureID];
            pictures.RemoveAt(randomPictureID);

            //choose 2 random cards
            for (int j = 0; j < 2; j++)
            {
                int randomCardID = Random.Range(0, cardList.Count);
                cardList[randomCardID].SetFront(randomPicture);

                cardList.RemoveAt(randomCardID);
            }
        }

    }


    public void startSelectingCard(Card card)
    {
        currentCardsRotating += 1;
    }

    public void SelectCard(Card card)
    {
        if (status == GameStatus.waiting_on_first_card)
        {
            selectedCards[0] = (card);
            status = GameStatus.waiting_on_second_card;
        }
        else if (status == GameStatus.waiting_on_second_card)
        {
            selectedCards[1] = (card);
            CheckForMatchingPair();
        }
    }

    private void CheckForMatchingPair()
    {
        if (selectedCards[0].GetFront() == selectedCards[1].GetFront())
        {
            status = GameStatus.match_found;
        }
        else
        {
            status = GameStatus.no_match_found;
        }
    }

    private void RotateBackOrRemovePair()
    {
        selectedCards[0].gameObject.SetActive(false);
        selectedCards[1].gameObject.SetActive(false);

        selectedCards[0] = null;
        selectedCards[1] = null;

        status = GameStatus.waiting_on_first_card;

    }

    public bool CanSelectCard()
    {
        if (currentCardsRotating < 2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}