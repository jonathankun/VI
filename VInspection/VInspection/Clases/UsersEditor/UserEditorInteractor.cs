using Android.Content;
using Android.Util;
using System;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UServices;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.UsersEditor
{
    public class UserEditorInteractor : UserEditorInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public UserEditorInteractor(Context context)
        {
            mContext = context;
        }

        public async void actualizarUsuario_SQLServer(UserEditorOnRequestFinished callback, User user)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia actualizar Usuario a la BD de SQLServer");

            RestClient<User> restClient = new RestClient<User>();

            try
            {
                var respuesta = await restClient.PutAsync(TIPO_USER, user.IdUsuario, user);
                Log.Info("actualizarUsuario_SQLServer", Convert.ToString(respuesta));
                callback.finalizarActividad();
            }
            catch (Exception ex)
            {
                Log.Error("actualizarUsuario_SQLServer", ex.Message);
            }
        }

        public async void ObtenerUsuario_SQLServer(UserEditorOnRequestFinished callback, int id)
        {
            callback.mostrarProgreso(true);

            Log.Info(TAG, "Se inicia obtener Usuario a la BD de SQLServer");

            RestClient<User> restClient = new RestClient<User>();
            var user = new User();

            try
            {
                user = await restClient.GetAsyncById(TIPO_USER, id);
                Log.Info("ObtenerUsuario_SQLServer", Convert.ToString(user));
                callback.mostrarProgreso(false);
                callback.preparDatosUsuarioObtenido(user);
            }
            catch (Exception ex)
            {
                Log.Error("ObtenerUsuario_SQLServer", ex.Message);
            }
        }
    }

    public interface UserEditorInteractorInterface
    {
        void actualizarUsuario_SQLServer(UserEditorOnRequestFinished callback, User user);
        void ObtenerUsuario_SQLServer(UserEditorOnRequestFinished callback, int id);
    }


    public interface UserEditorOnRequestFinished
    {
        void mostrarProgreso(bool mostrar);
        void preparDatosUsuarioObtenido(User user);
        void finalizarActividad();
    }
}