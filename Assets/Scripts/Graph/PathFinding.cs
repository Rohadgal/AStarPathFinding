using System.Collections.Generic;
using UnityEngine;

public class PathFinding{
    public List<GameGraph.Node> AStarSearch(GameGraph.Node startNode, GameGraph.Node targetNode) {
        //// Create Stopwatch instance
        //Stopwatch stopwatch = new Stopwatch();
        //// Start measuring time
        //stopwatch.Start();

        // Nodes to be evaluated
        List<GameGraph.Node> openSet = new List<GameGraph.Node>();
        // Nodes already evaluated
        HashSet<GameGraph.Node> closedSet = new HashSet<GameGraph.Node>(); 
        // Add the start node to the open set
        openSet.Add(startNode); 

        while (openSet.Count > 0) { // Continue until there are no more nodes to evaluate
                                    // Find the node with the lowest combined cost (fCost) in the open set
            GameGraph.Node currentNode = openSet[0];
            // Set int i set to 1 to exclude the initial current node from being compared to itself
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            }
            // Remove the current node from the open set
            openSet.Remove(currentNode);
            // Add the current node to the closed set (already evaluated)
            closedSet.Add(currentNode); 
            // If the target node is found, reconstruct the path
            if (currentNode == targetNode) {
                //// Stop measuring time
                //stopwatch.Stop();
                //// Get elapsed time in milliseconds
                //long elapsedTimeMs = stopwatch.ElapsedMilliseconds;
                //UnityEngine.Debug.Log("Elapsed time (ms): " + elapsedTimeMs);
                //// Get elapsed time in ticks
                //long elapsedTimeTicks = stopwatch.ElapsedTicks;
                //UnityEngine.Debug.Log("Elapsed time (ticks): " + elapsedTimeTicks);

                // Return the path from start to target node
                return RetracePath(startNode, targetNode); 
            }
            // Evaluate neighbors of the current node
            foreach (GameGraph.Edge edge in currentNode.edges) {
                GameGraph.Node neighbor = edge.connectedNode;

                if (closedSet.Contains(neighbor)) {
                    // Skip this neighbor as it has already been evaluated
                    continue; 
                }
                // Calculate the cost to reach the neighbor node from the current node
                float newCostToNeighbor = currentNode.gCost + edge.distance;

                // Update the neighbor node's costs and parent if it's a better path
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
                    // Update the cost to reach the neighbor
                    neighbor.gCost = newCostToNeighbor;
                    // Heuristic cost (estimated distance to target)
                    neighbor.hCost = Vector3.Distance(neighbor.gameObject.transform.position, targetNode.gameObject.transform.position);
                    // Set the parent of the neighbor node to the current node
                    neighbor.parent = currentNode; 

                    if (!openSet.Contains(neighbor)) {
                        // Add the neighbor to the open set if it's not already there
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

        // Traverse the parent nodes from endNode to startNode to reconstruct the path
        while (currentNode != startNode) {
            // Add the current node to the path
            path.Add(currentNode);
            // Move to the parent node
            currentNode = currentNode.parent; 
        }
        // Add the start node to the path
        path.Add(currentNode);
        // Reverse the path to get it from startNode to endNode
        path.Reverse(); 
        return path;
    }



    // Dijkstra's algorithm search to find the shortest path from startNode to targetNode
    public List<GameGraph.Node> DijkstraSearch(GameGraph.Node startNode, GameGraph.Node targetNode, List<GameGraph.Node> nodes) {
            //// Create Stopwatch instance
            //Stopwatch dstopwatch = new Stopwatch();
            //// Start measuring time
            //dstopwatch.Start();

        // distances is a dictionary that saves the distance to each node
        Dictionary<GameGraph.Node, float> distances = new Dictionary<GameGraph.Node, float>();
        // previousNodes is a dictionary that saves the previous node to the current key node
        Dictionary<GameGraph.Node, GameGraph.Node> previousNodes = new Dictionary<GameGraph.Node, GameGraph.Node>();
        // univisitedNodes is a HashSet that contains all the unvisited nodes of the graph
        HashSet<GameGraph.Node> unvisitedNodes = new HashSet<GameGraph.Node>();

        // Go through all the nodes in the graph
        foreach (GameGraph.Node node in nodes) {
            // Set initially the distance to all the nodes from the dictionary to infinity
            distances[node] = Mathf.Infinity;
            // set the previous node of all the nodes from the dictionary to null
            previousNodes[node] = null;
            // add all the nodes from the GameGrpah to univistedNodes Hashset
            unvisitedNodes.Add(node);
        }
        // Set the node from the parameter startNode to be the starting node
        distances[startNode] = 0;
        // Loop until all the node have been visited and therefore removed from the unvisitedNodes HashSet
        while (unvisitedNodes.Count > 0) {
            // Set a node called currentNode to null every loop
            GameGraph.Node currentNode = null;
            // Go through all the nodes in the unvisitedNodes HashSet
            foreach (GameGraph.Node node in unvisitedNodes) {
                // If check to find the closest node from the unvisitedNodes HashSet
                if (currentNode == null || distances[node] < distances[currentNode]) {
                    // Set node with the closest distance to currentNode
                    currentNode = node;
                }
            }
            // Remove the current node from the HashSet unvisitedNodes
            unvisitedNodes.Remove(currentNode);
            // If currentNode is the same as the targetNode break from loop
            if (currentNode == targetNode) {
                break;
            }
            // Go through all the edges of the currentNode
            foreach (GameGraph.Edge edge in currentNode.edges) {
                // Get distance from the start Node to the next posible shortest path node
                float tentativeDistance = distances[currentNode] + edge.distance;
                // Check if the tentativeDistance is less than the distance to the next node connected to the edge which
                // will be bigger if the distance to the node in distances is still set to infinity
                if (tentativeDistance < distances[edge.connectedNode]) {
                    // Set the distance from the start node to the node connected by an edge to the current node
                    distances[edge.connectedNode] = tentativeDistance;
                    // Set the currentNode as the previousNode to the following node in the shortest path
                    previousNodes[edge.connectedNode] = currentNode;
                }
            }
        }
        // Construct the shortest path
        List<GameGraph.Node> shortestPath = new List<GameGraph.Node>();
        // Start the path from the targetNode
        GameGraph.Node current = targetNode;
        // Loop while the node is not empty
        while (current != null) {
            // Add the current node to the shortestPath
            shortestPath.Add(current);
            // Set the previousNode from the current node as the new current node
            current = previousNodes[current];
        }
        // Reverse the path to get it from startNode to targetNode
        shortestPath.Reverse(); 

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
