using System.Collections;
using UnityEngine;

namespace Assets.git
{
    public class Explosion : MonoBehaviour
    {
        public float damage = 35;
        public float duration = 1;
        float deletionTime = 0;

        private IPlayer author;

        public void setAuthor(IPlayer author)
        {
            this.author = author;
        }

        void Start()
        {
            deletionTime = Time.time + duration;
            StartCoroutine(shrinkAndDestroy());
        }

        void OnTriggerEnter(Collider other)
        {
            // Debug.Log(other + " entered " + this);

            var destructible = other.GetComponent<IDestructible>();

            if (destructible != null)
            {
                destructible.damage(damage, author);
            }
        }

        IEnumerator shrinkAndDestroy()
        {
            while (Time.time < deletionTime)
            {
                transform.localScale = transform.localScale * 0.9f;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
