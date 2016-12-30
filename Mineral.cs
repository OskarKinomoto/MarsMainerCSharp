using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarsMiner
{
	class Mineral
	{
		static Random rnd = new Random();

		private enum Type
		{
			Gold = 0,
			Lapis = 1,
			Silver = 2,
			Platinium = 3,
			Rubin = 4,
			Copper = 5,
			Saphire = 6,
			Salt = 7,
			Emerald = 8,
		}

		private readonly int[] Prices = new int[]{ 100, 50, 20, 250, 65, 5, 125, 3, 35 };
			
		private readonly Sprites.Name sprite;
		private readonly int price;

		private Type type = Type.Gold;

		private Mineral(Type type, Sprites.Name sprite)
		{
			this.type = type;
			this.sprite = sprite;
		}

		public Sprites.Name GetSprite()
		{
			return sprite;
		}

		private static float gauss(float x, float avg, float sigma)
		{
			return (float)(Math.Exp((-Math.Pow(x - avg, 2)) / (2 * sigma * sigma)) / (sigma * Math.Sqrt(2 * Math.PI)));
		}

		private static float limitedGauss(float x, float avg, float sigma, float minDepth, float maxDepth, float amplitude)
		{
			if (-x < minDepth)
				return 0;
			if (-x > maxDepth && maxDepth != -1)
				return 0;
			return gauss(-x, avg, sigma) * amplitude;
		}

		private static bool savedToFile = false;

		private static void SaveToFile()
		{
			if (savedToFile)
				return;
			savedToFile = true;

			System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";

			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

			using (StreamWriter writetext = new StreamWriter("propabilities.txt")) {
				for (int depth = 0; depth < 400; ++depth) {
					var prob = CalcPropabilitesOnDepth(-depth);

					string del = "";
					for (int i = 0; i < prob.Count; ++i) {
						writetext.Write(del + prob[i].ToString());
						del = "\t";
					}
					writetext.Write("\n");
				}
			}
		}

		private static List<float> CalcPropabilitesOnDepth(int TileDepth)
		{
			SaveToFile();

			List<float> ret = new List<float>();


			if (Preferences.MineralsDeveloper) {
				int max = (int)Enum.GetValues(typeof(Type)).Cast<Type>().Last();
				for (int i = 0; i < max; ++i) {
					ret.Add(1);
				}
			} else {
				// Gold
				ret.Add(limitedGauss(TileDepth, 50, 100, 10, -1, 1));

				// Lapis
				ret.Add(limitedGauss(TileDepth, 30, 10, 0, -1, .2f));

				// Silver
				ret.Add(limitedGauss(TileDepth, 30, 100, 0, -1, 1.1f) + .002f);

				// Platinium
				ret.Add(limitedGauss(TileDepth, 80, 30, 50, -1, .5f));

				// Rubin
				ret.Add(limitedGauss(TileDepth, 200, 100, 80, -1, 1));

				// Copper
				ret.Add(limitedGauss(TileDepth, -20, 30, 0, -1, 0.7f) + .01f / (float)Math.Log(-TileDepth));

				// Saphire
				ret.Add(limitedGauss(TileDepth, 150, 70, 70, -1, .6f));

				// Salt
				ret.Add(limitedGauss(TileDepth, -20, 30, 0, -1, 0.7f));

				// Emerald
				ret.Add(limitedGauss(TileDepth, 20, 30, 0, -1, 0.7f));
			}

			// Normalize
			float sum = 0;
			foreach (float f in ret)
				sum += f;

			for (int i = 0; i < ret.Count; ++i)
				ret[i] /= sum;

			return ret;
		}

		public static Mineral RandomByDepth(int TileDepth)
		{
			int rand = rnd.Next(1, 1000);
			float normalizedRand = rand / 1000f;

			var propabilities = CalcPropabilitesOnDepth(TileDepth);

			int i = -1;
			for (float f = 0; f < normalizedRand; f += propabilities[++i])
				;
			
			switch (i) {
			case (int) Type.Gold:
				return Gold;
			case 1: 
				return Lapis;
			case 2:
				return Silver;
			case 3:
				return Platinium;
			case 4:
				return Rubin;
			case 5:
				return Copper;
			case 6:
				return Saphire;
			case 7:
				return Salt;
			case 8:
				return Emerald;
			}

			throw new Exception();
		}

		public static bool operator==(Mineral m1, Mineral m2)
		{
			if (object.ReferenceEquals(m1, null) && object.ReferenceEquals(m2, null))
				return true;

			if (object.ReferenceEquals(m1, null) || object.ReferenceEquals(m2, null))
				return false;
			
			return m1.type == m2.type;
		}

		public static bool operator!=(Mineral m1, Mineral m2)
		{
			if (m1 == null && m2 == null)
				return false;

			if (m1 == null || m2 == null)
				return true;

			return m1.type != m2.type;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public int BasePrice()
		{
			return Prices[(int)type];
		}

		public static Mineral Gold = new Mineral(Type.Gold, Sprites.Name.TileGold);
		public static Mineral Silver = new Mineral(Type.Silver, Sprites.Name.TileSilver);
		public static Mineral Lapis = new Mineral(Type.Lapis, Sprites.Name.TileLapis);
		public static Mineral Rubin = new Mineral(Type.Rubin, Sprites.Name.TileRubin);
		public static Mineral Saphire = new Mineral(Type.Saphire, Sprites.Name.TileSaphire);
		public static Mineral Copper = new Mineral(Type.Copper, Sprites.Name.TileCopper);
		public static Mineral Platinium = new Mineral(Type.Platinium, Sprites.Name.TilePlatinium);
		public static Mineral Salt = new Mineral(Type.Salt, Sprites.Name.TileSalt);
		public static Mineral Emerald = new Mineral(Type.Emerald, Sprites.Name.TileEmerald);
	}
}

