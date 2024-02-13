using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    
    public List<GameGraph.Node> AStarSearch(GameGraph.Node startNode, GameGraph.Node targetNode) {
        List<GameGraph.Node> openSet = new List<GameGraph.Node>(); // Nodes to be evaluated
        HashSet<GameGraph.Node> closedSet = new HashSet<GameGraph.Node>(); // Nodes already evaluated

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            GameGraph.Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                // Found the target node, reconstruct the path
                return RetracePath(startNode, targetNode); 
            }

            foreach (GameGraph.Edge edge in currentNode.edges) {
                GameGraph.Node neighbor = edge.connectedNode;

                if (closedSet.Contains(neighbor)) {
                    continue; // Skip this neighbor as it has already been evaluated
                }

                float newCostToNeighbor = currentNode.gCost + edge.distance;
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = Vector3.Distance(neighbor.gameObject.transform.position, targetNode.gameObject.transform.position);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // Path not found
        return null;
    }

    // Retrace the path from startNode to endNode
    public List<GameGraph.Node> RetracePath(GameGraph.Node startNode, GameGraph.Node endNode) {
        List<GameGraph.Node> path = new List<GameGraph.Node>();
        GameGraph.Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse(); // Reverse the path to get it from startNode to endNode
        return path;
    }
}
