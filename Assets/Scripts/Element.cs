using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public bool isMine;
    public bool isFailed;
    public bool notMine;

    [Header("Textures")]
    public Sprite[] emptySprites;
    public Sprite mineSprite;
    
    void Start()
    {
        //Randomly decide the chance of being a mine
        isMine = Random.value < 0.15;

        //Registering to Grid
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        PlayField.elements[x, y] = this;
    }

    //Click to a element
    void OnMouseUpAsButton()
    {

        //If it's a mine
        if (isMine)
        {
            //Play the sound
            isFailed = true;

            //Uncover all mines
            PlayField.UncoverMines();

            //Game over
            print("Game Over");
        }
        //If it's not a mine
        else
        {
            notMine = true;
            //Show adjacent mine number
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            LoadTextures(PlayField.adjacentMines(x, y));

            //Uncover the area without mines
            PlayField.FFuncover(x, y, new bool[PlayField.width, PlayField.height]);

            //Find out if the game is won
            if (PlayField.isFinished())
            {
                print("You made it!!");
            }
        }
    }

    public void LoadTextures(int adjacentCount)
    {
        if (isMine)
        {
            GetComponent<SpriteRenderer>().sprite = mineSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptySprites[adjacentCount];
        }
    }

    public bool isCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
    }
}
