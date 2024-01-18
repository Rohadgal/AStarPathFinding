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

    /* Setters */

    public Node(Vector2 pos, List<Edge> edges, int holistic){
        this.pos = pos;
        this._edges = edges;
        this._holistic = holistic;
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

    /* Getters */

    public Edge getCorretEdge()
    {
        return _correctEdge;
    }

    public bool isEndNode() {  return _endNode; }

    public List<Edge> GetEdges() { return _edges; }

    public int getHolistic() { return _holistic; }

    public void checkNodesInArea(){
        float radius = _radius;
        Collider[] percievedNodes = Physics.OverlapSphere(this.transform.position, radius);
        foreach(Collider col in percievedNodes){
            if(col.transform.position != this.transform.position){
                //print("hEY " + this.name + " collides with " + col.name);
                Edge newEdge = new Edge(this.GetComponent<Node>(), col.GetComponent<Node>(), (int)Vector3.Distance(this.transform.position, col.transform.position));
                if(_edges == null) {
                    _edges = new List<Edge>();
                }
                _edges.Add(newEdge);
                //print("From: " + newEdge.getNodeFrom().name + " to: " + newEdge.getNodeTo().name + " with cost: " + newEdge.getCost());
            }
        }
    }
}
