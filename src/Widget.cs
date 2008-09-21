/* Written by Eric Butler <eric@extremeboredom.net> */

using System.Collections;
using System.Reflection;
using System;

namespace GladeCodeGenerator
{
	public class Widget
	{
		public Widget (string type, string name)
		{
			this.type = type;
			this.name = name;
		}

		private string type;
		private string name;

		private WidgetCollection widgets = new WidgetCollection();
		private ArrayList signals = new WidgetCollection();
		
		public bool GenerateCode = false;

		public string Type {
			get {
				return type;
			}
		}

		public string Name {
			get {
				return name;
			}
		}

		public WidgetCollection Widgets {
			get {
				return widgets;
			}
		}

		public ArrayList Signals {
			get {
				return signals;
			}
		}
	}
}
