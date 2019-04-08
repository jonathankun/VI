using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using static VInspection.Clases.Utils.UVInspectionModels;

namespace VInspection.Clases.Utils
{
    public class UServices
    {
        // [APP_SCRIPT]
        public const string CAMPO_RESULT = "result";
        public const string JSON_VEHICULOS = "vehiculos";
        public const string JSON_CHECKLIST = "checklist";
        public const string JSON_DATOS = "datos";
        public const string JSON_FECHAS = "fechas";
        public const string JSON_BANDERAS = "banderas";

        // Funciones
        public const string INSERTAR_DATOS = "insertarDatos";
        public const string CONSEGUIR_FECHAS = "conseguirFechas";
        public const string SINCRONIZAR_RESUMEN = "sincronizarResumen";
        public const string SINCRONIZAR_CHECKLIST = "sincronizarCheckList";

        //[CODIGOS]
        public const int CODE_DATOS_PRICIPALES = 0;
        public const int CODE_TABLA_ITEMS = 1;
        public const int CODE_TABLA_COMENTARIOS = 2;
        public const int CODE_BOTIQUIN = 3;
        public const int CODE_SEGURIDAD_VEHICULAR = 4;
        public const int CODE_VIGILANCIA = 5;

        //TIPO DE LISTAS DE BASE DE DATOS
        public const string TIPO_USER = "Usuarios";
        public const string TIPO_VEHICLES = "Vehiculos";
        public const string TIPO_DOCUMENTS = "Documentos";
        public const string TIPO_CHECKLIST = "Preusos";
        public const string TIPO_CHECKLISTSUMMARY = "ResumenPreusos";

        //public const string Wifi_Name = "Daniel-Nokia530";
        public const string Wifi_Name = "Statkraft-Internet";
        //private const string WebServiceUrl = "http://192.168.110.68/";

        private static ISharedPreferences mUserPreferences = Application.Context.GetSharedPreferences("ConfigurationInfo", FileCreationMode.Private);
        //private static string WebServiceUrl = "http://" + mUserPreferences.GetString("ConfigurationIp", "172.20.10.3") + "/";

        //private static string WebServiceUrl = "http://" + mUserPreferences.GetString("ConfigurationIp", "localhost:45108") + "/";
        private static string WebServiceUrl = "https://vinspection-jrivera.azurewebsites.net/";

        //private static string WebServiceUrl_Users = WebServiceUrl + "VInspection/api/UsersApi/";
        //private static string WebServiceUrl_Vehicles = WebServiceUrl + "VInspection/api/VehiclesApi/";
        //private static string webServiceUrl_Documents = WebServiceUrl + "VInspection/api/DocumentsApi/";
        //private static string WebServiceUrl_CheckLists = WebServiceUrl + "VInspection/api/CheckListsApi/";
        //private static string WebServiceUrl_CheckListSummaries = WebServiceUrl + "VInspection/api/CheckListSummariesApi/";

        private static string WebServiceUrl_Users = WebServiceUrl + "api/UsersApi/";
        private static string WebServiceUrl_Vehicles = WebServiceUrl + "api/VehiclesApi/";
        private static string webServiceUrl_Documents = WebServiceUrl + "api/DocumentsApi/";
        private static string WebServiceUrl_CheckLists = WebServiceUrl + "api/CheckListsApi/";
        private static string WebServiceUrl_CheckListSummaries = WebServiceUrl + "api/CheckListSummariesApi/";



        public static string getUrl(string tipo)
        {
            switch (tipo)
            {
                case TIPO_USER:
                    return WebServiceUrl_Users;
                case TIPO_VEHICLES:
                    return WebServiceUrl_Vehicles;
                case TIPO_DOCUMENTS:
                    return webServiceUrl_Documents;
                case TIPO_CHECKLIST:
                    return WebServiceUrl_CheckLists;
                case TIPO_CHECKLISTSUMMARY:
                    return WebServiceUrl_CheckListSummaries;
                default:
                    return "";
            }
        }

        public static List<VILayoutsId> GetLayoutIds()
        {
            var listLayoutIds = new List<VILayoutsId>
            {
                new VILayoutsId
                {
                    id = CODE_DATOS_PRICIPALES,
                    LayoutId = Resource.Layout.tab_table0_fragment
                },
                new VILayoutsId
                {
                    id = CODE_TABLA_ITEMS,
                    LayoutId = Resource.Layout.tab_table1_fragment
                },
                new VILayoutsId
                {
                    id = CODE_TABLA_COMENTARIOS,
                    LayoutId = Resource.Layout.tab_table2_fragment
                },
                new VILayoutsId
                {
                    id = CODE_BOTIQUIN,
                    LayoutId = Resource.Layout.tab_table3_fragment
                },
                new VILayoutsId
                {
                    id = CODE_SEGURIDAD_VEHICULAR,
                    LayoutId = Resource.Layout.tab_table4_fragment
                }
            };

            return listLayoutIds;
        }

        public static int getLayoutId(int Page)
        {
            VILayoutsId Layout = GetLayoutIds().Find(x => x.id == Page);
            return Layout.LayoutId;
        }


        public static List<Table> GetTables()
        {
            var listTables = new List<Table>
            {
                new Table
                {
                    id = CODE_DATOS_PRICIPALES,
                    title = "Datos principales",
                    noData = 7,
                    Inputs = new int[] {Resource.Id.ET_T0_Item01,
                                        Resource.Id.ET_T0_Item02,
                                        Resource.Id.ET_T0_Item03,
                                        Resource.Id.CB_T0_Item04,
                                        Resource.Id.ET_T0_Item05,
                                        Resource.Id.ET_T0_Item06,
                                        Resource.Id.ET_T0_Item07 }
                },
                new Table
                {
                    id = CODE_TABLA_ITEMS,
                    title = "Items",
                    noData = 30,
                    Inputs = new int[] {Resource.Id.CB_T1_Item01,
                                        Resource.Id.CB_T1_Item02,
                                        Resource.Id.CB_T1_Item03,
                                        Resource.Id.CB_T1_Item04,
                                        Resource.Id.CB_T1_Item05,
                                        Resource.Id.CB_T1_Item06,
                                        Resource.Id.CB_T1_Item07,
                                        Resource.Id.CB_T1_Item08,
                                        Resource.Id.CB_T1_Item09,
                                        Resource.Id.CB_T1_Item10,
                                        Resource.Id.CB_T1_Item11,
                                        Resource.Id.CB_T1_Item12,
                                        Resource.Id.CB_T1_Item13,
                                        Resource.Id.CB_T1_Item14,
                                        Resource.Id.CB_T1_Item15,
                                        Resource.Id.CB_T1_Item16,
                                        Resource.Id.CB_T1_Item17,
                                        Resource.Id.CB_T1_Item18,
                                        Resource.Id.CB_T1_Item19,
                                        Resource.Id.CB_T1_Item20,
                                        Resource.Id.CB_T1_Item21,
                                        Resource.Id.CB_T1_Item22,
                                        Resource.Id.CB_T1_Item23,
                                        Resource.Id.CB_T1_Item24,
                                        Resource.Id.CB_T1_Item25,
                                        Resource.Id.CB_T1_Item26,
                                        Resource.Id.CB_T1_Item27,
                                        Resource.Id.CB_T1_Item28,
                                        Resource.Id.CB_T1_Item29,
                                        Resource.Id.CB_T1_Item30 }
                },
                new Table
                {
                    id = CODE_TABLA_COMENTARIOS,
                    title = "Comentarios",
                    noData = 9,
                    Inputs = new int[] {Resource.Id.ET_T2_Item00,
                                        Resource.Id.ET_T2_Item01,
                                        Resource.Id.SP_T2_Item01,
                                        Resource.Id.ET_T2_Item02,
                                        Resource.Id.SP_T2_Item02,
                                        Resource.Id.ET_T2_Item03,
                                        Resource.Id.SP_T2_Item03,
                                        Resource.Id.ET_T2_Item04,
                                        Resource.Id.SP_T2_Item04 }
                },
                new Table
                {
                    id = CODE_BOTIQUIN,
                    title = "Botiquin",
                    noData = 12,
                    Inputs = new int[] {Resource.Id.CB_T3_Item01,
                                        Resource.Id.CB_T3_Item02,
                                        Resource.Id.CB_T3_Item03,
                                        Resource.Id.CB_T3_Item04,
                                        Resource.Id.CB_T3_Item05,
                                        Resource.Id.CB_T3_Item06,
                                        Resource.Id.CB_T3_Item07,
                                        Resource.Id.CB_T3_Item08,
                                        Resource.Id.CB_T3_Item09,
                                        Resource.Id.CB_T3_Item10,
                                        Resource.Id.CB_T3_Item11,
                                        Resource.Id.ET_T3_Item12 }
                },
                new Table
                {
                    id = CODE_SEGURIDAD_VEHICULAR,
                    title = "Seguridad Vehicular",
                    noData = 9,
                    Inputs = new int[] {Resource.Id.ET_T4_Item01,
                                        Resource.Id.ET_T4_Item02,
                                        Resource.Id.ET_T4_Item03,
                                        Resource.Id.ET_T4_Item04,
                                        Resource.Id.CB_T4_Item05,
                                        Resource.Id.CB_T4_Item06,
                                        Resource.Id.CB_T4_Item07,
                                        Resource.Id.CB_T4_Item08,
                                        Resource.Id.CB_T4_Item09 }
                },
                new Table
                {
                    id = CODE_VIGILANCIA,
                    title = "Comentarios de Vigilancia",
                    noData = 1,
                    Inputs = new int[] {Resource.Id.ET_GuardComments }
                }
            };
            return listTables;
        }

        public static string getTitle(int idTabla)
        {
            Table Table = GetTables().Find(x => x.id == idTabla);
            return Table.title;
        }

        public static int getNoData(int idTabla)
        {
            Table Table = GetTables().Find(x => x.id == idTabla);
            return Table.noData;
        }

        public static int getInputs(int idTabla, int noInputs)
        {
            Table Table = GetTables().Find(x => x.id == idTabla);
            return Table.Inputs[noInputs];
        }
    }
}