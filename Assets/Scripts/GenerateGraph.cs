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

        foreach (Node node in nodes){
            node.setHolistic(node.transform, endNode.transform);
            //print("Node: " + node.name + " holstic: " + node.getHolistic());
        }

        graph = new Graph();
        graph.setNodes(nodes);

        foreach (Node node in graph.getNodes()) {
            print("Node: " + node.name + " holstic: " + node.getHolistic());
            List<Edge> tempList =  node.GetEdges();
            foreach (Edge edge in tempList) {
                print("From: " + edge.getNodeFrom().name + " to: " + edge.getNodeTo().name + " with cost: " + edge.getCost());
            }
        }
    }


}
