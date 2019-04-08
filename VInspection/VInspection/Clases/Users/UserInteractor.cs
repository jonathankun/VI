using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UVInspectionModels;
using Android.Util;

namespace VInspection.Clases.Users
{
    public class UserInteractor : UserInteractorInterface
    {
        private Context mContext;

        public const string TAG = "DEBUG LOG";

        public UserInteractor(Context context)
        {
            mContext = context;
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
                Log.Info("obtenerTablaUsuarios_SQLite", ex.Message);
                return null;
            }
        }
    }

    public interface UserInteractorInterface
    {
        List<User> obtenerTablaUsuarios_SQLite();
    }
}