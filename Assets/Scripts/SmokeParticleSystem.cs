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

    public int numberOfParticles = 150;
    public float sidewaysSpreadFactor = 4;
    public float forwardSpreadFactor = 2;
    public float verticalSpreadFactor = 2;
    public float verticalSpreadVariance = 3;
    public float rejectionRate = 7;
    public float scaleWithTime = 1.005f;
    public float decelerateWithTime = 0.99f;
    public float scale =  0.1f;
    public float systemLife = 15;
    public float particleLife = 5;


    /*******************\
      Private variables 
    \*******************/

    private Random randomNumberGenerator;
    private OurParticle[] particles;
    private GameObject[] particleCubes;    
    private Material smokeMaterial;   
    private Vector3 localFwd;
    private Vector3 localRight;
    private int lastUsedParticle = 0;

    //Use rejection rate for tuning rather than this variable
    private int newParticlesPerFrame = 1;

    /***************\
        Functions
    \***************/

    void Start ()
    {
        GameObject kingObj = GameObject.FindWithTag(GameConstants.GameObjectsTags.king);
        localFwd = kingObj.transform.localRotation * Vector3.forward;
        localRight = kingObj.transform.localRotation * Vector3.right;

        randomNumberGenerator = new Random();
        particles = new OurParticle[numberOfParticles];
        particleCubes = new GameObject[numberOfParticles];
        smokeMaterial = Resources.Load<Material>(GameConstants.Materials.smoke);
        smokeMaterial.SetVector(GameConstants.ShaderProperties.smokeOrigin, transform.position);
    }
	
	
	void Update ()
    {
        systemLife -= Time.deltaTime;

        //Kill the system
        if (systemLife <= 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this);
            return;
        }

        //Spawn new particles when relevant
        else if (systemLife > particleLife)
        {            
            for (int i = 0; i < newParticlesPerFrame; i++)
            {
                float reject = Random.Range(0, 10);
                if (reject < 5) break;

                int unusedParticle = GetFirstUnusedParticle();
                RespawnParticle(unusedParticle);
            }
        }

        //Update all excisting particles
        for(int i = 0; i < numberOfParticles; i++)
        {
            OurParticle particle = particles[i];
            if (particle == null) continue;

            particle.life -= Time.deltaTime;
            if(particle.life >= 0.0f)
            {
                //Update postition
                particle.position -= particle.velocity * Time.deltaTime * scale * scale;
                particleCubes[i].transform.position = particle.position;

                //Particle speed and size vary over time
                Vector3 v = particle.velocity;
                particle.velocity = new Vector3(v.x * decelerateWithTime, v.y, v.z * decelerateWithTime);
                particleCubes[i].transform.localScale *= scaleWithTime;
            }
            else
            {
                Destroy(particleCubes[i]);
                particleCubes[i] = null;
                particles[i] = null;
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

        float sidewaySpread = Random.Range(-sidewaysSpreadFactor, sidewaysSpreadFactor);
        float fwdSpread = Random.Range(-forwardSpreadFactor, 0.0f);
        float ascend = Random.Range(-verticalSpreadFactor - verticalSpreadVariance, -verticalSpreadFactor);

        particle.velocity = localFwd * fwdSpread + Vector3.up * ascend + localRight * sidewaySpread;
        particle.position = transform.position;
        particle.life = particleLife;

        particleCubes[particleIdx].transform.localScale = new Vector3(scale, scale, scale);
    }
}


class OurParticle
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector4 color;
    public float life;
}