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

namespace VInspection.Clases.Summary
{
    public class SummaryViewModel : SummaryViewModelInterface, TablesOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private SummaryFragmentViewInterface mView;
        private SummaryInteractor mInteractor;

        public SummaryViewModel(SummaryFragmentViewInterface view,
                       SummaryInteractor Interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = Interactor;
        }

        public void RefreshChecKListBDE(CheckListSummary resumen, string comments, int state)
        {
            resumen.ComentariosVigilancia = comments;
            resumen.Estado = state;
            if (UConnection.conectadoWifi())
            {
                mInteractor.actualizarResumenPreUso_SQLServer(resumen, this);
            }
            else
            {
                mView.showDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public CheckListSummary ObtenerResumenPreUsoBDI(int id)
        {
            List<CheckListSummary> lista = mInteractor.obtenerTablaPreUsos_SQLite();
            CheckListSummary Resumen = lista.Find(x => x.IdResumen == id);
            return Resumen;
        }

        public Vehicle ObtenerVehiculoBDI(string placa)
        {
            List<Vehicle> lista = mInteractor.obtenerTablaVehiculos_SQLite();
            Vehicle vehiculo = lista.Find(x => x.Placa == placa);
            return vehiculo;
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
        }

        public void resumenPreUsoActualizado()
        {
            mView.showRefreshCheckListSummaryAlert();
            mView.finishActivity();
        }
    }

    public interface SummaryViewModelInterface
    {
        void RefreshChecKListBDE(CheckListSummary resumen,  string comments, int state);

        CheckListSummary ObtenerResumenPreUsoBDI(int id);
        Vehicle ObtenerVehiculoBDI(string placa);
    }
}