using Android.Content;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Start
{
    public class StartInteractor : StartActivityInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public StartInteractor(Context context)
        {
            mContext = context;
        }

        public async void descargarListaUsuarios_SQLServer(StartActicityOnRequestFinished callback)
        {
            callback.levantarBanderaUsuarios(false);

            Log.Info(TAG, "Se inicia descargar de Lista de Usuarios de SQLServer");

            RestClient<User> restClient = new RestClient<User>();
            var usersList = new List<User>();

            try
            {
                usersList = await restClient.GetUsersName(TIPO_USER);
                callback.levantarBanderaUsuarios(true);
                callback.compararListaUsuarios(usersList);
            }
            catch (Exception ex)
            {
                Log.Error("descargarListaUsuarios_SQLServer", ex.Message);
                callback.errorDescargaUsuarios();
            }
        }

        public async void descargarListaVehiculos_SQLServer(StartActicityOnRequestFinished callback)
        {
            callback.levantarBanderaVehiculos(false);

            Log.Info(TAG, "Se inicia descargar de Lista de Vehiculos de SQLServer");

            RestClient<Vehicle> restClient = new RestClient<Vehicle>();
            var vehiclesList = new List<Vehicle>();

            try
            {
                vehiclesList = await restClient.GetAsync(TIPO_VEHICLES);
                callback.levantarBanderaVehiculos(true);
                callback.compararListaVehiculos(vehiclesList);
            }
            catch (Exception ex)
            {
                Log.Error("descargarListaVehiculos_SQLServer", ex.Message);
                callback.errorDescargaVehiculos();
            }
        }

        public async void descargarListaPreUsosDelDia_SQLServer(string fecha, StartActicityOnRequestFinished callback)
        {
            callback.levantarBanderaPreUsos(false);

            Log.Info(TAG, "Se inicia descargar de Lista de Pre-Usos del dia desde SQLServer");

            RestClient<CheckListSummary> restClient = new RestClient<CheckListSummary>();
            var CheckLists = new List<CheckListSummary>();

            try
            {
                CheckLists = await restClient.GetCheckListSummariesByBrowser(TIPO_CHECKLISTSUMMARY, fecha);
                callback.levantarBanderaPreUsos(true);
                callback.compararListaPreUsos(CheckLists);
            }
            catch (Exception ex)
            {
                Log.Error("descargarListaCheckLists_SQLServer", ex.Message);
                callback.errorDescargaVehiculos();
            }
        }

        public void CrearBasedeDatos_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.CreateDataBase();
                Log.Info(TAG, "Se creo Base de Datos SQLite");
            }
            catch (Exception ex)
            {
                Log.Error("CrearBasedeDatos_SQLite", ex.Message);
            }
        }

        public List<User> obtenerTablaUsuarios_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                List<User> lista = db.SelectTableUser();
                Log.Info("obtenerTablaUsuarios_SQLite", lista.ToString());
                return lista;
            }
            catch (Exception ex)
            {
                Log.Error("obtenerTablaUsuarios_SQLite", ex.Message);
                return null;
            }
        }

        public bool insertarTablaUsuarios_SQLite(List<User> lista)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.InsertIntoTableListUser(lista);
                Log.Info("insertarTablaUsuarios_SQLite", lista.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("insertarTablaUsuarios_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarTablaUsuarios_SQLite()
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteTableUser();
                Log.Info(TAG, "borrarTablaVehiculos_SQLite");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("borrarTablaVehiculos_SQLite", ex.Message);
                return false;
            }
        }

        public bool actualizarUsuarios_SQLite(User usuario)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.UpdateUser(usuario);
                Log.Info("actualizarUsuarios_SQLite", JsonConvert.SerializeObject(usuario));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("actualizarUsuarios_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarUsuario_SQLite(User usuario)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteUser(usuario);
                Log.Info("borrarUsuario_SQLite", usuario.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("borrarUsuario_SQLite", ex.Message);
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
                Log.Error("insertarTablaVehiculos_SQLite", ex.Message);
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
                Log.Error("borrarTablaVehiculos_SQLite", ex.Message);
                return false;
            }
        }

        public bool actualizarVehiculo_SQLite(Vehicle vehiculo)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.UpdateVehicle(vehiculo);
                Log.Info("actualizarVehiculo_SQLite", JsonConvert.SerializeObject(vehiculo));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("actualizarVehiculo_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarVehiculo_SQLite(Vehicle vehiculo)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteVehicle(vehiculo);
                Log.Info("borrarVehiculo_SQLite", vehiculo.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("borrarVehiculo_SQLite", ex.Message);
                return false;
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
                Log.Error("obtenerTablaPreUsos_SQLite", ex.Message);
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
                Log.Error("insertarTablaPreUsos_SQLite", ex.Message);
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
                Log.Error("borrarTablaPreUsos_SQLite", ex.Message);
                return false;
            }
        }

        public bool actualizarPreUso_SQLite(CheckListSummary preUso)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.UpdateCheckListSumary(preUso);
                Log.Info("actualizarPreUsos_SQLite", JsonConvert.SerializeObject(preUso));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("actualizarPreUsos_SQLite", ex.Message);
                return false;
            }
        }

        public bool borrarPreUso_SQLite(CheckListSummary checkList)
        {
            try
            {
                UDataBase db = new UDataBase(mContext);
                db.DeleteCheckListSumary(checkList);
                Log.Info("borrarPreUso_SQLite", checkList.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("borrarPreUso_SQLite", ex.Message);
                return false;
            }
        }

    }

    public interface StartActivityInteractorInterface
    {
        void descargarListaUsuarios_SQLServer(StartActicityOnRequestFinished callback);
        void descargarListaVehiculos_SQLServer(StartActicityOnRequestFinished callback);
        void descargarListaPreUsosDelDia_SQLServer(string fecha, StartActicityOnRequestFinished callback);

        void CrearBasedeDatos_SQLite();

        List<User> obtenerTablaUsuarios_SQLite();
        bool insertarTablaUsuarios_SQLite(List<User> lista);
        bool borrarTablaUsuarios_SQLite();
        bool actualizarUsuarios_SQLite(User usuario);
        bool borrarUsuario_SQLite(User usuario);

        List<Vehicle> obtenerTablaVehiculos_SQLite();
        bool insertarTablaVehiculos_SQLite(List<Vehicle> lista);
        bool borrarTablaVehiculos_SQLite();
        bool actualizarVehiculo_SQLite(Vehicle vehiculo);
        bool borrarVehiculo_SQLite(Vehicle vehiculo);

        List<CheckListSummary> obtenerTablaPreUsos_SQLite();
        bool insertarTablaPreUsos_SQLite(List<CheckListSummary> lista);
        bool borrarTablaPreUsos_SQLite();
        bool actualizarPreUso_SQLite(CheckListSummary preUso);
        bool borrarPreUso_SQLite(CheckListSummary checkList);
    }

    public interface StartActicityOnRequestFinished
    {
        void compararListaUsuarios(List<User> output);
        void levantarBanderaUsuarios(bool bandera);
        void errorDescargaUsuarios();

        void compararListaVehiculos(List<Vehicle> output);
        void levantarBanderaVehiculos(bool bandera);
        void errorDescargaVehiculos();

        void compararListaPreUsos(List<CheckListSummary> output);
        void levantarBanderaPreUsos(bool bandera);
        void errorDescargaPreUsos();
    }
}