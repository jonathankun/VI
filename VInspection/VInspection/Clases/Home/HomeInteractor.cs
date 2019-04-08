using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Home
{
    public class HomeInteractor : HomeInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public HomeInteractor(Context context)
        {
            mContext = context;
        }

        public async void descargarListaPreUsosDelDia_SQLServer(string date, HomeOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia descargar de Lista de Pre-Usos del dia desde SQLServer");

            RestClient<CheckListSummary> restClient = new RestClient<CheckListSummary>();
            var checkLists = new List<CheckListSummary>();

            try
            {
                checkLists = await restClient.GetCheckListSummariesByBrowser(TIPO_CHECKLISTSUMMARY, date);
                callback.mostrarProgreso(false);
                callback.compararListaPreUsos(checkLists);
            }
            catch (Exception ex)
            {
                Log.Info("descargarListaPreUsosDelDia_SQLServer", ex.Message);
                callback.errorDescargaPreUsos();
            }
        }

        public async void agregarPreUso_SQLServer(CheckList checkList, HomeOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia agregar PreUso a la BD de SQLServer");

            RestClient<CheckList> restClient = new RestClient<CheckList>();

            try
            {
                var respuesta = await restClient.PostAsync(TIPO_CHECKLIST, checkList);
                Log.Info("agregarPreUso_SQLServer", Convert.ToString(respuesta));
                //callback.preUsoEnviado();
                callback.mostrarProgreso(false);
            }
            catch (Exception ex)
            {
                Log.Error("agregarPreUso_SQLServer", ex.Message);
            }
        }

        public async void agregarResumenPreUso_SQLServer(CheckListSummary summary, HomeOnRequestFinished callback)
        {
            //callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia agregar Resumen de PreUso a la BD de SQLServer");

            RestClient<CheckListSummary> restClient = new RestClient<CheckListSummary>();

            try
            {
                var respuesta = await restClient.PostAsync(TIPO_CHECKLISTSUMMARY, summary);
                Log.Info("agregarResumenPreUso_SQLServer", Convert.ToString(respuesta));
                //callback.preUsoEnviado();
            }
            catch (Exception ex)
            {
                Log.Error("agregarResumenPreUso_SQLServer", ex.Message);
            }
        }

        public async void actualizarVehiculo_SQLServer(Vehicle vehicle, HomeOnRequestFinished callback)
        {
            //callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia actualizar Vehiculo en la BD de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();

            try
            {
                var respuesta = await restClient.PutAsync(TIPO_VEHICLES, vehicle.IdVehiculo, vehicle);
                Log.Info("actualizarVehiculo_SQLServer", Convert.ToString(respuesta));
                //callback.preUsoEnviado();
            }
            catch (Exception ex)
            {
                Log.Error("actualizarVehiculo_SQLServer", ex.Message);
            }
        }



        public void CrearBasedeDatos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.CreateDataBase();
            }
            catch (Exception ex)
            {
                Log.Info("CrearBasedeDatos_SQLite", ex.Message);
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

        public bool insertarTablaPreUsos_SQLite(List<CheckListSummary> lista)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.InsertIntoTableListCheckListSumary(lista);
                Log.Info("insertarTablaPreUsos_SQLite", lista.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("insertarTablaPreUsos_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarTablaPreUsos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteTableCheckListSumary();
                Log.Info(TAG, "borrarTablaPreUsos_SQLite");
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("borrarTablaPreUsos_SQLite", ex.Message);
                return false;
            }
        }

        public bool actualizarPreUsos_SQLite(CheckListSummary preUso)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.UpdateCheckListSumary(preUso);
                Log.Info("actualizarPreUsos_SQLite", preUso.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("actualizarPreUsos_SQLite", ex.Message);
                return false;
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

    public interface HomeInteractorInterface
    {
        void descargarListaPreUsosDelDia_SQLServer(string date, HomeOnRequestFinished callback);
        void agregarPreUso_SQLServer(CheckList checkList, HomeOnRequestFinished callback);

        void CrearBasedeDatos_SQLite();

        List<CheckListSummary> obtenerTablaPreUsos_SQLite();
        bool insertarTablaPreUsos_SQLite(List<CheckListSummary> lista);
        bool borrarTablaPreUsos_SQLite();
        bool actualizarPreUsos_SQLite(CheckListSummary preUso);



        void agregarResumenPreUso_SQLServer(CheckListSummary summary, HomeOnRequestFinished callback);
        void actualizarVehiculo_SQLServer(Vehicle vehicle, HomeOnRequestFinished callback);
        List<Vehicle> obtenerTablaVehiculos_SQLite();
    }

    public interface HomeOnRequestFinished
    {
        void compararListaPreUsos(List<CheckListSummary> output);
        void errorDescargaPreUsos();
        void mostrarProgreso(bool mostrar);
    }
}