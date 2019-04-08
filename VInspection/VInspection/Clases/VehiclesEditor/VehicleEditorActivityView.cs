using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using static VInspection.Clases.Utils.UVInspectionModels;
using static VInspection.Clases.Vehicles.VehicleActivityView;
using static VInspection.Clases.Utils.UTime;

namespace VInspection.Clases.VehiclesEditor
{
    [Activity(Label = "Vehiculo", Theme = "@style/AppTheme.Normal")]
    public class VehicleEditorActivityView : BaseActivity, VehicleEditorActivityViewInteface, DataConfirmationListener
    {
        EditText mPlaca, mMarca, mModelo, mArea, mEncargado, mUMantto, mFUMantto, mKilometraje, mCentral;
        AutoCompleteTextView mResponsable;
        public static string[] LISTA_USUARIOS;
        View mEstado;
        Button mAccept, mCancel;
        int Bandera_Edicion;

        View mTablaEdicion;
        ProgressBar mProgreso;

        private int mVehicleId;
        private int mFunction;

        public const string BANDERA_RESPUESTA = "Bandera";
        public static Result RESULT_OK;
        public static Result RESULT_CANCELLED;

        private const int CODE_DIALOG_REFRESH_OR_DELETE = 1;

        bool mResultFalg = false;

        Context context = Application.Context;

        public VehicleEditorInteractor mInteractor;

        public VehicleEditorViewModel mViewModel { get; private set; }

        public void setViewModel(VehicleEditorViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.vehicle_editor_activity);

            mInteractor = new VehicleEditorInteractor(ApplicationContext);
            mViewModel = new VehicleEditorViewModel(this, mInteractor);

            mVehicleId = Intent.GetIntExtra(EXTRA_VEHICLE_ID, -1);
            Log.Info(TAG, "El ID es: " + mVehicleId);
            mFunction = Intent.GetIntExtra(EXTRA_FUNCTION, -1);
            Log.Info(TAG, "La Funcion es: " + mFunction);

            mPlaca = FindViewById<EditText>(Resource.Id.ET_Placa);
            mMarca = FindViewById<EditText>(Resource.Id.ET_Marca);
            mModelo = FindViewById<EditText>(Resource.Id.ET_Modelo);
            mResponsable = FindViewById<AutoCompleteTextView>(Resource.Id.ET_Responsable);
            LISTA_USUARIOS = mViewModel.obtenerListaUsuariosBDI();
            ArrayAdapter adaptador = new ArrayAdapter<string>(BaseContext, Android.Resource.Layout.SimpleDropDownItem1Line, LISTA_USUARIOS);
            mResponsable.Adapter = adaptador;
            mArea = FindViewById<EditText>(Resource.Id.ET_Area);
            mEncargado = FindViewById<EditText>(Resource.Id.ET_Encargado);
            mUMantto = FindViewById<EditText>(Resource.Id.ET_UMantto);
            mFUMantto = FindViewById<EditText>(Resource.Id.ET_FUMantto);
            mKilometraje = FindViewById<EditText>(Resource.Id.ET_Kilometraje);
            mCentral = FindViewById<EditText>(Resource.Id.ET_Central);
            mEstado = FindViewById<View>(Resource.Id.V_Estado);

            mTablaEdicion = FindViewById<View>(Resource.Id.tabla_edicion);
            mProgreso = FindViewById<ProgressBar>(Resource.Id.pb_progreso);

            mAccept = FindViewById<Button>(Resource.Id.b_accept);
            mCancel = FindViewById<Button>(Resource.Id.b_cancel);

            mFUMantto.Click += (o, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    mFUMantto.Text = DateToString(time);
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            mAccept.Click += (o, e) =>
            {
                if (Bandera_Edicion == 1)
                {
                    Vehicle vehiculo = new Vehicle();

                    vehiculo.IdVehiculo = mVehicleId;
                    vehiculo.Placa = mPlaca.Text;
                    vehiculo.Marca = mMarca.Text;
                    vehiculo.Modelo = mModelo.Text;
                    vehiculo.Responsable = mResponsable.Text;
                    vehiculo.Area = mArea.Text;
                    vehiculo.Encargado = mEncargado.Text;
                    vehiculo.KUMantto = Convert.ToUInt32(mUMantto.Text);
                    vehiculo.FUMantto = StringToDate(mFUMantto.Text);
                    vehiculo.Kilometraje = Convert.ToUInt32(mKilometraje.Text);
                    vehiculo.Central = mCentral.Text;
                    mViewModel.EvaluarEstadoVehiculoYAgregarBDE(mVehicleId, vehiculo);
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

            prepearData(mVehicleId);
        }

        public override void Finish()
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
            base.Finish();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_delete, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (id)
            {
                case Resource.Id.action_delete:
                    mViewModel.BorrarrVehiculoBDE(mVehicleId);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void prepearEdition(int funcion)
        {
            switch (funcion)
            {
                case 1:
                    mPlaca.Enabled = true;
                    mMarca.Enabled = true;
                    mModelo.Enabled = true;
                    mResponsable.Enabled = true;
                    mArea.Enabled = true;
                    mEncargado.Enabled = true;
                    mUMantto.Enabled = true;
                    mFUMantto.Enabled = true;
                    mKilometraje.Enabled = true;
                    mCentral.Enabled = true;
                    mEstado.Enabled = true;

                    mAccept.Text = "Guardar";
                    mCancel.Text = "Atras";

                    Bandera_Edicion = 1;
                    break;
                case 2:
                    mPlaca.Enabled = false;
                    mMarca.Enabled = false;
                    mModelo.Enabled = false;
                    mResponsable.Enabled = false;
                    mArea.Enabled = false;
                    mEncargado.Enabled = false;
                    mUMantto.Enabled = false;
                    mFUMantto.Enabled = false;
                    mKilometraje.Enabled = false;
                    mCentral.Enabled = false;
                    mEstado.Enabled = false;

                    mAccept.Text = "Editar";
                    mCancel.Text = "Atras";

                    Bandera_Edicion = 0;
                    break;
            }
        }

        public void prepearData(int id)
        {
            if (id != 0)
            {
                Vehicle VIV = GetVehiculo(id);

                mPlaca.Text = VIV.Placa;
                mMarca.Text = VIV.Marca;
                mModelo.Text = VIV.Modelo;
                mEncargado.Text = VIV.Encargado;
                mArea.Text = VIV.Area;
                mResponsable.Text = VIV.Responsable;
                mUMantto.Text = VIV.KUMantto.ToString();
                mFUMantto.Text = DateToString(VIV.FUMantto);
                mKilometraje.Text = VIV.Kilometraje.ToString();
                mCentral.Text = VIV.Central;
                switch (VIV.Estado)
                {
                    case 0:
                        mEstado.SetBackgroundResource(Resource.Color.color_estado_1);
                        break;
                    case 1:
                        mEstado.SetBackgroundResource(Resource.Color.color_estado_2);
                        break;
                    case 2:
                        mEstado.SetBackgroundResource(Resource.Color.color_estado_3);
                        break;
                }
            }
            else
            {
                mPlaca.Text = "";
                mMarca.Text = "";
                mModelo.Text = "";
                mResponsable.Text = "";
                mArea.Text = "";
                mEncargado.Text = "";
                mUMantto.Text = "";
                mFUMantto.Text = "";
                mKilometraje.Text = "";
                mCentral.Text = "";
                mEstado.SetBackgroundResource(Resource.Color.Color1);
            }
        }

        public Vehicle GetVehiculo(int id)
        {
            Vehicle vehiculo = mViewModel.ObtenerVehiculoBDI(id);

            return vehiculo;
        }

        public void showProgress(bool mostrar)
        {
            mTablaEdicion.Visibility = mostrar ? ViewStates.Gone : ViewStates.Visible;
            mProgreso.Visibility = mostrar ? ViewStates.Visible : ViewStates.Gone;
        }

        public void ShowDialog(string message, string okText, string cancelText)
        {
            DataConfirmationDialog frag = DataConfirmationDialog.NewInstance(CODE_DIALOG_REFRESH_OR_DELETE, message, okText, cancelText, this);
            frag.Show(FragmentManager, message);
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


        public void OnDataSetAccept(int requestCode)
        {
            if (requestCode == CODE_DIALOG_REFRESH_OR_DELETE)
            {
                //Sin Accion
            }
        }

        public void OnDataSetCancel(int requestCode)
        {
            if (requestCode == CODE_DIALOG_REFRESH_OR_DELETE)
            {
                //Sin Accion
            }
        }
    }

    public interface VehicleEditorActivityViewInteface
    {
        void setViewModel(VehicleEditorViewModel viewModel);
        void prepearEdition(int function);
        void prepearData(int id);
        void showProgress(bool mostrar);
        void ShowDialog(string message, string okText, string cancelText);
        void finishActivity();

        void raiseResultFlag(bool bandera);
    }
}