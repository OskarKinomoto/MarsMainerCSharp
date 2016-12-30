using System;

namespace MarsMiner
{
	public class Layer
	{
		private float layerPosition;
		private Layer(float l)
		{
			layerPosition = l;
		}

		public static implicit operator float(Layer l)
		{
			return l.layerPosition;
		}

		public static Layer operator++(Layer layer)
		{
			layer.layerPosition += 0.01f;
			return layer;
		}

		public static Layer operator+(Layer layer, float s)
		{
			return new Layer(layer.layerPosition + s);
		}

		public static Layer Background = new Layer(-10f);


		public static Layer Robot = new Layer(5f);


		public static Layer Window = new Layer(8f);
		public static Layer StatusText = new Layer(10f);
	}
}

