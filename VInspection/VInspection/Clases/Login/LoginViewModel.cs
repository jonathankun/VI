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

namespace VInspection.Clases.Login
{
    public class LoginViewModel : VehicleViewModelInterface, VehicleOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private LoginActivityViewInterface mView;
        private LoginInteractor mInteractor;

        public LoginViewModel(LoginActivityViewInterface view,
                                LoginInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;
        }

        public void ConsultarUsuarioBDE(User user)
        {
            if (UConnection.conectadoWifi())
            {
                mInteractor.consultarUsuario_SQLServer(user, this);
            }
            else
            {
                mView.showDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
        }

        public void evaluarUsuario(User usuarioEvaluar, User usuarioRecibido)
        {
            if(usuarioEvaluar.Contrasena == usuarioRecibido.Contrasena)
            {
                mView.userQueryResultOk(usuarioRecibido);
            }
            else
            {
                mView.wrongPassword();
            }
        }

        public void errorNoReconocido()
        {
            mView.unknowUser();
        }
    }



    public interface VehicleViewModelInterface
    {
        void ConsultarUsuarioBDE(User user);
    }
}