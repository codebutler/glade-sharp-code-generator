//
// Author: John Luke  <jluke@cfl.rr.com>
// License: LGPL
//

/* From Monodevelop */


using System;
using Gtk;



public class FolderDialog : FileSelector
{
	public FolderDialog (string title) : base (title, FileChooserAction.SelectFolder)
	{
		this.SelectMultiple = false;
	}
}

