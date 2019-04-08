using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using static VInspection.Clases.Utils.UVInspectionModels;
using VInspection.Clases.Utils;

namespace VInspection.Clases.Login
{
    [Activity(Label = "LoginActivityView", Theme = "@style/AppTheme.Normal")]
    public class LoginActivityView : AppCompatActivity, LoginActivityViewInterface, DataConfirmationListener
    {
        public const string TAG = "DEBUG LOG";

        public const int REQUEST_CODE_GOOGLE_PLAY_SERVICES = 1;
        public const int REQUEST_CODE_AUTHORIZATION = 2;
        public const int REQUEST_CODE_NEW_CHOOSE_ACCOUNT = 3;
        public const int REQUEST_CODE_ACCOUNTS_PERMISSION = 4;

        private const int CODE_DIALOG_CONNECT = 1;

        public const string EXTRA_NAV_DRAWER_ID = "NAV_DRAWER_ID";

        public LoginInteractor mInteractor;
        public LoginViewModel mViewModel { get; private set; }

        private View mLoadingIndicator;
        private View mLoginContainer;

        private EditText mUserName;
        private EditText mPassword;

        private Button mLoginButton;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        public void setViewModel(LoginViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.login_activity);

            mInteractor = new LoginInteractor(ApplicationContext);
            mViewModel = new LoginViewModel(this, mInteractor);

            // Setup UI
            mLoadingIndicator = FindViewById(Resource.Id.login_progress);

            mLoginContainer = FindViewById(Resource.Id.login_container);

            mUserName = FindViewById<EditText>(Resource.Id.txtUserName);
            mPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            mLoginButton = FindViewById<Button>(Resource.Id.btnLogin);
            mLoginButton.Click += mLoginButton_Click;
        }

        void mLoginButton_Click(object sender, EventArgs e)
        {
            User user = new User()
            {
                Cuenta = mUserName.Text.Trim(),
                Contrasena = mPassword.Text.Trim()
            };

            Log.Info(TAG, "Cuenta: " + user.Cuenta + "; Contrasena: " + user.Contrasena);

            mViewModel.ConsultarUsuarioBDE(user);
        }

        public void userQueryResultOk(User user)
        {
            ISharedPreferencesEditor edit = mUserPreferences.Edit();
            edit.PutInt("IdUsuario", user.IdUsuario);
            edit.PutString("Nombre", user.Nombre);
            edit.PutString("Cuenta", user.Cuenta);
            edit.PutString("Contrasena", user.Contrasena);
            edit.PutString("Area", user.Area);
            edit.PutString("Perfil", user.Perfil);
            edit.Apply();

            showStartScreen();
            Finish();
        }

        public void wrongPassword()
        {
            Log.Info(TAG, "Contraseña incorrecta");
            Toast.MakeText(this, "Contraseña incorrecta", ToastLength.Long).Show();
        }

        public void unknowUser()
        {
            Log.Info(TAG, "Usuario no reconocido");
            Toast.MakeText(this, "Usuario no reconocido", ToastLength.Long).Show();
        }

        public void showProgress(bool show)
        {
            mLoginContainer.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            mLoadingIndicator.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        public void showStartScreen()
        {
            Log.Info(TAG, "Entro a showStartScreen");
            Intent intent = new Intent(this, typeof(Start.StartActivityView));
            StartActivity(intent);
        }

        public void showDialog(string message, string okText, string cancelText)
        {
            DataConfirmationDialog frag = DataConfirmationDialog.NewInstance(CODE_DIALOG_CONNECT, message, okText, cancelText, this);
            frag.Show(FragmentManager, message);
        }

        public void OnDataSetAccept(int requestCode)
        {
            if (requestCode == CODE_DIALOG_CONNECT)
            {
                //Sin Accion
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            if (requestCode == CODE_DIALOG_CONNECT)
            {
                //Sin Accion
            }
        }
    }


    public interface LoginActivityViewInterface
    {
        void setViewModel(LoginViewModel viewModel);
        void userQueryResultOk(User user);
        void wrongPassword();
        void unknowUser();
        void showProgress(bool show);
        void showDialog(string message, string okText, string cancelText);

    }
}