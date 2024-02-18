using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    
    public List<GameGraph.Node> AStarSearch(GameGraph.Node startNode, GameGraph.Node targetNode) {

            //// Create Stopwatch instance
            //Stopwatch stopwatch = new Stopwatch();

            //// Start measuring time
            //stopwatch.Start();



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
                    //// Stop measuring time
                    //stopwatch.Stop();

                    //// Get elapsed time in milliseconds
                    //long elapsedTimeMs = stopwatch.ElapsedMilliseconds;
                    //UnityEngine.Debug.Log("Elapsed time (ms): " + elapsedTimeMs);

                    //// Get elapsed time in ticks
                    //long elapsedTimeTicks = stopwatch.ElapsedTicks;
                    //UnityEngine.Debug.Log("Elapsed time (ticks): " + elapsedTimeTicks);

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

        path.Add(currentNode);

        path.Reverse(); // Reverse the path to get it from startNode to endNode
        return path;
    }



    // Dijkstra's algorithm search to find the shortest path from startNode to targetNode
    public List<GameGraph.Node> DijkstraSearch(GameGraph.Node startNode, GameGraph.Node targetNode, List<GameGraph.Node> nodes) {
            //// Create Stopwatch instance
            //Stopwatch dstopwatch = new Stopwatch();

            //// Start measuring time
            //dstopwatch.Start();
        Dictionary<GameGraph.Node, float> distances = new Dictionary<GameGraph.Node, float>();
        Dictionary<GameGraph.Node, GameGraph.Node> previousNodes = new Dictionary<GameGraph.Node, GameGraph.Node>();
        HashSet<GameGraph.Node> unvisitedNodes = new HashSet<GameGraph.Node>();

        foreach (GameGraph.Node node in nodes) {
            distances[node] = Mathf.Infinity;
            previousNodes[node] = null;
            unvisitedNodes.Add(node);
        }

        distances[startNode] = 0;

        while (unvisitedNodes.Count > 0) {
            GameGraph.Node currentNode = null;
            foreach (GameGraph.Node node in unvisitedNodes) {
                if (currentNode == null || distances[node] < distances[currentNode]) {
                    currentNode = node;
                }
            }

            unvisitedNodes.Remove(currentNode);

            if (currentNode == targetNode) {
                break;
            }

            foreach (GameGraph.Edge edge in currentNode.edges) {
                float tentativeDistance = distances[currentNode] + edge.distance;
                if (tentativeDistance < distances[edge.connectedNode]) {
                    distances[edge.connectedNode] = tentativeDistance;
                    previousNodes[edge.connectedNode] = currentNode;
                }
            }
        }

        // Construct the shortest path
        List<GameGraph.Node> shortestPath = new List<GameGraph.Node>();
        GameGraph.Node current = targetNode;
        while (current != null) {
            shortestPath.Add(current);
            current = previousNodes[current];
        }
        shortestPath.Reverse(); // Reverse the path to get it from startNode to targetNode
            //// Stop measuring time
            //dstopwatch.Stop();

            //// Get elapsed time in milliseconds
            //long elapsedTimeMs = dstopwatch.ElapsedMilliseconds;
            //UnityEngine.Debug.Log("dElapsed time (ms): " + elapsedTimeMs);

            //// Get elapsed time in ticks
            //long elapsedTimeTicks = dstopwatch.ElapsedTicks;
            //UnityEngine.Debug.Log("dElapsed time (ticks): " + elapsedTimeTicks);

        return shortestPath;
    }
}
