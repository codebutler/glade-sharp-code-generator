using System;
using System.Reflection;

namespace GladeCodeGenerator
{
	public class Util
	{
		public static Type GetTypeFromString(string typeName)
		{
	//		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			foreach (Assembly assembly in Generator.AssembliesToSearch) {
				Type type = assembly.GetType(typeName);
				if (type != null)
					return type;
			}
			return null;
		}

		

		// TODO: Verify that this is the correct thing to do
		public static string ConvertType (string gladeName)
		{
			/* Find the index of the second capitol letter */
			
			char[] chars = gladeName.Substring(1).ToCharArray();
			
			for (int x = 0; x < chars.Length; x++) {
				Char thisChar = chars[x];
				if (Char.IsUpper(thisChar)) {
					string namespaceName = gladeName.Substring(0,x+1);
					string className = gladeName.Substring(x+1);
		
					//Console.WriteLine("Namespace: " + namespaceName);
					//Console.WriteLine("Class: " + className);
					
					return namespaceName + "." + className; //TODO: "." is language-dependent, replace with CodeDom
				}
			}
			return null;
		}

		// Everything below came from the Gtk# source:

		public static Type ConvertSignal (Type widgetType, string gladeName)
		{
			//Console.WriteLine("ConvertSignal: " + widgetType.ToString() + " " + gladeName);
			System.Reflection.MemberFilter signalFilter = new System.Reflection.MemberFilter (SignalFilter);
			System.Reflection.MemberInfo[] evnts = widgetType.
                                        FindMembers (System.Reflection.MemberTypes.Event,
                                             System.Reflection.BindingFlags.Instance
                                             | System.Reflection.BindingFlags.Static
                                             | System.Reflection.BindingFlags.Public
                                             | System.Reflection.BindingFlags.NonPublic,
                                             signalFilter, gladeName);

			return (evnts[0] as EventInfo).EventHandlerType;
		}



		/* matches events to GLib signal names */
		private static bool SignalFilter (System.Reflection.MemberInfo m, object filterCriteria)
		{
			string signame = (filterCriteria as string);
			object[] attrs = m.GetCustomAttributes (typeof (GLib.SignalAttribute), true);
			if (attrs.Length > 0)
			{
				foreach (GLib.SignalAttribute a in attrs)
				{
					if (signame == a.CName)
					{
						return true;
					}
				}
				return false;
			}
			else
			{
				/* this tries to match the names when no attibutes are present.
				   It is only a fallback. */
				signame = signame.ToLower ().Replace ("_", "");
				string evname = m.Name.ToLower ();
				return signame == evname;
			}
		}
	}
}
