using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConwayPlayer : MonoBehaviour {

    public Color color;
    public Text scoreText;

    private HashSet<ConwayCell> queuedCells = new HashSet<ConwayCell>();

    public int score;

    public void queueCellForPlacement(ConwayCell cell)
    {
        queuedCells.Add(cell);
    }

    public void placeCells()
    {
        score += queuedCells.Count;

        foreach (ConwayCell cell in queuedCells)
        {
            cell.changeOwner(this);
        }
        queuedCells.Clear();
    }

    public void Update()
    {
        scoreText.text = score.ToString();
    }

    internal void dequeueCellForPlacement(ConwayCell conwayCell)
    {
        queuedCells.Remove(conwayCell);
    }
}
