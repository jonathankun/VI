using Android.Util;
using System;
using System.Collections.Generic;
using static VInspection.Clases.Utils.UTime;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Start
{
    public class StartViewModel : StartActivityViewModelInterface, StartActicityOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private StartActivityViewInterface mView;
        private StartInteractor mInteractor;

        public StartViewModel(StartActivityViewInterface view,
                                      StartInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;

            mInteractor.CrearBasedeDatos_SQLite();
        }

        public void ObtenerListaUsuariosBDE()
        {
            Log.Info(TAG, "Entro en ObtenerListaUsuariosBDE");
            mInteractor.descargarListaUsuarios_SQLServer(this);
        }

        public void ObtenerListaVehiculosBDE()
        {
            Log.Info(TAG, "Entro en ObtenerListaVehiculosBDE");
            mInteractor.descargarListaVehiculos_SQLServer(this);
        }

        public void ObtenerListaCheckListsBDE()
        {
            Log.Info(TAG, "Entro en ObtenerListaCheckListsBDE");
            string fecha = PrepareDateToBrowser(DateTime.Now);
            Log.Info(TAG, "se solicitarán Pre-Usos del día: " + fecha);
            mInteractor.descargarListaPreUsosDelDia_SQLServer(fecha, this);
        }

        public void compararListaUsuarios(List<User> output)
        {
            List<User> lista = mInteractor.obtenerTablaUsuarios_SQLite();
            Log.Info(TAG, "la lista interna de usuarios tiene " + lista.Count + " elementos");
            Log.Info(TAG, "la lista recibida de usuarios tiene " + output.Count + " elementos");
            if (lista.Count != output.Count)
            {
                Log.Info(TAG, "la lista de usuarios recibida es diferente");
                mInteractor.borrarTablaUsuarios_SQLite();
                mInteractor.insertarTablaUsuarios_SQLite(output);
            }
            else
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i] != output[i])
                    {
                        Log.Info(TAG, "se actualizo el vehiculo con ID: " + output[i].IdUsuario);
                        mInteractor.actualizarUsuarios_SQLite(output[i]);
                    }
                }
            }
            mView.downloadUsers_Finished();
        }

        public void levantarBanderaUsuarios(bool bandera)
        {
            mView.raiseFlag1(bandera);
        }

        public void errorDescargaUsuarios()
        {
            Log.Info(TAG, "errorDescargaVehiculos");
            mView.downloadUsers_Finished();
        }

        public void compararListaVehiculos(List<Vehicle> output)
        {
            List<Vehicle> lista = mInteractor.obtenerTablaVehiculos_SQLite();
            Log.Info(TAG, "la lista interna de vehiculos tiene " + lista.Count + " elementos");
            Log.Info(TAG, "la lista recibida de vehiculos tiene " + output.Count + " elementos");
            if (lista.Count != output.Count)
            {
                Log.Info(TAG, "la lista de vehiculos recibida es diferente");
                mInteractor.borrarTablaVehiculos_SQLite();
                mInteractor.insertarTablaVehiculos_SQLite(output);
            }
            else
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i] != output[i])
                    {
                        Log.Info(TAG, "se actualizo el vehiculo con ID: " + output[i].IdVehiculo);
                        mInteractor.actualizarVehiculo_SQLite(output[i]);
                    }
                }
            }
            mView.downloadVehicles_Finished();
        }

        public void levantarBanderaVehiculos(bool bandera)
        {
            mView.raiseFlag2(bandera);
        }

        public void errorDescargaVehiculos()
        {
            Log.Info(TAG, "errorDescargaVehiculos");
            mView.downloadVehicles_Finished();
        }


        public void compararListaPreUsos(List<CheckListSummary> output)
        {
            List<CheckListSummary> lista = mInteractor.obtenerTablaPreUsos_SQLite();
            Log.Info(TAG, "la lista interna de checklists tiene " + lista.Count + " elementos");
            Log.Info(TAG, "la lista recibida de checklists tiene " + output.Count + " elementos");
            if (lista.Count != output.Count)
            {
                Log.Info(TAG, "la lista de checklists recibida es diferente");
                mInteractor.borrarTablaPreUsos_SQLite();
                mInteractor.insertarTablaPreUsos_SQLite(output);
            }
            else
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i] != output[i])
                    {
                        Log.Info(TAG, "se actualizo el checklist con ID: " + output[i].IdResumen);
                        mInteractor.actualizarPreUso_SQLite(output[i]);
                    }
                }
            }

            mView.downloadCheckLists_Finished();
        }

        public void levantarBanderaPreUsos(bool bandera)
        {
            mView.raiseFlag3(bandera);
        }

        public void errorDescargaPreUsos()
        {
            Log.Info(TAG, "errorDescargaCheckLists");
            mView.downloadVehicles_Finished();
        }
    }

    public interface StartActivityViewModelInterface
    {
        void ObtenerListaUsuariosBDE();
        void ObtenerListaVehiculosBDE();
        void ObtenerListaCheckListsBDE();
    }
}