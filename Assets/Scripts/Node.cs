using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{
    Vector2 pos;
    List<Edge> edges;
    int holistic;

    public Node(Vector2 pos, List<Edge> edges, int holistic){
        this.pos = pos;
        this.edges = edges;
        this.holistic = holistic;
    }
}
