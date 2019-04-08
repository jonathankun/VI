using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VInspectionWebService.Models;

namespace VInspectionWebService.Controllers
{
    public class SummariesGeneratorApiController : ApiController
    {
        public static string MOTIVO_NOTIFICAR = "Notificar";
        public static string MOTIVO_RECHAZAR = "Rechazar";

        private VInspectionEntities db = new VInspectionEntities();

        public bool Actualizar_KilometrajoVehiculo(CheckList checkList)
        {
            VehiclesApiController vehiclesController = new VehiclesApiController();

            Vehicle vehicle = db.Vehicles.FirstOrDefault(x => x.Placa == checkList.Placa);
            Debug.Print("\n\n" + JsonConvert.SerializeObject(vehicle) + "\n\n");

            vehicle.Kilometraje = checkList.Kilometraje;

            if (checkList.BanderaMantto == 2 && checkList.Mantto == "1")
            {
                vehicle.FUMantto = checkList.Fecha;
                vehicle.KUMantto = checkList.Kilometraje;
            }

            vehiclesController.PutVehicle(vehicle.IdVehiculo, vehicle);

            return true;
        }

        public bool Guardar_ResumenPreUsos(CheckList checkList)
        {
            CheckListSummary summary = new CheckListSummary
            {
                IdResumen = checkList.IdPreUso,
                Vehiculo = checkList.Placa,
                Conductor = checkList.Conductor,
                Fecha = checkList.Fecha,
                Kilometraje = checkList.Kilometraje,
                Produccion = checkList.Produccion,
                Destino = checkList.Destino,
                MensajeMantto = GenerarMensajeMantto(checkList),
                Items1 = ObtenerItemsPresentes(checkList),
                Items2 = ObtenerItemsFaltantes(checkList),
                Comentarios1 = checkList.ComentariosAdicionales,
                Comentarios2 = ObtenerComentarios(checkList),
                Botiquin1 = ObtenerItemsPresentesBotiquin(checkList),
                Botiquin2 = ObtenerItemsFaltantesBotiquin(checkList),
                Botiquin3 = checkList.ComentariosBotiquin,
                Seguridad1 = ObtenerMensajeSOAT(checkList),
                Seguridad2 = ObtenerMensajeRevTecnica(checkList),
                Seguridad3 = ObtenerDocumentosPresentes(checkList),
                Seguridad4 = ObtenerDocumentosFaltantes(checkList),
                BanderaMantto = checkList.BanderaMantto,
                BanderaItems = checkList.BanderaItems,
                BanderaComentarios = checkList.BanderaComentarios,
                BanderaBotiquin = checkList.BanderaBotiquin,
                BanderaDocumentos = checkList.BanderaDocumentos,
                BanderaPrincipal = checkList.BanderaPrincipal,
                Buscador = checkList.Buscador,
                Garitas = checkList.Garitas
            };

            summary.BanderaMensajes = 0;

            if (checkList.BanderaPrincipal == 1)
            {
                bool result = Enviar_CorreoNotificacion(summary, MOTIVO_NOTIFICAR);

                if (result == true)
                {
                    summary.BanderaMensajes = 1;
                }
            }

            CheckListSummariesApiController checkListSummaryController = new CheckListSummariesApiController();

            Debug.Print(JsonConvert.SerializeObject(summary));

            checkListSummaryController.PostCheckListSummary(summary);

            return true;
        }

        public bool Enviar_CorreoNotificacion(CheckListSummary summary , string motivo)
        {
            MessagesSenderApiController messagesSender = new MessagesSenderApiController();

            Vehicle vehicle = db.Vehicles.FirstOrDefault(x => x.Placa == summary.Vehiculo);

            List<string> destinatarios = Obtener_Destinatarios(vehicle, summary, motivo);

            string asunto = "";

            if (motivo == MOTIVO_RECHAZAR)
            {
                asunto = asunto + "Rechazo de ";
            }

            asunto = asunto + "Pre-Uso Vehicular - " + summary.Vehiculo + " - " + summary.Fecha + " - Area Encargada: " + vehicle.Area;

            string contenido = "";

            string titulo = "", d_mantto = "", d_items = "", d_coment = "", d_botiquin = "", d_documen = "", d_vigilancia = "";

            titulo = "El pre - uso vehicular del vehiculo " + vehicle.Marca + "-" + vehicle.Modelo + " con N° de Placa " + summary.Vehiculo +
                                " que ha sido realizado por " + summary.Conductor + " tiene el siguiente detalle: \n\n" +
                                "Lugar de origen: " + summary.Produccion + "\n" +
                                "Lugar de destino: " + summary.Destino + "\n" +
                                "Kilometraje inicial: " + summary.Kilometraje + "\n\n";

            if (summary.BanderaMantto == 1)
            {
                d_mantto = "* Es necesario realizar el siguiente mantenimiento al vehiculo.\n\n";
            }
            else if (summary.BanderaMantto == 2)
            {
                d_mantto = "* El kilometraje para mantenimiento se encuentra próximo.\n\n";
            }

            if (summary.BanderaItems == 1)
            {
                d_items = "* El vehiculo tiene los siguientes Items en mal estado o carece de ellos:\n\n" + summary.Items2 + "\n\n";
            }

            if (summary.BanderaComentarios == 1)
            {
                d_coment = "* El vehiculo tiene las siguientes observaciones:\n\n" + summary.Comentarios1 + "\n\n" + summary.Comentarios2;
            }

            if (summary.BanderaBotiquin == 1)
            {
                d_botiquin = "* El botiquin tiene los siguientes Items en mal estado o carece de ellos:\n\n" + summary.Botiquin2 + "\n";
                if (summary.Botiquin3.Trim().Length > 0)
                {
                    d_botiquin = d_botiquin + "* Observaciones en el Botiquin:\n\n" + summary.Botiquin3 + "\n\n";
                }
            }

            if (summary.BanderaDocumentos == 1)
            {
                if (summary.Seguridad1.Trim().Length > 0)
                {
                    d_documen = d_documen + "* Se tienen las siguientes observaciones del SOAT del vehiculo: \n\n" + summary.Seguridad1 + "\n\n";
                }

                if (summary.Seguridad2.Trim().Length > 0)
                {
                    d_documen = d_documen + "* Se tienen las siguientes observaciones de la Revisión Técnica del vehiculo: \n\n" + summary.Seguridad2 + "\n\n";
                }

                if (summary.Seguridad4.Trim().Length > 0)
                {
                    d_documen = d_documen + "* El vehiculo no cuenta con la siguiente documentación:\n\n" + summary.Seguridad4 + "\n\n";
                }
            }

            if (motivo == MOTIVO_RECHAZAR)
            {
                d_vigilancia = "EL PRE-USO HA SIDO RECHAZADO CON LAS SIGUIENTES OBSERVACIONES: \n\n" + summary.ComentariosVigilancia;
            }

            contenido = titulo + d_mantto + d_items + d_coment + d_botiquin + d_documen + d_vigilancia;

            bool result = messagesSender.SendEmail("Administrador de Plantas", destinatarios, asunto, contenido);

            return true;
        }

        public List<string> Obtener_Destinatarios(Vehicle vehicle, CheckListSummary summary, string reason)
        {
            List<string> destinatarios = new List<string>();

            string responsable = vehicle.Responsable.Trim();

            destinatarios.Add(responsable.Replace(" ", ".") + "@statkraft.com");

            List<User> encargados = db.Users.Where(x => x.Perfil == "Responsable").ToList();
            User UsuarioEncargado = new User();
            string encargado;


            if (reason == MOTIVO_NOTIFICAR)
            {
                if (summary.BanderaMantto == 1 || summary.BanderaItems == 1 || summary.BanderaComentarios == 1)
                {
                    for (int i = 0; i < encargados.Count; i++)
                    {
                        if (encargados[i].Area == "Planeamiento")
                        {
                            UsuarioEncargado = encargados[i];
                            encargado = UsuarioEncargado.Nombre.Trim();
                            destinatarios.Add(encargado.Replace(" ", ".") + "@statkraft.com");
                        }
                    }
                }

                if (summary.BanderaBotiquin == 1)
                {
                    for (int i = 0; i < encargados.Count; i++)
                    {
                        if (encargados[i].Area == "HSS")
                        {
                            UsuarioEncargado = encargados[i];
                            encargado = UsuarioEncargado.Nombre.Trim();
                            destinatarios.Add(encargado.Replace(" ", ".") + "@statkraft.com");
                        }
                    }
                }
            }

            if (reason == MOTIVO_RECHAZAR)
            {
                for (int i = 0; i < encargados.Count; i++)
                {
                    if (encargados[i].Area == "Security")
                    {
                        UsuarioEncargado = encargados[i];
                        encargado = UsuarioEncargado.Nombre.Trim();
                        destinatarios.Add(encargado.Replace(" ", ".") + "@statkraft.com");
                    }
                }
            }

            return destinatarios;
        }

        public string GenerarMensajeMantto(CheckList checklist)
        {
            string mensaje = "";

            if (checklist.BanderaMantto == 1)
            {
                mensaje = "El mantenimiento por kilometraje se encuntra próximo.";
            }
            else if (checklist.BanderaMantto == 2)
            {
                if (checklist.Mantto == "1")
                {
                    mensaje = "Se realizó el mantenimiento en el kilometraje " + checklist.Kilometraje + ".";
                }
                else
                {
                    mensaje = "Ya es necesario realizarse el mantenimiento por kilometraje.";
                }
            }

            return mensaje;
        }

        public string ObtenerItemsPresentes(CheckList checklist)
        {
            string itemsPresentes = "";

            if (checklist.SistemaDireccion == "1") itemsPresentes = itemsPresentes + "- Sistema de Dirección\n";
            if (checklist.SistemaFrenos == "1") itemsPresentes = itemsPresentes + "- Sistema de Frenos\n";
            if (checklist.Faros == "1") itemsPresentes = itemsPresentes + "- Faros (delanteros y posteriores)\n";
            if (checklist.LucesDireccionales == "1") itemsPresentes = itemsPresentes + "- Luces Direccionales\n";
            if (checklist.Asientos == "1") itemsPresentes = itemsPresentes + "- Asientos\n";
            if (checklist.Cinturones == "1") itemsPresentes = itemsPresentes + "- Cinturones en cada asiento\n";
            if (checklist.Vidrios == "1") itemsPresentes = itemsPresentes + "- Vidrios\n";
            if (checklist.LimpiaParabrisas == "1") itemsPresentes = itemsPresentes + "- Limpia Parabrisas\n";
            if (checklist.EspejoInterno == "1") itemsPresentes = itemsPresentes + "- Espejos Retrovisores Internos\n";
            if (checklist.EspejoExterno == "1") itemsPresentes = itemsPresentes + "- Espejos Retrovisores Externos\n";
            if (checklist.NivelAceite == "1") itemsPresentes = itemsPresentes + "- Nivel de Aceite\n";
            if (checklist.NivelAgua == "1") itemsPresentes = itemsPresentes + "- Nivel de Agua / Refrigerante\n";
            if (checklist.Combustible == "1") itemsPresentes = itemsPresentes + "- Combustible\n";
            if (checklist.Claxon == "1") itemsPresentes = itemsPresentes + "- Claxon\n";
            if (checklist.AlarmaRetorceso == "1") itemsPresentes = itemsPresentes + "- Alarma de retroceso\n";
            if (checklist.RelojesIndicadores == "1") itemsPresentes = itemsPresentes + "- Relojes Indicadores / Panel de Control\n";
            if (checklist.Neumaticos == "1") itemsPresentes = itemsPresentes + "- Neumáticos\n";
            if (checklist.NeumaticoRepuesto == "1") itemsPresentes = itemsPresentes + "- Neumático de repuesto\n";
            if (checklist.Extintor == "1") itemsPresentes = itemsPresentes + "- Extintor PQS tipo ABC\n";
            if (checklist.ConosSeguridad == "1") itemsPresentes = itemsPresentes + "- Conos de Seguridad (02)\n";
            if (checklist.SogaArrastre == "1") itemsPresentes = itemsPresentes + "- Soga de arrastre\n";
            if (checklist.Botiquin == "1") itemsPresentes = itemsPresentes + "- Botiquín\n";
            if (checklist.HerramientasLlaves == "1") itemsPresentes = itemsPresentes + "- Herramienta y llave de ruedas\n";
            if (checklist.GataPalanca == "1") itemsPresentes = itemsPresentes + "- Gata y Palanca\n";
            if (checklist.Triangulo == "1") itemsPresentes = itemsPresentes + "- Triángulos de seguridad (02)\n";
            if (checklist.Linterna == "1") itemsPresentes = itemsPresentes + "- Linterna\n";
            if (checklist.Cunas == "1") itemsPresentes = itemsPresentes + "- Cuñas\n";
            if (checklist.Carroceria == "1") itemsPresentes = itemsPresentes + "- Carrocería\n";
            if (checklist.Pertiga == "1") itemsPresentes = itemsPresentes + "- Pértiga\n";
            if (checklist.Circulina == "1") itemsPresentes = itemsPresentes + "- Circulina\n";

            return itemsPresentes;
        }

        public string ObtenerItemsFaltantes(CheckList checklist)
        {
            string itemsFaltantes = "";

            if (checklist.SistemaDireccion == "0") itemsFaltantes = itemsFaltantes + "- Sistema de Dirección\n";
            if (checklist.SistemaFrenos == "0") itemsFaltantes = itemsFaltantes + "- Sistema de Frenos\n";
            if (checklist.Faros == "0") itemsFaltantes = itemsFaltantes + "- Faros (delanteros y posteriores)\n";
            if (checklist.LucesDireccionales == "0") itemsFaltantes = itemsFaltantes + "- Luces Direccionales\n";
            if (checklist.Asientos == "0") itemsFaltantes = itemsFaltantes + "- Asientos\n";
            if (checklist.Cinturones == "0") itemsFaltantes = itemsFaltantes + "- Cinturones en cada asiento\n";
            if (checklist.Vidrios == "0") itemsFaltantes = itemsFaltantes + "- Vidrios\n";
            if (checklist.LimpiaParabrisas == "0") itemsFaltantes = itemsFaltantes + "- Limpia Parabrisas\n";
            if (checklist.EspejoInterno == "0") itemsFaltantes = itemsFaltantes + "- Espejos Retrovisores Internos\n";
            if (checklist.EspejoExterno == "0") itemsFaltantes = itemsFaltantes + "- Espejos Retrovisores Externos\n";
            if (checklist.NivelAceite == "0") itemsFaltantes = itemsFaltantes + "- Nivel de Aceite\n";
            if (checklist.NivelAgua == "0") itemsFaltantes = itemsFaltantes + "- Nivel de Agua / Refrigerante\n";
            if (checklist.Combustible == "0") itemsFaltantes = itemsFaltantes + "- Combustible\n";
            if (checklist.Claxon == "0") itemsFaltantes = itemsFaltantes + "- Claxon\n";
            if (checklist.AlarmaRetorceso == "0") itemsFaltantes = itemsFaltantes + "- Alarma de retroceso\n";
            if (checklist.RelojesIndicadores == "0") itemsFaltantes = itemsFaltantes + "- Relojes Indicadores / Panel de Control\n";
            if (checklist.Neumaticos == "0") itemsFaltantes = itemsFaltantes + "- Neumáticos\n";
            if (checklist.NeumaticoRepuesto == "0") itemsFaltantes = itemsFaltantes + "- Neumático de repuesto\n";
            if (checklist.Extintor == "0") itemsFaltantes = itemsFaltantes + "- Extintor PQS tipo ABC\n";
            if (checklist.ConosSeguridad == "0") itemsFaltantes = itemsFaltantes + "- Conos de Seguridad (02)\n";
            if (checklist.SogaArrastre == "0") itemsFaltantes = itemsFaltantes + "- Soga de arrastre\n";
            if (checklist.Botiquin == "0") itemsFaltantes = itemsFaltantes + "- Botiquín\n";
            if (checklist.HerramientasLlaves == "0") itemsFaltantes = itemsFaltantes + "- Herramienta y llave de ruedas\n";
            if (checklist.GataPalanca == "0") itemsFaltantes = itemsFaltantes + "- Gata y Palanca\n";
            if (checklist.Triangulo == "0") itemsFaltantes = itemsFaltantes + "- Triángulos de seguridad (02)\n";
            if (checklist.Linterna == "0") itemsFaltantes = itemsFaltantes + "- Linterna\n";
            if (checklist.Cunas == "0") itemsFaltantes = itemsFaltantes + "- Cuñas\n";
            if (checklist.Carroceria == "0") itemsFaltantes = itemsFaltantes + "- Carrocería\n";
            if (checklist.Pertiga == "0") itemsFaltantes = itemsFaltantes + "- Pértiga\n";
            if (checklist.Circulina == "0") itemsFaltantes = itemsFaltantes + "- Circulina\n";

            return itemsFaltantes;
        }

        public string ObtenerComentarios(CheckList checklist)
        {
            string comentarios1 = "";
            string comentarios2 = "";
            string comentarios3 = "";
            string comentarios4 = "";

            string prioridad1 = "";
            string prioridad2 = "";
            string prioridad3 = "";
            string prioridad4 = "";

            if (checklist.Prioridad1 == 0) { prioridad1 = "BAJA"; } else if (checklist.Prioridad1 == 1) { prioridad1 = "MEDIA"; } else if (checklist.Prioridad1 == 2) { prioridad1 = "ALTA"; }
            if (checklist.Prioridad2 == 0) { prioridad2 = "BAJA"; } else if (checklist.Prioridad2 == 1) { prioridad2 = "MEDIA"; } else if (checklist.Prioridad2 == 2) { prioridad2 = "ALTA"; }
            if (checklist.Prioridad3 == 0) { prioridad3 = "BAJA"; } else if (checklist.Prioridad3 == 1) { prioridad3 = "MEDIA"; } else if (checklist.Prioridad3 == 2) { prioridad3 = "ALTA"; }
            if (checklist.Prioridad4 == 0) { prioridad4 = "BAJA"; } else if (checklist.Prioridad4 == 1) { prioridad4 = "MEDIA"; } else if (checklist.Prioridad4 == 2) { prioridad4 = "ALTA"; }

            if (checklist.Observacion1 != "" && checklist.Observacion1 != null) comentarios1 = checklist.Observacion1 + " - PRIORIDAD " + prioridad1;
            if (checklist.Observacion2 != "" && checklist.Observacion2 != null) comentarios2 = checklist.Observacion2 + " - PRIORIDAD " + prioridad2;
            if (checklist.Observacion3 != "" && checklist.Observacion3 != null) comentarios3 = checklist.Observacion3 + " - PRIORIDAD " + prioridad3;
            if (checklist.Observacion4 != "" && checklist.Observacion4 != null) comentarios4 = checklist.Observacion4 + " - PRIORIDAD " + prioridad4;

            string comentarios = comentarios1 + "\n" + comentarios2 + "\n" + comentarios3 + "\n" + comentarios4;
            return comentarios;
        }

        public string ObtenerItemsPresentesBotiquin(CheckList checklist)
        {

            string itemsPresentes = "";

            if (checklist.CajaSoporte == "1") itemsPresentes = itemsPresentes + "- Estado de caja y soporte de botiquin\n";
            if (checklist.Alcohol == "1") itemsPresentes = itemsPresentes + "- Alcohol 70° 500ml\n";
            if (checklist.Jabon == "1") itemsPresentes = itemsPresentes + "- Jabón antiséptico\n";
            if (checklist.Algodon == "1") itemsPresentes = itemsPresentes + "- Algodón 50gr.\n";
            if (checklist.Aposito == "1") itemsPresentes = itemsPresentes + "- Apósito esterilizado 10x10cm.\n";
            if (checklist.Bandas == "1") itemsPresentes = itemsPresentes + "- Bandas adhesivas\n";
            if (checklist.Esparadrapo == "1") itemsPresentes = itemsPresentes + "- Esparadrapo 2.5cm. x 5m.\n";
            if (checklist.Gasas1 == "1") itemsPresentes = itemsPresentes + "- Gasas estériles fraccionadas 10x10 cm.\n";
            if (checklist.Gasas2 == "1") itemsPresentes = itemsPresentes + "- Guantes quirúrgicos esterilizados 7 1/2\n";
            if (checklist.Tijera == "1") itemsPresentes = itemsPresentes + "- Tijeras de punta roma de 3 pulgadas\n";
            if (checklist.Venda == "1") itemsPresentes = itemsPresentes + "- Venda elástica 4 X 5 Y\n";

            return itemsPresentes;
        }

        public string ObtenerItemsFaltantesBotiquin(CheckList checklist)
        {

            string itemsFaltantes = "";

            if (checklist.CajaSoporte == "0") itemsFaltantes = itemsFaltantes + "- Estado de caja y soporte de botiquin\n";
            if (checklist.Alcohol == "0") itemsFaltantes = itemsFaltantes + "- Alcohol 70° 500ml\n";
            if (checklist.Jabon == "0") itemsFaltantes = itemsFaltantes + "- Jabón antiséptico\n";
            if (checklist.Algodon == "0") itemsFaltantes = itemsFaltantes + "- Algodón 50gr.\n";
            if (checklist.Aposito == "0") itemsFaltantes = itemsFaltantes + "- Apósito esterilizado 10x10cm.\n";
            if (checklist.Bandas == "0") itemsFaltantes = itemsFaltantes + "- Bandas adhesivas\n";
            if (checklist.Esparadrapo == "0") itemsFaltantes = itemsFaltantes + "- Esparadrapo 2.5cm. x 5m.\n";
            if (checklist.Gasas1 == "0") itemsFaltantes = itemsFaltantes + "- Gasas estériles fraccionadas 10x10 cm.\n";
            if (checklist.Gasas2 == "0") itemsFaltantes = itemsFaltantes + "- Guantes quirúrgicos esterilizados 7 1/2\n";
            if (checklist.Tijera == "0") itemsFaltantes = itemsFaltantes + "- Tijeras de punta roma de 3 pulgadas\n";
            if (checklist.Venda == "0") itemsFaltantes = itemsFaltantes + "- Venda elástica 4 X 5 Y\n";

            return itemsFaltantes;
        }

        public string ObtenerMensajeSOAT(CheckList checklist)
        {
            string mensaje = "";

            if (checklist.EstadoSOAT != "1")
            {
                if (checklist.NuevaFechaSOAT.ToString().Length > 0)
                {
                    DateTime fecha = (DateTime)checklist.NuevaFechaSOAT;
                    mensaje = "El SOAT se actualizó con fecha " + fecha.ToShortDateString();
                }
                else
                {
                    mensaje = "El SOAT necesita ser actualizado";
                }
            }

            return mensaje;
        }

        public string ObtenerMensajeRevTecnica(CheckList checklist)
        {
            string mensaje = "";

            if (checklist.EstadRevTecnica != "1")
            {
                if (checklist.NuevaFechaRevTecnica.ToString().Length > 0)
                {
                    DateTime fecha = (DateTime)checklist.NuevaFechaRevTecnica;
                    mensaje = "La Revisión Técnica se actualizó con fecha " + fecha.ToShortDateString();
                }
                else
                {
                    mensaje = "La Revisión Técnica necesita ser actualizada";
                }
            }

            return mensaje;
        }

        public string ObtenerDocumentosPresentes(CheckList checklist)
        {
            string presentes = "";

            if (checklist.SOAT == "1") { presentes = presentes + "- SOAT\n"; }
            if (checklist.RevTecnica == "1") { presentes = presentes + "- Revisión Técnica\n"; }
            if (checklist.TarjetaPropiedad == "1") { presentes = presentes + "- Tarjeta de propiedad\n"; }
            if (checklist.CartillaSeguridad == "1") { presentes = presentes + "- Cartilla de Seguridad\n"; }
            if (checklist.CartillaERP == "1") { presentes = presentes + "- Cartilla de ERP\n"; }

            return presentes;
        }

        public string ObtenerDocumentosFaltantes(CheckList checklist)
        {
            string faltantes = "";

            if (checklist.SOAT != "1") { faltantes = faltantes + "- SOAT\n"; }
            if (checklist.RevTecnica != "1") { faltantes = faltantes + "- Revisión Técnica\n"; }
            if (checklist.TarjetaPropiedad != "1") { faltantes = faltantes + "- Tarjeta de propiedad\n"; }
            if (checklist.CartillaSeguridad != "1") { faltantes = faltantes + "- Cartilla de Seguridad\n"; }
            if (checklist.CartillaERP != "1") { faltantes = faltantes + "- Cartilla de ERP\n"; }

            return faltantes;
        }


        public bool ActualizarKilometrajeMantto(CheckList checkList)
        {
            VehiclesApiController vehiclesController = new VehiclesApiController();

            Vehicle vehicle = (Vehicle)vehiclesController.GetVehicleByPlate(checkList.Placa);

            vehicle.KUMantto = checkList.Kilometraje;
            vehicle.FUMantto = checkList.Fecha;

            Debug.Print(JsonConvert.SerializeObject(vehicle));

            vehiclesController.PutVehicle(vehicle.IdVehiculo, vehicle);

            return true;
        }

        public bool Evaluar_ActualizacionesDocumentos(CheckList checkList)
        {
            bool bandera = false;

            if (checkList.BanderaDocumentos == 1)
            {
                if (checkList.NuevaFechaSOAT != null || checkList.NuevaFechaRevTecnica != null)
                {
                    Debug.Print("Se Actualizaran documentos");
                    bandera = true;
                }
            }

            return bandera;
        }

        public bool Guardar_ActualizacionesDocumentos(CheckList checkList)
        {
            DocumentsApiController documentsController = new DocumentsApiController();

            Document documento = db.Documents.FirstOrDefault(x => x.Placa == checkList.Placa);

            if (checkList.NuevaFechaSOAT != null)
            {
                Debug.Print("Se Actualizaran fecha del SOAT al día: " + checkList.NuevaFechaSOAT);
                documento.FechaSOAT = (DateTime)checkList.NuevaFechaSOAT;
            }

            if (checkList.NuevaFechaRevTecnica != null)
            {
                Debug.Print("Se Actualizaran fecha de la RevTecnica al día: " + checkList.NuevaFechaRevTecnica);
                documento.FechaRevTecnica = (DateTime)checkList.NuevaFechaRevTecnica;
            }

            Debug.Print(JsonConvert.SerializeObject(documento));

            documentsController.PutDocument(documento.IdDocumento, documento);

            return true;
        }
    }
}
