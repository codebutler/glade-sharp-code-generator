BINDIR=../bin
DATADIR=../data

ASSEMBLY=$(BINDIR)/GtkGladeSharpCodeGenerator.exe

MCS=mcs -debug -warn:0

RESOURCES=\
	-resource:$(DATADIR)/gladecodegenerator.glade			\
	-resource:$(DATADIR)/pixmaps/accellabel.png			\
	-resource:$(DATADIR)/pixmaps/alignment.png			\
	-resource:$(DATADIR)/pixmaps/app.png				\
	-resource:$(DATADIR)/pixmaps/arrow.png				\
	-resource:$(DATADIR)/pixmaps/button.png				\
	-resource:$(DATADIR)/pixmaps/calendar.png			\
	-resource:$(DATADIR)/pixmaps/checkbutton.png			\
	-resource:$(DATADIR)/pixmaps/checkmenuitem.png			\
	-resource:$(DATADIR)/pixmaps/colorbutton.png			\
	-resource:$(DATADIR)/pixmaps/colorselectiondialog.png		\
	-resource:$(DATADIR)/pixmaps/colorselection.png			\
	-resource:$(DATADIR)/pixmaps/combo.png				\
	-resource:$(DATADIR)/pixmaps/custom.png				\
	-resource:$(DATADIR)/pixmaps/dialog.png				\
	-resource:$(DATADIR)/pixmaps/dockitem.png			\
	-resource:$(DATADIR)/pixmaps/dock.png				\
	-resource:$(DATADIR)/pixmaps/drawingarea.png			\
	-resource:$(DATADIR)/pixmaps/druidpageedge.png			\
	-resource:$(DATADIR)/pixmaps/druid-page-finish.png		\
	-resource:$(DATADIR)/pixmaps/druidpagefinish.png		\
	-resource:$(DATADIR)/pixmaps/druid-page-standard.png		\
	-resource:$(DATADIR)/pixmaps/druidpagestandard.png		\
	-resource:$(DATADIR)/pixmaps/druid-page-start.png		\
	-resource:$(DATADIR)/pixmaps/druidpagestart.png			\
	-resource:$(DATADIR)/pixmaps/druid.png				\
	-resource:$(DATADIR)/pixmaps/entry.png				\
	-resource:$(DATADIR)/pixmaps/eventbox.png			\
	-resource:$(DATADIR)/pixmaps/expander.png			\
	-resource:$(DATADIR)/pixmaps/filechooserbutton.png		\
	-resource:$(DATADIR)/pixmaps/fileselection.png			\
	-resource:$(DATADIR)/pixmaps/fontbutton.png			\
	-resource:$(DATADIR)/pixmaps/fontselectiondialog.png		\
	-resource:$(DATADIR)/pixmaps/fontselection.png			\
	-resource:$(DATADIR)/pixmaps/frame.png				\
	-resource:$(DATADIR)/pixmaps/handlebox.png			\
	-resource:$(DATADIR)/pixmaps/hbox.png				\
	-resource:$(DATADIR)/pixmaps/hbuttonbox.png			\
	-resource:$(DATADIR)/pixmaps/hpaned.png				\
	-resource:$(DATADIR)/pixmaps/hscrollbar.png			\
	-resource:$(DATADIR)/pixmaps/hseparator.png			\
	-resource:$(DATADIR)/pixmaps/imagemenuitem.png			\
	-resource:$(DATADIR)/pixmaps/image.png				\
	-resource:$(DATADIR)/pixmaps/label.png				\
	-resource:$(DATADIR)/pixmaps/menubar.png			\
	-resource:$(DATADIR)/pixmaps/menuitem.png			\
	-resource:$(DATADIR)/pixmaps/menu.png				\
	-resource:$(DATADIR)/pixmaps/menutoolbutton.png			\
	-resource:$(DATADIR)/pixmaps/messagedialog.png			\
	-resource:$(DATADIR)/pixmaps/notebook.png			\
	-resource:$(DATADIR)/pixmaps/optionmenu.png			\
	-resource:$(DATADIR)/pixmaps/progressbar.png			\
	-resource:$(DATADIR)/pixmaps/radiobutton.png			\
	-resource:$(DATADIR)/pixmaps/radiomenuitem.png			\
	-resource:$(DATADIR)/pixmaps/scrolledwindow.png			\
	-resource:$(DATADIR)/pixmaps/separatormenuitem.png		\
	-resource:$(DATADIR)/pixmaps/separatortoolitem.png		\
	-resource:$(DATADIR)/pixmaps/spinbutton.png			\
	-resource:$(DATADIR)/pixmaps/statusbar.png			\
	-resource:$(DATADIR)/pixmaps/table.png				\
	-resource:$(DATADIR)/pixmaps/textview.png			\
	-resource:$(DATADIR)/pixmaps/togglebutton.png			\
	-resource:$(DATADIR)/pixmaps/toggletoolbutton.png		\
	-resource:$(DATADIR)/pixmaps/toolbar.png			\
	-resource:$(DATADIR)/pixmaps/toolbutton.png			\
	-resource:$(DATADIR)/pixmaps/toolitem.png			\
	-resource:$(DATADIR)/pixmaps/treeview.png			\
	-resource:$(DATADIR)/pixmaps/vbox.png				\
	-resource:$(DATADIR)/pixmaps/vbuttonbox.png			\
	-resource:$(DATADIR)/pixmaps/viewport.png			\
	-resource:$(DATADIR)/pixmaps/vpaned.png				\
	-resource:$(DATADIR)/pixmaps/vscrollbar.png			\
	-resource:$(DATADIR)/pixmaps/vseparator.png			\
	-resource:$(DATADIR)/pixmaps/window.png

REFERENCES=\
	-pkg:gtk-sharp-2.0		\
	-pkg:glade-sharp-2.0

FILES = GtkGladeCodeGenerator.cs 	\
	Generator.cs  			\
	Widget.cs  			\
	WidgetCollection.cs 		\
	Language.cs 			\
	Signal.cs 			\
	Util.cs  			\
	FileSelector.cs 		\
	FolderDialog.cs  		\
	RunOnMainThread.cs

all: $(ASSEMBLY)

$(ASSEMBLY): $(FILES) $(DATADIR)/gladecodegenerator.glade
	$(MCS) -out:$@ $(RESOURCES) $(REFERENCES) $(FILES) 
	echo "Done! Now run 'mono --debug GtkGladeSharpCodeGenerator.exe' in ../bin/"

clean:
	rm -f $(ASSEMBLY)
	rm -f $(ASSEMBLY).mdb
