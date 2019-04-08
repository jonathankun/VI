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

namespace VInspection.Clases.Users
{
    public class UserViewModel : UserViewModelInterface
    {
        public const string TAG = "DEBUG LOG";

        private UserActivityViewInterface mView;
        private UserInteractor mInteractor;

        public UserViewModel(UserActivityViewInterface view,
                             UserInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;
        }

        public List<User> ObtenerListaUsuariosBDI()
        {
            List<User> lista = mInteractor.obtenerTablaUsuarios_SQLite();
            return lista;
        }
    }

    public interface UserViewModelInterface
    {
        List<User> ObtenerListaUsuariosBDI();
    }
}