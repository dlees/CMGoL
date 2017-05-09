using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLoader : MonoBehaviour {


    public GameObject contentPanel;
    public GameObject conwayCellTemplate;
    public List<ConwayPlayer> players;
    public int width = 20;
    public int height = 20;    

    private ConwayCell[,] world;

    void Start () {
        world = new ConwayCell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height ; j++)
            {
                GameObject conwayCell = Instantiate(conwayCellTemplate) as GameObject;

                // Ideally the world should load the data here and this class should read the data
                conwayCell.GetComponent<ConwayCell>().x = i;
                conwayCell.GetComponent<ConwayCell>().y = j;

                conwayCell.transform.parent = contentPanel.transform;
                conwayCell.transform.localScale = Vector3.one;

                world[i, j] = conwayCell.GetComponent<ConwayCell>();
            }
        }


        InvokeRepeating("Calculate", 0, 5);
    }

    public void Calculate()
    {
        CalculateNewWorld();

        CalculatePlayerScore();
    }

    private void CalculateNewWorld()
    {

        ConwayPlayer[,] newOwners = new ConwayPlayer[width, height];
        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                newOwners[i, j] = calculateNewOwner(i, j);
            }
        }

        for (int i = 0; i < world.GetLength(0); i++)
        {
            for (int j = 0; j < world.GetLength(1); j++)
            {
                world[i, j].changeOwner(newOwners[i, j]);
            }
        }
    }

    private ConwayPlayer calculateNewOwner(int i, int j)
    {
        List<ConwayPlayer> neighborOwnership = getNeighborOwnership(i, j);

        Debug.Log(i + "," + j + ": " + neighborOwnership.Count);

        if (world[i,j].owner == null)
        {
            if (neighborOwnership.Count > 2 && neighborOwnership.Count < 4)
            {
                return neighborOwnership[0];
            }
            else
            {
                return null;
            }
        }

        // overcrowded or too sparse
        if (neighborOwnership.Count > 3 || neighborOwnership.Count < 2)
        {
            return null;
        }
        else
        {
            return world[i, j].owner;
        }
    }

    private List<ConwayPlayer> getNeighborOwnership(int x, int y)
    {
        List<ConwayPlayer> ownershipInfo = new List<ConwayPlayer>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i ==0 && j == 0)
                {
                    continue;
                }
                ConwayPlayer ownerOfNeighbor = world[modWithNegative(x + i, world.GetLength(0)) , modWithNegative(y + j, world.GetLength(1))].owner;
                if (ownerOfNeighbor != null)
                {
                    ownershipInfo.Add(ownerOfNeighbor);
                }
            }
        }
        return ownershipInfo;
    }

    private int modWithNegative(int num, int mod)
    {
        return num % mod >= 0 ? num % mod: mod-1;
    }

    private void CalculatePlayerScore()
    {
        Dictionary<ConwayPlayer, int> scores = createZeroScores();

        foreach (ConwayCell cell in world)
        {
            if (cell.owner != null)
            {
                scores[cell.owner]++;
            }
        }
        updatePlayerScoreText(scores);
    }

    private void updatePlayerScoreText(Dictionary<ConwayPlayer, int> scores)
    {
        foreach (ConwayPlayer player in players)
        {
            player.score = scores[player];
        }
    }

    private Dictionary<ConwayPlayer, int> createZeroScores()
    {
        Dictionary<ConwayPlayer, int> scores = new Dictionary<ConwayPlayer, int>(players.Count);

        foreach (ConwayPlayer player in players)
        {
            scores.Add(player, 0);
        }
        return scores;
    }
    
}
