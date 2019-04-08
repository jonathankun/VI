using Android.App;
using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UTime;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Tables
{
    public class TablesViewModel : TablesViewModelInterface, TablesOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        public const int VALIDATE_OPTION_NEXT = 1;
        public const int VALIDATE_OPTION_PREVIOUS = 2;

        private TablesFragmentViewInterface mView;
        private TablesInteractor mInteractor;
        private int mPosition;

        Context context = Application.Context;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        private ISharedPreferences mCheckListPreferences = Application.Context.GetSharedPreferences("CheckListInfo", FileCreationMode.Private);
        private ISharedPreferences mDocumentPreferences = Application.Context.GetSharedPreferences("DocumentInfo", FileCreationMode.Private);

        public TablesViewModel(int position,
                       TablesFragmentViewInterface view,
                       TablesInteractor Interactor)
        {
            mPosition = position;
            mView = view;
            mView.setViewModel(this);
            mInteractor = Interactor;
        }

        public void validarOpcion(int mPosition, bool guardado, int validateOption)
        {
            switch (validateOption)
            {
                case VALIDATE_OPTION_NEXT:
                    if (guardado == true)
                    {
                        if (mPosition + 1 == 5)
                        {
                            CheckList checkList = obtenerPreferenciasCheckList();
                            Log.Info(TAG, "Los datos del PreUso son: " + checkList);
                            if (UConnection.conectadoWifi())
                            {
                                mInteractor.agregarPreUso_SQLServer(checkList, this);
                            }
                            else
                            {
                                mView.showDialog(TablesActivityView.CODE_DIALOG_SET, "No hay conexión Wifi ¿Deseas guardar los datos para cuando tengas conexión?", "Aceptar", "Cancelar");
                            }
                        }
                        else if (mPosition + 1 != 5)
                        {
                            if (mPosition + 1 == 1)
                            {
                                string placa = mCheckListPreferences.GetString("Placa", String.Empty);
                                Log.Info(TAG, "la placa es: " + placa);
                                if (UConnection.conectadoWifi())
                                {
                                    mInteractor.descargarDocumentosDeVehiculo_SQLServer(placa, this);
                                }
                                else
                                {
                                    mView.showDialog(TablesActivityView.CODE_DIALOG_GET, "Debes encender la antena Wifi", "Ok", "False");
                                }
                            }
                            mView.goNext();
                        }
                    }
                    break;

                case VALIDATE_OPTION_PREVIOUS:
                    if (mPosition + 1 == 1)
                    {
                        mView.showDialog(TablesActivityView.CODE_DIALOG_DISCART, "¿Desea descartar los datos ingresados?", "Aceptar", "Cancelar");
                    }
                    else if (mPosition + 1 != 1)
                    {
                        mView.goPrevious();
                    }
                    break;
            }
        }

        public CheckList obtenerPreferenciasCheckList()
        {
            CheckList checkList = new CheckList()
            {
                Fecha = DateTime.Now,
                Placa = mCheckListPreferences.GetString("Placa", String.Empty),
                Kilometraje = mCheckListPreferences.GetInt("Kilometraje", 0),
                Mantto = mCheckListPreferences.GetString("Mantto", String.Empty),
                Produccion = mCheckListPreferences.GetString("Produccion", String.Empty),
                Destino = mCheckListPreferences.GetString("Destino", String.Empty),
                Conductor = mCheckListPreferences.GetString("Conductor", String.Empty),
                BanderaMantto = mCheckListPreferences.GetInt("BanderaMantto", 0),
                SistemaDireccion = mCheckListPreferences.GetString("SistemaDireccion", String.Empty),
                SistemaFrenos = mCheckListPreferences.GetString("SistemaFrenos", String.Empty),
                Faros = mCheckListPreferences.GetString("Faros", String.Empty),
                LucesDireccionales = mCheckListPreferences.GetString("LucesDireccionales", String.Empty),
                Asientos = mCheckListPreferences.GetString("Asientos", String.Empty),
                Cinturones = mCheckListPreferences.GetString("Cinturones", String.Empty),
                Vidrios = mCheckListPreferences.GetString("Vidrios", String.Empty),
                LimpiaParabrisas = mCheckListPreferences.GetString("LimpiaParabrisas", String.Empty),
                EspejoInterno = mCheckListPreferences.GetString("EspejoInterno", String.Empty),
                EspejoExterno = mCheckListPreferences.GetString("EspejoExterno", String.Empty),
                NivelAceite = mCheckListPreferences.GetString("NivelAceite", String.Empty),
                NivelAgua = mCheckListPreferences.GetString("NivelAgua", String.Empty),
                Combustible = mCheckListPreferences.GetString("Combustible", String.Empty),
                Claxon = mCheckListPreferences.GetString("Claxon", String.Empty),
                AlarmaRetorceso = mCheckListPreferences.GetString("AlarmaRetorceso", String.Empty),
                RelojesIndicadores = mCheckListPreferences.GetString("RelojesIndicadores", String.Empty),
                Neumaticos = mCheckListPreferences.GetString("Neumaticos", String.Empty),
                NeumaticoRepuesto = mCheckListPreferences.GetString("NeumaticoRepuesto", String.Empty),
                Extintor = mCheckListPreferences.GetString("Extintor", String.Empty),
                ConosSeguridad = mCheckListPreferences.GetString("ConosSeguridad", String.Empty),
                SogaArrastre = mCheckListPreferences.GetString("SogaArrastre", String.Empty),
                Botiquin = mCheckListPreferences.GetString("Botiquin", String.Empty),
                HerramientasLlaves = mCheckListPreferences.GetString("HerramientasLlaves", String.Empty),
                GataPalanca = mCheckListPreferences.GetString("GataPalanca", String.Empty),
                Triangulo = mCheckListPreferences.GetString("Triangulo", String.Empty),
                Linterna = mCheckListPreferences.GetString("Linterna", String.Empty),
                Cunas = mCheckListPreferences.GetString("Cunas", String.Empty),
                Carroceria = mCheckListPreferences.GetString("Carroceria", String.Empty),
                Pertiga = mCheckListPreferences.GetString("Pertiga", String.Empty),
                Circulina = mCheckListPreferences.GetString("Circulina", String.Empty),
                BanderaItems = mCheckListPreferences.GetInt("BanderaItems", 0),
                ComentariosAdicionales = mCheckListPreferences.GetString("ComentariosAdicionales", String.Empty),
                Observacion1 = mCheckListPreferences.GetString("Observacion1", String.Empty),
                Prioridad1 = mCheckListPreferences.GetInt("Prioridad1", 0),
                Observacion2 = mCheckListPreferences.GetString("Observacion2", String.Empty),
                Prioridad2 = mCheckListPreferences.GetInt("Prioridad2", 0),
                Observacion3 = mCheckListPreferences.GetString("Observacion3", String.Empty),
                Prioridad3 = mCheckListPreferences.GetInt("Prioridad3", 0),
                Observacion4 = mCheckListPreferences.GetString("Observacion4", String.Empty),
                Prioridad4 = mCheckListPreferences.GetInt("Prioridad4", 0),
                BanderaComentarios = mCheckListPreferences.GetInt("BanderaComentarios", 0),
                CajaSoporte = mCheckListPreferences.GetString("CajaSoporte", String.Empty),
                Alcohol = mCheckListPreferences.GetString("Alcohol", String.Empty),
                Jabon = mCheckListPreferences.GetString("Jabon", String.Empty),
                Algodon = mCheckListPreferences.GetString("Algodon", String.Empty),
                Aposito = mCheckListPreferences.GetString("Aposito", String.Empty),
                Bandas = mCheckListPreferences.GetString("Bandas", String.Empty),
                Esparadrapo = mCheckListPreferences.GetString("Esparadrapo", String.Empty),
                Gasas1 = mCheckListPreferences.GetString("Gasas1", String.Empty),
                Gasas2 = mCheckListPreferences.GetString("Gasas2", String.Empty),
                Tijera = mCheckListPreferences.GetString("Tijera", String.Empty),
                Venda = mCheckListPreferences.GetString("Venda", String.Empty),
                ComentariosBotiquin = mCheckListPreferences.GetString("ComentariosBotiquin", String.Empty),
                BanderaBotiquin = mCheckListPreferences.GetInt("BanderaBotiquin", 0),
                EstadoSOAT = mCheckListPreferences.GetString("EstadoSOAT", String.Empty),
                EstadRevTecnica = mCheckListPreferences.GetString("EstadRevTecnica", String.Empty),
                SOAT = mCheckListPreferences.GetString("SOAT", String.Empty),
                RevTecnica = mCheckListPreferences.GetString("RevTecnica", String.Empty),
                TarjetaPropiedad = mCheckListPreferences.GetString("TarjetaPropiedad", String.Empty),
                CartillaSeguridad = mCheckListPreferences.GetString("CartillaSeguridad", String.Empty),
                CartillaERP = mCheckListPreferences.GetString("CartillaERP", String.Empty),
                BanderaDocumentos = mCheckListPreferences.GetInt("BanderaDocumentos", 0),
            };

            if (mCheckListPreferences.GetString("NuevaFechaSOAT", String.Empty) != "" && checkList.EstadoSOAT == "0") { checkList.NuevaFechaSOAT = DateTime.Parse(mCheckListPreferences.GetString("NuevaFechaSOAT", String.Empty)); }
            if (mCheckListPreferences.GetString("NuevaFechaRevTecnica", String.Empty) != "" && checkList.EstadRevTecnica == "0") { checkList.NuevaFechaRevTecnica = DateTime.Parse(mCheckListPreferences.GetString("NuevaFechaRevTecnica", String.Empty)); }

            checkList.BanderaPrincipal = 0;

            if (checkList.BanderaMantto == 1 || checkList.BanderaItems == 1 || checkList.BanderaComentarios == 1 || checkList.BanderaBotiquin == 1 || checkList.BanderaDocumentos == 1)
            {
                checkList.BanderaPrincipal = 1;
            }

            checkList.Buscador = PrepareDateToBrowser(checkList.Fecha) + "@" + checkList.Placa + "@" + checkList.Produccion + "@" + checkList.Destino + "@" + checkList.Conductor;
            checkList.Garitas = checkList.Produccion + "@" + checkList.Destino;

            return checkList;
        }

        public bool guardarPreferenciasCheckList(int mPosition, List<string> values, int Flag)
        {
            Log.Info(TAG, "La lista de datos de la tabla " + (mPosition + 1) + " son:\n" + String.Join(",", values.ToArray()));
            Log.Info(TAG, "La bandera de la tabla " + (mPosition + 1) + " es:\n" + Flag);

            ISharedPreferencesEditor editCheckList = mCheckListPreferences.Edit();

            /*foreach(string value in values)
            {
                if (value == null || value == String.Empty || value == "")
                {
                    Log.Info(TAG, "Los valores tienen datos vacios");
                    return false;
                }
            }*/

            switch (mPosition)
            {
                case 0:
                    editCheckList.PutString("Placa", values[0]);
                    editCheckList.PutString("Modelo", values[1]);
                    editCheckList.PutInt("Kilometraje", Int32.Parse(values[2]));
                    editCheckList.PutString("Mantto", values[3]);
                    editCheckList.PutString("Produccion", values[4]);
                    editCheckList.PutString("Destino", values[5]);
                    editCheckList.PutString("Conductor", values[6]);
                    editCheckList.PutInt("BanderaMantto", Flag);
                    break;
                case 1:
                    editCheckList.PutString("SistemaDireccion", values[0]);
                    editCheckList.PutString("SistemaFrenos", values[1]);
                    editCheckList.PutString("Faros", values[2]);
                    editCheckList.PutString("LucesDireccionales", values[3]);
                    editCheckList.PutString("Asientos", values[4]);
                    editCheckList.PutString("Cinturones", values[5]);
                    editCheckList.PutString("Vidrios", values[6]);
                    editCheckList.PutString("LimpiaParabrisas", values[7]);
                    editCheckList.PutString("EspejoInterno", values[8]);
                    editCheckList.PutString("EspejoExterno", values[9]);
                    editCheckList.PutString("NivelAceite", values[10]);
                    editCheckList.PutString("NivelAgua", values[11]);
                    editCheckList.PutString("Combustible", values[12]);
                    editCheckList.PutString("Claxon", values[13]);
                    editCheckList.PutString("AlarmaRetorceso", values[14]);
                    editCheckList.PutString("RelojesIndicadores", values[15]);
                    editCheckList.PutString("Neumaticos", values[16]);
                    editCheckList.PutString("NeumaticoRepuesto", values[17]);
                    editCheckList.PutString("Extintor", values[18]);
                    editCheckList.PutString("ConosSeguridad", values[19]);
                    editCheckList.PutString("SogaArrastre", values[20]);
                    editCheckList.PutString("Botiquin", values[21]);
                    editCheckList.PutString("HerramientasLlaves", values[22]);
                    editCheckList.PutString("GataPalanca", values[23]);
                    editCheckList.PutString("Triangulo", values[24]);
                    editCheckList.PutString("Linterna", values[25]);
                    editCheckList.PutString("Cunas", values[26]);
                    editCheckList.PutString("Carroceria", values[27]);
                    editCheckList.PutString("Pertiga", values[28]);
                    editCheckList.PutString("Circulina", values[29]);
                    editCheckList.PutInt("BanderaItems", Flag);
                    break;
                case 2:
                    editCheckList.PutString("ComentariosAdicionales", values[0]);
                    editCheckList.PutString("Observacion1", values[1]);
                    editCheckList.PutInt("Prioridad1", Int32.Parse(values[2]));
                    editCheckList.PutString("Observacion2", values[3]);
                    editCheckList.PutInt("Prioridad2", Int32.Parse(values[4]));
                    editCheckList.PutString("Observacion3", values[5]);
                    editCheckList.PutInt("Prioridad3", Int32.Parse(values[6]));
                    editCheckList.PutString("Observacion4", values[7]);
                    editCheckList.PutInt("Prioridad4", Int32.Parse(values[8]));
                    editCheckList.PutInt("BanderaComentarios", Flag);
                    break;
                case 3:
                    editCheckList.PutString("CajaSoporte", values[0]);
                    editCheckList.PutString("Alcohol", values[1]);
                    editCheckList.PutString("Jabon", values[2]);
                    editCheckList.PutString("Algodon", values[3]);
                    editCheckList.PutString("Aposito", values[4]);
                    editCheckList.PutString("Bandas", values[5]);
                    editCheckList.PutString("Esparadrapo", values[6]);
                    editCheckList.PutString("Gasas1", values[7]);
                    editCheckList.PutString("Gasas2", values[8]);
                    editCheckList.PutString("Tijera", values[9]);
                    editCheckList.PutString("Venda", values[10]);
                    editCheckList.PutString("ComentariosBotiquin", values[11]);
                    editCheckList.PutInt("BanderaBotiquin", Flag);
                    break;
                case 4:
                    editCheckList.PutString("EstadoSOAT", values[0]);
                    editCheckList.PutString("NuevaFechaSOAT", values[1]);
                    editCheckList.PutString("EstadRevTecnica", values[2]);
                    editCheckList.PutString("NuevaFechaRevTecnica", values[3]);
                    editCheckList.PutString("SOAT", values[4]);
                    editCheckList.PutString("RevTecnica", values[5]);
                    editCheckList.PutString("TarjetaPropiedad", values[6]);
                    editCheckList.PutString("CartillaSeguridad", values[7]);
                    editCheckList.PutString("CartillaERP", values[8]);
                    editCheckList.PutInt("BanderaDocumentos", Flag);
                    break;
            }

            editCheckList.Apply();



            return true;
        }

        public string[] obtenerListaPlacas()
        {
            List<string> Lista_Placas = new List<string>();

            List<Vehicle> vehiculos = mInteractor.obtenerTablaVehiculos_SQLite();

            for (int i = 0; i < vehiculos.Count; i++)
            {
                Lista_Placas.Add(vehiculos[i].Placa);
            }

            string[] Lista = new string[Lista_Placas.Count];
            Lista = Lista_Placas.ToArray();

            Log.Info(TAG, "La lista de placas es: " + String.Join(",",Lista));

            return Lista;
        }

        public string obtenerModeloDePlaca(string Placa)
        {
            try
            {
                List<Vehicle> vehiculos = mInteractor.obtenerTablaVehiculos_SQLite();
                Vehicle vehiculo = vehiculos.Find(x => x.Placa == Placa);
                return vehiculo.Modelo;
            }
            catch
            {
                return "";
            }
        }

        public int evaluarKilometraje(string Placa, string Kilometraje)
        {
            int Bandera = 0;

            if (Placa != String.Empty)
            {
                List<Vehicle> vehiculos = mInteractor.obtenerTablaVehiculos_SQLite();
                Vehicle vehiculo = vehiculos.Find(x => x.Placa == Placa);
            
                double UMantto = vehiculo.KUMantto;
                double Kilometros = Convert.ToUInt32(Kilometraje);

                double dif = Kilometros - UMantto;
                if (dif > 4800 && dif <= 5000)
                {
                    Bandera = 1;
                }
                else if (dif > 5000)
                {
                    Bandera = 2;
                }
            }

            return Bandera;
        }

        public int evaluarItems(List<string> TableData)
        {
            int Bandera = 0;
            for (int i = 0; i < TableData.Count - 2; i++)
            {
                if (TableData[i] == "0")
                {
                    Bandera = 1;
                    break;
                }
            }
            return Bandera;
        }

        public int evaluarComentarios(List<string> TableData)
        {
            int Bandera = 0;
            for (int i = 0; i < TableData.Count; i++)
            {
                if (i == 0 || i % 2 != 0)
                {
                    if (TableData[i] != "")
                    {
                        Bandera = 1;
                        break;
                    }
                }
            }
            return Bandera;
        }

        public int evaluarBotiquin(List<string> TableData)
        {
            int Bandera = 0;
            for (int i = 0; i < TableData.Count - 1; i++)
            {
                if (TableData[i] == "0")
                {
                    Bandera = 1;
                    break;
                }
            }

            string Comentario = TableData[TableData.Count - 1];
            Log.Info(TAG, "Comentario:" + Comentario);
            if (Comentario != "")
            {
                Bandera = 1;
            }
            return Bandera;
        }

        public int evaluarDocumentacion(List<string> TableData)
        {
            int Bandera = 0;
            for (int i = 0; i < TableData.Count; i++)
            {
                if (i != 1 && i != 3)
                {
                    if (TableData[i] == "0")
                    {
                        Bandera = 1;
                        break;
                    }
                }
            }
            return Bandera;
        }

        public void preUsoEnviado()
        {
            mView.showSentCheckListAlert();
            //mView.showHomeScreen();
            mView.finishActivity();
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
        }

        public void organizarFechas(Document output)
        {
            ISharedPreferencesEditor editDocument = mDocumentPreferences.Edit();
            editDocument.PutInt("IdDocumento", output.IdDocumento);
            editDocument.PutString("Placa", output.Placa);
            editDocument.PutString("FechaSOAT", output.FechaSOAT.ToString());
            editDocument.PutString("FechaRevTecnica", output.FechaRevTecnica.ToString());
            editDocument.Apply();
        }

        public void errorDescargaDocumentos()
        {
            //mView.errorRefreshnig();
        }
}

    public interface TablesViewModelInterface
    {
        void validarOpcion(int mPosition, bool guardado, int validateOption);
        string[] obtenerListaPlacas();
        string obtenerModeloDePlaca(string Placa);

        CheckList obtenerPreferenciasCheckList();
        bool guardarPreferenciasCheckList(int mPosition, List<string> values, int Flag);

        int evaluarKilometraje(string Placa, string Kilometraje);
        int evaluarItems(List<string> TableData);
        int evaluarComentarios(List<string> TableData);
        int evaluarBotiquin(List<string> TableData);
        int evaluarDocumentacion(List<string> TableData);
    }
}