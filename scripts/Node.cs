using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    private NodeManager.BoxColor currentColor;
    private SpriteRenderer spRenderer;
    private bool isSelectable = true;
    private bool isSelected = false;

    private int[] row = new int[] { 1, -1, 0 , 0};
    private int[] col = new int[] { 0, 0, 1  , -1};

    public int x { get; private set; }
    public int y { get; private set; }

    public event EventHandler OnNodeCanBeSelected;
    public event EventHandler OnNodeCanNotBeSelected;
    public event EventHandler OnNodeGenerated;
    public event EventHandler OnNodeSelected;
    public event EventHandler OnNodeNotSelected;




    private void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();

        MouseLogic.Instance.OnNodesPope += Instance_OnNodesPope;

        CanNotBeSelected();
        GenerateAnotherInstance();
    }


    private void Instance_OnNodesPope(object sender, EventArgs e)
    {
        if(currentColor != NodeManager.BoxColor.BOMB)
        {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            int nodeX = x + row[i];
            int nodeY = y + col[i];
            if (nodeX >= 0 && nodeY >= 0 && nodeX < 7 && nodeY < 6)
            {
                Node NeighaborNode = NodeManager.Instance.nodesArray[nodeX, nodeY];

                if (NeighaborNode.isSelected)
                {
                    List<Node> nodeImpacted = NodeManager.Instance.BombNode(this);
                    for (int c = 0; c < nodeImpacted.Count; c++)
                    {
                        nodeImpacted[c].GenerateAnotherInstance();
                    }

                }

            }


        }
    }



    public void GenerateAnotherInstance()
    {
        int bombChance = 5;
        int randomBombNumber = UnityEngine.Random.Range(0, 100);

        if (randomBombNumber < bombChance)
        {
            currentColor = NodeManager.BoxColor.BOMB;
            spRenderer.color = Color.black;
            isSelectable = false;
        }
        else
        {
            int randomColorNumber = UnityEngine.Random.Range(0, 4);

            switch (randomColorNumber)
            {
                case 0:
                    currentColor = NodeManager.BoxColor.YELLOW;
                    spRenderer.color = Color.yellow;
                    isSelectable = true;
                    break;
                case 1:
                    currentColor = NodeManager.BoxColor.BLUE;
                    spRenderer.color = Color.blue;
                    isSelectable = true;
                    break;
                case 2:
                    currentColor = NodeManager.BoxColor.GREEN;
                    spRenderer.color = Color.green;
                    isSelectable = true;
                    break;
                case 3:
                    currentColor = NodeManager.BoxColor.PURPUL;
                    spRenderer.color = Color.purple;
                    isSelectable = true;
                    break;
            }
        }

        gameObject.SetActive(true);
        OnNodeGenerated?.Invoke(this, EventArgs.Empty);
    }
    public void OnCanBeSelected()
    {
        OnNodeCanBeSelected?.Invoke(this , EventArgs.Empty);
    }

    public void NodeSelected()
    {
        isSelected = true;
        OnNodeSelected?.Invoke(this, EventArgs.Empty);
    }
    public void NodeRemoveSelected()
    {
        OnNodeNotSelected?.Invoke(this, EventArgs.Empty);
        isSelected = false;
    }

    public void CanNotBeSelected()
    {
        OnNodeCanNotBeSelected?.Invoke(this, EventArgs.Empty);
    }

    public NodeManager.BoxColor GetNodeColor()
    {
        return currentColor;
    }


    public void SetNodePosition(int x , int y)
    {
        this.x = x;
        this.y = y;
    }
    public bool GetIsSelectable()
    {
        return isSelectable;
    }

    public void HideTheNode()
    {
        gameObject.SetActive(false);
        GenerateAnotherInstance();

      /*  if (currentColor != NodeManager.BoxColor.BOMB) return;

        List<Node> bombList = NodeManager.Instance.BombNode(this);

        for (int i = 0; i < bombList.Count; i++)
        {
            bombList[i].GenerateAnotherInstance();
        }/*/

    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
}
