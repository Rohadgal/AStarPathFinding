using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameGraph : MonoBehaviour
{
    public float edgeRadius = 1f; // Specify the radius within which edges will be created

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

    Node targetNode;
    List<Node> nodes = new List<Node>();
    List<Node> starSearchNodes = new List<Node>();
    List<Node> dijkstraSearchNodes = new List<Node>();
    PathFinding pathFinding = new PathFinding();

    void Start() {
        // Find node GameObjects in the scene and add them to the graph
        FindNodesInScene();

        // Connect nodes with edges
        CreateEdges();

        //int it = 0;
        //foreach (Node node in nodes) {

        //    Debug.Log("index: " + it + " name: " + nodes[it].gameObject.name);

        //    it++;
        //}

        starSearchNodes = pathFinding.AStarSearch(nodes[61], nodes[245]);
        dijkstraSearchNodes = pathFinding.DijkstraSearch(nodes[61], nodes[245], nodes);

       // Debug.LogWarning("Nodes in star: " + starSearchNodes.Count);
        //Debug.LogWarning("Nodes in dijk: " + dijkstraSearchNodes.Count);

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
        Debug.Log("Nodes list amount: " + nodes.Count);
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

    //// A* algorithm search to find the shortest path from startNode to targetNode
    //List<Node> AStarSearch(Node startNode, Node targetNode) {
    //    List<Node> openSet = new List<Node>(); // Nodes to be evaluated
    //    HashSet<Node> closedSet = new HashSet<Node>(); // Nodes already evaluated

    //    openSet.Add(startNode);

    //    while (openSet.Count > 0) {
    //        Node currentNode = openSet[0];
    //        for (int i = 1; i < openSet.Count; i++) {
    //            if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)) {
    //                currentNode = openSet[i];
    //            }
    //        }

    //        openSet.Remove(currentNode);
    //        closedSet.Add(currentNode);

    //        if (currentNode == targetNode) {
    //            // Found the target node, reconstruct the path
    //            return RetracePath(startNode, targetNode);
    //        }

    //        foreach (Edge edge in currentNode.edges) {
    //            Node neighbor = edge.connectedNode;

    //            if (closedSet.Contains(neighbor)) {
    //                continue; // Skip this neighbor as it has already been evaluated
    //            }

    //            float newCostToNeighbor = currentNode.gCost + edge.distance;
    //            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
    //                neighbor.gCost = newCostToNeighbor;
    //                neighbor.hCost = Vector3.Distance(neighbor.gameObject.transform.position, targetNode.gameObject.transform.position);
    //                neighbor.parent = currentNode;

    //                if (!openSet.Contains(neighbor)) {
    //                    openSet.Add(neighbor);
    //                }
    //            }
    //        }
    //    }

    //    // Path not found
    //    return null;
    //}

    //// Retrace the path from startNode to endNode
    //List<Node> RetracePath(Node startNode, Node endNode) {
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while (currentNode != startNode) {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }

    //    path.Reverse(); // Reverse the path to get it from startNode to endNode
    //    return path;
    //}

    //// Dijkstra's algorithm search to find the shortest path from startNode to targetNode
    //List<Node> DijkstraSearch(Node startNode, Node targetNode) {
    //    Dictionary<Node, float> distances = new Dictionary<Node, float>();
    //    Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
    //    HashSet<Node> unvisitedNodes = new HashSet<Node>();

    //    foreach (Node node in nodes) {
    //        distances[node] = Mathf.Infinity;
    //        previousNodes[node] = null;
    //        unvisitedNodes.Add(node);
    //    }

    //    distances[startNode] = 0;

    //    while (unvisitedNodes.Count > 0) {
    //        Node currentNode = null;
    //        foreach (Node node in unvisitedNodes) {
    //            if (currentNode == null || distances[node] < distances[currentNode]) {
    //                currentNode = node;
    //            }
    //        }

    //        unvisitedNodes.Remove(currentNode);

    //        if (currentNode == targetNode) {
    //            break;
    //        }

    //        foreach (Edge edge in currentNode.edges) {
    //            float tentativeDistance = distances[currentNode] + edge.distance;
    //            if (tentativeDistance < distances[edge.connectedNode]) {
    //                distances[edge.connectedNode] = tentativeDistance;
    //                previousNodes[edge.connectedNode] = currentNode;
    //            }
    //        }
    //    }

    //    // Construct the shortest path
    //    List<Node> shortestPath = new List<Node>();
    //    Node current = targetNode;
    //    while (current != null) {
    //        shortestPath.Add(current);
    //        current = previousNodes[current];
    //    }
    //    shortestPath.Reverse(); // Reverse the path to get it from startNode to targetNode
    //    return shortestPath;
    //}


    //void UpdateTargetNode() {
    //    // Get the player's position
    //    Vector3 playerPosition = GetPlayerPosition(); // Implement this method to get the player's position

    //    // Find the closest node to the player's position
    //    float minDistance = Mathf.Infinity;
    //    foreach (Node node in nodes) {
    //        float distance = Vector3.Distance(playerPosition, node.gameObject.transform.position);
    //        if (distance < minDistance) {
    //            minDistance = distance;
    //            targetNode = node;
    //        }
    //    }
    //}
}
