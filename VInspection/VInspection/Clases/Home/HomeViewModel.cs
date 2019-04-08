using Android.App;
using Android.Content;
using Android.Util;
using System;
using System.Collections.Generic;
using VInspection.Clases.Utils;
using static VInspection.Clases.Utils.UTime;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Home
{
    public class HomeViewModel : HomeViewModelInterface, HomeOnRequestFinished
    {
        public const string TAG = "DEBUG LOG";

        private HomeActivityViewInterface mView;
        private HomeInteractor mInteractor;

        Context context = Application.Context;

        public HomeViewModel(HomeActivityViewInterface view,
                             HomeInteractor interactor)
        {
            mView = view;
            mView.setViewModel(this);
            mInteractor = interactor;
        }

        public void ObtenerListaPreUsosBDE()
        {
            string fecha = PrepareDateToBrowser(DateTime.Now);
            Log.Info(TAG, "se solicitarán Pre-Usos del día: " + fecha);
            if (UConnection.conectadoWifi())
            {
                mInteractor.descargarListaPreUsosDelDia_SQLServer(fecha, this);
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public List<CheckListSummary> ObtenerListaPreUsosBDI()
        {
            List<CheckListSummary> lista = mInteractor.obtenerTablaPreUsos_SQLite();
            return lista;
        }

        public List<Vehicle> ObtenerListaVehiculosBDI()
        {
            List<Vehicle> lista = mInteractor.obtenerTablaVehiculos_SQLite();
            return lista;
        }

        public void GenerarPreusoTestOKBDE()
        {
            CheckList checkList = new CheckList
            {
                Fecha = DateTime.Now,
                Placa = "F6F-745",
                Kilometraje = 23000,
                Mantto = "0",
                Produccion = "Lima",
                Destino = "Oroya",
                Conductor = "Richard Narro",

                SistemaDireccion = "1",
                SistemaFrenos = "1",
                Faros = "1",
                LucesDireccionales = "1",
                Asientos = "1",
                Cinturones = "1",
                Vidrios = "1",
                LimpiaParabrisas = "1",
                EspejoInterno = "1",
                EspejoExterno = "1",
                NivelAceite = "1",
                NivelAgua = "1",
                Combustible = "1",
                Claxon = "1",
                AlarmaRetorceso = "1",
                RelojesIndicadores = "1",
                Neumaticos = "1",
                NeumaticoRepuesto = "1",
                Extintor = "1",
                ConosSeguridad = "1",
                SogaArrastre = "1",
                Botiquin = "1",
                HerramientasLlaves = "1",
                GataPalanca = "1",
                Triangulo = "1",
                Linterna = "1",
                Cunas = "1",
                Carroceria = "1",
                Pertiga = "0",
                Circulina = "0",

                ComentariosAdicionales = "",
                Observacion1 = "",
                Prioridad1 = 0,
                Observacion2 = "",
                Prioridad2 = 0,
                Observacion3 = "",
                Prioridad3 = 0,
                Observacion4 = "",
                Prioridad4 = 0,

                CajaSoporte = "1",
                Alcohol = "1",
                Jabon = "1",
                Algodon = "1",
                Aposito = "1",
                Bandas = "1",
                Esparadrapo = "1",
                Gasas1 = "1",
                Gasas2 = "1",
                Tijera = "1",
                Venda = "1",
                ComentariosBotiquin = "",

                EstadoSOAT = "1",
                NuevaFechaSOAT = DateTime.Parse("2017-01-08"),
                EstadRevTecnica = "1",
                NuevaFechaRevTecnica = DateTime.Parse("2017-01-08"),
                SOAT = "1",
                RevTecnica = "1",
                TarjetaPropiedad = "1",
                CartillaSeguridad = "1",
                CartillaERP = "1",

                BanderaMantto = 0,
                BanderaItems = 0,
                BanderaComentarios = 0,
                BanderaBotiquin = 0,
                BanderaDocumentos = 0,
                BanderaPrincipal = 0
            };

            checkList.Buscador = PrepareDateToBrowser(checkList.Fecha) + "@" + checkList.Placa + "@" + checkList.Produccion + "@" + checkList.Destino + "@" + checkList.Conductor;
            checkList.Garitas = checkList.Produccion + "@" + checkList.Destino;

            if (UConnection.conectadoWifi())
            {
                mInteractor.agregarPreUso_SQLServer(checkList, this);
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public void GenerarPreusoTestBadBDE()
        {
            CheckList checkList = new CheckList
            {
                Fecha = DateTime.Now,
                Placa = "F6F-745",
                Kilometraje = 23000,
                Mantto = "0",
                Produccion = "Lima",
                Destino = "Oroya",
                Conductor = "José Sifuentes",

                SistemaDireccion = "1",
                SistemaFrenos = "1",
                Faros = "1",
                LucesDireccionales = "1",
                Asientos = "1",
                Cinturones = "1",
                Vidrios = "1",
                LimpiaParabrisas = "1",
                EspejoInterno = "1",
                EspejoExterno = "1",
                NivelAceite = "1",
                NivelAgua = "1",
                Combustible = "1",
                Claxon = "1",
                AlarmaRetorceso = "1",
                RelojesIndicadores = "1",
                Neumaticos = "0",
                NeumaticoRepuesto = "0",
                Extintor = "0",
                ConosSeguridad = "0",
                SogaArrastre = "0",
                Botiquin = "0",
                HerramientasLlaves = "0",
                GataPalanca = "0",
                Triangulo = "0",
                Linterna = "0",
                Cunas = "0",
                Carroceria = "0",
                Pertiga = "0",
                Circulina = "0",

                ComentariosAdicionales = "Abolladura en la puerta posterior derecha",
                Observacion1 = "Faros delanteros no encienden",
                Prioridad1 = 1,
                Observacion2 = "Dirección vibra a más de 90Km/h",
                Prioridad2 = 2,
                Observacion3 = "Cinturones porteriores no funcionan",
                Prioridad3 = 2,
                Observacion4 = "Cocada de llantas delanteras desgastadas",
                Prioridad4 = 0,

                CajaSoporte = "1",
                Alcohol = "1",
                Jabon = "1",
                Algodon = "1",
                Aposito = "1",
                Bandas = "1",
                Esparadrapo = "1",
                Gasas1 = "1",
                Gasas2 = "1",
                Tijera = "0",
                Venda = "0",
                ComentariosBotiquin = "No hay tijeras y las vendas estan utilizadas",

                EstadoSOAT = "0",
                NuevaFechaSOAT = DateTime.Parse("2017-10-08"),
                EstadRevTecnica = "0",
                NuevaFechaRevTecnica = DateTime.Parse("2017-10-05"),
                SOAT = "1",
                RevTecnica = "1",
                TarjetaPropiedad = "0",
                CartillaSeguridad = "0",
                CartillaERP = "1",

                BanderaMantto = 1,
                BanderaItems = 1,
                BanderaComentarios = 1,
                BanderaBotiquin = 1,
                BanderaDocumentos = 1,
                BanderaPrincipal = 1
            };

            checkList.Buscador = PrepareDateToBrowser(checkList.Fecha) + "@" + checkList.Placa + "@" + checkList.Produccion + "@" + checkList.Destino + "@" + checkList.Conductor;
            checkList.Garitas = checkList.Produccion + "@" + checkList.Destino;

            if (UConnection.conectadoWifi())
            {
                mInteractor.agregarPreUso_SQLServer(checkList, this);
            }
            else
            {
                mView.ShowDialog("Debes encender la antena Wifi", "Ok", "False");
            }
        }

        public void compararListaPreUsos(List<CheckListSummary> output)
        {
            List<CheckListSummary> lista = new List<CheckListSummary>();
            if (output == null)
            {
                Log.Info(TAG, "la lista recibida esta vacia");
                mView.showNoCheckListSummaries();
            }
            else
            {
                lista = mInteractor.obtenerTablaPreUsos_SQLite();
                Log.Info(TAG, "la lista interna de PreUsos tiene " + lista.Count + " elementos");
                Log.Info(TAG, "la lista recibida de PreUsos tiene " + output.Count + " elementos");
                if (lista.Count != output.Count)
                {
                    Log.Info(TAG, "la lista de vehiculos recibida es diferente");
                    mInteractor.borrarTablaPreUsos_SQLite();
                    mInteractor.insertarTablaPreUsos_SQLite(output);
                }
                else
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i] != output[i])
                        {
                            Log.Info(TAG, "se actualizo el PreUso con ID: " + output[i].IdResumen);
                            mInteractor.actualizarPreUsos_SQLite(output[i]);
                        }
                    }
                }
            }
            lista = mInteractor.obtenerTablaPreUsos_SQLite();
            mView.showCheckListSummaries(lista);
        }

        public void errorDescargaPreUsos()
        {
            mView.errorRefreshnig();
        }

        public void mostrarProgreso(bool mostrar)
        {
            mView.showProgress(mostrar);
        }
    }


    public interface HomeViewModelInterface
    {
        void ObtenerListaPreUsosBDE();
        List<CheckListSummary> ObtenerListaPreUsosBDI();
        List<Vehicle> ObtenerListaVehiculosBDI();
        void GenerarPreusoTestOKBDE();
        void GenerarPreusoTestBadBDE();
    }
}