using Android.Util;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Vehicles
{
    public class VehicleViewModel : VehicleViewModelInterface, VehicleOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private VehicleActivityViewInterface mView;
        private VehicleInteractor mInteractor;

        public VehicleViewModel(VehicleActivityViewInterface view,
                                VehicleInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;
        }

        public void ObtenerListaVehiculosBDE()
        {
            if (UConnection.conectadoWifi())
            {
                mInteractor.descargarListaVehiculos_SQLServer(this);
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public List<Vehicle> ObtenerListaVehiculosBDI()
        {
            List<Vehicle> lista = mInteractor.obtenerTablaVehiculos_SQLite();
            return lista;
        }

        public void compararListaVehiculos(List<Vehicle> output)
        {
            List<Vehicle> lista = new List<Vehicle>();
            if (output == null)
            {
                Log.Info(TAG, "la lista recibida esta vacia");
                mView.showNoVehicle();
            }
            else
            {
                lista = mInteractor.obtenerTablaVehiculos_SQLite();
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
            }
            lista = mInteractor.obtenerTablaVehiculos_SQLite();
            mView.showVehicleList(lista);
        }

        public void errorDescargaVehiculos()
        {
            mView.errorRefreshnig();
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
        }
    }


    public interface VehicleViewModelInterface
    {
        void ObtenerListaVehiculosBDE();
        List<Vehicle> ObtenerListaVehiculosBDI();
    }
}