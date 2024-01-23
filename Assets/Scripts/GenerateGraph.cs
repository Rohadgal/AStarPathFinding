using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGraph : MonoBehaviour{

    Graph graph;

    [SerializeField]
    public List<Node> nodes;
    Node endNode;

    private void Start(){
       createGraph();
    }

    private void createGraph(){
        foreach (Node node in nodes){
            node.checkNodesInArea();
            if (node.isEndNode()){
                endNode = node;
            }
        }

        // Set holistic of each node measuring the distance between the current node and the end node
        foreach (Node node in nodes){
            node.setHolistic(node.transform, endNode.transform);
        }

        graph = new Graph();
        graph.setNodes(nodes);

        // Print in console the cost of the edges and the holistic
        foreach (Node node in graph.getNodes()) {
            print("Node: " + node.name + " holstic: " + node.getHolistic());
            List<Edge> tempList =  node.GetEdges();
            foreach (Edge edge in tempList) {
                print("From: " + edge.getNodeFrom().name + " to: " + edge.getNodeTo().name + " with cost: " + edge.getDistance());
            }
        }
    }
}
