using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;

using System;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Start
{
    [Activity(Label = "V Inspection", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Normal.Inicio")]
    public class StartActivityView : BaseActivity, StartActivityViewInterface, DataConfirmationListener
    {
        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        User user;

        private const int CODE_DIALOG_REFRESH = 1;
        private const int CODE_DIALOG_RETRY = 2;

        public StartInteractor mInteractor;
        public StartViewModel mViewModel { get; private set; }

        bool bandera1 = false, bandera2 = false, bandera3 = false;
        bool descarga1 = false, descarga2 = false, descarga3 = false;

        /**int time1 = 30000;
        int time2 = 10000;
        AsyncTimer timer1, timer2;*/

        public void setViewModel(StartViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Log.Info(TAG, "\n\nnOnCreate\n\n");

            SetContentView(Resource.Layout.start_activity);

            mInteractor = new StartInteractor(ApplicationContext);
            mViewModel = new StartViewModel(this, mInteractor);            

            user = new User()
            {
                IdUsuario =  mUserPreferences.GetInt("IdUsuario", 0),
                Nombre =     mUserPreferences.GetString("Nombre", String.Empty),
                Cuenta =     mUserPreferences.GetString("Cuenta", String.Empty),
                Contrasena = mUserPreferences.GetString("Contrasena", String.Empty),
                Perfil =     mUserPreferences.GetString("Perfil", String.Empty)
            };

            DescargarValoresNecesarios();
        }

        /**private bool OnFristTimer()
        {
            Log.Info(TAG, "\n\n\n\nSe realizo el primer intento por " + time1 / 1000 + " segundos\n\n\n\n");

            return true;
        }*/

        public void DescargarValoresNecesarios()
        {
            if (UConnection.conectadoWifi() == true)
            {
                /**if (UConnection.conextadoRedInterna() == true)
                {*/
                if (user.Nombre != String.Empty && user.Perfil == PERFIL_DESARROLLADOR)
                {
                    downloadUsers();
                    downloadVehicles();
                    downloadCheckLists();
                }
                else if (user.Nombre != String.Empty && (user.Perfil == PERFIL_SUPERVISOR || user.Perfil == PERFIL_RESPONSABLE))
                {
                    downloadUsers();
                    downloadVehicles();
                    downloadCheckLists();
                }
                else if (user.Nombre != String.Empty && user.Perfil == PERFIL_VIGILANTE)
                {
                    bandera1 = true;
                    descarga1 = true;
                    bandera2 = true;
                    descarga2 = true;
                    downloadCheckLists();
                    downloadVehicles();
                }
                else if (user.Nombre != String.Empty && user.Perfil == PERFIL_CONDUCTOR)
                {
                    bandera1 = true;
                    descarga1 = true;
                    bandera3 = true;
                    descarga3 = true;
                    downloadVehicles();
                }
                else
                {
                    DeleteUserPreferences();
                }
                /**}
                else
                {
                    Log.Info(TAG, "No está conectado a " + UServices.Wifi_Name);
                    DataConfirmationDialog frag = DataConfirmationDialog
                    .NewInstance("No estas conectado a '"+ UServices.Wifi_Name +"' ¿Deseas continuar?", "Continuar", "Finalizar", this);
                    frag.Show(FragmentManager, "TAG: continuar fuera de la red '" + UServices.Wifi_Name + "'");
                }*/
            }
            else
            {
                Log.Info(TAG, "No hay conexión Wifi");
                DataConfirmationDialog frag = DataConfirmationDialog
                    .NewInstance(CODE_DIALOG_REFRESH, "No hay conexión Wifi ¿Deseas continuar sin conexión?", "Continuar", "Finalizar", this);
                frag.Show(FragmentManager, "TAG: continuar sin Wifi");
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

        public void GoToHomeActivity()
        {
            Log.Info(TAG, "Entro en GoToHomeActivity");

            Intent intent = new Intent(this, typeof(Home.HomeActivityView));
            this.StartActivity(intent);
            Finish();
        }

        public void DeleteUserPreferences()
        {
            ISharedPreferencesEditor editUser = mUserPreferences.Edit();
            editUser.PutInt("IdUsuario", 0);
            editUser.PutString("Nombre", String.Empty);
            editUser.PutString("Cuenta", String.Empty);
            editUser.PutString("Contrasena", String.Empty);
            editUser.PutString("Perfil", String.Empty);
            editUser.Apply();
            showLoginScreen();
            Finish();
        }

        public void downloadUsers()
        {
            Log.Info(TAG, "Entro en downloadUsers");
            mViewModel.ObtenerListaUsuariosBDE();
        }

        public void downloadUsers_Finished()
        {
            descarga1 = true;
            if (descarga2 == true && descarga3 == true)
            {
                if (bandera2 == true && bandera3 == true)
                {
                    Log.Info(TAG, "Descarga de Usuarios Finalizada");
                    GoToHomeActivity();
                }
                else
                {
                    Log.Info(TAG, "No se pudo descarga los Usuarios");
                    DeleteUserPreferences();
                }
            }
        }

        public void downloadVehicles()
        {
            Log.Info(TAG, "Entro en downloadVehicles");
            mViewModel.ObtenerListaVehiculosBDE();
        }

        public void downloadVehicles_Finished()
        {
            descarga2 = true;
            if(descarga1 == true && descarga3 == true)
            {
                if (bandera1 == true && bandera3 == true)
                {
                    Log.Info(TAG, "Descarga de Usuarios Vehiculos");
                    GoToHomeActivity();
                }
                else
                {
                    Log.Info(TAG, "No se pudo descarga los Vehiculos");
                    DeleteUserPreferences();
                }
            }
        }

        public void downloadCheckLists()
        {
            Log.Info(TAG, "Entro en downloadCheckLists");
            mViewModel.ObtenerListaCheckListsBDE();
        }

        public void downloadCheckLists_Finished()
        {
            descarga3 = true;
            if (descarga1 == true && descarga2 == true)
            {
                if (bandera1 == true && bandera2 == true)
                {
                    Log.Info(TAG, "Descarga de Usuarios Pre-Usos");
                    GoToHomeActivity();
                }
                else
                {
                    Log.Info(TAG, "No se pudo descarga los Pre-Usos");
                    DeleteUserPreferences();
                }
            }
        }

        public void raiseFlag1(bool flag)
        {
            Log.Info(TAG, "Se levanto la Bandera 1 es: " + flag.ToString());
            bandera1 = flag;
        }

        public void raiseFlag2(bool flag)
        {
            Log.Info(TAG, "Se levanto la Bandera 2 es: " + flag.ToString());
            bandera2 = flag;
        }

        public void raiseFlag3(bool flag)
        {
            Log.Info(TAG, "Se levanto la Bandera 3 es: " + flag.ToString());
            bandera3 = flag;
        }

        public void OnDataSetAccept(int requestCode)
        {
            Intent intent;

            switch (requestCode)
            {
                case CODE_DIALOG_REFRESH:
                    bandera1 = true;
                    bandera2 = true;
                    bandera3 = true;

                    intent = new Intent(this, typeof(Home.HomeActivityView));
                    this.StartActivity(intent);
                    Finish();
                    break;

                case CODE_DIALOG_RETRY:
                    //DescargarValoresNecesarios();
                    intent = new Intent(this, typeof(StartActivityView));
                    this.StartActivity(intent);
                    Finish();
                    break;
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            switch (requestCode)
            {
                case CODE_DIALOG_REFRESH:
                    Finish();
                    break;
                case CODE_DIALOG_RETRY:
                    Finish();
                    break;
            }
        }
    }

    public interface StartActivityViewInterface
    {
        void setViewModel(StartViewModel viewModel);
        void downloadUsers();
        void downloadUsers_Finished();
        void downloadVehicles();
        void downloadVehicles_Finished();
        void downloadCheckLists();
        void downloadCheckLists_Finished();

        void raiseFlag1(bool flag);
        void raiseFlag2(bool flag);
        void raiseFlag3(bool flag);
    }
}