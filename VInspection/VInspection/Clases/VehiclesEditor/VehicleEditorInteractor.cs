using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.VehiclesEditor
{
    public class VehicleEditorInteractor : VehicleEditorInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public VehicleEditorInteractor(Context context)
        {
            mContext = context;
        }

        public async void agregarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, Vehicle vehicle)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia agregar vehiculo a la BD de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();

            try
            {
                var respuesta = await restClient.PostAsync(TIPO_VEHICLES, vehicle);
                Log.Info("agregarVehiculo_SQLServer", Convert.ToString(respuesta));
                //callback.mostrarProgreso(false);
                callback.finalizarActividad();
            }
            catch (Exception ex)
            {
                Log.Error("agregarVehiculo_SQLServer", ex.Message);
            }
        }

        public async void actualizarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, Vehicle vehicle)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia actualizar vehiculo a la BD de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();

            try
            {
                var respuesta = await restClient.PutAsync(TIPO_VEHICLES, vehicle.IdVehiculo, vehicle);
                Log.Info("actualizarVehiculo_SQLServer", Convert.ToString(respuesta));
                //callback.mostrarProgreso(false);
                callback.finalizarActividad();
            }
            catch (Exception ex)
            {
                Log.Error("actualizarVehiculo_SQLServer", ex.Message);
            }
        }

        public async void eliminarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, int idvehicle)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia eliminar vehiculo a la BD de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();

            try
            {
                var respuesta = await restClient.DeleteAsync(TIPO_VEHICLES, idvehicle);
                Log.Info("eliminarVehiculo_SQLServer", Convert.ToString(respuesta));
                //callback.mostrarProgreso(false);
                callback.finalizarActividad();
            }
            catch (Exception ex)
            {
                Log.Error("eliminarVehiculo_SQLServer", ex.Message);
            }
        }

        public bool agregarVehiculo_SQLite(Vehicle vehicle)
        {
            UDataBase db = new UDataBase(mContext);
            var respuesta = db.InsertIntoTableVehicle(vehicle);
            return respuesta;
        }

        public bool actualizarVehiculo_SQLite(Vehicle vehicle)
        {
            UDataBase db = new UDataBase(mContext);
            var respuesta = db.UpdateVehicle(vehicle);
            return respuesta;
        }

        public List<Vehicle> obtenerTablaVehiculos_SQLite()
        {
            UDataBase db = new UDataBase(mContext);
            List<Vehicle> lista = db.SelectTableVehicle();
            return lista;
        }

        public List<User> obtenerTablaUsuarios_SQLite()
        {
            UDataBase db = new UDataBase(mContext);
            List<User> lista = db.SelectTableUser();
            return lista;
        }
    }

    public interface VehicleEditorInteractorInterface
    {
        void agregarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, Vehicle vehicle);
        void actualizarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, Vehicle vehicle);
        void eliminarVehiculo_SQLServer(VehicleEditorOnRequestFinished callback, int idvehicle);
        bool agregarVehiculo_SQLite(Vehicle vehicle);
        bool actualizarVehiculo_SQLite(Vehicle vehicle);
        List<Vehicle> obtenerTablaVehiculos_SQLite();
        List<User> obtenerTablaUsuarios_SQLite();
    }


    public interface VehicleEditorOnRequestFinished
    {
        void mostrarProgreso(bool mostrar);
        void finalizarActividad();
    }
}