using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameGraph : MonoBehaviour
{
    public static GameGraph s_instance;
    public float edgeRadius = 1f; // Specify the radius within which edges will be created
    public GameObject playerPos;
    public GameObject enemyPos;

    public class Node {
        public float gCost;
        public float hCost;
        public float fCost { get { return gCost + hCost; } }
        public Node parent;


        public GameObject gameObject;
        public Vector3 position;
        public List<Edge> edges = new List<Edge>();

        // Constructor
        public Node(GameObject obj) {
            gameObject = obj;
            position = obj.transform.position;
        }
    }

    // Edge class
    public class Edge {
       public Node connectedNode;
       public float distance;

       public Edge(Node node, float dist) {
           connectedNode = node;
           distance = dist;
       }
    }

    Node playerTargetNode;
    Node enemySourceNode;
    List<Node> nodes = new List<Node>();
    public List<Node> starSearchNodes = new List<Node>();
    public List<Node> dijkstraSearchNodes = new List<Node>();
    PathFinding pathFinding = new PathFinding();

    public bool positionUpdated = false;


    private void Awake() {

        FindNodesInScene();

        // Connect nodes with edges
        CreateEdges();

        UpdateSourceNode();
        UpdateTargetNode();

        starSearchNodes = pathFinding.AStarSearch(enemySourceNode, playerTargetNode);
        dijkstraSearchNodes = pathFinding.DijkstraSearch(enemySourceNode, playerTargetNode, nodes);
    }
    void Start() {
        // Find node GameObjects in the scene and add them to the graph
        

        //int it = 0;
        //foreach (Node node in nodes) {

        //    Debug.Log("index: " + it + " name: " + nodes[it].gameObject.name);

        //    it++;
        //}



        // Debug.LogWarning("Nodes in star: " + starSearchNodes.Count);
        //Debug.LogWarning("Nodes in dijk: " + dijkstraSearchNodes.Count);

    }

    private void Update() {
        UpdateSourceNode();
        UpdateTargetNode();

        starSearchNodes = pathFinding.AStarSearch(enemySourceNode, playerTargetNode);
        dijkstraSearchNodes = pathFinding.DijkstraSearch(enemySourceNode, playerTargetNode, nodes);
    }

    void FindNodesInScene() {
        int i = 0;
        GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("Node"); // Assuming nodes have a "Node" tag
        int LayerWalkable = LayerMask.NameToLayer("Walkable");
        foreach (GameObject obj in nodeObjects) {
            if (obj.layer == LayerMask.NameToLayer("Walkable"))
            {
                nodes.Add(new Node(obj));
                i++;
            }
        }
        //Debug.Log("Nodes found: " + i);
       // Debug.Log("Nodes list amount: " + nodes.Count);
    }

    void CreateEdges() {
        // Connect nodes with edges
        for (int i = 0; i < nodes.Count; i++) {
            Node currentNode = nodes[i];
            for (int j = i + 1; j < nodes.Count; j++) {
                Node targetNode = nodes[j];
                // Check if an edge between currentNode and targetNode already exists
                bool edgeExists = false;
                foreach (Edge edge in currentNode.edges) {
                    if (edge.connectedNode == targetNode) {
                        edgeExists = true;
                        break;
                    }
                }
                if (!edgeExists) {
                    float distance = Vector3.Distance(currentNode.gameObject.transform.position, targetNode.gameObject.transform.position);
                    if (distance <= edgeRadius) {
                        // Create an edge between nodes
                        Edge newEdge = new Edge(targetNode, distance);
                        currentNode.edges.Add(newEdge);
                        targetNode.edges.Add(new Edge(currentNode, distance)); // Add edge to targetNode as well
                    }
                }
            }
        }
    }
    void OnDrawGizmos() {
        foreach (Node node in nodes) {
            foreach (Edge edge in node.edges) {
                Gizmos.color = Color.green; // You can set any color you like
                Gizmos.DrawLine(node.gameObject.transform.position, edge.connectedNode.gameObject.transform.position);
            }
        }

        foreach (Node node in starSearchNodes) {
            foreach (Edge edge in node.edges) {
                if (starSearchNodes.Contains(edge.connectedNode)) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.gameObject.transform.position, edge.connectedNode.gameObject.transform.position);
                }
            }
        }

        foreach (Node node in dijkstraSearchNodes) {
            foreach (Edge edge in node.edges) {
                if (dijkstraSearchNodes.Contains(edge.connectedNode)) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(node.gameObject.transform.position, edge.connectedNode.gameObject.transform.position);
                }
            }
        }
    }

    void UpdateTargetNode() {
        // Get the player's position
        Vector3 playerPosition = playerPos.gameObject.transform.position; //GetPlayerPosition(); // Implement this method to get the player's position

        // Find the closest node to the player's position
        float minDistance = Mathf.Infinity;
        foreach (Node node in nodes) {
            float distance = Vector3.Distance(playerPosition, node.gameObject.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                playerTargetNode = node;
            }
        }
    }

    void UpdateSourceNode() {
        // Get the player's position
        Vector3 enemyPosition = enemyPos.gameObject.transform.position; //GetPlayerPosition(); // Implement this method to get the player's position

        // Find the closest node to the player's position
        float minDistance = Mathf.Infinity;
        foreach (Node node in nodes) {
            float distance = Vector3.Distance(enemyPosition, node.gameObject.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                enemySourceNode = node;
            }
        }
    }
}
