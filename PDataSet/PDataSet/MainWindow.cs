using System;
using Gtk;
using System.Data;
using MySql.Data.MySqlClient;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		//Creamos conexion
		MySqlConnection mySqlConnection = new MySqlConnection (
			"DataSource=localhost;Database=dbprueba;User ID=root;Password=sistemas"
		);
		mySqlConnection.Open ();
		string selectSql = "select * from articulo";
		MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter (selectSql, mySqlConnection);

		//Ahora voy a construir un DataSet y lo voy a rellenar
		DataSet dataSet = new DataSet ();
		mySqlDataAdapter.Fill (dataSet);

		show (dataSet);

		DataTable dataTable = dataSet.Tables[0];
		DataRow dataRow = dataTable.Rows [0];
		//Modificamos una fila
		dataRow ["nombre"] = DateTime.Now.ToString ();//Con hora y fecha actual

		new MySqlCommandBuilder (mySqlDataAdapter);
		mySqlDataAdapter.Update (dataSet);

		mySqlDataAdapter.RowUpdating += delegate(object sender, MySqlRowUpdatingEventArgs e){
			Console.WriteLine("e.CommandType.CommandText={0}", e.Command.CommandText);
		};





	}

	private void show (DataSet dataSet){
		DataTable dataTable = dataSet.Tables[0];

		foreach (DataColumn dataColumn in dataTable.Columns)
			Console.WriteLine (dataColumn.ColumnName);
			Console.WriteLine ();

		foreach (DataRow dataRow in dataTable.Rows)
			foreach (DataColumn dataColumn in dataTable.Columns)
				Console.WriteLine ("{0}={1}",dataColumn.ColumnName, dataRow[dataColumn]);

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
