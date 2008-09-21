using GLib;
using System.Reflection;

public class RunOnMainThread {
	private object methodClass;
	private string methodName;
	private object[] arguments;
	
	public static void Run(object methodClass, string methodName, object[] arguments) {
		new RunOnMainThread(methodClass, methodName, arguments);
	}
	
	public RunOnMainThread(object methodClass, string methodName, object[] arguments) {
		this.methodClass = methodClass;
        this.methodName = methodName;
		this.arguments = arguments;
		GLib.Idle.Add(new IdleHandler(Go));
	}
	private bool Go() {
		methodClass.GetType().InvokeMember(methodName, BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public |
BindingFlags.NonPublic | BindingFlags.InvokeMethod, null,methodClass, arguments);
		return false;
	}
}
