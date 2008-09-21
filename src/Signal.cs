/* Written by Eric Butler <eric@extremeboredom.net> */

using System;
using System.Reflection;

namespace GladeCodeGenerator
{
	public class Signal
	{
		public Signal (Widget parentWidget, string name, string handler)
		{
			this.name = name;
			this.handler = handler;
			this.widget = parentWidget;
		}

		private string name;
		private string handler;
		
		private Widget widget;

		public string Name {
			get {
				return name;
			}
		}

		public string Handler {
			get {
				return handler;
			}
		}

		public Type GetEventHandlerType() {
			Type widgetType = Util.GetTypeFromString(Util.ConvertType(widget.Type));

			if (widgetType == null)
				throw new Exception("Unable to resolve type: " + widget.Type);
				
			return Util.ConvertSignal(widgetType, name);
		}

	}
}
