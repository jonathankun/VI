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

namespace VInspection.Clases.UsersEditor
{
    public class UserEditorViewModel : UserEditorViewModelInterface, UserEditorOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private UserEditorActivityViewInterface mView;
        private UserEditorInteractor mInteractor;

        public UserEditorViewModel(UserEditorActivityViewInterface view,
                             UserEditorInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;

        }
        public void ActualizarBDE(int id, User usuario)
        {
            usuario.Buscador = usuario.Nombre + "@" + usuario.Area + "@" + usuario.Perfil;

            mInteractor.actualizarUsuario_SQLServer(this, usuario);
        }

        public void ObtenerUserBDE(int id)
        {
            mInteractor.ObtenerUsuario_SQLServer(this, id);
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
            mView.raiseResultFlag(true);
            //mView.finishActivity();
        }

        public void preparDatosUsuarioObtenido(User user)
        {
            mView.prepearObteinedData(user);
        }

        public void finalizarActividad()
        {
            mView.raiseResultFlag(true);
            mView.finishActivity();
        }
    }

    public interface UserEditorViewModelInterface
    {
        void ActualizarBDE(int id, User usuario);
        void ObtenerUserBDE(int id);
    }
}