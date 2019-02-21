
namespace RosSharp.RosBridgeClient
{
	static class PointCloud2Extensions
	{
		public static Type[] ToArray<Type>(this Messages.Sensor.PointCloud2 data, string name)
		{
			System.Type[] TypeNo = { typeof(object),
					typeof(sbyte), typeof(byte),
					typeof(short), typeof(ushort),
					typeof(long), typeof(ulong),
					typeof(float), typeof(double) };

			int offset = -1;
			foreach (Messages.Sensor.PointField f in data.fields)
			{
				if (f.name == name)
				{
					offset = f.offset;
					if (typeof(Type) != TypeNo[f.datatype]) throw new System.ArgumentException("Type missmatch in " + name + " payload data type is : " + TypeNo[f.datatype].ToString());
					break;
				}
			}
			if (offset < 0) throw new System.ArgumentException("No such field " + name + "in PointCloud2");
			Type[] ret = new Type[data.width * data.height];
			int rpos = offset;
			int count = 0;
			//UnityEngine.Debug.Log(name + "  offset:" + offset.ToString() +" ps:"+ data.point_step.ToString()+" rs:"+data.row_step.ToString()+" h:"+data.height.ToString() + " w:" + data.width.ToString());
			for (int y = 0; y < data.height; y++)
			{
				int pos = rpos;
				for (int x = 0; x < data.width; x++)
				{
					//This else-if storm will be optimized out. There is  no speed problem.(may be) 
					if (typeof(Type) == typeof(sbyte)) ret[count] = (Type)(object)(sbyte)data.data[pos];
					else if (typeof(Type) == typeof(byte)) ret[count] = (Type)(object)data.data[pos];
					else if (typeof(Type) == typeof(short)) ret[count] = (Type)(object)System.BitConverter.ToInt16(data.data, pos);
					else if (typeof(Type) == typeof(ushort)) ret[count] = (Type)(object)System.BitConverter.ToUInt16(data.data, pos);
					else if (typeof(Type) == typeof(long)) ret[count] = (Type)(object)System.BitConverter.ToInt32(data.data, pos);
					else if (typeof(Type) == typeof(ulong)) ret[count] = (Type)(object)System.BitConverter.ToUInt32(data.data, pos);
					else if (typeof(Type) == typeof(float)) ret[count] = (Type)(object)System.BitConverter.ToSingle(data.data, pos);
					else if (typeof(Type) == typeof(double)) ret[count] = (Type)(object)System.BitConverter.ToDouble(data.data, pos);
					count++;
					pos += data.point_step;
				}
				rpos += data.row_step;
			}
			return ret;
		}
}
}