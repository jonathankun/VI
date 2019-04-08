using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UTime;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace VInspection.Clases.Tables
{
    public class TablesFragmentView : SupportFragment, TablesFragmentViewInterface
    {
        public const string TAG = "DEBUG LOG";
        public const string ARG_PAGE = "PAGE";

        protected int mPage;

        View rootView;

        protected TablesViewModel mViewModel;

        // Contenedor fragmento
        protected Activity actividadHost;
        private TablesFragmentCallbacks mFragmentCallbacks;

        protected List<TextInputLayout> mInputsLayouts;
        protected NestedScrollView mScrollContainer;

        AutoCompleteTextView N_Placa;
        public static string[] LISTA_PLACAS;
        TextView Modelo, Kilometraje;
        TableRow FilaAdvertencia, FilaAlarma;
        CheckBox CheckAdvertencia;
        TextView Nombre_Conductor;
        TextView Fecha_SOAT, Fecha_RevTecnica;
        EditText N_Fecha_SOAT, N_Fecha_RevTecnica;
        TableRow Fila_SOAT, Fila_RevTecnica;
        View Estado_Fecha_SOAT, Estado_Fecha_RevTecnica;
        EditText Estado_SOAT, Estado_RevTecnica;

        public const int DIALOG_ID_FECHASOAT = 0;
        public const int DIALOG_ID_FECHAREV = 1;

        private ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("UserInfo", FileCreationMode.Private);
        private ISharedPreferences mDocumentPreferences = Application.Context.GetSharedPreferences("DocumentInfo", FileCreationMode.Private);

        public TablesFragmentView()
        {
            // Required empty public constructor
        }

        public static TablesFragmentView NewInstance(int position)
        {   
            TablesFragmentView tablesFragmentView = new TablesFragmentView();

            Bundle args = new Bundle();
            args.PutInt(ARG_PAGE, position);
            tablesFragmentView.Arguments = args;
            return tablesFragmentView;
        }

        public void setViewModel(TablesViewModel viewModel)
        {
            mViewModel = viewModel;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetHasOptionsMenu(true);

            Bundle args = Arguments;
            if (args != null)
            {
                mPage = args.GetInt(ARG_PAGE);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(UServices.getLayoutId(mPage), container, false);

            mScrollContainer = rootView.FindViewById<NestedScrollView>(Resource.Id.scroll_container);

            if (mPage == 0)
            {
                N_Placa = rootView.FindViewById<AutoCompleteTextView>(Resource.Id.ET_T0_Item01);
                LISTA_PLACAS = mViewModel.obtenerListaPlacas();
                ArrayAdapter adaptador = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleDropDownItem1Line, LISTA_PLACAS);
                N_Placa.Adapter = adaptador;

                Modelo = rootView.FindViewById<TextView>(Resource.Id.ET_T0_Item02);

                N_Placa.TextChanged += (o, e) =>
                {
                    String modeloFila = mViewModel.obtenerModeloDePlaca(N_Placa.Text.ToString());
                    Modelo.Text = modeloFila;
                };

                Kilometraje = rootView.FindViewById<TextView>(Resource.Id.ET_T0_Item03);
                FilaAdvertencia = rootView.FindViewById<TableRow>(Resource.Id.T0_fila_advertencia);//Mantenimiento se encuentra proximo
                FilaAlarma = rootView.FindViewById<TableRow>(Resource.Id.T0_fila_alarma); //alarma mas de 5000
                CheckAdvertencia = rootView.FindViewById<CheckBox> (Resource.Id.CB_T0_Item04);

                Kilometraje.TextChanged += (o, e) =>
                {
                    if (Kilometraje.Length() > 0)
                    {
                        int Resultado = mViewModel.evaluarKilometraje(N_Placa.Text.ToString(), Kilometraje.Text.ToString());
                        if (Resultado == 2)
                        {
                            Log.Info(TAG, "resultado: " + Resultado);
                            FilaAlarma.Visibility = ViewStates.Visible;
                            FilaAdvertencia.Visibility = ViewStates.Gone;
                        }
                        else if (Resultado == 1)
                        {
                            Log.Info(TAG, "resultado: " + Resultado);
                            FilaAlarma.Visibility = ViewStates.Gone;
                            FilaAdvertencia.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            Log.Info(TAG, "resultado: " + Resultado);
                            FilaAlarma.Visibility = ViewStates.Gone;
                            FilaAdvertencia.Visibility = ViewStates.Gone;
                        }
                    }
                };

                Nombre_Conductor = rootView.FindViewById<TextView>(Resource.Id.ET_T0_Item07);
                String Conductor = mUserPreferences.GetString("Nombre", String.Empty);
                Nombre_Conductor.Text = Conductor;
                Nombre_Conductor.Enabled = false;
            }

            if(mPage == 4)
            {
                Fecha_SOAT = rootView.FindViewById<TextView>(Resource.Id.TV_T4_Item01);
                Fecha_RevTecnica = rootView.FindViewById<TextView>(Resource.Id.TV_T4_Item02);

                Estado_Fecha_SOAT = rootView.FindViewById(Resource.Id.color_estado_doc_SOAT);
                Estado_SOAT = rootView.FindViewById<EditText>(Resource.Id.ET_T4_Item01);
                Fila_SOAT = rootView.FindViewById<TableRow>(Resource.Id.TR_T4_Row_SOAT);
                N_Fecha_SOAT = rootView.FindViewById<EditText>(Resource.Id.ET_T4_Item02);
                N_Fecha_SOAT.Click += (o, e) =>
                {
                    mFragmentCallbacks.showDateDialog(DIALOG_ID_FECHASOAT);
                };

                Estado_Fecha_RevTecnica = rootView.FindViewById(Resource.Id.color_estado_doc_RevTecnica);
                Estado_RevTecnica = rootView.FindViewById<EditText>(Resource.Id.ET_T4_Item03);
                Fila_RevTecnica = rootView.FindViewById<TableRow>(Resource.Id.TR_T4_Row_RevTecnica);
                N_Fecha_RevTecnica = rootView.FindViewById<EditText>(Resource.Id.ET_T4_Item04);
                N_Fecha_RevTecnica.Click += (o, e) =>
                {
                    mFragmentCallbacks.showDateDialog(DIALOG_ID_FECHAREV);
                };

                string F_SOAT = DateToString(DateTime.Parse(mDocumentPreferences.GetString("FechaSOAT", String.Empty)));
                string F_RevTecnica = DateToString(DateTime.Parse(mDocumentPreferences.GetString("FechaRevTecnica", String.Empty)));

                Fecha_SOAT.Text = F_SOAT;
                Fecha_RevTecnica.Text = F_RevTecnica;

                evaluar_Fechas(F_SOAT, F_RevTecnica);
            }

            RetainInstance = true;

            return rootView;
        }

        public void setdateSOAT(DateTime fecha)
        {
            N_Fecha_SOAT.Text = DateToString(fecha);
        }
        public void setdateRevTecnica(DateTime fecha)
        {
            N_Fecha_RevTecnica.Text = DateToString(fecha);
        }

        public override void OnResume()
        {
            base.OnResume();
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            actividadHost = activity;
            mFragmentCallbacks = (TablesFragmentCallbacks)activity;
            /*if (activity instanceof TablesFragmentCallbacks) 
            {
                actividadHost = activity;
                mCallbacks = (TablesFragmentCallbacks)activity;
            }
            else
            {
                throw new RuntimeException("La actividad "
                        + activity.getClass().getSimpleName() + " no ha implementado "
                        + TableFragmentCallbacks.class.getSimpleName());
            }*/
        }
        
        public override void OnDetach()
        {
            base.OnDetach();
            actividadHost = null;
        }

        public void validateDataForNext()
        {
            bool guardardo = saveTableData(TablesViewModel.VALIDATE_OPTION_NEXT);
            mViewModel.validarOpcion(mPage, guardardo, TablesViewModel.VALIDATE_OPTION_NEXT);
        }

        public void validateDataForPrev()
        {
            bool guardardo = saveTableData(TablesViewModel.VALIDATE_OPTION_PREVIOUS);
            mViewModel.validarOpcion(mPage, guardardo, TablesViewModel.VALIDATE_OPTION_PREVIOUS);
        }

        public bool saveTableData(int option)
        {
            List<string> TableData = new List<string>();
            int FlagData = 0;

            View input;
            EditText ET_input;
            string Str_input;
            CheckBox ACB_input;

            bool guardado = true;

            switch (mPage)
            {
                case 0:

                    for (int i = 0; i < UServices.getNoData(mPage); i++)
                    {
                        if (i != 3)
                        {
                            input = rootView.FindViewById(UServices.getInputs(mPage, i));
                            ET_input = (EditText)input;
                            Str_input = ET_input.Text.ToString();
                            TableData.Add(Str_input);
                            if (i == 2)
                            {
                                FlagData = mViewModel.evaluarKilometraje(TableData[0], Str_input);
                            }
                        }
                        else
                        {
                            input = rootView.FindViewById(UServices.getInputs(mPage, i));
                            ACB_input = (CheckBox)input;
                            Str_input = "0";
                            if (ACB_input.Checked) { Str_input = "1"; }
                            TableData.Add(Str_input);
                        }
                    }
                    guardado = mViewModel.guardarPreferenciasCheckList(mPage, TableData, FlagData);
                    break;

                case 1:
                    for (int i = 0; i < UServices.getNoData(mPage); i++)
                    {
                        input = rootView.FindViewById(UServices.getInputs(mPage, i));
                        ACB_input = (CheckBox)input;
                        Str_input = "0";
                        if (ACB_input.Checked) { Str_input = "1"; }
                        TableData.Add(Str_input);
                    }
                    FlagData = mViewModel.evaluarItems(TableData);
                    mViewModel.guardarPreferenciasCheckList(mPage, TableData, FlagData);
                    break;

                case 2:
                    input = rootView.FindViewById(UServices.getInputs(mPage, 0));
                    ET_input = (EditText)input;
                    Str_input = ET_input.Text.ToString();
                    TableData.Add(Str_input);
                    for (int i = 1; i < (UServices.getNoData(mPage) + 1) / 2; i++)
                    {
                        input = rootView.FindViewById(UServices.getInputs(mPage, 2 * i - 1));
                        ET_input = (EditText)input;
                        Str_input = ET_input.Text.ToString();
                        if (Str_input.Length != 0)
                        {
                            TableData.Add(Str_input);
                            input = rootView.FindViewById(UServices.getInputs(mPage, 2 * i));
                            Spinner S_input = (Spinner)input;
                            Str_input = S_input.SelectedItemPosition.ToString();
                            TableData.Add(Str_input);
                        }
                        else
                        {
                            Str_input = "";
                            TableData.Add(Str_input);
                            Str_input = "0";
                            TableData.Add(Str_input);
                        }
                    }
                    FlagData = mViewModel.evaluarComentarios(TableData);
                    mViewModel.guardarPreferenciasCheckList(mPage, TableData, FlagData);
                    break;

                case 3:
                    for (int i = 0; i < UServices.getNoData(mPage) - 1; i++)
                    {
                        input = rootView.FindViewById(UServices.getInputs(mPage, i));
                        ACB_input = (CheckBox)input;
                        Str_input = "0";
                        if (ACB_input.Checked) { Str_input = "1"; }
                        TableData.Add(Str_input);
                    }
                    input = rootView.FindViewById(UServices.getInputs(mPage, UServices.getNoData(mPage) - 1));
                    ET_input = (EditText)input;
                    Str_input = ET_input.Text.ToString();
                    TableData.Add(Str_input);

                    FlagData = mViewModel.evaluarBotiquin(TableData);
                    mViewModel.guardarPreferenciasCheckList(mPage, TableData, FlagData);
                    break;

                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        input = rootView.FindViewById(UServices.getInputs(mPage, i));
                        ET_input = (EditText)input;
                        Str_input = ET_input.Text.ToString();
                        TableData.Add(Str_input);
                    }
                    for (int i = 4; i < UServices.getNoData(mPage); i++)
                    {
                        input = rootView.FindViewById(UServices.getInputs(mPage, i));
                        ACB_input = (CheckBox)input;
                        Str_input = "0";
                        if (ACB_input.Checked) { Str_input = "1"; }
                        TableData.Add(Str_input);
                    }

                    FlagData = mViewModel.evaluarDocumentacion(TableData);
                    mViewModel.guardarPreferenciasCheckList(mPage, TableData, FlagData);
                    break;

                default:
                    break;
            }

            if (guardado == false && option == TablesViewModel.VALIDATE_OPTION_NEXT)
            {
                mFragmentCallbacks.showDialog(TablesActivityView.CODE_DIALOG_ALERT, "Debes completar todos los items", "Ok", "False");
                return guardado;
            }
            else
            {
                return guardado;
            }
        }

        public void showInputLayoutError(int i)
        {
            mInputsLayouts[i].Error = GetString(Resource.String.error_dato_invalido);
            mScrollContainer.ScrollTo(0, (int)mInputsLayouts[i].GetY());
        }

        public void goNext ()
        {
            mFragmentCallbacks.goToNex();
        }

        public void goPrevious()
        {
            mFragmentCallbacks.goToPrev();
        }

        public void showHomeScreen()
        {
            Intent intent = new Intent();
            intent = new Intent(Activity, typeof(Home.HomeActivityView));
            this.StartActivity(intent);
            Activity.Finish();
        }
        
        public void showDialog(int requsetCode, string message, string okText, string cancelText)
        {
            mFragmentCallbacks.showDialog(requsetCode, message, okText, cancelText);
        }

        public void evaluar_Fechas(String F_SOAT, String F_RevTecnica)
        {
            String Fecha_Actual = obtenerFecha();

            int DFS = compararFechas(F_SOAT, Fecha_Actual);

            if (DFS > 0)
            {
                Estado_Fecha_SOAT.SetBackgroundResource(Resource.Color.color_estado_1);
                Estado_SOAT.Text = "1";
                Fila_SOAT.Visibility = ViewStates.Gone;
            }
            else if (DFS == 0)
            {
                Estado_Fecha_SOAT.SetBackgroundResource(Resource.Color.color_estado_2);
                Estado_SOAT.Text = "0";
                Fila_SOAT.Visibility = ViewStates.Visible;
            }
            else
            {
                Estado_Fecha_SOAT.SetBackgroundResource(Resource.Color.color_estado_3);
                Estado_SOAT.Text = "0";
                Fila_SOAT.Visibility = ViewStates.Visible;
            }

            int DFR = UTime.compararFechas(F_RevTecnica, Fecha_Actual);

            if (DFR > 0)
            {
                Estado_Fecha_RevTecnica.SetBackgroundResource(Resource.Color.color_estado_1);
                Estado_RevTecnica.Text = "1";
                Fila_RevTecnica.Visibility = ViewStates.Gone;
            }
            else if (DFR == 0)
            {
                Estado_Fecha_RevTecnica.SetBackgroundResource(Resource.Color.color_estado_2);
                Estado_RevTecnica.Text = "0";
                Fila_RevTecnica.Visibility = ViewStates.Visible;
            }
            else
            {
                Estado_Fecha_RevTecnica.SetBackgroundResource(Resource.Color.color_estado_3);
                Estado_RevTecnica.Text = "0";
                Fila_RevTecnica.Visibility = ViewStates.Visible;
            }
        }

        public void showProgress(bool show)
        {
            mFragmentCallbacks.showProgress(show);
        }

        public void showSentCheckListAlert()
        {
            mFragmentCallbacks.raiseResultFlag(true);
            Toast.MakeText(Activity, "Pre-Uso Enviado", ToastLength.Long).Show();
        }

        public void finishActivity()
        {
            mFragmentCallbacks.finishActivity();
        }
    }

    public interface TablesFragmentViewInterface
    {
        void setViewModel(TablesViewModel viewModel);
        void showInputLayoutError(int i);
        void goNext();
        void goPrevious();
        void showHomeScreen();
        void showDialog(int requestCode, string message, string okText, string cancelText);
        void showProgress(bool show);
        void showSentCheckListAlert();
        void finishActivity();
    }

    public interface TablesFragmentCallbacks
    {
        void goToNex();
        void goToPrev();
        void showProgress(bool show);
        void showDateDialog(int id);
        void showDialog(int requestCode, string message, string okText, string cancelText);
        Toolbar getToolbar();
        void raiseResultFlag(bool flag);
        void finishActivity();
    }
}