using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConwayCell : MonoBehaviour {
    
    public Image image;

    public ConwayPlayer owner = null; 

    public int x;
    public int y;

    bool isSelected = false;

    public void changeOwner(ConwayPlayer player)
    {
        if (player == owner)
        {
            return;
        } 
       
        owner = player;
        if (owner != null)
        {
            image.color = player.color;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void queueCellToLocalPlayer()
    {
        if (isSelected == false)
        {
            ConwayPlayer player = getLocalPlayer();
            image.color = new Color(player.color.r, player.color.g, player.color.b, .5f);
            player.queueCellForPlacement(this);
            isSelected = true;
        }
        else
        {
            ConwayPlayer player = getLocalPlayer();
            image.color = Color.white;
            player.dequeueCellForPlacement(this);
            isSelected = false;
        }

    }
    public void Update()
    {
        if (owner != null)
        {
            gameObject.GetComponent<Button>().enabled = false;
            isSelected = false;
        }
    }

    private ConwayPlayer getLocalPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<ConwayPlayer>();
    }
}
