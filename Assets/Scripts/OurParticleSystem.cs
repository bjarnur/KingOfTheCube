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
public class OurParticleSystem : MonoBehaviour
{

    /*******************\
      Tunable variables 
    \*******************/

    public int numberOfParticles = 100;
    public int newParticlesPerFrame = 2;
    public Vector3 initialVelocity = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 minimalOffset = new Vector3(0.1f, 0.1f, 0.1f);

    /*******************\
      Private variables 
    \*******************/

    Random randomNumberGenerator;
    private OurParticle[] particles;
    private GameObject[] particleCubes;
    int lastUsedParticle = 0;

    /***************\
        Functions
    \***************/


    void Start ()
    {
        randomNumberGenerator = new Random();
        particles = new OurParticle[numberOfParticles];
        particleCubes = new GameObject[numberOfParticles];
	}
	
	
	void Update ()
    {
        //Spawn new particles when relevant
        for (int i = 0; i < newParticlesPerFrame; i++)
        {
            int unusedParticle = GetFirstUnusedParticle();
            RespawnParticle(unusedParticle);
        }

        //Update all excisting particles
        for(int i = 0; i < numberOfParticles; i++)
        {
            OurParticle particle = particles[i];
            if (particle == null) continue;

            particle.life -= Time.deltaTime;
            if(particle.life >= 0.0f)
            {
                particle.position -= particle.velocity * Time.deltaTime;
                particleCubes[i].transform.position = particle.position;
            }
        }

        //Render particles
        //Material material = new Material(Shader.Find("SmokeShader"));

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
            particleCubes[particleIdx] = particleCube;
        }            

        float spread = Random.Range(-5f, 5f);
        float ascend = Random.Range(-10f, -5f);
        Vector3 randomVector = new Vector3(spread, -5f, spread);
        particle.position = transform.position;
        particle.velocity = randomVector;
        particle.life = 2f;
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