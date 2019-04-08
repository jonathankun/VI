using Android.Content;
using Android.Util;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Utils
{
    public class UDataBase
    {
        Context mContext;

        public static string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static string path = System.IO.Path.Combine(folder, "VInspection.db");

        public UDataBase(Context context)
        {
            mContext = context;
        }

        public bool CreateDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.CreateTable<User>();
                    connection.CreateTable<Vehicle>();
                    connection.CreateTable<CheckList>();
                    connection.CreateTable<CheckListSummary>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        //Operaciones para admnistrar la base de datos de Usuarios

        public bool InsertIntoTableUser(User VIUD)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Insert(VIUD);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableListUser(List<User> ListVIUD)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.InsertAll(ListVIUD);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool UpdateUser(User VIUD)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<Vehicle>("UPDATE User set Nombre=?, Cuenta=?, Contrasena=?, Perfil=?, Area=?, Buscador=?" +
                        "                                           Where IdUsuario=?",
                                                                    VIUD.Nombre, VIUD.Cuenta, VIUD.Contrasena, VIUD.Perfil, VIUD.Area, VIUD.Buscador,
                                                                    VIUD.IdUsuario);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteUser(User VIUD)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Delete(VIUD);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTableUser()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.DeleteAll<User>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableUser(int IdUsuario)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<User>("SELECT * FROM User Where IdUsuario=? ", IdUsuario);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<User> SelectTableUser()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    return connection.Table<User>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        //Operaciones para admnistrar la base de datos de Vehiculos

        public bool InsertIntoTableVehicle(Vehicle VIV)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Insert(VIV);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableListVehicles(List<Vehicle> ListVIV)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.InsertAll(ListVIV);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool UpdateVehicle(Vehicle VIV)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<Vehicle>("UPDATE Vehicle set Placa=?, Marca=?, Modelo=?, Responsable=?, Area=?, Encargado=?, KUMantto=?, FUMantto=?, Kilometraje=?, Central=?, Estado =?, Buscador=? " +
                                                                         "Where IdVehiculo=? ",
                                                                         VIV.Placa, VIV.Marca, VIV.Modelo, VIV.Responsable, VIV.Area, VIV.Encargado, VIV.KUMantto, VIV.FUMantto, VIV.Kilometraje, VIV.Central, VIV.Estado, VIV.Buscador,
                                                                         VIV.IdVehiculo);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteVehicle(Vehicle VIV)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Delete(VIV);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTableVehicles()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.DeleteAll<Vehicle>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableVehicle(int IdVehiculo)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<Vehicle>("SELECT * FROM Vehicle Where IdVehiculo=? ", IdVehiculo);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<Vehicle> SelectTableVehicle()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    return connection.Table<Vehicle>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }


        //Operaciones para admnistrar la base de datos de CheckList

        public bool InsertIntoTableCheckList(CheckList VICL)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Insert(VICL);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableListCheckLists(List<CheckList> ListVICL)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.InsertAll(ListVICL);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool UpdateCheckList(CheckList VICL)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<CheckList>("UPDATE CheckList set Fecha=?, Placa=?, Kilometraje=?, Mantto=?, Produccion=?, Destino=?, Conductor=?, " +
                                                                         "SistemaDireccion=?, SistemaFrenos=?, Faros=?, LucesDireccionales=?, Asientos=?, Cinturones=?, EspejoInterno=?, EspejoExterno=?, NivelAceite=?," +
                                                                         "NivelAgua=?, Combustible=?, Claxon=?, AlarmaRetorceso=?, RelojesIndicadores=?, Neumaticos=?, NeumaticoRepuesto=?, Extintor=?, ConosSeguridad=?," +
                                                                         "SogaArrastre=?, Botiquin=?, HerramientasLlaves=?, GataPalanca=?, Triangulo=?, Linterna=?, Cunas=?, Carroceria=?, Pertiga=?, Circulina=?," +
                                                                         "ComentariosAdicionales=?, Observacion1=?, Prioridad1=?, Observacion2=?, Prioridad2=?, Observacion3=?, Prioridad3=?, Observacion4=?, Prioridad4=?," +
                                                                         "CajaSoporte=?, Alcohol=?, Jabon=?, Algodon=?, Aposito=?, Bandas=?, Esparadrapo=?, Gasas1=?, Gasas2=?, Tijera=?, Venda=?, ComentariosBotiquin=?," +
                                                                         "EstadoSOAT=?, NuevaFechaSOAT=?, EstadRevTecnica=?, NuevaFechaRevTecnica=?, SOAT=?, RevTecnica=?, TarjetaPropiedad=?, CartillaSeguridad=?, CartillaERP=?," +
                                                                         "BanderaMantto=?, BanderaItems=?, BanderaComentarios=?, BanderaBotiquin=?, BanderaDocumentos=?, BanderaPrincipal=?," +
                                                                         "Buscador=?, Garitas=?" +
                                                                         "Where IdPreUso=? ",
                                                                         VICL.Fecha, VICL.Placa, VICL.Kilometraje, VICL.Mantto, VICL.Produccion, VICL.Destino, VICL.Conductor,
                                                                         VICL.SistemaDireccion, VICL.SistemaFrenos, VICL.Faros, VICL.LucesDireccionales, VICL.Asientos, VICL.Cinturones, VICL.EspejoInterno, VICL.EspejoExterno, VICL.NivelAceite,
                                                                         VICL.NivelAgua, VICL.Combustible, VICL.Claxon, VICL.AlarmaRetorceso, VICL.RelojesIndicadores, VICL.Neumaticos, VICL.NeumaticoRepuesto, VICL.Extintor, VICL.ConosSeguridad,
                                                                         VICL.SogaArrastre, VICL.Botiquin, VICL.HerramientasLlaves, VICL.GataPalanca, VICL.Triangulo, VICL.Linterna, VICL.Cunas, VICL.Carroceria, VICL.Pertiga, VICL.Circulina,
                                                                         VICL.ComentariosAdicionales, VICL.Observacion1, VICL.Prioridad1, VICL.Observacion2, VICL.Prioridad2, VICL.Observacion3, VICL.Prioridad3, VICL.Observacion4, VICL.Prioridad4,
                                                                         VICL.CajaSoporte, VICL.Alcohol, VICL.Jabon, VICL.Algodon, VICL.Aposito, VICL.Bandas, VICL.Esparadrapo, VICL.Gasas1, VICL.Gasas2, VICL.Tijera, VICL.Venda, VICL.ComentariosBotiquin,
                                                                         VICL.EstadoSOAT, VICL.NuevaFechaSOAT, VICL.EstadRevTecnica, VICL.NuevaFechaRevTecnica, VICL.SOAT, VICL.RevTecnica, VICL.TarjetaPropiedad, VICL.CartillaSeguridad, VICL.CartillaERP,
                                                                         VICL.BanderaMantto, VICL.BanderaItems, VICL.BanderaComentarios, VICL.ComentariosBotiquin, VICL.BanderaDocumentos, VICL.BanderaPrincipal,
                                                                         VICL.Buscador, VICL.Garitas,
                                                                         VICL.IdPreUso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteCheckList(CheckList VICL)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Delete(VICL);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTableCheckLists()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.DeleteAll<CheckList>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableCheckList(int IdPreUso)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<CheckList>("SELECT * FROM CheckList Where IdPreUso=? ", IdPreUso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<CheckList> SelectTableCheckList()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    return connection.Table<CheckList>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }


        //Operaciones para admnistrar la base de datos de Resumenes de Pre-Usos

        public bool InsertIntoTableCheckListSumary(CheckListSummary VICLS)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Insert(VICLS);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTableListCheckListSumary(List<CheckListSummary> ListVICLS)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.InsertAll(ListVICLS);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool UpdateCheckListSumary(CheckListSummary VICLS)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<Vehicle>("UPDATE CheckListSummary set Vehiculo=?, Conductor=?, Fecha=?, Kilometraje=?, Produccion=?, Destino=?, MensajeMantto=?," +
                                                                    "Items1=?, Items2=?, Comentarios1=?, Comentarios2=?, Botiquin1=?, Botiquin2=?, Seguridad1=?, Seguridad2=?, Seguridad3=?," +
                                                                    "BanderaMantto=?, BanderaItems=?, BanderaComentarios=?, BanderaBotiquin=?, BanderaDocumentos=?, BanderaPrincipal=?," +
                                                                    "BanderaMensajes=?, " +
                                                                    "ComentariosVigilancia=?, Buscador=?, Garitas=?, Estado=?" +
                                                                    "Where IdResumen=? ",
                                                                    VICLS.Vehiculo, VICLS.Conductor, VICLS.Fecha, VICLS.Kilometraje, VICLS.Produccion, VICLS.Destino, VICLS.MensajeMantto,
                                                                    VICLS.Items1, VICLS.Items2, VICLS.Comentarios1, VICLS.Comentarios2, VICLS.Botiquin1, VICLS.Botiquin2, VICLS.Seguridad1, VICLS.Seguridad2, VICLS.Seguridad3,
                                                                    VICLS.BanderaMantto, VICLS.BanderaItems, VICLS.BanderaComentarios, VICLS.BanderaBotiquin, VICLS.BanderaDocumentos, VICLS.BanderaPrincipal,
                                                                    VICLS.BanderaMensajes,
                                                                    VICLS.ComentariosVigilancia, VICLS.Buscador, VICLS.Garitas, VICLS.Estado,
                                                                    VICLS.IdResumen);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteCheckListSumary(CheckListSummary VICLS)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Delete(VICLS);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTableCheckListSumary()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.DeleteAll<CheckListSummary>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTableCheckListSumary(string IdResumenPreUso)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    connection.Query<CheckListSummary>("SELECT * FROM CheckListSummary Where IdResumen=? ", IdResumenPreUso);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<CheckListSummary> SelectTableCheckListSumary()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "VInspection.db")))
                {
                    return connection.Table<CheckListSummary>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }
    }
}