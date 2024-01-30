using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    Node _from;
    Node _to;
    int _distance;

    public Edge(ref Node from, ref Node to, int distance){
        _from = from;
        _to = to;
        _distance = distance;
    }

    public Edge(Node from, Node to, int distance)
    {
        _from = from;
        _to = to;
        _distance = distance;
    }

    public Edge(){
    }

    public void setEdge(ref Node from, ref Node to, int cost){
        _from = from;
        _to = to;
        _distance = cost;
    }

    public void setDistance(int distance){
        _distance = distance;
    }

    public Node getNodeFrom(){
        return _from;
    }

    public Node getNodeTo(){
        return _to;
    }

    public int getDistance() { return _distance; }
}
