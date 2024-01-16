using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    Node _from;
    Node _to;
    int _cost;

    public Edge(ref Node from, ref Node to, int cost){
        _from = from;
        _to = to;
        _cost = cost;
    }

    public Edge(){
    }

    public void setEdge(ref Node from, ref Node to, int cost){
        _from = from;
        _to = to;
        _cost = cost;
    }

    public Node getNodeFrom(){
        return _from;
    }

    public Node getNodeTo(){
        return _to;
    }

    public int getCost() { return _cost; }
}
