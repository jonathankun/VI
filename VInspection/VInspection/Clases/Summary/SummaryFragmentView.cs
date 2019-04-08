using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using static VInspection.Clases.Utils.UVInspectionModels;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace VInspection.Clases.Summary
{
    public class SummaryFragmentView : SupportFragment, SummaryFragmentViewInterface
    {
        public const string ARG_PAGE = "PAGE";
        public const string ARG_ID_CHECKLIST = "ID_CHECKLSIT";

        public const string TAG = "DEBUG LOG";

        protected int mPage;
        protected int mIdCheckList;

        View rootView;

        View TableIndicator;
        View Detalle1, Detalle2, Detalle3, Detalle4, Detalle5;
        View Separdor1, Separdor2, Separdor3, Separdor4, Separdor5;
        EditText ComentariosVigilante;

        protected SummaryViewModel mViewModel;

        // Contenedor fragmento
        protected Activity actividadHost;
        private SummaryFragmentCallbacks mFragmentCallbacks;

        protected NestedScrollView mScrollContainer;

        CheckListSummary VICLS = new CheckListSummary();
        Vehicle VIV = new Vehicle();

        public SummaryFragmentView()
        {
            // Required empty public constructor
        }

        public static SummaryFragmentView NewInstance(int position, int idCheckLsit)
        {
            SummaryFragmentView summaryFragmentView = new SummaryFragmentView();

            Bundle args = new Bundle();
            args.PutInt(ARG_PAGE, position);
            args.PutInt(ARG_ID_CHECKLIST, idCheckLsit);
            summaryFragmentView.Arguments = args;

            return summaryFragmentView;
        }

        public void setViewModel(SummaryViewModel viewModel)
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
                mIdCheckList = args.GetInt(ARG_ID_CHECKLIST);
            }
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            rootView = inflater.Inflate(Resource.Layout.tab_summary_fragment, container, false);

            mScrollContainer = rootView.FindViewById<NestedScrollView>(Resource.Id.scroll_container);

            TableIndicator = mScrollContainer.FindViewById(Resource.Id.V_table_indicator);
            Separdor1 = mScrollContainer.FindViewById(Resource.Id.V_separator_1);
            Detalle1 = mScrollContainer.FindViewById(Resource.Id.L_detalle_1);
            Separdor2 = mScrollContainer.FindViewById(Resource.Id.V_separator_2);
            Detalle2 = mScrollContainer.FindViewById(Resource.Id.L_detalle_2);
            Separdor3 = mScrollContainer.FindViewById(Resource.Id.V_separator_3);
            Detalle3 = mScrollContainer.FindViewById(Resource.Id.L_detalle_3);
            Separdor4 = mScrollContainer.FindViewById(Resource.Id.V_separator_4);
            Detalle4 = mScrollContainer.FindViewById(Resource.Id.L_detalle_4);
            Separdor5 = mScrollContainer.FindViewById(Resource.Id.V_separator_5);
            Detalle5 = mScrollContainer.FindViewById(Resource.Id.L_detalle_5);

            VICLS = mViewModel.ObtenerResumenPreUsoBDI(mIdCheckList);
            VIV = mViewModel.ObtenerVehiculoBDI(VICLS.Vehiculo);

            Log.Info(TAG, "El CheckList es: " + VICLS);

            bool bandera = false;
            TextView Texto1, Texto2, Texto3, Texto4;
            string resumen1, resumen2, resumen3, resumen4;

            switch (mPage)
            {
                case 0:
                    if (VICLS.BanderaMantto == 0)
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                    }
                    else
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        bandera = true;
                    }
                    Separdor1.Visibility = ViewStates.Visible;
                    Detalle1.Visibility = ViewStates.Visible;

                    resumen1 =  "El pre - uso vehicular del vehiculo " + VIV.Marca + "-" + VIV.Modelo + " con N° de Placa " + VICLS.Vehiculo +
                                " que ha sido realizado por " + VICLS.Conductor + " tiene el siguiente detalle: \n\n" +
                                "Lugar de origen: " + VICLS.Produccion + "\n" +
                                "Lugar de destino: " + VICLS.Destino + "\n" +
                                "Kilometraje inicial: " + VICLS.Kilometraje;

                    Texto1 = Detalle1.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                    Texto1.Text = resumen1;
                    if (bandera)
                    {
                        Separdor2.Visibility = ViewStates.Visible;
                        Detalle2.Visibility = ViewStates.Visible;
                        Texto2 = Detalle2.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                        Texto2.Text = VICLS.MensajeMantto;
                    }
                    Separdor3.Visibility = ViewStates.Gone;
                    Detalle3.Visibility = ViewStates.Gone;
                    Separdor4.Visibility = ViewStates.Gone;
                    Detalle4.Visibility = ViewStates.Gone;
                    Separdor5.Visibility = ViewStates.Gone;
                    Detalle5.Visibility = ViewStates.Gone;
                    break;
                case 1:
                    if (VICLS.BanderaItems == 0)
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                    }
                    else
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        bandera = true;
                    }
                    if (VICLS.Items1.Trim().Length > 0)
                    {
                        Separdor1.Visibility = ViewStates.Visible;
                        Detalle1.Visibility = ViewStates.Visible;
                        Texto1 = Detalle1.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                        resumen1 = "El vehiculo tiene los siguientes Items en buen estado:\n\n" + VICLS.Items1;
                        Texto1.Text = resumen1;
                    }
                    if (bandera)
                    {
                        Separdor2.Visibility = ViewStates.Visible;
                        Detalle2.Visibility = ViewStates.Visible;
                        Texto2 = Detalle2.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                        resumen2 = "El vehiculo tiene los siguientes Items en mal estado o carece de ellos:\n\n" + VICLS.Items2;
                        Texto2.Text = resumen2;
                    }
                    Separdor3.Visibility = ViewStates.Gone;
                    Detalle3.Visibility = ViewStates.Gone;
                    Separdor4.Visibility = ViewStates.Gone;
                    Detalle4.Visibility = ViewStates.Gone;
                    Separdor5.Visibility = ViewStates.Gone;
                    Detalle5.Visibility = ViewStates.Gone;
                    break;
                case 2:
                    if (VICLS.BanderaComentarios == 0)
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                    }
                    else
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        bandera = true;
                    }
                    Separdor1.Visibility = ViewStates.Visible;
                    Detalle1.Visibility = ViewStates.Visible;
                    Texto1 = Detalle1.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                    resumen1 = "No hay comentarios adicionales";
                    Texto1.Text = resumen1;
                    if (bandera)
                    {
                        if (VICLS.Comentarios1.Trim().Length > 0)
                        {
                            resumen1 = VICLS.Comentarios1;
                            Texto1.Text = resumen1;
                        }
                        if (VICLS.Comentarios2.Trim().Length > 0)
                        {
                            Separdor2.Visibility = ViewStates.Visible;
                            Detalle2.Visibility = ViewStates.Visible;
                            Texto2 = Detalle2.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen2 = VICLS.Comentarios2;
                            Texto2.Text = resumen2;
                        }
                    }
                    Separdor3.Visibility = ViewStates.Gone;
                    Detalle3.Visibility = ViewStates.Gone;
                    Separdor4.Visibility = ViewStates.Gone;
                    Detalle4.Visibility = ViewStates.Gone;
                    Separdor5.Visibility = ViewStates.Gone;
                    Detalle5.Visibility = ViewStates.Gone;
                    break;
                case 3:
                    if (VICLS.BanderaBotiquin == 0)
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                    }
                    else
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        bandera = true;
                    }
                    if (VICLS.Botiquin1.Trim().Length > 0)
                    {
                        Separdor1.Visibility = ViewStates.Visible;
                        Detalle1.Visibility = ViewStates.Visible;
                        Texto1 = Detalle1.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                        resumen1 = "El vehiculo cuenta con los siguientes Items en el Botiquin:\n\n" + VICLS.Botiquin1;
                        Texto1.Text = resumen1;
                    }
                    if (bandera)
                    {
                        if (VICLS.Botiquin2.Trim().Length > 0)
                        {
                            Separdor2.Visibility = ViewStates.Visible;
                            Detalle2.Visibility = ViewStates.Visible;
                            Texto2 = Detalle2.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen2 = "El vehiculo no cuenta con los siguientes Items en el Botiquin:\n\n" + VICLS.Botiquin2;
                            Texto2.Text = resumen2;
                        }
                        if (VICLS.Botiquin3.Trim().Length > 0)
                        {
                            Separdor3.Visibility = ViewStates.Visible;
                            Detalle3.Visibility = ViewStates.Visible;
                            Texto3 = Detalle3.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen3 = "Observaciones en el Botiquin:\n\n" + VICLS.Botiquin3;
                            Texto3.Text = resumen3;
                        }
                    }
                    Separdor4.Visibility = ViewStates.Gone;
                    Detalle4.Visibility = ViewStates.Gone;
                    Separdor5.Visibility = ViewStates.Gone;
                    Detalle5.Visibility = ViewStates.Gone;
                    break;
                case 4:
                    if (VICLS.BanderaDocumentos == 0)
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_1);
                    }
                    else
                    {
                        TableIndicator.SetBackgroundResource(Resource.Color.color_estado_2);
                        bandera = true;
                    }

                    if (bandera)
                    {
                        if (VICLS.Seguridad1.Trim().Length > 0)
                        {
                            Separdor1.Visibility = ViewStates.Visible;
                            Detalle1.Visibility = ViewStates.Visible;
                            Texto1 = Detalle1.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen1 = "Se tienen las siguientes observaciones del SOAT del vehiculo: \n\n" + VICLS.Seguridad1;
                            Texto1.Text = resumen1;
                        }

                        if (VICLS.Seguridad2.Trim().Length > 0)
                        {
                            Separdor2.Visibility = ViewStates.Visible;
                            Detalle2.Visibility = ViewStates.Visible;
                            Texto2 = Detalle2.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen2 = "Se tienen las siguientes observaciones de la Revisión Técnica del vehiculo: \n\n" + VICLS.Seguridad2;
                            Texto2.Text = resumen2;
                        }
                    }

                    if (VICLS.Seguridad3.Trim().Length > 0)
                    {
                        Separdor3.Visibility = ViewStates.Visible;
                        Detalle3.Visibility = ViewStates.Visible;
                        Texto3 = Detalle3.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                        resumen3 = "El vehiculo cuenta con los siguientes documentos:\n\n" + VICLS.Seguridad3;
                        Texto3.Text = resumen3;
                    }

                    if (bandera)
                    {
                        if (VICLS.Seguridad4.Trim().Length > 0)
                        {
                            Separdor4.Visibility = ViewStates.Visible;
                            Detalle4.Visibility = ViewStates.Visible;
                            Texto4 = Detalle4.FindViewById<TextView>(Resource.Id.TV_detalle_de_resumen);
                            resumen4 = "El vehiculo no cuenta con los siguientes documentos:\n\n" + VICLS.Seguridad4;
                            Texto4.Text = resumen4;
                        }
                    }
                    Separdor5.Visibility = ViewStates.Gone;
                    Detalle5.Visibility = ViewStates.Gone;
                    break;
                case 5:
                    TableIndicator.Visibility = ViewStates.Gone;
                    Separdor1.Visibility = ViewStates.Gone;
                    Detalle1.Visibility = ViewStates.Gone;
                    Separdor2.Visibility = ViewStates.Gone;
                    Detalle2.Visibility = ViewStates.Gone;
                    Separdor3.Visibility = ViewStates.Gone;
                    Detalle3.Visibility = ViewStates.Gone;
                    Separdor4.Visibility = ViewStates.Gone;
                    Detalle4.Visibility = ViewStates.Gone;
                    Separdor5.Visibility = ViewStates.Visible;
                    Detalle5.Visibility = ViewStates.Visible;
                    ComentariosVigilante = Detalle5.FindViewById<EditText>(Resource.Id.ET_GuardComments);
                    if(VICLS.Estado == 1)
                    {
                        ComentariosVigilante.Enabled = false;

                        if (VICLS.ComentariosVigilancia != "")
                        {
                            ComentariosVigilante.Text = VICLS.ComentariosVigilancia;
                        }
                        else
                        {
                            ComentariosVigilante.Text = "No hay comentarios de vigilancia";
                        }
                    }
                    break;
            }

            RetainInstance = true;

            return rootView;
        }

        public override void OnResume()
        {
            base.OnResume();
            //mSumaryPresenter.start();
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            actividadHost = activity;
            mFragmentCallbacks = (SummaryFragmentCallbacks)activity;
        }

        public override void OnDetach()
        {
            base.OnDetach();
            actividadHost = null;
        }

        public void validateCheckListSummary()
        {
            string comments = ComentariosVigilante.Text;
            mViewModel.RefreshChecKListBDE(VICLS, comments, 1);
        }

        public void refuseCheckListSummary()
        {
            string comments = ComentariosVigilante.Text;
            mViewModel.RefreshChecKListBDE(VICLS, comments, 2);
        }

        public void finishActivity()
        {
            mFragmentCallbacks.finishActivity();
        }

        public void showProgress(bool show)
        {
            mFragmentCallbacks.showProgress(show);
        }

        public void showRefreshCheckListSummaryAlert()
        {
            mFragmentCallbacks.raiseResultFlag(true);
            Toast.MakeText(Activity, "Pre-Uso Actualizado", ToastLength.Long).Show();
        }

        public void showDialog(string message, string okText, string cancelText)
        {
            mFragmentCallbacks.showDialog(message, okText, cancelText);
        }
    }




    public interface SummaryFragmentViewInterface
    {
        void setViewModel(SummaryViewModel viewModel);
        void finishActivity();
        void showProgress(bool show);
        void showRefreshCheckListSummaryAlert();
        void showDialog(string message, string okText, string cancelText);
    }

    public interface SummaryFragmentCallbacks
    {
        void showProgress(bool show);
        void finishActivity();
        void raiseResultFlag(bool flag);
        void showDialog(string message, string okText, string cancelText);
    }
}