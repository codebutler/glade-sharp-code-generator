//
// GtkGladeCodeGenerator.cs
//
// Authors:
//  Eric Butler <eric@extremeboredom.net>
//  See AUTHORS for a full list of contributors.
//
// (C) 2005, Eric Butler <eric@extremeboredom.net>
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// // without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



using System;
using System.IO;
using System.Collections;
using GladeCodeGenerator;

using System.Reflection;
public class GtkGladeCodeGenerator
{
	public static void Main()
	{
		/*foreach (AssemblyName name in Assembly.GetExecutingAssembly().GetReferencedAssemblies()) {
			AppDomain.CurrentDomain.Load(name);
			Console.WriteLine("Loading: " + name.FullName);
		}*/
		
		new GtkGladeCodeGenerator();
	}

	Gtk.Window window;

	[Glade.Widget]
	Gtk.EventBox mainEventBox;

	[Glade.Widget]
	Gtk.Entry inputFileEntry;

	[Glade.Widget]
	Gtk.Entry outputDirectoryEntry;

	[Glade.Widget]
	Gtk.TreeView widgetTree;
	
	[Glade.Widget]
	Gtk.ComboBox languageComboBox;

	[Glade.Widget]
	Gtk.Label statusLabel;

	[Glade.Widget]
	Gtk.ProgressBar progressBar;
	
	Generator codegen;
	
	Gtk.TreeStore widgetTreeStore;

	public GtkGladeCodeGenerator()
	{
		Gtk.Application.Init();

		Glade.XML glade = new Glade.XML(null, "gladecodegenerator.glade", "MainWindow", null);
		glade.Autoconnect(this);
		window = (Gtk.Window)glade.GetWidget("MainWindow");

		//mainEventBox.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(0xff,0xff,0xff));
		mainEventBox.ModifyBg(Gtk.StateType.Normal, mainEventBox.Style.White);

		widgetTreeStore =  new Gtk.TreeStore(typeof(Gdk.Pixbuf), typeof(string), typeof(bool), typeof(Widget));

		Gtk.TreeViewColumn completeColumn = new Gtk.TreeViewColumn();

		Gtk.CellRendererToggle toggleRenderer = new Gtk.CellRendererToggle();
		toggleRenderer.Toggled += new Gtk.ToggledHandler (on_toggleRenderer_Toggled);
		completeColumn.PackStart(toggleRenderer, false);
		completeColumn.SetCellDataFunc(toggleRenderer, new Gtk.TreeCellDataFunc(toggleRendererFunc));
	
		Gtk.CellRendererPixbuf imageRenderer = new Gtk.CellRendererPixbuf();
		completeColumn.PackStart(imageRenderer, false);
		completeColumn.AddAttribute(imageRenderer, "pixbuf", 0);
		completeColumn.AddAttribute(imageRenderer, "visible", 2);

		Gtk.CellRendererText textRenderer = new Gtk.CellRendererText();
		completeColumn.PackStart(textRenderer, false);
		completeColumn.AddAttribute(textRenderer, "text", 1);

		widgetTree.AppendColumn(completeColumn);

		widgetTree.Model = widgetTreeStore;

		widgetTreeStore.SetSortColumnId(2,Gtk.SortType.Ascending);

		languageComboBox.Active = 0;
		
		window.Show();

		Gtk.Application.Run();

	}

	private void toggleRendererFunc (Gtk.TreeViewColumn tree_column, Gtk.CellRenderer cell, Gtk.TreeModel tree_model, Gtk.TreeIter iter)
	{
		Gtk.CellRendererToggle toggle = (cell as Gtk.CellRendererToggle);
		Widget currentWidget = (tree_model.GetValue(iter, 3) as Widget);
		if (currentWidget == null) {
			toggle.Visible = false;
		} else {
			toggle.Active = currentWidget.GenerateCode;
			toggle.Visible = true;
		}
	}
		
	private void on_toggleRenderer_Toggled(object o, Gtk.ToggledArgs args)
	{
		Gtk.TreeIter iter;
		if (widgetTreeStore.GetIterFromString (out iter, args.Path)) {
			//bool val = (bool) widgetTreeStore.GetValue (iter, 0);
			//widgetTreeStore.SetValue (iter, 0, !val);
			Widget widget = (widgetTreeStore.GetValue(iter, 3) as Widget);
		
			Gtk.TreeIter parent;
			if (widgetTreeStore.IterParent (out parent, iter) == false) {
				if (!widget.GenerateCode == false) {
					widget.GenerateCode = false;
					Gtk.TreeIter childIter;
					while (widgetTreeStore.IterChildren(out childIter, iter) == true) {
						widgetTreeStore.Remove(ref childIter);
					}

				} else {
					currentWindowIter = iter;
					typeIters.Clear();
					PopulateTreeWithWidgets( widget.Widgets );
					widgetTree.ExpandRow(widgetTreeStore.GetPath(iter), false);
					widget.GenerateCode = true;
				}
			} else {
				/*if (!val == true) {
					(widgetTreeStore.GetValue(iter, 5) as Widget).GenerateCode = true;
				} else {
					(widgetTreeStore.GetValue(iter, 5) as Widget).GenerateCode = false;
				}*/
				widget.GenerateCode = !widget.GenerateCode;
			}
		}

		int count = codegen.CountSelectedWidgets();
		if (count == 1)
			statusLabel.Text = "There is 1 widget selected.";
		else if (count == 0)
			statusLabel.Text = "There are no widgets selected.";
		else
			statusLabel.Text = "There are " + count + " total widgets selected.";
	}

	
	private void on_browseInputFile_clicked(object o, EventArgs e)
	{
		FileSelector fileSelector = new FileSelector("Select Glade XML File");
		if (fileSelector.Run() == (int)Gtk.ResponseType.Ok)
			inputFileEntry.Text = fileSelector.Filename;
		fileSelector.Destroy();
	}

	
	private void on_loadFileButton_clicked(object o, EventArgs e)
	{
		widgetTreeStore.Clear();

		if (File.Exists(inputFileEntry.Text) == true) {
			statusLabel.Text = "There are no widgets selected.";
			codegen = new Generator(inputFileEntry.Text);
			codegen.BeginLoadFile(new AsyncCallback(FileLoaded));
			progressBar.Visible = true;
			pulse = true;
			Gtk.Timeout.Add(60, new Gtk.Function(ProgressPulse));
		} else {
			Gtk.MessageDialog dialog = new Gtk.MessageDialog(window,
				       Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, 
				       "The specified file does not exist.");
			dialog.Run(); 
			dialog.Destroy();
		}
	}

	bool pulse;

	private void FileLoaded(IAsyncResult result) {
		codegen.EndLoadFile(result);
		RunOnMainThread.Run(this, "DoFileLoaded", null);
	}
	private void DoFileLoaded() {
		pulse = false;
		progressBar.Visible = false;
		FileInfo fileInfo = new FileInfo(inputFileEntry.Text);
		if (outputDirectoryEntry.Text == "") {
			outputDirectoryEntry.Text = Path.Combine(Path.Combine(Environment.CurrentDirectory,"generated"), fileInfo.Name);
		 }
		PopulateTreeWithWindows(codegen.Widgets);
	}


	private bool ProgressPulse()
	{
		progressBar.Pulse();
		return pulse;
	}


	private Gtk.TreeIter currentWindowIter;
	private Hashtable typeIters = new Hashtable();

	private void PopulateTreeWithWindows(WidgetCollection windows)
	{
		foreach (Widget widget in windows) {
			if (widget.Type == "GtkWindow" | widget.Type == "GtkMenu" | widget.Type == "GnomeApp" | widget.Type.EndsWith("Dialog")) {
				Gdk.Pixbuf image = GetImage(widget.Type);
				currentWindowIter = widgetTreeStore.AppendValues(image, widget.Name, true, widget);
			}
		}
	}
	
	private void PopulateTreeWithWidgets(WidgetCollection widgets)
	{

		foreach (Widget widget in widgets) {

			Gdk.Pixbuf image = GetImage(widget.Type);
					
			if (typeIters[widget.Type] == null) {
				string name = widget.Type; //.Substring(3);
				if (name.EndsWith("x"))
					name += "es";
				else if (name.EndsWith("y"))
					name = name.Substring(0, name.Length -1) + "ies";
				else
					name += "s";

				Gtk.TreeIter newIter = widgetTreeStore.AppendValues(currentWindowIter, image, name, true, null);
				typeIters.Add(widget.Type, newIter);
			}
			Gtk.TreeIter parentIter = (Gtk.TreeIter)typeIters[widget.Type];
			widgetTreeStore.AppendValues(parentIter, null, widget.Name, false, widget);
		
			PopulateTreeWithWidgets(widget.Widgets);
		}
	}

	//TODO: Fix this up:
	private Gdk.Pixbuf GetImage(string type)
	{
		string imageName = "";
		
		if (type.StartsWith("Gnome")) {
			imageName = type.Substring(5).ToLower() + ".png";
		} else if (type.StartsWith("Gtk")) {
			imageName = type.Substring(3).ToLower() + ".png";
		} else if (type.StartsWith("Bonobo")) {
			imageName = type.Substring(6).ToLower() + ".png";
		} else {
			throw new Exception("Unknown type: " + type);
		}

		if (imageName.IndexOf("dialog") > -1 | imageName.EndsWith("about.png"))
			imageName = "dialog.png";
		else if (imageName.IndexOf("combo") > -1)
			imageName = "combo.png";
		
		try {
			Gdk.Pixbuf image = new Gdk.Pixbuf(System.Reflection.Assembly.GetExecutingAssembly(), imageName);
			return image;
		} catch (Exception ex) {
			return new Gtk.IconTheme().LoadIcon(Gtk.Stock.MissingImage, 22, Gtk.IconLookupFlags.UseBuiltin);
		}
	}

	private void on_generateButton_clicked(object o, EventArgs e)
	{
		try {
			if (Directory.Exists(outputDirectoryEntry.Text) == false) {
				Gtk.MessageDialog dialog = new Gtk.MessageDialog(window, 
						Gtk.DialogFlags.Modal, Gtk.MessageType.Question, Gtk.ButtonsType.YesNo, 
						"The specified directory does not exist, would you like it to be created?");
			
				if (dialog.Run() == (int)Gtk.ResponseType.Yes) {
					Directory.CreateDirectory(outputDirectoryEntry.Text);
					dialog.Destroy();
				} else {
					dialog.Destroy();
					return;
				}
				
			}
			
			DirectoryInfo dir = new DirectoryInfo(outputDirectoryEntry.Text);
			if (dir.GetFiles().Length > 0) {
			
				Gtk.MessageDialog dialog = new Gtk.MessageDialog(window, 
						Gtk.DialogFlags.Modal, Gtk.MessageType.Question, Gtk.ButtonsType.YesNo,
					       	"The specified directory is not empty, any files with the same " +
						"names as your top-level widgets will be over-written. Do you want to continue?");
			
				if (dialog.Run() != (int)Gtk.ResponseType.Yes) {
					dialog.Destroy();
					return;
				 } else {
					dialog.Destroy();
				 }
			}
			
			if (languageComboBox.Active == 0)
				codegen.GenerateCode(Language.CSharp, outputDirectoryEntry.Text);
			else if (languageComboBox.Active == 1)
				codegen.GenerateCode(Language.VisualBasic, outputDirectoryEntry.Text);
			else if (languageComboBox.Active == 2)
				codegen.GenerateCode(Language.Boo, outputDirectoryEntry.Text);
			else if (languageComboBox.Active == 3)
				codegen.GenerateCode(Language.Nemerle, outputDirectoryEntry.Text);
			else
				throw new Exception ("What is " + languageComboBox.Active + "??");
			
			Gtk.MessageDialog msg = new Gtk.MessageDialog(window, Gtk.DialogFlags.Modal,
				       Gtk.MessageType.Info, Gtk.ButtonsType.Ok,
				       "Code generation completed!!");
			msg.Show();
			msg.Run();
			msg.Destroy();

			//TODO: This sucks!
			System.Diagnostics.Process.Start("gnome-open", outputDirectoryEntry.Text);

			
		} catch (Exception ex) {
			Console.WriteLine(ex);
			Gtk.MessageDialog errordialog = new Gtk.MessageDialog(window, 
					Gtk.DialogFlags.Modal, 
					Gtk.MessageType.Error, 
					Gtk.ButtonsType.Ok,
					ex.Message);

			errordialog.Run();
			errordialog.Destroy();
		}
	}

	private void on_MainWindow_delete_event (object o, Gtk.DeleteEventArgs e)
	{
		Gtk.Application.Quit();
	}

	private void fileQuitMenuItemActivated (object o, EventArgs e)
	{
		Gtk.Application.Quit();
	}

	private void helpAboutMenuItemActivated (object o, EventArgs e)
	{
		Glade.XML glade = new Glade.XML(null, "gladecodegenerator.glade", "AboutDialog", null);
		Gtk.Dialog dialog = (Gtk.Dialog)glade.GetWidget("AboutDialog");
		dialog.Show();
		dialog.Run();
		dialog.Destroy();
	}

	private void on_browseOutputFile_clicked (object o, EventArgs e)
	{
		FolderDialog folderDialog = new FolderDialog("Select folder to write code to");
		if (folderDialog.Run() == (int)Gtk.ResponseType.Ok) {
			outputDirectoryEntry.Text = folderDialog.CurrentFolder;
		}
		folderDialog.Destroy();
	}
}
