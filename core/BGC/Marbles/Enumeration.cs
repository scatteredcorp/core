using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BGC.Marbles {
	public abstract class Enumeration : IComparable
	{
		public byte Type { get; private set; }
		public string Name { get; private set; }


		protected Enumeration(byte type, string name) {
			Type = type;
			Name = name;
		}

		public override string ToString() => Name;

		public static IEnumerable<T> GetAll<T>() where T : Enumeration
		{
			var fields = typeof(T).GetFields(BindingFlags.Public |
			                                 BindingFlags.Static |
			                                 BindingFlags.DeclaredOnly);

			return fields.Select(f => f.GetValue(null)).Cast<T>();
		}

		public override bool Equals(object obj)
		{
			var otherValue = obj as Enumeration;

			if (otherValue == null)
				return false;

			var typeMatches = GetType().Equals(obj.GetType());
			var valueMatches = Type.Equals(otherValue.Type);

			return typeMatches && valueMatches;
		}

		public int CompareTo(object other) {
			if (Type > (byte) other) {
				return 1;
			} else if (Type == (byte) other) {
				return 0;
			}

			return -1;
		}

		// Other utility methods ...
	}
}
