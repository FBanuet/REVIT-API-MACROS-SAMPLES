/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 23/02/2021
 * Time: 10:37 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace PRINTEST
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("405580B3-128D-4842-9FCA-8E9EF4CED373")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		public void PRINTTEST()
		{
			
						//variables GLOBALES
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			Selection sel = uidoc.Selection;
			ICollection<ElementId> ids = sel.GetElementIds();
			
			
			// MÉTODO DE SELECCIÓN
			
			ElementId psId = new FilteredElementCollector(doc).OfClass(typeof(PrintSetting))
				.First(x => x.Name == "PDF Architect 8_Horizontal").Id;
			
			ViewSet vset = ViewsetAddViews(ids,doc);
			
			// TRANSACCIÓN ARRANCE
			using(Transaction trans = new Transaction(doc,"PRINTING"))
			{
				trans.Start();
				
				doc.Print(vset,true);
				
				
				
				trans.Commit();
			}
			      
			TaskDialog.Show("DATARCHITECTS","OPERACION EXITOSA");
			
			
		}
		
		private ViewSet ViewsetAddViews(ICollection<ElementId> ViewIds , Document doc)
		{
			
			
			
			ViewSet vset = new ViewSet();
			
			
			
			//COLOCAR AQUI EL NOMBRE DE LA CONFIGURACIÓN DE IMPRESIÓN(PRINTSETTINGS)
			string match = "WRK-2020";
			
			
			//CICLO 
			foreach(ElementId eleid in ViewIds)
			{
				Element el = doc.GetElement(eleid);
				View view = el as View;
				
				vset.Insert(view);
			}
			
			PrintManager pm = doc.PrintManager;
			
			//EL NOMBRE TAL CUAL APARECE EN REVIT DE LA IMPRESORA 
			pm.SelectNewPrintDriver("Bluebeam PDF");
			
			//RANGO DE IMPREISÓN
			pm.PrintRange = PrintRange.Select;
			
			//pm.CombinedFile =  true ; QUITAR COMBINE PDF O INSTANCIAR MÉTODO DE CONFIGURACIÓN ACTUAL [EN PROCESO]

			ViewSheetSetting vss = pm.ViewSheetSetting;
			vss.CurrentViewSheetSet.Views = vset;
			
			using(Transaction trans = new Transaction(doc,"GENERATEVIEWSET"))
			{
				trans.Start();
				string SetName = "'" + match + " SHEETS";
				
				try
				{
					vss.SaveAs(SetName);
					
					
				}
				catch(Autodesk.Revit.Exceptions.InvalidOperationException)
				{
					TaskDialog.Show("WARNING!,ERROR!",SetName + "YA ESTA EN USO , USE OTRO  NOMBRE");
					trans.RollBack();
					
				}
				trans.Commit();
			}
			
			return vset;
		}
		
//		public void selections()
//		{
//		}
	}
}
