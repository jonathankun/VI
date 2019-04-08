using System;

namespace VInspection.Clases.Utils
{
    public class UVInspectionModels
    {
        public class User
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Cuenta { get; set; }
            public string Contrasena { get; set; }
            public string Perfil { get; set; }
            public string Area { get; set; }
            public string Buscador { get; set; }
        }

        public class VILayoutsId
        {
            public int id { get; set; }
            public int LayoutId { get; set; }
        }

        public class Table
        {
            public int id { get; set; }
            public string title { get; set; }
            public int noData { get; set; }
            public int[] Inputs { get; set; }
        }

        public class Vehicle
        {
            public int IdVehiculo { get; set; }
            public string Placa { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Responsable { get; set; }
            public string Area { get; set; }
            public string Encargado { get; set; }
            public double KUMantto { get; set; }
            public DateTime FUMantto { get; set; }
            public double Kilometraje { get; set; }
            public string Central { get; set; }
            public int Estado { get; set; }
            public string Buscador { get; set; }
        }

        public class Document
        {
            public int IdDocumento { get; set; }
            public string Placa { get; set; }
            public DateTime FechaSOAT { get; set; }
            public DateTime FechaRevTecnica { get; set; }
        }

        public class CheckList
        {
            public int IdPreUso { get; set; }
            public DateTime Fecha { get; set; }

            public string Placa { get; set; }
            public double Kilometraje { get; set; }
            public string Mantto { get; set; }
            public string Produccion { get; set; }
            public string Destino { get; set; }
            public string Conductor { get; set; }

            public string SistemaDireccion { get; set; }
            public string SistemaFrenos { get; set; }
            public string Faros { get; set; }
            public string LucesDireccionales { get; set; }
            public string Asientos { get; set; }
            public string Cinturones { get; set; }
            public string Vidrios { get; set; }
            public string LimpiaParabrisas { get; set; }
            public string EspejoInterno { get; set; }
            public string EspejoExterno { get; set; }
            public string NivelAceite { get; set; }
            public string NivelAgua { get; set; }
            public string Combustible { get; set; }
            public string Claxon { get; set; }
            public string AlarmaRetorceso { get; set; }
            public string RelojesIndicadores { get; set; }
            public string Neumaticos { get; set; }
            public string NeumaticoRepuesto { get; set; }
            public string Extintor { get; set; }
            public string ConosSeguridad { get; set; }
            public string SogaArrastre { get; set; }
            public string Botiquin { get; set; }
            public string HerramientasLlaves { get; set; }
            public string GataPalanca { get; set; }
            public string Triangulo { get; set; }
            public string Linterna { get; set; }
            public string Cunas { get; set; }
            public string Carroceria { get; set; }
            public string Pertiga { get; set; }
            public string Circulina { get; set; }

            public string ComentariosAdicionales { get; set; }
            public string Observacion1 { get; set; }
            public Nullable<int> Prioridad1 { get; set; }
            public string Observacion2 { get; set; }
            public Nullable<int> Prioridad2 { get; set; }
            public string Observacion3 { get; set; }
            public Nullable<int> Prioridad3 { get; set; }
            public string Observacion4 { get; set; }
            public Nullable<int> Prioridad4 { get; set; }

            public string CajaSoporte { get; set; }
            public string Alcohol { get; set; }
            public string Jabon { get; set; }
            public string Algodon { get; set; }
            public string Aposito { get; set; }
            public string Bandas { get; set; }
            public string Esparadrapo { get; set; }
            public string Gasas1 { get; set; }
            public string Gasas2 { get; set; }
            public string Tijera { get; set; }
            public string Venda { get; set; }
            public string ComentariosBotiquin { get; set; }

            public string EstadoSOAT { get; set; }
            public Nullable<DateTime> NuevaFechaSOAT { get; set; }
            public string EstadRevTecnica { get; set; }
            public Nullable<DateTime> NuevaFechaRevTecnica { get; set; }
            public string SOAT { get; set; }
            public string RevTecnica { get; set; }
            public string TarjetaPropiedad { get; set; }
            public string CartillaSeguridad { get; set; }
            public string CartillaERP { get; set; }

            public int BanderaMantto { get; set; }
            public int BanderaItems { get; set; }
            public int BanderaComentarios { get; set; }
            public int BanderaBotiquin { get; set; }
            public int BanderaDocumentos { get; set; }
            public int BanderaPrincipal { get; set; }

            public string Buscador { get; set; }
            public string Garitas { get; set; }
        }

        public class CheckListSummary
        {
            public int IdResumen { get; set; }
            public string Vehiculo { get; set; }
            public string Conductor { get; set; }
            public DateTime Fecha { get; set; }
            public double Kilometraje { get; set; }
            public string Produccion { get; set; }
            public string Destino { get; set; }
            public string MensajeMantto { get; set; }
            public string Items1 { get; set; }
            public string Items2 { get; set; }
            public string Comentarios1 { get; set; }
            public string Comentarios2 { get; set; }
            public string Botiquin1 { get; set; }
            public string Botiquin2 { get; set; }
            public string Botiquin3 { get; set; }
            public string Seguridad1 { get; set; }
            public string Seguridad2 { get; set; }
            public string Seguridad3 { get; set; }
            public string Seguridad4 { get; set; }
            public int BanderaMantto { get; set; }
            public int BanderaItems { get; set; }
            public int BanderaComentarios { get; set; }
            public int BanderaBotiquin { get; set; }
            public int BanderaDocumentos { get; set; }
            public int BanderaPrincipal { get; set; }
            public int BanderaMensajes { get; set; }
            public string ComentariosVigilancia { get; set; }
            public string Buscador { get; set; }
            public string Garitas { get; set; }
            public int Estado { get; set; }
        }
    }
}