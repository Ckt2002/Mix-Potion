using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public List<int> testList;

        private void Start()
        {
            StartCoroutine(TestListIterating(testList));
        }

        public static IEnumerator TestListIterating(List<int> testList)
        {
            int currentNum = 5;
            while (testList.Count > 0)
            {
                Debug.Log(testList[0]);

                testList.RemoveAt(0);
                ++currentNum;
                testList.Add(currentNum);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}