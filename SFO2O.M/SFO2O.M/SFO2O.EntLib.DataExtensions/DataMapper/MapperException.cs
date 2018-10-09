using System;
using System.Runtime.Serialization;

namespace SFO2O.EntLib.DataExtensions.DataMapper
{
	/// <summary>
	/// 数据对象映射异常
	/// </summary>
	[Serializable]
	public class MapperException : Exception
	{
		public MapperException()
		{
		}
		public MapperException(string message)
			: base(message)
		{
		}
		protected MapperException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
		public MapperException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
