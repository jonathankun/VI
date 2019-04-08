using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using static VInspection.Clases.Users.UserActivityView;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.UsersEditor
{
    [Activity(Label = "Usuario", Icon = "@drawable/icon", Theme = "@style/AppTheme.Normal")]
    public class UserEditorActivityView : BaseActivity, UserEditorActivityViewInterface
    {
        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);

        User user;
        EditText mNombre, mArea, mCuenta, mContrasena;
        TableRow mFilaPerfil;
        Spinner mPerfil;
        ArrayAdapter adapter;

        Button mAccept, mCancel;
        int Bandera_Edicion;

        View mTablaEdicion;
        ProgressBar mProgreso;

        private int mUserId;
        private int mFunction;

        bool mResultFalg = false;

        public const string BANDERA_RESPUESTA = "Bandera";
        public static Result RESULT_OK;

        Context context = Application.Context;

        public UserEditorInteractor mInteractor;

        public UserEditorViewModel mViewModel { get; private set; }

        public void setViewModel(UserEditorViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.user_editor_activity);

            mInteractor = new UserEditorInteractor(ApplicationContext);
            mViewModel = new UserEditorViewModel(this, mInteractor);

            user = new User()
            {
                IdUsuario = mUserPreferences.GetInt("IdUsuario", 0),
                Nombre = mUserPreferences.GetString("Nombre", String.Empty),
                Cuenta = mUserPreferences.GetString("Cuenta", String.Empty),
                Contrasena = mUserPreferences.GetString("Contrasena", String.Empty),
                Area = mUserPreferences.GetString("Area", String.Empty),
                Perfil = mUserPreferences.GetString("Perfil", String.Empty)
            };

            mUserId = Intent.GetIntExtra(EXTRA_USER_ID, -1);
            Log.Info(TAG, "El ID es: " + mUserId);
            mFunction = Intent.GetIntExtra(EXTRA_FUNCTION, -1);
            Log.Info(TAG, "La Funcion es: " + mFunction);

            mNombre = FindViewById<EditText>(Resource.Id.ET_UT_Item01);
            mArea = FindViewById<EditText>(Resource.Id.ET_UT_Item02);
            mCuenta = FindViewById<EditText>(Resource.Id.ET_UT_Item03);
            mContrasena = FindViewById<EditText>(Resource.Id.ET_UT_Item04);
            mFilaPerfil = FindViewById<TableRow>(Resource.Id.UT_SpinnerFile);
            mPerfil = FindViewById <Spinner> (Resource.Id.UT_S_Item05);

            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.profiles, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            mPerfil.Adapter = adapter;

            mTablaEdicion = FindViewById<View>(Resource.Id.tabla_edicion);
            mProgreso = FindViewById<ProgressBar>(Resource.Id.pb_progreso);

            mAccept = FindViewById<Button>(Resource.Id.b_accept);
            mCancel = FindViewById<Button>(Resource.Id.b_cancel);

            mAccept.Text = "Editar";

            mAccept.Click += (o, e) =>
            {
                if (Bandera_Edicion == 1)
                {
                    user.Nombre = mNombre.Text;
                    user.Area = mArea.Text;
                    user.Cuenta = mCuenta.Text;
                    user.Contrasena = mContrasena.Text;

                    ISharedPreferencesEditor edit = mUserPreferences.Edit();
                    edit.PutInt("IdUsuario", user.IdUsuario);
                    edit.PutString("Nombre", user.Nombre);
                    edit.PutString("Cuenta", user.Cuenta);
                    edit.PutString("Contrasena", user.Contrasena);
                    edit.PutString("Perfil", user.Perfil);
                    edit.PutString("Area", user.Area);
                    edit.Apply();

                    mViewModel.ActualizarBDE(user.IdUsuario, user);
                }
                else
                {
                    prepearEdition(1);
                    mAccept.Text = "Actualizar";
                }
            };

            mCancel.Click += (o, e) =>
            {
                Finish();
            };

            prepearEdition(mFunction);

            prepearData(mUserId);
        }

        public override void Finish()
        {
            if (mUserId == -1)
            {
                Intent intent = new Intent(this, typeof(Home.HomeActivityView));
                this.StartActivity(intent);
            }
            else
            {
                if (mResultFalg == true)
                {
                    Log.Info(TAG, "Resultado OK");
                    Intent intent = new Intent();
                    intent.PutExtra(BANDERA_RESPUESTA, 1);
                    SetResult(RESULT_OK, intent);
                }
                else
                {
                    Log.Info(TAG, "Resultado Cancelado");
                    //SetResult(RESULT_CANCELLED, null);
                    Intent intent = new Intent();
                    intent.PutExtra(BANDERA_RESPUESTA, 0);
                    SetResult(RESULT_OK, intent);
                }
            }

            base.Finish();
        }

        public void prepearEdition(int function)
        {
            switch (function)
            {
                case 1:
                    mNombre.Enabled = true;
                    mArea.Enabled = true;
                    mCuenta.Enabled = true;
                    mContrasena.Enabled = true;
                    mPerfil.Enabled = true;

                    mAccept.Text = "Guardar";
                    mCancel.Text = "Atras";

                    Bandera_Edicion = 1;
                    break;
                case 2:
                    mNombre.Enabled = false;
                    mArea.Enabled = false;
                    mCuenta.Enabled = false;
                    mContrasena.Enabled = false;
                    mPerfil.Enabled = false;

                    mAccept.Text = "Editar";
                    mCancel.Text = "Atras";

                    Bandera_Edicion = 0;
                    break;
            }
        }

        public void prepearData(int id)
        {
            if (id == -1)
            {
                mNombre.Text = user.Nombre;
                mArea.Text = user.Area;
                mCuenta.Text = user.Cuenta;
                mContrasena.Text = user.Contrasena;
                mFilaPerfil.Visibility = ViewStates.Gone;
            }
            else if (id == 0)
            {
                mNombre.Text = "";
                mArea.Text = "";
                mCuenta.Text = "";
                mContrasena.Text = "";
                mFilaPerfil.Visibility = ViewStates.Visible;
            }
            else if (id > 0)
            {
                mFilaPerfil.Visibility = ViewStates.Visible;
                GetUserAndShow(id);
            }
        }

        public void prepearObteinedData(User user)
        {
            mNombre.Text = user.Nombre;
            mArea.Text = user.Area;
            mCuenta.Text = user.Cuenta;
            mContrasena.Text = user.Contrasena;
            mPerfil.SetSelection(adapter.GetPosition(user.Perfil));
        }

        public void GetUserAndShow(int id)
        {
            mViewModel.ObtenerUserBDE(id);
        }

        public void showProgress(bool mostrar)
        {
            mTablaEdicion.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            mProgreso.Visibility = mostrar ? ViewStates.Visible : ViewStates.Gone;
        }

        public void ShowDialog(string message, string okText, string cancelText)
        {
            throw new NotImplementedException();
        }

        public void finishActivity()
        {
            Log.Info(TAG, "Ingreso a finishActivity");
            Finish();
        }

        public void raiseResultFlag(bool bandera)
        {
            mResultFalg = bandera;
        }
    }

    public interface UserEditorActivityViewInterface
    {
        void setViewModel(UserEditorViewModel viewModel);
        void prepearEdition(int function);
        void prepearData(int id);
        void prepearObteinedData(User user);
        void showProgress(bool mostrar);
        void ShowDialog(string message, string okText, string cancelText);
        void finishActivity();

        void raiseResultFlag(bool bandera);
    }
}