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
public abstract class PointCloud2Visualizer : MonoBehaviour
{
	protected PointCloud2 pc2;
	private bool isMessageReceived;
	public void SetData(PointCloud2 p)
	{
		pc2 = p;
		isMessageReceived = true;
	}

	public void Start()
	{
		isMessageReceived = false;
	}

	protected abstract void Visualize();
	protected abstract void DestroyObjects();
	protected void Update()
    {
        if (!isMessageReceived)return;
        Visualize();
		isMessageReceived = true;
    }

    protected void OnDisable()
    {
        DestroyObjects();
    }
    protected Color GetColor(float distance,float rmin,float rmax)
    {
        float h_min = (float)0;
        float h_max = (float)0.5;

        float h = (float)(h_min + (distance - rmin) / (rmax - rmin) * (h_max - h_min));
        float s = (float)1.0;
        float v = (float)1.0;

        return Color.HSVToRGB(h, s, v);
    }

}
 