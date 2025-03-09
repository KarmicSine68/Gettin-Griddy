/******************************************************************************
 * Author: Skylar Turner
 * File Name: Pathfinder.cs
 * Creation Date: 3/5/2025
 * Brief: Handles the calculations necessary for pathfinding.
 * ***************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    /// <summary>
    /// Handles finding the path for the enemy.
    /// </summary>
    /// <param name="start">The tile the enemy is on</param>
    /// <param name="target">The tile the player is on</param>
    /// <returns></returns>
    public static List<TileBehaviour> FindPath(TileBehaviour start, TileBehaviour target)
    {
        List<TileBehaviour> openSet = new List<TileBehaviour> { start };
        HashSet<TileBehaviour> closedSet = new HashSet<TileBehaviour>();

        Dictionary<TileBehaviour, TileBehaviour> cameFrom = new Dictionary<TileBehaviour, TileBehaviour>();
        Dictionary<TileBehaviour, float> gScore = new Dictionary<TileBehaviour, float>();
        Dictionary<TileBehaviour, float> fScore = new Dictionary<TileBehaviour, float>();

        gScore[start] = 0;
        fScore[start] = GetHeuristic(start, target);

        while (openSet.Count > 0)
        {
            TileBehaviour current = GetLowestFScore(openSet, fScore);
            if (current == target)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach(TileBehaviour neighbor in current.GetNeighbors())
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentativeGScore = gScore[current] + 1;
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetHeuristic(neighbor, target);
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the Heuristic value for a tile
    /// </summary>
    /// <param name="a">The first tile</param>
    /// <param name="b">The second tile</param>
    /// <returns>The tile with the best score</returns>
    private static float GetHeuristic(TileBehaviour a, TileBehaviour b)
    {
        return Mathf.Abs(a.gridLocation.x - b.gridLocation.x) + Mathf.Abs(a.gridLocation.y - b.gridLocation.y);
    }

    private static TileBehaviour GetLowestFScore(List<TileBehaviour> openSet, Dictionary<TileBehaviour, float> fScore)
    {
        TileBehaviour best = openSet[0];
        foreach(var tile in openSet)
        {
            if(fScore[tile] < fScore[best])
            {
                best = tile;
            }
        }
        return best;
    }

    /// <summary>
    /// Reconstructs the path that the enemy will take and reverses it so the enemy can follow it
    /// </summary>
    /// <param name="cameFrom"></param>
    /// <param name="current"></param>
    /// <returns>The path the enemy is to follow</returns>
    private static List<TileBehaviour> ReconstructPath(Dictionary<TileBehaviour, TileBehaviour> cameFrom, TileBehaviour current)
    {
        List<TileBehaviour> path = new List<TileBehaviour> { current };
        while(cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}
