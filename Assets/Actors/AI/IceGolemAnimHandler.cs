using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
public class IceGolemAnimHandler : MonoBehaviour
{
    public int layer = 6;
    public ClipTransition idle;
    public ClipTransition hurt;
    public ClipTransition stun;
    public ClipTransition tink;
    AnimancerComponent animancer;
    ActorTimeTravelHandler timeTravelHandler;
    bool frozen;
    Actor actor;

    [SerializeField] World world;
    enum World
    {
        Default,
        World1,
        World2
    }
    // Start is called before the first frame update

    private void Awake()
    {
        SetRenderersToWorld();
    }
    void Start()
    {
        animancer = this.GetComponent<AnimancerComponent>();
        actor = this.GetComponent<Actor>();
        timeTravelHandler = this.GetComponent<ActorTimeTravelHandler>();
        animancer.Layers[layer].IsAdditive = true;
        animancer.Layers[layer].Weight = 1f;
        PlayIdle();

        actor.OnHurt.AddListener(PlayHurt);
        actor.OnCritVulnerable.AddListener(PlayStun);
        actor.OnBlock.AddListener(PlayTink);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTravelHandler.IsFrozen() && animancer.Layers[layer].Speed > 0f)
        {
            animancer.Layers[layer].Speed = 0f;
        }
        else if (!timeTravelHandler.IsFrozen() && animancer.Layers[layer].Speed == 0f)
        {
            animancer.Layers[layer].Speed = 1f;
        }
    }

    public void PlayIdle()
    {
        if (idle == null || idle.Clip == null) return;
        animancer.Layers[layer].Play(idle);
    }

    public void PlayHurt()
    {
        if (hurt == null || hurt.Clip == null) return;
        AnimancerState state = animancer.Layers[layer].Play(hurt);
        state.Events.OnEnd = PlayIdle;
    }

    public void PlayStun()
    {
        if (stun == null || stun.Clip == null) return;
        AnimancerState state = animancer.Layers[layer].Play(stun);
        state.Events.OnEnd = PlayIdle;
    }

    public void PlayTink()
    {
        if (tink == null || tink.Clip == null) return;
        AnimancerState state = animancer.Layers[layer].Play(tink);
        state.Events.OnEnd = PlayIdle;
        //this.GetComponent<AudioSource>().Play()
    }

    public void SetRenderersToWorld()
    {
        string terrainL = "Terrain";
        string defaultL = "Default";
        string actorL = "Actors";
        if (world == World.World1)
        {
            terrainL = "Terrain_World1Only";
            actorL = "Actors_World1Only";
            defaultL = "World1Only";
        }
        else if (world == World.World2)
        {
            terrainL = "Terrain_World2Only";
            actorL = "Actors_World2Only";
            defaultL = "World2Only";
        }
        foreach (Renderer r in this.GetComponentsInChildren<Renderer>()) 
        {
            if (LayerMask.LayerToName(r.gameObject.layer).ToLower().Contains("terrain"))
            {
                r.gameObject.layer = LayerMask.NameToLayer(terrainL);
            }
            else if (LayerMask.LayerToName(r.gameObject.layer).ToLower().Contains("actor"))
            {
                r.gameObject.layer = LayerMask.NameToLayer(actorL);
            }
            else
            {
                r.gameObject.layer = LayerMask.NameToLayer(defaultL);
            }
        }
    }
}
