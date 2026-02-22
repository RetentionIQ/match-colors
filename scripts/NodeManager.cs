using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    private int width = 7;
    private int height = 6;
    public Node[,] nodesArray { get; set; }
    private int[] row = new int[] { -1, 1, 0, 0, 1, -1, -1, 1 };
    private int[] col = new int[] { 0, 0, 1, -1  , 1 , -1 , 1 , -1};




    public static NodeManager Instance { get; private set; }

    public enum BoxColor
    {
        YELLOW,
        BLUE,
        GREEN,
        PURPUL,
        BOMB
    }


    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nodesArray = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodesArray[x, y] = transform.Find(x + "," + y).GetComponent<Node>();
                nodesArray[x, y].SetNodePosition(x, y);
            }
        }
    }

    public List<Node> GetNodeGroup(Node currentNode)
    {
        List <Node> selectedNodes = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(currentNode);
        bool[,] isChecked = new bool[width,height];
        isChecked[currentNode.x, currentNode.y] = true;

        while(queue.Count > 0)
        {
            Node prevoiusNode = queue.Dequeue();
            
            for (int i = 0; i < 8; i++)
            {
                int rows = prevoiusNode.x + row[i];
                int cols = prevoiusNode.y + col[i];

                if (rows >= 0 && cols >= 0 && rows < width && cols < height && prevoiusNode.GetNodeColor() == nodesArray[rows, cols].GetNodeColor() && !isChecked[rows, cols])
                {
                    nodesArray[rows, cols].OnCanBeSelected();
                    selectedNodes.Add(nodesArray[rows, cols]);
                    isChecked[rows, cols] = true;
                    queue.Enqueue(nodesArray[rows, cols]);
                }

            }
        }
        
        return selectedNodes;

    }
    public List<Node> GetNieghaborNodes(Node currentNode)
    {
        List<Node> selectedNodes = new List<Node>();
        selectedNodes.Add(currentNode);
        bool[,] isChecked = new bool[width, height];
        isChecked[currentNode.x, currentNode.y] = true;
        for (int i = 0; i < 8; i++)
        {
            int rows = currentNode.x + row[i];
            int cols = currentNode.y + col[i];

            if (rows >= 0 && cols >= 0 && rows < width && cols < height && currentNode.GetNodeColor() == nodesArray[rows, cols].GetNodeColor() && !isChecked[rows, cols])
            {
                //HAS_SAME_COLOR_AND_NEXT_TO_IT!
                selectedNodes.Add(nodesArray[rows, cols]);
                isChecked[rows, cols] = true;
            }

        }
        return selectedNodes;


    }
    public List<Node> BombNode(Node currentNode)
    {
        List<Node> selectedNodes = new List<Node>();
        int range = 2;

        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                int nodeX = currentNode.x + x;
                int nodeY = currentNode.y + y;
                if (nodeX < 0 || nodeY < 0 || nodeX > 7 || nodeY > 6) continue;

                if(Mathf.Abs(x) + Mathf.Abs(y) > range)
                {
                    continue;
                }
                if (nodeX < 0 || nodeY < 0 || nodeX >= 7 || nodeY >= 6)
                {
                    continue;
                }
                    selectedNodes.Add(nodesArray[nodeX, nodeY]);
            }
        }

        return selectedNodes;

    }

}
