using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState { STATIC, CHASE, REST, MOVING, DEFAULT };


public enum EnemyBehavior { EnemyBehavior1, EnemyBehavior2, EnemyBehavior3 };


public class Enemy : MonoBehaviour
{
   protected PathFinder pathFinder;
   public Tile mapGenerator;
   protected Queue<Tile> path;
   protected GameObject playerGameObject;


   public Tile currentTile;
   protected Tile targetTile;
   public Vector3 velocity;


   public float speed = 1.0f;
   public float visionDistance = 5;
   public int maxCounter = 5;
   protected int playerCloseCounter;


   protected EnemyState state = EnemyState.DEFAULT;
   protected Material material;


   public EnemyBehavior behavior = EnemyBehavior.EnemyBehavior1;


   void Start()
   {
       path = new Queue<Tile>();
       pathFinder = new PathFinder();
       playerGameObject = GameObject.FindWithTag("Player");
       playerCloseCounter = maxCounter;
       material = GetComponent<MeshRenderer>().material;
   }


   void Update()
   {
       if (mapGenerator.state == MapState.DESTROYED) return;


       if (playerGameObject.GetComponent<Player>().IsGoalReached() || playerGameObject.GetComponent<Player>().IsPlayerDead())
       {
           return;
       }


       switch(behavior)
       {
           case EnemyBehavior.EnemyBehavior1:
               HandleEnemyBehavior1();
               break;
           case EnemyBehavior.EnemyBehavior2:
               HandleEnemyBehavior2();
               break;
           case EnemyBehavior.EnemyBehavior3:
               HandleEnemyBehavior3();
               break;
           default:
               break;
       }
   }


   public void Reset()
   {
       Debug.Log("enemy reset");
       path.Clear();
       state = EnemyState.DEFAULT;
       currentTile = FindWalkableTile();
       transform.position = currentTile.transform.position;
   }


   Tile FindWalkableTile()
   {
       Tile newTarget = null;
       int randomIndex = 0;
       while (newTarget == null || !newTarget.mapTile.Walkable)
       {
           randomIndex = (int)(Random.value * mapGenerator.width * mapGenerator.height - 1);
           newTarget = GameObject.Find("MapGenerator").transform.GetChild(randomIndex).GetComponent<Tile>();
       }
       return newTarget;
   }


   private void HandleEnemyBehavior1()
   {
       switch (state)
       {
           case EnemyState.DEFAULT:
               material.color = Color.white;
              
               if (path.Count <= 0) path = pathFinder.RandomPath(currentTile, 20);
              
               if (path.Count > 0)
               {
                   targetTile = path.Dequeue();
                   state = EnemyState.MOVING;
               }
               break;


           case EnemyState.MOVING:
               velocity = targetTile.gameObject.transform.position - transform.position;
               transform.position += velocity.normalized * speed * Time.deltaTime;


               if (Vector3.Distance(transform.position, targetTile.gameObject.transform.position) <= 0.05f)
               {
                   currentTile = targetTile;
                   state = EnemyState.DEFAULT;
               }


               break;


           default:
               state = EnemyState.DEFAULT;
               break;
       }
   }


   private void HandleEnemyBehavior2()
   {
      
   }


   private void HandleEnemyBehavior3()
   {
      
   }


   private Tile GetLastPlayerTile()
   {
       Player player = playerGameObject.GetComponent<Player>();
       return player.CurrentTile; 
   }


   private void MoveAlongPath(List<Tile> path)
   {
       if (path.Count > 0)
       {
           Tile nextTile = path[0];
           Vector3 targetPosition = nextTile.transform.position;
           velocity = targetPosition - transform.position;
           transform.position += velocity.normalized * speed * Time.deltaTime;


         
           if (Vector3.Distance(transform.position, targetPosition) <= 0.05f)
           {
               currentTile = nextTile;
               path.RemoveAt(0);
           }
       }
   }


   private Tile GetClosestTile(Vector3 targetPosition)
   {
       Tile closestTile = null;
       float closestDistance = float.MaxValue;


       foreach (Transform tileTransform in mapGenerator.transform)
       {
           Tile tile = tileTransform.GetComponent<Tile>();
           if (tile.mapTile.Walkable)
           {
               float distance = Vector3.Distance(targetPosition, tile.transform.position);
               if (distance < closestDistance)
               {
                   closestDistance = distance;
                   closestTile = tile;
               }
           }
       }


       return closestTile;
   }
}
