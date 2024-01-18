using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graph 
{ 
    List<Node> nodes;
    Queue<Node> toAnalize = new Queue<Node>();
    Queue<Node> visited = new Queue<Node>();

    public Graph(){
        nodes = new List<Node>();
    }

    public List<Node> getNodes() { return nodes; }

    public void setNodes(List<Node> nodesList) {
        nodes = nodesList;
    }

    public void Initialize(){
        Edge edge1 = new Edge();
        Edge edge2 = new Edge();
        List<Edge> list1 = new List<Edge>();
        list1.Add(edge1);
        list1.Add(edge2);
        Node node1 = new Node(new Vector2(2,3), list1, 10);

        Edge edge3 = new Edge();
        Edge edge4 = new Edge();
        List<Edge> list2 = new List<Edge>();
        list2.Add(edge2);
        list2.Add(edge3);
        list2.Add(edge4);
        Node node2 = new Node(new Vector2(2,4), list2, 12);

        Edge edge5 = new Edge();
        Edge edge6 = new Edge();
        List<Edge> list3 = new List<Edge>();
        list3.Add(edge4);
        list3.Add(edge5);
        list3.Add(edge6);
        Node node3 = new Node(new Vector2(2,5), list3, 14);
        
        Edge edge7 = new Edge();
        List<Edge> list4 = new List<Edge>();
        list4.Add(edge6);
        list4.Add(edge7);
        Node node4 = new Node(new Vector2(2,6), list4, 16);

        Edge edge8 = new Edge();
        List<Edge> list5 = new List<Edge>();
        list5.Add(edge5);
        list5.Add(edge8);
        Node node5 = new Node(new Vector2(5,5), list5, 13);

        Edge edge9 = new Edge();
        List<Edge> list6 = new List<Edge>();
        list6.Add(edge1);
        list6.Add(edge3);
        list6.Add(edge8);
        list6.Add(edge9);
        Node node6 = new Node(new Vector2(5,3), list6, 3);

        Edge edge10 = new Edge();
        List<Edge> list7 = new List<Edge>();
        list7.Add(edge9);
        list7.Add(edge10);
        Node node7 = new Node(new Vector2(9,5), list7, 3);

        Edge edge11 = new Edge();
        List<Edge> list8 = new List<Edge>();
        list8.Add(edge7);
        list8.Add(edge11);
        Node node8 = new Node(new Vector2(10,6), list8, 7);
        
        List<Edge> list9 = new List<Edge>();
        Node node9 = new Node(new Vector2(10,1), list9, 0);

        edge1.setEdge(ref node1, ref node6, 3);
        edge2.setEdge(ref node1, ref node2, 1);
        edge3.setEdge(ref node2, ref node6, 5);
        edge4.setEdge(ref node2, ref node3, 1);
        edge5.setEdge(ref node3, ref node5, 4);
        edge6.setEdge(ref node3, ref node4, 1);
        edge7.setEdge(ref node4, ref node8, 10);
        edge8.setEdge(ref node5, ref node6, 4);
        edge9.setEdge(ref node6, ref node7, 25);
        edge10.setEdge(ref node7, ref node9, 6);
        edge11.setEdge(ref node8, ref node9, 7);
        
    }

    public void AStar(){
        
    }

    private void Analize(Node node){
        Queue<Node> toAnalize = new Queue<Node>();
        Queue<Node> hold = new Queue<Node>();
        int cost = 0;
        int minCost = 0;
        int smallestNodeCostIndex = 0;
        for (int i = 0; i < node.GetEdges().Count; i++) {
            Edge edge = node.GetEdges()[i];
            cost = edge.getNodeTo().getHolistic() + edge.getCost();
            if(i == 0){
                minCost = cost;
                continue;
            }
            if(minCost < cost){
                minCost = cost;
                smallestNodeCostIndex = i;
            }
        }
        toAnalize.Enqueue(node.GetEdges()[smallestNodeCostIndex].getNodeTo());

        for(int i = 0; i <= node.GetEdges().Count; i++){
            if(i != smallestNodeCostIndex) {
                hold.Enqueue(node.GetEdges()[i].getNodeTo());
            }
        }
    }

    void generateGraph()
    {

    }
}
