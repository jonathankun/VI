using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Vehicles
{
    public class VehicleInteractor : VehicleInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public VehicleInteractor(Context context)
        {
            mContext = context;
        }

        public async void descargarListaVehiculos_SQLServer(VehicleOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia descargar de Lista de Vehiculos de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();
            var vehiclesList = new List<Vehicle>();

            try
            {
                vehiclesList = await restClient.GetAsync(TIPO_VEHICLES);
                callback.mostrarProgreso(false);
                callback.compararListaVehiculos(vehiclesList);
            }
            catch (Exception ex)
            {
                Log.Info("descargarListaVehiculos_SQLServer", ex.Message);
                callback.errorDescargaVehiculos();
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
                Log.Info("obtenerTablaVehiculos_SQLite", ex.Message);
                return null;
            }
        }

        public bool insertarTablaVehiculos_SQLite(List<Vehicle> lista)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.InsertIntoTableListVehicles(lista);
                Log.Info("insertarTablaVehiculos_SQLite", lista.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("insertarTablaVehiculos_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarTablaVehiculos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteTableVehicles();
                Log.Info(TAG, "borrarTablaVehiculos_SQLite");
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("borrarTablaVehiculos_SQLite", ex.Message);
                return false;
            }
        }

        public bool actualizarVehiculo_SQLite(Vehicle vehiculo)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.UpdateVehicle(vehiculo);
                Log.Info("actualizarVehiculo_SQLite", vehiculo.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("actualizarVehiculo_SQLite", ex.Message);
                return false;
            }
        }
    }

    public interface VehicleInteractorInterface
    {
        void descargarListaVehiculos_SQLServer(VehicleOnRequestFinished callback);

        List<Vehicle> obtenerTablaVehiculos_SQLite();
        bool insertarTablaVehiculos_SQLite(List<Vehicle> lista);
        bool borrarTablaVehiculos_SQLite();
        bool actualizarVehiculo_SQLite(Vehicle vehiculo);
    }

    public interface VehicleOnRequestFinished
    {
        void compararListaVehiculos(List<Vehicle> output);
        void errorDescargaVehiculos();
        void mostrarProgreso(bool mostrar);
    }
}