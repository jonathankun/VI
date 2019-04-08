using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Login
{
    public class LoginInteractor : VehicleInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public LoginInteractor(Context context)
        {
            mContext = context;
        }

        public async void consultarUsuario_SQLServer(User user, VehicleOnRequestFinished callback)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia la consulta de Usuarios de SQLServer");

            RestClient<User> restClient = new RestClient<User>();
            User usuarioRecibido = new User();

            try
            {
                usuarioRecibido = await restClient.GetUserByAccount(TIPO_USER, user.Cuenta);
                callback.mostrarProgreso(false);
                callback.evaluarUsuario(user, usuarioRecibido);
            }
            catch (Exception ex)

            {
                Log.Info("consultarUsuario_SQLServer", ex.Message);
                callback.errorNoReconocido();
            }
        }
    }


    public interface VehicleInteractorInterface
    {
        void consultarUsuario_SQLServer(User user, VehicleOnRequestFinished callback);
    }

    public interface VehicleOnRequestFinished
    {
        void mostrarProgreso(bool mostrar);
        void evaluarUsuario(User usuarioEvaluar,User usuarioRecibido);
        void errorNoReconocido();
    }
}