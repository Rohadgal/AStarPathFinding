using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{
    Vector2 pos;
    List<Edge> _edges;
    int _holistic;
    bool _visited;
    Edge _correctEdge;
    [SerializeField]
    bool _endNode;
    [SerializeField]
    float _radius = 3.0f;
    float _walkedPath;
    float _cost;

    /* Setters */
    public Node(){
        _walkedPath = -1;
    }

    public Node(Vector2 pos, List<Edge> edges, int holistic){
        this.pos = pos;
        this._edges = edges;
        this._holistic = holistic;
        _walkedPath = -1;
    }

    public void setEdgeList(List<Edge> edges){
        _edges = edges;
    }

    public void setCorrectEdge(Edge edge){
        _correctEdge = edge;
    }

    public void setHolistic(Transform from, Transform to){
        _holistic = (int)Vector2.Distance(from.position, to.position);
    }

    public void setCost(float cost)
    {
        _cost = cost;
    }

    public void setWalkedPath(float walkedPath)
    {
        _walkedPath = walkedPath;
    }

    /* Getters */

    public Edge getCorrectEdge()
    {
        return _correctEdge;
    }

    public bool isEndNode() {  return _endNode; }

    public List<Edge> GetEdges() { return _edges; }

    public int getHolistic() { return _holistic; }

    public float getCost() { return _cost; }

    public float getWalkedPath() { return _walkedPath; }

    public void checkNodesInArea(){
        float radius = _radius;
        // Use overlapSphere to detect nodes in area
        Collider[] percievedNodes = Physics.OverlapSphere(this.transform.position, radius);
        foreach(Collider col in percievedNodes){
            if(col.transform.position != this.transform.position){
                // Create edge with cost using the distance between nodes
                Edge newEdge = new Edge(this.GetComponent<Node>(), col.GetComponent<Node>(), (int)Vector3.Distance(this.transform.position, col.transform.position));
                if(_edges == null) {
                    _edges = new List<Edge>();
                }
                // Add the edges created to each node´s list
                _edges.Add(newEdge);
            }
        }
    }
}
