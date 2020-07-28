using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip EmptyElementSurroundedWithMinesSound;
    public AudioClip EmptyElementsSound;
    public AudioClip PressedToMineSound;

    public static AudioSource audioSource;


    [Header("The Grid")]
    public static int width = 10;
    public static int height = 13;
    public static Element[,] elements = new Element[width, height];

    void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

     void Update()
     {
        if (FindObjectOfType<Element>().notMine)
        {
            audioSource.PlayOneShot(EmptyElementSurroundedWithMinesSound);
        }

        if (FindObjectOfType<Element>().isFailed)
        {
            audioSource.PlayOneShot(PressedToMineSound);
        }
     }

    public static void UncoverMines()
    {
        foreach (Element lev in elements)
        {
            if (lev.isMine) lev.LoadTextures(0);
        }
    }

    //Find out if there is a mine at the selected coordinates
    public static bool MineAt(int x , int y)
    {
        //Check if coordinates in range? then start checking for mines
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return elements[x, y].isMine;
        }
        return false;
    }

    public static int adjacentMines(int x , int y)
    {
        int count = 0;

        #region  Count adjacent mines
        if (MineAt(x, y + 1)) ++count; //For the top
        if (MineAt(x, y - 1)) ++count; //For the bottom
        if (MineAt(x + 1, y)) ++count; //For the right
        if (MineAt(x - 1, y)) ++count; //For the left
        if (MineAt(x + 1, y + 1)) ++count; //For the top-right
        if (MineAt(x - 1, y + 1)) ++count; //For the top-left
        if (MineAt(x + 1, y - 1)) ++count; //For the bottom-right
        if (MineAt(x - 1, y - 1)) ++count; //For the bottom-left
        #endregion

        return count;
    }
    
    /// <summary>
    /// If selected element returns empty then we will check the surrounding elements with Flood-Fill Algorithm to see if they are empty or not
    /// </summary>
    /// <param name="x">Selected elements x posisition</param>
    /// <param name="y">Selected elements y position</param>
    /// <param name="visited">A boolean list for checked surrounding elements</param>
    public static void FFuncover(int x , int y , bool[,] visited)
    {
        //Check if coordinates are in range
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            if (visited[x, y])
            {
                return;
            }
            else
            {
                //Uncover the element
                elements[x, y].LoadTextures(adjacentMines(x, y));

                //Check if the visited element has any mines around it? if it is then stop
                if(adjacentMines(x,y) > 0)
                {
                    return;
                }

                //set to visited
                visited[x, y] = true;

                //Recursion
                FFuncover(x - 1, y, visited);
                FFuncover(x + 1, y, visited);
                FFuncover(x, y - 1, visited);
                FFuncover(x, y + 1, visited);
            }
        }
    }

    public static bool isFinished()
    {
        //Try to find a covered element that is no mine
        foreach (Element lev in elements)
        {
            if(lev.isCovered() && !lev.isMine)
            {
                return false;
            }
        }
        //If there are mines in all the other covered elements then return true
        return true;
    }
}
