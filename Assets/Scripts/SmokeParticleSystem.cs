using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class to produce custom paricle effects. Can in the 
 * future be extended to include tunable physical simulation
 * for the particles. 
 * 
 * The class is based on the particle system tutorial from 
 * learnopengl.com: https://learnopengl.com/In-Practice/2D-Game/Particles
 * 
 */
public class SmokeParticleSystem : MonoBehaviour
{

    /*******************\
      Tunable variables 
    \*******************/

    public int numberOfParticles = 200;
    public int newParticlesPerFrame = 1;
    public float scale = 0.1f;

    [HideInInspector]
    public int axis;

    /*******************\
      Private variables 
    \*******************/

    Random randomNumberGenerator;
    private OurParticle[] particles;
    private GameObject[] particleCubes;
    int lastUsedParticle = 0;
    Material smokeMaterial;

    /***************\
        Functions
    \***************/


    void Start ()
    {
        randomNumberGenerator = new Random();
        particles = new OurParticle[numberOfParticles];
        particleCubes = new GameObject[numberOfParticles];
        smokeMaterial = Resources.Load<Material>("Materials/Mat_Smoke");
    }
	
	
	void Update ()
    {
        //Spawn new particles when relevant
        for (int i = 0; i < newParticlesPerFrame; i++)
        {
            float reject = Random.Range(0, 10);
            if (reject < 7) break;

            int unusedParticle = GetFirstUnusedParticle();
            RespawnParticle(unusedParticle);
        }

        //Update all excisting particles
        for(int i = 0; i < numberOfParticles; i++)
        {
            OurParticle particle = particles[i];
            if (particle == null) continue;

            particle.life -= Time.deltaTime;
            //if(particle.life >= 0.0f)
            if(true)
            {
                particle.position -= particle.velocity * Time.deltaTime * scale * scale;
                particleCubes[i].transform.position = particle.position;
                particleCubes[i].transform.localScale *= 1.005f;  
            }
        }
	}

    private int GetFirstUnusedParticle()
    {
        for(int i = lastUsedParticle; i < numberOfParticles; i++)
        {
            if(particles[i] == null || particles[i].life <= 0.0f)
            {
                lastUsedParticle = i;
                return i;
            }
        }

        for(int i = 0; i < lastUsedParticle; i++)
        {
            if (particles[i] == null || particles[i].life <= 0.0f)
            {
                lastUsedParticle = i;
                return i;
            }
        }

        lastUsedParticle = 0;
        return 0;
    }

    private void RespawnParticle(int particleIdx)
    {
        OurParticle particle = particles[particleIdx];
        GameObject particleCube = particleCubes[particleIdx];
        if (particle == null)
        {
            particle = new OurParticle();
            particles[particleIdx] = particle;

            particleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            particleCube.transform.SetParent(transform, false);
            particleCube.transform.localScale *= scale;

            particleCube.GetComponent<Collider>().enabled = false;
            particleCube.GetComponent<Renderer>().material = smokeMaterial;
            particleCubes[particleIdx] = particleCube;
        }            

        float spread = Random.Range(-3f, 3f);
        float ascend = Random.Range(-10f, -5f);
        Vector3 randomVector;

        if (axis == 0) //Spread along X
            randomVector = new Vector3(spread, ascend, 0);
        else //Spread along z
            randomVector = new Vector3(0, ascend, spread);

        particle.position = transform.position;
        particle.velocity = randomVector;
        particle.life = 3f;

        particleCubes[particleIdx].transform.localScale = new Vector3(scale, scale, scale);
    }
}


class OurParticle
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector4 color;
    public float life;
    /*
    OurParticle(Vector3 x, Vector3 v, Vector4 c, float l)
    {
        this.position = x;
        this.velocity = v;
        this.color = c;
        this.life = l;
    }*/
}