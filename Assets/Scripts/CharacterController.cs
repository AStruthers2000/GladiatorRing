using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int health;
    public int defence;
    public int damage;
    public string behaviour;
    public float moveSpeed;
    public int energy;
    public string team;
    public string name;

    private AnimationState idle;
    private AnimationState walk;
    private AnimationState attack;

    private List<GameObject> viableChars = new List<GameObject>();
    private GameObject target;
    private Animation anim;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        foreach(AnimationState state in anim)
        {
            Debug.Log(state.name);
            if (state.name.ToLower().Contains("idle"))
            {
                idle = state;
            }
            if (state.name.ToLower().Contains("attack"))
            {
                attack = state;
            }
            if (state.name.ToLower().Contains("walk"))
            {
                walk = state;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckBehaviour();
        if (target)
        {
            MoveTowards();
        }
        else
        {
            anim.Play(idle.name);
        }
    }

    void MoveTowards()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 2)
        {
            anim.CrossFade(walk.name);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
            transform.LookAt(target.transform);
        }
        else
        {
            Attack();
        }
    }

    void Attack()
    {
        anim.CrossFade(attack.name);
        Debug.Log("Attacking " + target.GetComponent<CharacterController>().name);
    }

    void CheckBehaviour()
    {
        switch (this.behaviour)
        {
            case "aggressive":
                GameObject possibleTarget = null;
                float closest = 100.0f;
                foreach (GameObject possibleEnemy in viableChars)
                {
                    if (possibleEnemy.gameObject.GetComponent<CharacterController>().team != this.team)
                    {
                        float distance = Vector3.Distance(transform.position, possibleEnemy.gameObject.transform.position);
                        if (distance < closest)
                        {
                            possibleTarget = possibleEnemy;
                            closest = distance;
                        }
                    }
                }
                if(possibleTarget != this.target)
                {
                    this.target = possibleTarget;
                }
                break;
            default:
                Debug.Log("No behaviour defined so idk what you want me to do");
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if(other.collider.GetType() == typeof(SphereCollider))
        //{
        Debug.Log(other.gameObject.tag);
            if(other.gameObject.tag == "Character")
            {
            Debug.Log("Adding other gameobject");
                viableChars.Add(other.gameObject);
            }
        //}
    }

    void OnTriggerExit(Collider other)
    {
        //if(other.collider.GetType() == typeof(SphereCollider))
        //{
            if(other.gameObject.tag == "Character")
            {
                viableChars.Remove(other.gameObject);
            }
        //}
    }
}
