using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
	[SerializeField]
	FunctionLibrary.FunctionName function = FunctionLibrary.FunctionName.Wave;
	[SerializeField, Range(10, 100)]
	int resolution = 10;

	FunctionLibrary.FunctionName transitionFunction;
	bool transitioning=false;
	Transform[] points;
	float functionDuration = 1f;
	float duration = 0;

	void Awake()
	{
		float step = 2f / resolution;
		var scale = Vector3.one * step;
		
		points = new Transform[resolution * resolution];
		for (int i = 0; i < points.Length; i++)
		{
	
			Transform point = Instantiate(pointPrefab);
	
			point.localScale = scale;
			point.SetParent(transform, false);
			points[i] = point;
		}
	}

	void Update()
	{
		if ((int)function != (int)transitionFunction) {
			transitioning = true;
			duration += Time.deltaTime;
			if (duration >= functionDuration)
			{
				duration -= functionDuration;
				transitioning = false;
				transitionFunction = function;
			}
		}
        if (transitioning)
        {
			UpdateFunctionTransition();
		}
        else
        {
			UpdateFunction();
		}
       
		
	}

	void UpdateFunction()
    {
		FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
		float time = Time.time;
		float step = 2f / resolution;
		float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
		{
			if (x == resolution)
			{
				x = 0;
				z += 1;
				v = (z + 0.5f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = f(u, v, time);
		}
	}

	void UpdateFunctionTransition()
	{
		FunctionLibrary.Function
			from = FunctionLibrary.GetFunction(transitionFunction),
			to = FunctionLibrary.GetFunction(function);
		float progress = duration / functionDuration;
		float time = Time.time;
		float step = 2f / resolution;
		float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
		{

			if (x == resolution)
			{
				x = 0;
				z += 1;
				v = (z + 0.5f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = FunctionLibrary.Morph(
				u, v, time, from, to, progress
			);
		}
	}


}
