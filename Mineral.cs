/*
 * Copyright (C) 2016-2017 Oskar Świtalski – All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * Proprietary and confidential
 */

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
			
		private readonly Sprites.Name sprite;
		private readonly int price;

		private Type type = Type.Gold;
		private Distribution distribution;

		private Mineral(Type type, Sprites.Name sprite, int price, Distribution distribution)
		{
			this.type = type;
			this.sprite = sprite;
			this.price = price;
			this.distribution = distribution;
		}

		public Sprites.Name GetSprite()
		{
			return sprite;
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
				int max = (int)Enum.GetValues(typeof(Type)).Cast<Type>().Count();
				for (int i = 0; i < max; ++i) {
					ret.Add(1);
				}
			} else {
				TileDepth = -TileDepth;
				ret.Add(Gold.distribution.Propability(TileDepth));
				ret.Add(Lapis.distribution.Propability(TileDepth));
				ret.Add(Silver.distribution.Propability(TileDepth));
				ret.Add(Platinium.distribution.Propability(TileDepth));
				ret.Add(Rubin.distribution.Propability(TileDepth));
				ret.Add(Copper.distribution.Propability(TileDepth));
				ret.Add(Saphire.distribution.Propability(TileDepth));
				ret.Add(Salt.distribution.Propability(TileDepth));
				ret.Add(Emerald.distribution.Propability(TileDepth));
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
			return price;
		}

		public static Mineral Gold = new Mineral(Type.Gold, Sprites.Name.TileGold, 100, new TriangleDistribution(10, 100, 1));
		public static Mineral Silver = new Mineral(Type.Silver, Sprites.Name.TileSilver, 20, new TriangleDistribution(10, 100, 1));
		public static Mineral Lapis = new Mineral(Type.Lapis, Sprites.Name.TileLapis, 50, new TriangleDistribution(10, 100, 1));
		public static Mineral Rubin = new Mineral(Type.Rubin, Sprites.Name.TileRubin, 65, new TriangleDistribution(10, 100, 1));
		public static Mineral Saphire = new Mineral(Type.Saphire, Sprites.Name.TileSaphire, 125, new TriangleDistribution(10, 100, 1));
		public static Mineral Copper = new Mineral(Type.Copper, Sprites.Name.TileCopper, 5, new TriangleDistribution(10, 100, 1));
		public static Mineral Platinium = new Mineral(Type.Platinium, Sprites.Name.TilePlatinium, 250, new TriangleDistribution(10, 100, 1));
		public static Mineral Salt = new Mineral(Type.Salt, Sprites.Name.TileSalt, 3, new TriangleDistribution(0, 25, 1));
		public static Mineral Emerald = new Mineral(Type.Emerald, Sprites.Name.TileEmerald, 35, new TriangleDistribution(10, 100, 1));
	}
}

