/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

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

public class LaserScanVisualizerLines : LaserScanVisualizer
{
    [Range(0.001f, 0.01f)]
    public float objectWidth;
	public Material material;

	private GameObject[] LaserScan;
    private bool IsCreated = false;

    private void Create(int numOfLines)
    {

        LaserScan = new GameObject[numOfLines];
        for (int i = 0; i < numOfLines; i++)
        {
            LaserScan[i] = new GameObject("LaserScanLines");
            LaserScan[i].transform.parent = gameObject.transform;
            LaserScan[i].transform.localPosition = Vector3.zero;
            LaserScan[i].transform.localRotation = Quaternion.identity;
            LaserScan[i].AddComponent<LineRenderer>();
            LaserScan[i].GetComponent<LineRenderer>().material = material;
            LaserScan[i].GetComponent<LineRenderer>().useWorldSpace = false;
        }
        IsCreated = true;
    }

    protected override void Visualize()
    {
        if (!IsCreated)
            Create(directions.Length);

        for (int i = 0; i < directions.Length; i++)
        {
            LineRenderer lr = LaserScan[i].GetComponent<LineRenderer>();
            lr.startColor = GetColor(ranges[i]);
            lr.endColor = GetColor(ranges[i]);
            lr.startWidth = objectWidth;
            lr.endWidth = objectWidth;
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, ranges[i] * directions[i]);
        }
    }

    protected override void DestroyObjects()
    {
        if(! ( LaserScan is null))
            for (int i = 0; i < LaserScan.Length; i++)
                Destroy(LaserScan[i]);
        IsCreated = false;
    }
}