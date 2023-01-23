using System.Collections;
using UnityEngine;

namespace Project19
{
    public class DestroyObjectOverTime : MonoBehaviour
    {
        [SerializeField] private float _time;
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_time);
            Destroy(gameObject);
        }
    }
}