using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Summary
{
    public class SummaryInteractor : SummaryInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public SummaryInteractor(Context context)
        {
            mContext = context;
        }

        public async void actualizarResumenPreUso_SQLServer(CheckListSummary resumen, TablesOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia actualizar resumen de Pre-Uso en la BD de SQLServer");

            RestClient<CheckListSummary> restClient = new RestClient<CheckListSummary>();

            try
            {
                var respuesta = await restClient.PutAsync(TIPO_CHECKLISTSUMMARY, resumen.IdResumen, resumen);
                Log.Info("actualizarResumenPreUso_SQLServer", Convert.ToString(respuesta));
                callback.resumenPreUsoActualizado();
            }
            catch (Exception ex)
            {
                Log.Error("actualizarResumenPreUso_SQLServer", ex.Message);
            }
        }

        public List<CheckListSummary> obtenerTablaPreUsos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                List<CheckListSummary> lista = db.SelectTableCheckListSumary();
                Log.Info("obtenerTablaPreUsos_SQLite", lista.ToString());
                return lista;
            }
            catch (Exception ex)
            {
                Log.Info("obtenerTablaPreUsos_SQLite", ex.Message);
                return null;
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



    public interface SummaryInteractorInterface
    {
        List<CheckListSummary> obtenerTablaPreUsos_SQLite();
        List<Vehicle> obtenerTablaVehiculos_SQLite();

        void actualizarResumenPreUso_SQLServer(CheckListSummary resumen, TablesOnRequestFinished callback);
    }

    public interface TablesOnRequestFinished
    {
        void resumenPreUsoActualizado();
        void mostrarProgreso(bool mostrar);
    }
}