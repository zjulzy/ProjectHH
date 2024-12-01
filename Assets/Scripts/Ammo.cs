using Sirenix.OdinInspector;
using UnityEngine;
using Unity;
using Sirenix.Serialization;
using Unity.Mathematics;

namespace ProjectHH
{
    public class Ammo : MonoBehaviour
    {
        [LabelText("爆炸特效")]
        [AssetList]
        public ParticleSystem ExplosionEffect;
        
        [LabelText("伤害半径")]
        public float radius = 5.0F;
        Ammo()
        {
            Debug.Log("Ammo constructor");
        }

        public void Initialize()
        {
            Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * 200);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Ammo OnCollisionEnter");
            if (ExplosionEffect != null)
            {
                ParticleSystem explosion = Instantiate(ExplosionEffect, transform.position, quaternion.identity);
                explosion.Play();
            }
            // 检测伤害半径内敌人
            var enemies = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
            foreach (var r in enemies)
            {
                var gameObject = r.gameObject;
                var meshRender = gameObject.GetComponent<MeshRenderer>();
                if (meshRender != null)
                {
                    meshRender.material.color = Color.red;
                }
            }
            Destroy(gameObject);
        }
    }
}