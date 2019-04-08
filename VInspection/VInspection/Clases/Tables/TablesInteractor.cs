using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Tables
{
    public class TablesInteractor : TablesInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public TablesInteractor(Context context)
        {
            mContext = context;
        }

        public async void descargarDocumentosDeVehiculo_SQLServer(string placa, TablesOnRequestFinished callback)
        {
            Log.Info(TAG, "Se inicia descargar de Fechas de Vehiculo de SQLServer");

            RestClient<Document> restClient = new RestClient<Document>();
            var document = new Document();

            try
            {
                document = await restClient.GetDocumentsByPlate(TIPO_DOCUMENTS, placa);
                callback.organizarFechas(document);
            }
            catch (Exception ex)
            {
                Log.Error("descargarDocumentosDeVehiculo_SQLServer", ex.Message);
                callback.errorDescargaDocumentos();
            }
        }

        public async void agregarPreUso_SQLServer(CheckList checkList, TablesOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia agregar PreUso a la BD de SQLServer");

            RestClient<CheckList> restClient = new RestClient<CheckList>();

            try
            {
                var respuesta = await restClient.PostAsync(TIPO_CHECKLIST, checkList);
                Log.Info("agregarPreUso_SQLServer", Convert.ToString(respuesta));
                callback.preUsoEnviado();
            }
            catch (Exception ex)
            {
                Log.Error("agregarPreUso_SQLServer", ex.Message);
            }
        }


        public List<Vehicle> obtenerTablaVehiculos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                List<Vehicle> lista = db.SelectTableVehicle();
                Log.Info("obtenerTablaVehiculos_SQLite", lista.ToString());
                return lista;
            }
            catch (Exception ex)
            {
                Log.Error("obtenerTablaVehiculos_SQLite", ex.Message);
                return null;
            }
        }
    }


    public interface TablesInteractorInterface
    {
        void descargarDocumentosDeVehiculo_SQLServer(string placa, TablesOnRequestFinished callback);
        void agregarPreUso_SQLServer(CheckList checkList, TablesOnRequestFinished callback);
        List<Vehicle> obtenerTablaVehiculos_SQLite();
    }

    public interface TablesOnRequestFinished
    {
        void preUsoEnviado();
        void mostrarProgreso(bool mostrar);
        void organizarFechas(Document output);
        void errorDescargaDocumentos();
    }
}