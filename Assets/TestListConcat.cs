using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestListConcat : MonoBehaviour
{

    private List<List<float>> Lists;
    private List<float> L1;
    private List<float> L2;
    private List<float> L3;
    private List<float> L4;
    private List<float> L5;
    private List<float> L6;
    private List<float> L7;
    private List<float> L8;


    // Start is called before the first frame update
    void Start()
    {
        L1 = new List<float>();
        L2 = new List<float>();
        L3 = new List<float>();
        L4 = new List<float>();
        L5 = new List<float>();
        L6 = new List<float>();
        L7 = new List<float>();
        L8 = new List<float>();


        Lists = new List<List<float>>
        {
            L1, L2, L3, L4, L5, L6, L7, L8
        };


        foreach(var l in Lists)
        {
            for (int i = 0; i < 5; i++)
                l.Add(Random.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var iterations = 1000;
        var breakCount = 5;

        float checkLinq = 0;
        float checkSelf = 0;
        float checkSelfi = 0;

        // LINQ
        UnityEngine.Profiling.Profiler.BeginSample("LINQ");
        for (int i = 0; i < iterations; i++)
        {
            var linqList = Enumerable.Empty<float>();
            foreach(var l in Lists)
            {
                linqList = linqList.Concat(l);
            }


            var biggest = 0f;
            var count = 1;
            foreach (var f in linqList)
            {
                if (count > breakCount)
                    break;
                biggest = Mathf.Max(biggest, f);
                count++;
            }
            checkLinq += biggest;
        }
        UnityEngine.Profiling.Profiler.EndSample();

        

        // SELF
        UnityEngine.Profiling.Profiler.BeginSample("SELF Array while");
        for (int i = 0; i < iterations; i++)
        {
            var lists2 = new List<float>[] { L1, L2, L3, L4, L5, L6, L7, L8 };

            var biggest = 0f;
            var count = 1;
            foreach (var l in lists2)
            {
                if (count > breakCount)
                    break;

                foreach (var f in l)
                {
                    if (count > breakCount)
                        break;
                    biggest = Mathf.Max(biggest, f);
                    count++;
                }
                checkSelf += biggest;
            }
        }
        UnityEngine.Profiling.Profiler.EndSample();




        Debug.Log($"{checkLinq} , {checkSelf}, { checkSelfi}");
    }
}
