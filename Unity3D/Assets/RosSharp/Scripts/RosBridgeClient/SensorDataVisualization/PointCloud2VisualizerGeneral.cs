/*
Author:Akira NODA

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using RosSharp.RosBridgeClient.Messages.Sensor;
using RosSharp.RosBridgeClient; //To use PointCloud2.ToArray()
using System.Linq;
using System.Collections.Generic;
using RosSharp;
public class PointCloud2VisualizerGeneral : PointCloud2Visualizer
{
	public GameObject MarkerPrefab;
	private List<GameObject> markers;
	public string Channel = "a";
	public new void Start()
	{
		markers = new List<GameObject>();
		base.Start();
	}
	protected override void Visualize()
	{
		float[] x, y, z, c;
		try
		{
			//Be careful: some Point cloud use diffrect Coordinate system
			x = pc2.ToArray<float>("x");
			y = pc2.ToArray<float>("y");
			z = pc2.ToArray<float>("z");
			c = pc2.ToArray<float>(Channel);
		}
		catch (System.Exception e)
		{
			UnityEngine.Debug.LogError(e.ToString());
			return;
		}
		


		for (int i = markers.Count; i < x.Length; i++)
		{
			markers.Add(Instantiate(MarkerPrefab));
			markers[i].transform.parent = transform;
			markers[i].GetComponent<Renderer>().material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		}
		for (int i = x.Length; i < markers.Count; i++) markers[i].SetActive(false);
		float cmax = c.Max();
		float cmin = c.Min();
		for (int i = 0; i < x.Length; i++)
		{
			
			markers[i].SetActive(true);
			//Be careful: some Point cloud use diffrect Coordinate system
			markers[i].transform.localPosition = new Vector3(x[i], y[i], z[i]).Ros2Unity();
			markers[i].GetComponent<Renderer>().material.SetColor("_TintColor", GetColor(c[i],cmin,cmax));
		}



	}
	protected override void DestroyObjects()
	{

	}
	
}
 