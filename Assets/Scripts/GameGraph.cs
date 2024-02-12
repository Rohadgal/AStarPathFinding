using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class GameGraph : MonoBehaviour
{
    public float edgeRadius = 1f; // Specify the radius within which edges will be created

    // Node class
    class Node {
        public GameObject gameObject;
        public List<Edge> edges = new List<Edge>();

        public Node(GameObject obj) {
            gameObject = obj;
        }
    }

    // Edge class
     class Edge {
        public Node connectedNode;
        public float distance;

        public Edge(Node node, float dist) {
            connectedNode = node;
            distance = dist;
        }
    }

     List<Node> nodes = new List<Node>();

    void Start() {
        // Find node GameObjects in the scene and add them to the graph
        FindNodesInScene();

        // Connect nodes with edges
        CreateEdges();

        foreach(Node node in nodes) {
            Debug.Log("Node name: " + node.gameObject.name);
            foreach(Edge edge in node.edges) {
                Debug.Log("Connected to node: " + edge.connectedNode.gameObject.name);
            }
        }
    }

    void FindNodesInScene() {
        int i = 0;
        GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("Node"); // Assuming nodes have a "Node" tag
        foreach (GameObject obj in nodeObjects) {
            nodes.Add(new Node(obj));
            i++;
        }
        Debug.Log("Nodes found: " + i);
        Debug.Log("Nodes list amount: " + nodes.Count);
    }

    void CreateEdges() {
        // Connect nodes with edges
        for (int i = 0; i < nodes.Count; i++) {
            Node currentNode = nodes[i];
            for (int j = i + 1; j < nodes.Count; j++) {
                Node targetNode = nodes[j];
                float distance = Vector3.Distance(currentNode.gameObject.transform.position, targetNode.gameObject.transform.position);
                if (distance <= edgeRadius) {
                    // Create an edge between nodes
                    Edge newEdge = new Edge(targetNode, distance);
                    currentNode.edges.Add(newEdge);
                    targetNode.edges.Add(newEdge); // For undirected graph, add edge to both nodes

                }
            }
        }
    }
}
