using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBiomeMonster : Enemy
{
    GameObject player;

    private void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().ReportSpawn();
    }

    public override void Move()
    {
        if (health.IsDead())
        {
            turnOrder.EndTurn();
            return;
        }
        Node intruder = FindIntruder();
        if (intruder != null)
        {
            map = mapManager.GetMap();
            currentPosition = transform.position;
            float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(intruder.xCoor);
            float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(intruder.yCoor);
            if (xCoordinate > currentPosition.x) attacker.Attack(new Vector2(1, 0));
            else if (xCoordinate < currentPosition.x) attacker.Attack(new Vector2(-1, 0));
            else if (yCoordinate > currentPosition.y) attacker.Attack(new Vector2(0, 1));
            else if (yCoordinate < currentPosition.y) attacker.Attack(new Vector2(0, -1));
            return;
        }

        Hunting();
    }

    protected override Node FindIntruder()
    {
        Debug.Log("FindIntruder");
        map = mapManager.GetMap();
        currentPosition = transform.position;
        List<Node> neighbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);

        while (neighbourNodeList.Count > 0)
        {
            int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
            if (neighbourNodeList[randomNumber].Type != 4)
            {
                neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
            }
            else
            {
                GameObject entity = neighbourNodeList[randomNumber].entity;
                if (entity.GetComponent<Player>() != null)
                {
                    return neighbourNodeList[randomNumber];
                }
                else
                {
                    neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                }
            }
        }

        return null;
    }

    protected override void Hunting()
    {
        map = mapManager.GetMap();
        currentPosition = transform.position;



            Node start = map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)];
            Node target = map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)player.transform.position.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)player.transform.position.y)];

            List<Node> path = pathFind.FindPath(start, target);

            Debug.Log("AfterPathFind");
            if (path == null || path.Count == 0)
            {
                pathFind.FindPath(start, target);
                turnOrder.EndTurn();
                return;
            }

            float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(path[0].xCoor);
            float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(path[0].yCoor);

            if (path[0].Type != 1)
            {
                turnOrder.EndTurn();
            }
            else
            {
                Debug.Log("OnMove!");
                if (xCoordinate > currentPosition.x) movement.Move("right");
                else if (xCoordinate < currentPosition.x) movement.Move("left");
                else if (yCoordinate > currentPosition.y) movement.Move("up");
                else if (yCoordinate < currentPosition.y) movement.Move("down");
            }
        
    }

    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().ReportDying();
    }
}
