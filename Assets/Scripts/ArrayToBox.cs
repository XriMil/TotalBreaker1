using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArrayToBox : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform parentObject;
    //Start Position indicates where the first block should spawn.
    [SerializeField] private Vector2 startPos;

    [Header("Set Number of rows")]
    [SerializeField] public int rows;
    [SerializeField] private int columns;

    [Header("Upper limit of hit range per team")]
    [SerializeField] private int redTeam = 60;
    [SerializeField] private int greenTeam = 80;
    [SerializeField] private int blueTeam = 100;
    [SerializeField] private int purpleTeam = 120;
    [SerializeField] private int orangeTeam = 140;
    private int[] modeList = { 1, 2, 3, 4 };
    private int[,] boxMode;
    
    private int[,] MakeArray()
    {
        int height = rows;        /*Number of rows*/
        int width = columns;      /*Number of columns*/

        // Percentage that defines the lower limit of hits range per team
        float percentage = 0.15f;

        System.Random rand = new System.Random();

        // Calculate lower limit of hits per team
        int TeamLow(int team, float percent)
        {
            int a = team - (int)(team * percent);
            return a;
        }

        // Calculate random number of boxes per line
        int BoxNumLine(int w)
        {
            //absolute min number of boxes per line
            int minInit = (int)(w / 2) + 1;
            int threshMax = w + 1; //max limit fop upper range
            int threshMin = minInit + 2; //min limit for lower range

            //Select a value randomly between minInit and min threshold
            int minNum = rand.Next(minInit, threshMin + 1);
            // Select a value randomly from higher values
            int maxNum = rand.Next(width - 2, threshMax);

            int boxNum = rand.Next(minNum, maxNum);
            return boxNum;
        }

        // Calculate random initial position of boxes
        int BoxLoc(int w, int boxNumber)
        {
            // Select a value randomly between 0 and width - boxnumber
            int boxStart = rand.Next(0, (w - boxNumber) + 1);
            return boxStart;
        }

        // Gets values used for calculation of placing special boxes
        int[] GetBoxSpecial(int boxes, int pos, int team, int hits, int line)
        {
            int[] tmpspecial = { boxes, pos, team, hits, line };
            return tmpspecial;
        }

        // Calculates random position of a special box in a row
        int PosSpecial(int boxes, int pos)
        {
            //System.Random rand = new System.Random();
            int position = rand.Next(pos, (boxes + pos));
            return position;
        }

        // Chooses randomly depending on a and b args a special box
        int SelectSpecialBox(int a, int b)
        {
            int select = rand.Next(a, b);
            return modeList[select];
        }

        //Set color teamss
        int[] teamsList = { 0, 1, 2, 3, 4 };
        
        // 2D array to associate team with colors 
        int[,] teamsComplete = { { 0, redTeam }, { 1, greenTeam }, { 2, blueTeam }, { 3, purpleTeam }, { 4, orangeTeam } };

        // Create a zero matrix with dimensions of board
        // to save hit points per box
        int[,] pista = new int[height, width];
        for (int h = 0; h < height; h++)
            for (int w = 0; w < width; w++)
                pista[h, w] = 0;

        // Create a zero (preassigned values for normal box) matrix with 
        // dimensions of board to save mode of operation per box
        boxMode = new int[height, width];
        for (int j = 0; j < height; j++)
            for (int k = 0; k < width; k++)
                boxMode[j, k] = 0;


        //System.Random rand = new System.Random();
        int randBoxNum; int boxPos; int randTeam; int randHits;
        int[] previous = { 0, 0, 0, 0, 0 }; int[] special = { 0, 0, 0, 0, 0 };

        for (int x = 0; x < height; x++)
        {
            // Leave a blank row to the top of game board
            if (x == (height-7))
                continue;

            // Get color team
            randTeam = rand.Next(0, teamsList.Length);
            // Get number of boxes
            randBoxNum = BoxNumLine(width);
            // Get position of boxes
            boxPos = BoxLoc(width, randBoxNum);
            // Get number of hits
            randHits = rand.Next(TeamLow(teamsComplete[randTeam, 1], percentage), teamsComplete[randTeam, 1]);
            
            // Place boxes randomly to available positions 
            for (int y = 0; y < width; y++)
            {
                if (y >= boxPos && y < (randBoxNum + boxPos))
                    pista[x, y] = randHits;
            }

            // Check if must add a special box
            if (previous[2] > (teamsList.Length - 3))
            {
                // Check if previous new and previous lines are high color teams 
                special = GetBoxSpecial(randBoxNum, boxPos, randTeam, randHits, x);
                if (randTeam > (teamsList.Length - 3))
                {
                    // Get a random position to put special box
                    int a = PosSpecial(randBoxNum, boxPos);
                    boxMode[x, a] = SelectSpecialBox(2,4);
                }
                else if (randTeam > 0)
                {
                    int a = PosSpecial(randBoxNum, boxPos);
                    boxMode[x, a] = SelectSpecialBox(0, 2);
                }
           }
           // Update current value
           previous = GetBoxSpecial(randBoxNum, boxPos, randTeam, randHits, x);
        }
        return pista;
    }
    
    // Start is called before the first frame update
       
    void Start()
    {
        //boxMode = new int[rows,columns];
        LevelNum.linesRemained = rows - 7;
        ArrayToObjects(MakeArray(), boxMode);
    }

    public void ArrayToObjects(int[,] arrayHits,int[,] arrayMode)
    {
        //Calculate block width and height
        float blockWidth = blockPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float blockHeight = blockPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        //Next Position indicates where the next block should spawn
        Vector2 nextPos = startPos;
                        
        //Check all array positions:
        for (int i = arrayHits.GetLength(0) - 1; i >= 0; i--)
        {
            //int cnt = 0;
            for (int j = 0; j < arrayHits.GetLength(1); j++)
            //for (int j = 0; j < arrayHits.GetLength(1); j++)
            {
                //If array position value is greater than 0: Spawn Object as child of parentObj
                if (arrayHits[i, j] > 0)
                {
                    GameObject block = Instantiate(blockPrefab, nextPos, transform.rotation, parentObject);
                    //Set block color depending on number of hits                    
                    block.GetComponent<Box>().SetTeamColor(BoxTeamColor(arrayHits[i, j], arrayMode[i, j]));
                    //Set block sprite depending on its mode of operation
                    block.GetComponent<Box>().SetBoxSprite(arrayMode[i, j]);
                    //Set blocks hitpoints equal to array value
                    block.GetComponent<Box>().SetHitPoints(arrayHits[i, j]);
                    //Set absolute grid position of the box
                    block.GetComponent<Box>().SetAbsolutePosX(j);
                    block.GetComponent<Box>().SetAbsolutePosY(arrayHits.GetLength(0) - 1 - i);
                }
                
                //Calculate next X spawn position
                nextPos.x += blockWidth;
            }
            //Calculate next Y spawn position
            nextPos.y += blockHeight;
            nextPos.x = startPos.x;
        }
    }

    // Set Box Initial Color
    public Color32 BoxTeamColor(int hitsState, int specialColor)
    {
        Color32 boxColor = new Color32();
        //If box is special assign special color 
        //else assign appropriate team color 
        if (specialColor > 0)
            boxColor = new Color32(140, 120, 30, 255);
        else if (hitsState <= redTeam)
            boxColor = new Color32(225, 116, 116, 255);
        else if (hitsState <= greenTeam)
            boxColor = new Color32(36, 142, 36, 255);
        else if (hitsState <= blueTeam)
            boxColor = new Color32(71, 71, 226, 255);
        else if (hitsState <= purpleTeam)
            boxColor = new Color32(148, 98, 148, 255);
        else
            boxColor = new Color32(193, 104, 14, 255);

        return boxColor;
    }

    public bool CheckForWin()
    {
        if ((rows - 6) <= Spawner3.levelCount)
        {
            Box[] boxes = FindObjectsOfType<Box>();
            return boxes.Length == 0;
        }
        else
            return false;
    }

    /*//Move Blocks Parent down
    public void NextLevel()
    {
        parentObject.position -= new Vector3(0, blockPrefab.GetComponent<SpriteRenderer>().bounds.size.y);
    }
*/
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
        }*/
        if (CheckForWin())
            SceneManager.LoadScene(5);
    }
}
