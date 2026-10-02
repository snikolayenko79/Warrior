using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SpellBehaviour
{
    public GameObject projectilePrefab;
    public GameObject explosionPrefab;
    public float speed;
    public float distance;
    public float delay = 0;
    
    public override void Execute(ISpellCaster caster, OnSpellReadyToDamage  onSpellReadyToDamage)
    {
        StartCoroutine(ExecuteCoroutine(caster, onSpellReadyToDamage));
    }

    private IEnumerator ExecuteCoroutine(ISpellCaster caster, OnSpellReadyToDamage  onSpellReadyToDamage)
    {
        yield return null;
        yield return  new WaitForSeconds(delay);
        
        float elapsedTime;
        Color clr;
            
        GameObject projectile = Instantiate(projectilePrefab, caster.SpellSpawnPoint.position, caster.SpellSpawnPoint.rotation);
        projectile.transform.SetParent(this.transform, true);// отвязываем от персонажа, чтобы не влияло перемещение персонажа
        Renderer meshRenderer = projectile.GetComponentInChildren<Renderer>();
        if (meshRenderer)
        {
            clr = meshRenderer.material.color;
            clr.a = 0;
            meshRenderer.material.color = clr;
            elapsedTime = 0f;
            while (elapsedTime < 0.2f)
            {
                elapsedTime += Time.deltaTime;
                clr.a = Mathf.Lerp(0, 0.5f, elapsedTime / 0.2f);
                meshRenderer.material.color = clr;
                yield return null;
            }
             clr.a = 0.5f;
             meshRenderer.material.color = clr;
        }
        
        yield return null;
        
         Vector3 moveTotal = Vector3.zero;
         while (moveTotal.magnitude < distance)
         {
             Vector3 move = projectile.transform.forward.normalized * (speed * Time.deltaTime);
             projectile.transform.position += move;
             moveTotal += move;
             yield return null;
         }
         
         GameObject explosion = Instantiate(explosionPrefab, projectile.transform.position, projectile.transform.rotation);
         explosion.transform.SetParent(this.transform, true);// отвязываем от персонажа, чтобы не влияло перемещение персонажа
         //explosion.transform.position += moveTotal;
        
         Destroy(projectile.gameObject);
         
         yield return null;
        
         elapsedTime = 0;
         float scaleFactor = 1;
         Vector3 scale = explosion.transform.localScale;
         meshRenderer = explosion.GetComponentInChildren<Renderer>();
         
          while (elapsedTime < 0.2f)
          {
              elapsedTime += Time.deltaTime;
              scaleFactor = Mathf.Lerp(1, 2, elapsedTime / 0.2f);
              explosion.transform.localScale = scale * scaleFactor;
         
              if (meshRenderer)
              {
                  clr = meshRenderer.material.color;
                  clr.a = Mathf.Lerp(0.5f, 0, elapsedTime / 0.2f);
                  meshRenderer.material.color = clr;
              }
              
              yield return null;
          }

          onSpellReadyToDamage(explosion.GetComponent<Collider>());
          
          Destroy(explosion.gameObject);
    }
}