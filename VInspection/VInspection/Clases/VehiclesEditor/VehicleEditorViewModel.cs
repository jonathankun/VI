using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.VehiclesEditor
{
    public class VehicleEditorViewModel : VehicleEditorViewModelInterface, VehicleEditorOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private VehicleEditorActivityViewInteface mView;
        private VehicleEditorInteractor mInteractor;

        public VehicleEditorViewModel(VehicleEditorActivityViewInteface view,
                                      VehicleEditorInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;

        }

        public Vehicle ObtenerVehiculoBDI(int id)
        {
            Log.Info(TAG, "Ingreso a ObtenerVehiculo");

            List<Vehicle> ListaVehiculos = mInteractor.obtenerTablaVehiculos_SQLite();

            Log.Info(TAG, "La cantidad de vehiculos encontrados es: " + ListaVehiculos.Count);

            Vehicle vehiculo =  ListaVehiculos.Find(x => x.IdVehiculo == id);

            return vehiculo;
        }

        public string[] obtenerListaUsuariosBDI()
        {
            List<string> Lista_Usuarios = new List<string>();

            List<User> usuarios = mInteractor.obtenerTablaUsuarios_SQLite();

            for (int i = 0; i < usuarios.Count; i++)
            {
                Lista_Usuarios.Add(usuarios[i].Nombre);
            }

            string[] Lista = new string[Lista_Usuarios.Count];
            Lista = Lista_Usuarios.ToArray();

            Log.Info(TAG, "La lista de usuarios es: " + String.Join(",", Lista));

            return Lista;
        }

        public void EvaluarEstadoVehiculoYAgregarBDE(int id, Vehicle vehiculo)
        {
            float Kilo = (float)Convert.ToDouble(vehiculo.Kilometraje);
            float UMantto = (float)Convert.ToDouble(vehiculo.KUMantto);
            float dif = Kilo - UMantto;

            vehiculo.Estado = 0;

            if (dif> 4800 && dif<= 5000)
            {
                vehiculo.Estado = 1;
            }
            else if (dif > 5000)
            {
                vehiculo.Estado = 2;
            }

            vehiculo.Buscador = vehiculo.Placa + "@" + vehiculo.Marca + "@" + vehiculo.Modelo + "@" + vehiculo.Responsable + "@" + vehiculo.Area + "@" + vehiculo.Encargado + "@" + vehiculo.Central;

            if (UConnection.conectadoWifi())
            {
                if (id != 0)
                {
                    mInteractor.actualizarVehiculo_SQLServer(this, vehiculo);
                }
                else
                {
                    mInteractor.agregarVehiculo_SQLServer(this, vehiculo);
                }
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public void BorrarrVehiculoBDE(int id)
        {
            if (UConnection.conectadoWifi())
            {
                mInteractor.eliminarVehiculo_SQLServer(this, id);
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.raiseResultFlag(true);
            mView.finishActivity();
        }

        public void finalizarActividad()
        {
            mView.raiseResultFlag(true);
            mView.finishActivity();
        }
    }

    public interface VehicleEditorViewModelInterface
    {
        Vehicle ObtenerVehiculoBDI(int id);
        string[] obtenerListaUsuariosBDI();
        void EvaluarEstadoVehiculoYAgregarBDE(int id, Vehicle vehiculo);
        void BorrarrVehiculoBDE(int id);
    }
}