/*
Author: Akira Noda
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

namespace RosSharp.RosBridgeClient
{
	using Messages.Sensor;
    public class PointCloud2Subscriber : Subscriber<Messages.Sensor.PointCloud2>
    {
		public PointCloud2Visualizer visualizer;
		private PointCloud2 pc2;
		protected override void Start()
        {
			base.Start();
		}
		protected override void ReceiveMessage(PointCloud2 data)
        {
			visualizer.SetData(data);  
        }
    }
}