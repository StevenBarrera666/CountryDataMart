using Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.PeerToPeer;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;
namespace LogicBo
{
    public class InspectionsBo
    {

        #region Properties
        private readonly Entity.ModelEntities entities = new Entity.ModelEntities();
        private readonly ADO.ExecuteProcedures executeProcedures = new ADO.ExecuteProcedures();
        #endregion
        public DataTable GetStock()
        {
            var result = executeProcedures.DataTable("ENEL_CargarInventario", null);

            return result;
        }

        public DataTable GetFichasActivas()
        {
            var result = executeProcedures.DataTable("SP_LOADFICHASACTIVAS", null);

            return result;
        }

        /// <summary>
        /// Ejecuta un Stored Procedure o consulta SQL y retorna la primera columna de la primera fila.
        /// Ideal para operaciones INSERT que retornan SCOPE_IDENTITY() o consultas COUNT/MAX.
        /// </summary>
        /// <param name="storedProcedureOrQuery">Nombre del Stored Procedure o consulta SQL</param>
        /// <param name="parameters">Lista de SqlParameters (opcional)</param>
        /// <param name="commandType">Tipo de comando (por defecto StoredProcedure)</param>
        /// <returns>Objeto con el valor retornado, o null si no hubo resultado</returns>
        public object ExecuteScalar(string storedProcedureOrQuery, List<SqlParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            object result = null;

             string _connectionString = ConfigurationManager.AppSettings["conexP"].ToString();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureOrQuery, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = 120; // 2 minutos de timeout para procesos pesados

                    // Agregar parámetros si existen
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            // Prevenir problemas si el Value viene nulo
                            if (param.Value == null)
                            {
                                param.Value = DBNull.Value;
                            }
                            command.Parameters.Add(param);
                        }
                    }

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        // Registra o propaga la excepción con contexto claro
                        throw new Exception($"Error al ejecutar ExecuteScalar en '{storedProcedureOrQuery}': {ex.Message}", ex);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Versión genérica que castea automáticamente al tipo de dato esperado (ej: int, long, string).
        /// </summary>
        public T ExecuteScalar<T>(string storedProcedureOrQuery, List<SqlParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            object rawResult = ExecuteScalar(storedProcedureOrQuery, parameters, commandType);

            if (rawResult == null || rawResult == DBNull.Value)
            {
                return default;
            }

            // Convierte de forma segura (ej. de decimal/long devuelto por SQL a int de C#)
            Type targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            return (T)Convert.ChangeType(rawResult, targetType);
        }

        public DataTable GetTechnicalReportsIndex(int idHeadquarter)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=idHeadquarter},
            };
            var result = executeProcedures.DataTable("ENEL_LoadTechReportsByEnterprise", parameters);
            return result;
        }
        public DataTable GetProceduresReportsIndex(int idHeadquarter)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=idHeadquarter},
            };
            var result = executeProcedures.DataTable("ENEL_LoadProceduresReports", parameters);
            return result;
        }

        public DataTable GetLoadFATraces()
        {
            var result = executeProcedures.DataTable("SP_LOADTRACEDDM", null);
            return result;
        }

        public DataTable GetLoadFATracesDocumentar()
        {
            var result = executeProcedures.DataTable("SP_LOADTRACEDDM_DOCUMENTAR", null);
            return result;
        }

        public DataTable GetLoadFATracesAurorizar()
        {
            var result = executeProcedures.DataTable("SP_LOADTRACEDDM_AUTORIZAR", null);
            return result;
        }

        public DataTable GetLoadFATracesFacturar()
        {
            var result = executeProcedures.DataTable("SP_LOADTRACEDDM_FACTURAR", null);
            return result;
        }

        public DataTable GetlistEmpleados()
        {
            var result = executeProcedures.DataTable("sp_ListarEmpleados", null);
            return result;
        }

        public DataTable GetlistClientes()
        {
            var result = executeProcedures.DataTable("sp_ListarClientesDatosPersonales", null);
            return result;
        }

        public DataTable GetlistProveedores()
        {
            var result = executeProcedures.DataTable("sp_ListarProveedoresDatosPersonales", null);
            return result;
        }


        public DataTable ObtenerPerfilEmpleado(int? empleadoId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Id", SqlDbType=SqlDbType.Int,Value=empleadoId},
            };
            var result = executeProcedures.DataTable("SP_LOADEMPLEADO", parameters);
            return result;
        }

        public DataTable ObtenerPerfilEmpleadoFichas(int? empleadoId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Id", SqlDbType=SqlDbType.Int,Value=empleadoId},
            };
            var result = executeProcedures.DataTable("SP_LOADEMPLEADOFICHAS", parameters);
            return result;
        }

        public DataTable RegistrarTA(decimal fareWithTaxesTA, decimal fareWithOutTaxesTA, int fileIdTA, string mstrUser, decimal taxValueTA, int fileDetailIdTkt)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="FareWithTaxesTA", SqlDbType=SqlDbType.Decimal,Value=fareWithTaxesTA},
                new SqlParameter(){ ParameterName="FareWithOutTaxesTA", SqlDbType=SqlDbType.Decimal,Value=fareWithOutTaxesTA},
                new SqlParameter(){ ParameterName="FileIdTA", SqlDbType=SqlDbType.Int,Value=fileIdTA},
                new SqlParameter(){ ParameterName="mstrUserTA", SqlDbType=SqlDbType.VarChar,Value=(object)mstrUser ?? DBNull.Value},
                new SqlParameter(){ ParameterName="TaxValueTA", SqlDbType=SqlDbType.Decimal,Value=taxValueTA},
                new SqlParameter(){ ParameterName="FileDetailIDtkt", SqlDbType=SqlDbType.Int,Value=fileDetailIdTkt},
            };
            var result = executeProcedures.DataTable("SP_INSERTFILEDETAILTAFROMDM", parameters);
            return result;
        }

        public DataTable FileFacturado(int fileDetailId,string ReferenceNumber,string InvoiceNumber,string Estado)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="FileDetailid", SqlDbType=SqlDbType.Int,Value=fileDetailId},
                new SqlParameter(){ ParameterName="ReferenceNumber", SqlDbType=SqlDbType.VarChar,Value=ReferenceNumber},
                new SqlParameter(){ ParameterName="InvoiceNumber", SqlDbType=SqlDbType.VarChar,Value=InvoiceNumber},
                new SqlParameter(){ ParameterName="Estado", SqlDbType=SqlDbType.VarChar,Value=Estado}
            };
            return executeProcedures.DataTable("SP_FileFacturado", parameters);
        }

        public DataTable CrearRemision(int FileDetailIDTKT,
            int FileDetailIDTA,
            int FileIDTA,
            string mstrUser
            )
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="FileDetailIDTKT", SqlDbType=SqlDbType.Decimal,Value=FileDetailIDTKT},
                new SqlParameter(){ ParameterName="FileDetailIDTA", SqlDbType=SqlDbType.Decimal,Value=FileDetailIDTA},
                new SqlParameter(){ ParameterName="FileIDTA", SqlDbType=SqlDbType.Int,Value=FileIDTA},
                new SqlParameter(){ ParameterName="mstrUserTA", SqlDbType=SqlDbType.VarChar,Value=(object)mstrUser ?? DBNull.Value}
            };
            var result = executeProcedures.DataSet("SP_CREATEREFERENCEFROMDM", parameters);
            return result.Tables[5];
        }

        
        
        
     

        public DataTable ObtenerPerfilEmpleadoDataTable(int? empleadoId, int? fichaid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="EmpleadoId", SqlDbType=SqlDbType.Int,Value=empleadoId},
                 new SqlParameter(){ ParameterName="FichaId", SqlDbType=SqlDbType.Int,Value=fichaid},
            };
            var result = executeProcedures.DataTable("sp_ConsultarPerfilEmpleado", parameters);
            return result;
        }

        public DataSet ObtenerPerfilEmpleadoDataSet(int? empleadoId, int? fichaid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="EmpleadoId", SqlDbType=SqlDbType.Int,Value=empleadoId},
                new SqlParameter(){ ParameterName="FichaId", SqlDbType=SqlDbType.Int,Value=(object)fichaid ?? DBNull.Value},
            };
            return executeProcedures.DataSet("sp_ConsultarPerfilEmpleado", parameters);
        }

        public DataTable ObtenerComprobantesEmpleadoDataTable(int? empleadoId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="EmpleadoId", SqlDbType=SqlDbType.Int,Value=empleadoId},
            };
            var result = executeProcedures.DataTable("sp_ConsultarComprobantesEmpleado", parameters);
            return result;
        }
        public DataTable ObtenerTarifasClienteDataTable(int clienteId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter { ParameterName = "@ClienteId", SqlDbType = SqlDbType.Int, Value = clienteId }
    };

            return executeProcedures.DataTable("sp_ConsultarTarifasCliente", parameters);
        }
        public DataTable ObtenerPerfilClienteDataTable(int clienteId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@ClienteId", SqlDbType = SqlDbType.Int, Value = clienteId }
            };
            var result = executeProcedures.DataTable("sp_ConsultarPerfilCliente", parameters);
            return result;
        }

        public DataTable ObtenerPerfilProveedorDataTable(int clienteId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@ProveedorId", SqlDbType = SqlDbType.Int, Value = clienteId }
            };
            var result = executeProcedures.DataTable("sp_ConsultarPerfilProveedor", parameters);
            return result;
        }

        public DataTable RegistrarOActualizarCliente(ClienteRegistroViewModel model, string logoFinal)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
    {
        // Identificador (0 = Insertar, >0 = Actualizar)
        new SqlParameter { ParameterName = "@ClienteId", SqlDbType = SqlDbType.Int, Value = model.Id },
        new SqlParameter { ParameterName = "@EstadoEtiqueta", SqlDbType = SqlDbType.NVarChar, Value = model.EstadoEtiqueta ?? "Activo" },
        new SqlParameter { ParameterName = "@LogoUrl", SqlDbType = SqlDbType.NVarChar, Value = (object)logoFinal ?? DBNull.Value },

        // 1. Datos Personales
        new SqlParameter { ParameterName = "@NaturalezaId", SqlDbType = SqlDbType.Int, Value = model.NaturalezaId },
        new SqlParameter { ParameterName = "@TipoId", SqlDbType = SqlDbType.Int, Value = model.TipoId },
        new SqlParameter { ParameterName = "@RazonSocial", SqlDbType = SqlDbType.NVarChar, Value = model.RazonSocial },
        new SqlParameter { ParameterName = "@EstablecimientoComercial", SqlDbType = SqlDbType.NVarChar, Value = (object)model.EstablecimientoComercial ?? DBNull.Value },
        new SqlParameter { ParameterName = "@NombreComercial", SqlDbType = SqlDbType.NVarChar, Value = model.NombreComercial },
        new SqlParameter { ParameterName = "@TipoDocumentoId", SqlDbType = SqlDbType.Int, Value = model.TipoDocumentoId },
        new SqlParameter { ParameterName = "@NumeroDocumento", SqlDbType = SqlDbType.NVarChar, Value = model.NumeroDocumento },
        new SqlParameter { ParameterName = "@Telefono1", SqlDbType = SqlDbType.NVarChar, Value = model.Telefono1 },
        new SqlParameter { ParameterName = "@Telefono2", SqlDbType = SqlDbType.NVarChar, Value = (object)model.Telefono2 ?? DBNull.Value },
        new SqlParameter { ParameterName = "@CorreoPrincipal", SqlDbType = SqlDbType.NVarChar, Value = model.CorreoPrincipal },
        new SqlParameter { ParameterName = "@Correo2", SqlDbType = SqlDbType.NVarChar, Value = (object)model.Correo2 ?? DBNull.Value },
        new SqlParameter { ParameterName = "@Direccion", SqlDbType = SqlDbType.NVarChar, Value = model.Direccion },

        // 2. Datos Ubicación
        new SqlParameter { ParameterName = "@Origen", SqlDbType = SqlDbType.NVarChar, Value = model.Origen ?? "Nacional" },
        new SqlParameter { ParameterName = "@CiudadId", SqlDbType = SqlDbType.Int, Value = model.CiudadId },

        // 3. Cobros (Solo el tipo general; las tarifas van en el XML)
        new SqlParameter { ParameterName = "@TipoCobroId", SqlDbType = SqlDbType.Int, Value = model.TipoCobroId },

        // 4. Datos Fiscales
        new SqlParameter { ParameterName = "@ActividadEconomicaId", SqlDbType = SqlDbType.Int, Value = model.ActividadEconomicaId },
        new SqlParameter { ParameterName = "@IndustriaId", SqlDbType = SqlDbType.Int, Value = model.IndustriaId },
        new SqlParameter { ParameterName = "@RutaCamaraComercio", SqlDbType = SqlDbType.NVarChar, Value = (object)model.RutaCamaraComercio ?? DBNull.Value },
        new SqlParameter { ParameterName = "@RutaRut", SqlDbType = SqlDbType.NVarChar, Value = (object)model.RutaRut ?? DBNull.Value }
    };

            // Serialización XML de las tarifas
            string tarifasXml = null;
            if (model.Tarifas != null && model.Tarifas.Count > 0)
            {
                var xElement = new System.Xml.Linq.XElement("Tarifas",
                    model.Tarifas.Select(t => new System.Xml.Linq.XElement("Item",
                        new System.Xml.Linq.XAttribute("CategoriaTarifaId", t.CategoriaTarifaId),
                        new System.Xml.Linq.XAttribute("TarifaAdministrativa", t.TarifaAdministrativa.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                        new System.Xml.Linq.XAttribute("Descuento", t.Descuento.ToString(System.Globalization.CultureInfo.InvariantCulture))
                    ))
                );
                tarifasXml = xElement.ToString();
            }

            parameters.Add(new SqlParameter
            {
                ParameterName = "@TarifasXml",
                SqlDbType = SqlDbType.Xml,
                Value = (object)tarifasXml ?? DBNull.Value
            });

            var result = executeProcedures.DataTable("sp_RegistrarOActualizarCliente", parameters);
            return result;
        }

        public DataTable RegistrarOActualizarProveedor(ProveedorRegistroViewModel model, string logoFinal)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
        new SqlParameter { ParameterName = "@ProveedorId", SqlDbType = SqlDbType.Int, Value = model.Id },

        new SqlParameter { ParameterName = "@EstadoEtiqueta", SqlDbType = SqlDbType.NVarChar, Value = model.EstadoEtiqueta ?? "Activo" },
        new SqlParameter { ParameterName = "@LogoUrl", SqlDbType = SqlDbType.NVarChar, Value = (object)logoFinal ?? DBNull.Value },

        // 1. Datos Personales
        new SqlParameter { ParameterName = "@NaturalezaId", SqlDbType = SqlDbType.Int, Value = model.NaturalezaId },
        new SqlParameter { ParameterName = "@TipoId", SqlDbType = SqlDbType.Int, Value = model.TipoId },
        new SqlParameter { ParameterName = "@RazonSocial", SqlDbType = SqlDbType.NVarChar, Value = model.RazonSocial },
        new SqlParameter { ParameterName = "@EstablecimientoComercial", SqlDbType = SqlDbType.NVarChar, Value = (object)model.EstablecimientoComercial ?? DBNull.Value },
        new SqlParameter { ParameterName = "@NombreComercial", SqlDbType = SqlDbType.NVarChar, Value = model.NombreComercial },
        new SqlParameter { ParameterName = "@TipoDocumentoId", SqlDbType = SqlDbType.Int, Value = model.TipoDocumentoId },
        new SqlParameter { ParameterName = "@NumeroDocumento", SqlDbType = SqlDbType.NVarChar, Value = model.NumeroDocumento },
        new SqlParameter { ParameterName = "@Telefono1", SqlDbType = SqlDbType.NVarChar, Value = model.Telefono1 },
        new SqlParameter { ParameterName = "@Telefono2", SqlDbType = SqlDbType.NVarChar, Value = (object)model.Telefono2 ?? DBNull.Value },
        new SqlParameter { ParameterName = "@CorreoPrincipal", SqlDbType = SqlDbType.NVarChar, Value = model.CorreoPrincipal },
        new SqlParameter { ParameterName = "@Correo2", SqlDbType = SqlDbType.NVarChar, Value = (object)model.Correo2 ?? DBNull.Value },
        new SqlParameter { ParameterName = "@Direccion", SqlDbType = SqlDbType.NVarChar, Value = model.Direccion },

        // 2. Datos Ubicación
        new SqlParameter { ParameterName = "@Origen", SqlDbType = SqlDbType.NVarChar, Value = model.Origen ?? "Nacional" },
        new SqlParameter { ParameterName = "@CiudadId", SqlDbType = SqlDbType.Int, Value = model.CiudadId },

        // 3. Cobros (Valores directos)
        new SqlParameter { ParameterName = "@TipoCobroId", SqlDbType = SqlDbType.Int, Value = model.TipoCobroId },
        new SqlParameter { ParameterName = "@CategoriaTarifaId", SqlDbType = SqlDbType.Int, Value = model.CategoriaTarifaId },
        new SqlParameter { ParameterName = "@TarifaAdministrativa", SqlDbType = SqlDbType.Decimal, Value = model.TarifaAdministrativa },
        new SqlParameter { ParameterName = "@Descuentos", SqlDbType = SqlDbType.Decimal, Value = model.Descuentos },

        // 4. Datos Fiscales y Certificado Bancario
        new SqlParameter { ParameterName = "@ActividadEconomicaId", SqlDbType = SqlDbType.Int, Value = model.ActividadEconomicaId },
        new SqlParameter { ParameterName = "@IndustriaId", SqlDbType = SqlDbType.Int, Value = model.IndustriaId },
        new SqlParameter { ParameterName = "@RutaCamaraComercio", SqlDbType = SqlDbType.NVarChar, Value = (object)model.RutaCamaraComercio ?? DBNull.Value },
        new SqlParameter { ParameterName = "@RutaRut", SqlDbType = SqlDbType.NVarChar, Value = (object)model.RutaRut ?? DBNull.Value },
        new SqlParameter { ParameterName = "@RutaCertificadoBancario", SqlDbType = SqlDbType.NVarChar, Value = (object)model.RutaCertificadoBancario ?? DBNull.Value }
    };

            return executeProcedures.DataTable("sp_RegistrarOActualizarProveedor", parameters);
        }
        public DataTable RegistrarPerfilEmpleado(EmpleadoRegistroViewModel model, string fotoFinal)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                 new SqlParameter(){ ParameterName="@EmpleadoId", SqlDbType=SqlDbType.Int,Value = model.Id},
                        new SqlParameter(){ ParameterName="@Nombres", SqlDbType=SqlDbType.NVarChar, Value = model.Nombres},
                        new SqlParameter(){ ParameterName="@Apellidos", SqlDbType=SqlDbType.NVarChar, Value = model.Apellidos},
                        new SqlParameter(){ ParameterName="@FotoUrl", SqlDbType=SqlDbType.NVarChar, Value = fotoFinal},
                        new SqlParameter(){ ParameterName="@FichaCodigo", SqlDbType=SqlDbType.NVarChar, Value = model.FichaCodigo ?? "F1"},
                        new SqlParameter(){ ParameterName="@EtiquetaEstado", SqlDbType=SqlDbType.NVarChar, Value = model.EtiquetaEstado ?? "F1"},
                        new SqlParameter(){ ParameterName="@TipoIdentificacion", SqlDbType=SqlDbType.NVarChar, Value = model.TipoIdentificacion},
                        new SqlParameter(){ ParameterName="@NumeroIdentificacion", SqlDbType=SqlDbType.NVarChar, Value = model.NumeroIdentificacion},
                        new SqlParameter(){ ParameterName="@CorreoCorporativo", SqlDbType=SqlDbType.NVarChar, Value = model.CorreoCorporativo},
                        new SqlParameter(){ ParameterName="@TelefonoParticular", SqlDbType=SqlDbType.NVarChar,Value = (object)model.TelefonoParticular ?? DBNull.Value},
                        new SqlParameter(){ ParameterName="@TelefonoOficina", SqlDbType=SqlDbType.NVarChar, Value = (object)model.TelefonoOficina ?? DBNull.Value},
                        new SqlParameter(){ ParameterName="@FechaNacimiento", SqlDbType=SqlDbType.Date,Value = model.FechaNacimiento},
                        new SqlParameter(){ ParameterName="@Direccion", SqlDbType=SqlDbType.NVarChar,Value = (object)model.Direccion ?? DBNull.Value},
                        new SqlParameter(){ ParameterName="@CargoId", SqlDbType=SqlDbType.Int,Value = model.CargoId},
                        new SqlParameter(){ ParameterName="@AreaId", SqlDbType=SqlDbType.Int,Value = model.AreaId},
                        new SqlParameter(){ ParameterName="@EpsPrevisionId", SqlDbType=SqlDbType.Int,Value = model.EpsPrevisionId},
                        new SqlParameter(){ ParameterName="@Division", SqlDbType=SqlDbType.NVarChar,Value = model.Division},
                        new SqlParameter(){ ParameterName="@Empresa", SqlDbType=SqlDbType.NVarChar, Value = model.Empresa},
                        new SqlParameter(){ ParameterName="@CentroCostos", SqlDbType=SqlDbType.NVarChar, Value = model.CentroCostos},
                        new SqlParameter(){ ParameterName="@Supervisor", SqlDbType=SqlDbType.NVarChar, Value = (object)model.Supervisor ?? DBNull.Value},
                        new SqlParameter(){ ParameterName="@Suplente", SqlDbType=SqlDbType.NVarChar, Value = string.IsNullOrWhiteSpace(model.Suplente) ? "Sin Suplencia" : model.Suplente},
                        new SqlParameter(){ ParameterName="@TipoContrato", SqlDbType=SqlDbType.NVarChar,Value = model.TipoContrato},
                        new SqlParameter(){ ParameterName="@JornadaLaboral", SqlDbType=SqlDbType.NVarChar,Value = model.JornadaLaboral},
                        new SqlParameter(){ ParameterName="@FechaIngreso", SqlDbType=SqlDbType.Date,Value = model.FechaIngreso},
                        new SqlParameter(){ ParameterName="@SaldoVacacionesDias", SqlDbType=SqlDbType.Decimal,Value = model.SaldoVacacionesDias},
                        new SqlParameter(){ ParameterName="@NuevoEmpleadoId", SqlDbType=SqlDbType.Int,Value =0}

            };
            var result = executeProcedures.DataTable("sp_RegistrarEmpleado", parameters);
            return result;
        }



        public Dictionary<string, string> GetLoadGDS()
        {
            var result = executeProcedures.DataTable("SP_LOAD_GDS", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["Name"].ToString());
        }

        public Dictionary<string, string> GetLoadCanal()
        {
            var result = executeProcedures.DataTable("SP_LOADCANAL", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["canal"].ToString());
        }

        public Dictionary<string, string> GetLoadFormasPago()
        {
            var result = executeProcedures.DataTable("SP_LOADPaymentMethods", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["Name"].ToString());
        }

        public Dictionary<string, string> GetLoadPorcentaje()
        {
            var result = executeProcedures.DataTable("SP_LOAD_TA_Porcentajes", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["porcentaje"].ToString());
        }

        public Dictionary<string, string> GetLoadModalidad()
        {
            var result = executeProcedures.DataTable("SP_LOADMODALIDAD", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["descripcion"].ToString());
        }


        public bool GuardarDetalleDocumentacion(DocumentacionFilaModel model)
        {
            bool resultado = false;
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="opc", SqlDbType=SqlDbType.Int,Value=2},
                new SqlParameter(){ ParameterName="FileId", SqlDbType=SqlDbType.Int,Value=model.FileId},
                new SqlParameter(){ ParameterName="CustomerId", SqlDbType=SqlDbType.Int,Value=model.CustomerId},
                new SqlParameter(){ ParameterName="IdCanal", SqlDbType=SqlDbType.Int,Value=model.IdCanal},
                new SqlParameter(){ ParameterName="IdModalidad", SqlDbType=SqlDbType.Int,Value=model.IdModalidad},
                new SqlParameter(){ ParameterName="Solicitud", SqlDbType=SqlDbType.VarChar,Value=model.Solicitud},
                new SqlParameter(){ ParameterName="IdCentroCostos", SqlDbType=SqlDbType.Int,Value=model.IdCentroCostos},
                new SqlParameter(){ ParameterName="IdRazonViaje", SqlDbType=SqlDbType.Int,Value=model.IdRazonViaje},
                new SqlParameter(){ ParameterName="OrdenServicio", SqlDbType=SqlDbType.VarChar,Value=model.OrdenServicio}
                };

            var result = executeProcedures.DataTable("SP_FileDocumentado", parameters);
            if (result.Rows.Count == 1)
                resultado = true;
            return resultado;
        }

        public bool GuardarDetalleAutorizacion(AutorizacionFilaModel model)
        {
            bool resultado = false;
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="opc", SqlDbType=SqlDbType.Int,Value=2},
                new SqlParameter(){ ParameterName="FileId", SqlDbType=SqlDbType.Int,Value=model.FileId},
                new SqlParameter(){ ParameterName="CustomerId", SqlDbType=SqlDbType.Int,Value=model.CustomerId},
                new SqlParameter(){ ParameterName="FileDetailId", SqlDbType=SqlDbType.Int,Value=model.FileDetailID},
                new SqlParameter(){ ParameterName="PaymentMethodId", SqlDbType=SqlDbType.Int,Value=model.PaymentMethodId},
                new SqlParameter(){ ParameterName="PaymentCardId", SqlDbType=SqlDbType.VarChar,Value=model.PaymentCardId},
                new SqlParameter(){ ParameterName="FareTA", SqlDbType=SqlDbType.Decimal,Value=model.FareTA},
                new SqlParameter(){ ParameterName="IVATA", SqlDbType=SqlDbType.Decimal,Value=model.IVATA},
                new SqlParameter(){ ParameterName="TotalTA", SqlDbType=SqlDbType.Decimal,Value=model.FareWithTaxes},
                new SqlParameter(){ ParameterName="PedidoAgencia", SqlDbType=SqlDbType.VarChar,Value=(object)model.PedidoAgencia?.Trim() ?? DBNull.Value},
                new SqlParameter(){ ParameterName="VoucherAgencia", SqlDbType=SqlDbType.VarChar,Value=(object)model.VoucherAgencia?.Trim() ?? DBNull.Value},
                new SqlParameter(){ ParameterName="AutorizacionAgencia", SqlDbType=SqlDbType.VarChar,Value=(object)model.AutorizacionAgencia?.Trim() ?? DBNull.Value}
            };
            var result = executeProcedures.DataTable("SP_FileAutorizado", parameters);
            if (result.Rows.Count == 1)
                resultado = true;
            return resultado;
        }



        public Dictionary<string, string> GetLoadCentroCostos(int CustomerID)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="CustomerID", SqlDbType=SqlDbType.Int,Value=CustomerID},
            };
            var result = executeProcedures.DataTable("SP_LOADCOSTCENTERSBATCUSTOMERID", parameters);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["descripcion"].ToString());
        }

        public Dictionary<string, string> GetLoadTc(int CustomerID)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="CustomerID", SqlDbType=SqlDbType.Int,Value=CustomerID},
            };
            var result = executeProcedures.DataTable("SP_LOADTCBYCUSTOMER", parameters);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["Number"].ToString());
        }

        public Dictionary<string, string> GetLoadReasonCode(int CustomerID)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="CustomerID", SqlDbType=SqlDbType.Int,Value=CustomerID},
            };
            var result = executeProcedures.DataTable("SP_LOADREASONCODECUSTOMERID", parameters);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["descripcion"].ToString());
        }


        public DataTable GetLegalityReportsIndex(int idHeadquarter)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=idHeadquarter},
            };
            var result = executeProcedures.DataTable("ENEL_LoadLegalityReports", parameters);
            return result;
        }
        public DataTable GetFormatsReportsIndex(int idHeadquarter)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=idHeadquarter},
            };
            var result = executeProcedures.DataTable("ENEL_LoadFormatsReports", parameters);
            return result;
        }
        public DataTable GetCertificatesReportsIndex(int idHeadquarter)
        {

            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=idHeadquarter},
            };
            var result = executeProcedures.DataTable("ENEL_LoadCertificatesReports", parameters);
            return result;
        }
        public Dictionary<string, string> GetFactorByElement(string rfid, int headquarterid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="rfid", SqlDbType=SqlDbType.VarChar,Value=rfid},
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headquarterid},
            };
            var result = executeProcedures.DataTable("ENEL_LoadFactorsByRFID", parameters);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["factor"].ToString());
        }
        public DataTable GetStockByHeadquarter(int headquarterId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="SedeId", SqlDbType=SqlDbType.Int,Value=headquarterId}
                 };
            var result = executeProcedures.DataTable("ENEL_LoadStockBySede", parameters);

            return result;
        }
        public Dictionary<string, string> GetTypeReportDictionary()
        {
            var result = executeProcedures.DataTable("ENEL_LOADTECHNICALREPORTSTYPE", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["tipo"].ToString());
        }
        public Dictionary<string, string> GetFeaturesDictionary()
        {
            var result = executeProcedures.DataTable("ENEL_LOADFEATURES", null);
            return result.AsEnumerable().ToDictionary(row => row["id"].ToString(), row => row["Descripcion"].ToString());
        }
        public DataTable GetDatabyElement(int equipementId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="IDEncabezado", SqlDbType=SqlDbType.Int,Value=equipementId}
                 };
            var result = executeProcedures.DataTable("ENEL_CreateHV", parameters);

            return result;
        }
        public Dictionary<string, string> GetAreaDictionary()
        {
            var result = executeProcedures.DataTable("ENEL_LoadAreas", null);
            return result.AsEnumerable().ToDictionary(row => row["ID"].ToString(), row => row["Descripcion"].ToString());
        }
        public Dictionary<string, string> GetResourceByHeadquarter(int headquarterId)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headquarterId}
                 };
            var result = executeProcedures.DataTable("ENEL_LoadEmployee", parameters);
            return result.AsEnumerable().ToDictionary(row => row["ID"].ToString(), row => row["Inspector"].ToString());
        }
        public bool Unsuscribe(string tag, string comment)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=tag},
                new SqlParameter(){ ParameterName="Description", SqlDbType=SqlDbType.VarChar,Value=comment},
                 };
            var result = executeProcedures.DataTable("ENEL_RemoveFromStock", parameters);
            if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                throw new Exception(result.Rows[0][1].ToString());

            return true;
        }
        public bool EditEquipment(string RFID, int areaid, int empleadoid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=RFID},
                new SqlParameter(){ ParameterName="AreaID", SqlDbType=SqlDbType.Int,Value=areaid},
                new SqlParameter(){ ParameterName="EmpleadoID", SqlDbType=SqlDbType.Int,Value=empleadoid},
            };
                var result = executeProcedures.DataTable("ENEL_UpdateEquipment", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public DataTable GetInfoEquipment(string RFID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=RFID},
            };
                var result = executeProcedures.DataTable("ENEL_LoadInfoElementsByRFID", parameters);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public DataTable SearchManagementOfFindings(int headQuarterTypeid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="SedeID", SqlDbType=SqlDbType.Int,Value=headQuarterTypeid},
        };
            var result = executeProcedures.DataTable("ENEL_LOADManagementsByElement", parameters);
            return result;


        }


        public DataTable SearchDataFromFile(int Fileid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="fileid", SqlDbType=SqlDbType.Int,Value=Fileid},
        };
            var result = executeProcedures.DataTable("SP_LOADTRACEDDMVyFileid", parameters);
            return result;
        }


        public DataTable searchCCByFileid(int Fileid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="fileid", SqlDbType=SqlDbType.Int,Value=Fileid},
        };
            var result = executeProcedures.DataTable("SP_LOADCCByFileid", parameters);
            return result;
        }

        public DataTable searchIDEByFileid(int Fileid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="fileid", SqlDbType=SqlDbType.Int,Value=Fileid},
        };
            var result = executeProcedures.DataTable("SP_LOADIDEByFileid", parameters);
            return result;
        }

        public DataTable listCustomers()
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="intTravelAgencyBranchID", SqlDbType=SqlDbType.Int,Value=901},
                    new SqlParameter(){ ParameterName="strDocumentNumber", SqlDbType=SqlDbType.VarChar,Value='%'},
                    new SqlParameter(){ ParameterName="strFirstName", SqlDbType=SqlDbType.VarChar,Value='%'},
                    new SqlParameter(){ ParameterName="strLastName", SqlDbType=SqlDbType.VarChar,Value='%'},
                    new SqlParameter(){ ParameterName="DocumentTypeID", SqlDbType=SqlDbType.Int  ,Value=0}
            };
            var result = executeProcedures.DataTable("sp_ListCustomers", parameters);
            return result;
        }
        public DataTable addCustomerToFile(int customerID, int fileId, string userName)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="intFileID", SqlDbType=SqlDbType.Int,Value=fileId},
                    new SqlParameter(){ ParameterName="intCurrentID", SqlDbType=SqlDbType.Int,Value=customerID},
                    new SqlParameter(){ ParameterName="UserName", SqlDbType=SqlDbType.VarChar,Value=userName}
            };
            var result = executeProcedures.DataTable("sp_UpdatefCUtToFiles", parameters);
            return result;
        }

        public DataTable searchCustomers(int Fileid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="Fileid", SqlDbType=SqlDbType.Int,Value=Fileid},
        };
            var result = executeProcedures.DataTable("SP_LISTCUSTOMERBYFILEID", parameters);
            return result;
        }

        public DataTable SearchSchedulerInspections(int headQuarterTypeid, int Typeid, int ElementID)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headQuarterTypeid},
                    new SqlParameter(){ ParameterName="Typeid", SqlDbType=SqlDbType.Int,Value=Typeid},
                    new SqlParameter(){ ParameterName="ElementID", SqlDbType=SqlDbType.Int,Value=ElementID},
        };
            var result = executeProcedures.DataTable("ENEL_LOADSCHEINSPECTIONS", parameters);
            return result;


        }
        public DataTable SearchSchedulerInspectionsDetails(int headQuarterTypeid, int monthid, int ElementID, int yearid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headQuarterTypeid},
                    new SqlParameter(){ ParameterName="@monthid", SqlDbType=SqlDbType.Int,Value=monthid},
                    new SqlParameter(){ ParameterName="@ElementID", SqlDbType=SqlDbType.Int,Value=ElementID},
                    new SqlParameter(){ ParameterName="@yearid", SqlDbType=SqlDbType.Int,Value=yearid},

        };
            var result = executeProcedures.DataTable("ENEL_LOADSCHEINSPECTIONSDETAILS", parameters);
            return result;
        }
        public DataTable ManagementMaintenanceEquipment(int headQuarterTypeid)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="SedeID", SqlDbType=SqlDbType.Int,Value=headQuarterTypeid},
        };
            var result = executeProcedures.DataTable("ENEL_LOADClosedManagementsByElement", parameters);
            return result;
        }
        public DataTable SearchFindingsByElement(int ElementID)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter(){ ParameterName="ElementID", SqlDbType=SqlDbType.Int,Value=ElementID},
        };
            var result = executeProcedures.DataTable("ENEL_SearchFindingsByElement", parameters);
            return result;


        }
        public DataTable LoadDetailsEquipment(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                     new SqlParameter(){ ParameterName="id", SqlDbType=SqlDbType.Int,Value=id},
        };
            var result = executeProcedures.DataTable("ENEL_LOADELEMENTACTION", parameters);
            return result;
        }

        public bool FeaturesCreate(int headQuarterType, int cbxFeatures, string name, string file)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="TipoSedeid", SqlDbType=SqlDbType.Int,Value=headQuarterType},
                new SqlParameter(){ ParameterName="CaracterizacionDocid", SqlDbType=SqlDbType.Int,Value=cbxFeatures},
                new SqlParameter(){ ParameterName="Nombre", SqlDbType=SqlDbType.VarChar,Value=name},
                new SqlParameter(){ ParameterName="Archivo", SqlDbType=SqlDbType.VarChar,Value=file},
            };
                var result = executeProcedures.DataTable("ENEL_CreateFilesByFeatures", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return bool.Parse(result?.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool TechInfoCreate(int headQuarterType, int Elementoid, string marca, string modelo, string observaciones, string file)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headQuarterType},
                new SqlParameter(){ ParameterName="Elementoid", SqlDbType=SqlDbType.Int,Value=Elementoid},
                new SqlParameter(){ ParameterName="Marca", SqlDbType=SqlDbType.VarChar,Value=marca},
                new SqlParameter(){ ParameterName="Modelo", SqlDbType=SqlDbType.VarChar,Value=modelo},
                new SqlParameter(){ ParameterName="Observaciones", SqlDbType=SqlDbType.VarChar,Value=observaciones},
                new SqlParameter(){ ParameterName="Ficha", SqlDbType=SqlDbType.VarChar,Value=file},
            };
                var result = executeProcedures.DataTable("ENEL_CREATETECHINFO", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return bool.Parse(result?.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public bool ManagmentOfFindingsSave(int encabezadoId, int? accionTomarId, string gestionRealizar, string responsable, DateTime? fechaGestion, DateTime? fechaRealizado, string gestionRealizado, string archivoEvidencia, int tipo)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Encabezadoid", SqlDbType=SqlDbType.Int,Value=encabezadoId},
                new SqlParameter(){ ParameterName="AccionTomarID", SqlDbType=SqlDbType.Int,Value=accionTomarId},
                new SqlParameter(){ ParameterName="GestionRealizar", SqlDbType=SqlDbType.VarChar,Value=gestionRealizar},
                new SqlParameter(){ ParameterName="Responsable", SqlDbType=SqlDbType.VarChar,Value=responsable},
                new SqlParameter(){ ParameterName="FechaGestion", SqlDbType=SqlDbType.Date,Value=fechaGestion},
                new SqlParameter(){ ParameterName="FechaRealizado", SqlDbType=SqlDbType.Date,Value=fechaRealizado},
                new SqlParameter(){ ParameterName="GestionRealizado", SqlDbType=SqlDbType.VarChar,Value=gestionRealizado},
                new SqlParameter(){ ParameterName="ArchivoEvidencia", SqlDbType=SqlDbType.VarChar,Value=archivoEvidencia},
                new SqlParameter(){ ParameterName="Tipo", SqlDbType=SqlDbType.Int,Value=tipo},
            };
                var result = executeProcedures.DataTable("ENEL_UPDATEFINDINGSPLAN", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return bool.Parse(result?.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool UpdateInspection(int id, int estadoFinalId, int accionTomarId, DateTime fechaInspeccion, int inspectorId, int ubicationId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Encabezadoid", SqlDbType=SqlDbType.Int,Value=id},
                new SqlParameter(){ ParameterName="EstadoFinalId", SqlDbType=SqlDbType.Int,Value=estadoFinalId},
                new SqlParameter(){ ParameterName="AccionTomarId", SqlDbType=SqlDbType.Int,Value=accionTomarId},
                new SqlParameter(){ ParameterName="FechaInspeccion", SqlDbType=SqlDbType.Date,Value=fechaInspeccion},
                new SqlParameter(){ ParameterName="InspectorID", SqlDbType=SqlDbType.Int,Value=inspectorId},
                new SqlParameter(){ ParameterName="Ubicationid", SqlDbType=SqlDbType.Int,Value=ubicationId},
            };
                var result = executeProcedures.DataTable("ENEL_UPDATEINSPECTIONS", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return bool.Parse(result?.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool UpdateFactorsByInspection(int estadoId, string comentario, int factorId, int encabezadoId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Estadoid", SqlDbType=SqlDbType.Int,Value=estadoId},
                new SqlParameter(){ ParameterName="Comentario", SqlDbType=SqlDbType.VarChar,Value=comentario},
                new SqlParameter(){ ParameterName="Factorid", SqlDbType=SqlDbType.VarChar,Value=factorId},
                new SqlParameter(){ ParameterName="Encabezadoid", SqlDbType=SqlDbType.Int,Value=encabezadoId},
                          };
                var result = executeProcedures.DataTable("ENEL_UPDATEFACTORBYINSPECTION", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return bool.Parse(result?.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool UpdateVerificateActionPlan(int id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="id", SqlDbType=SqlDbType.Int,Value=id},
                          };
                var result = executeProcedures.DataTable("ENEL_UPDATEVERIFICATEACTIONPLAN", parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /// <summary>
        /// Ejecuta un Stored Procedure o consulta SQL y retorna los registros en un DataTable.
        /// </summary>
        /// <param name="storedProcedureOrQuery">Nombre del Stored Procedure o consulta SQL (SELECT).</param>
        /// <param name="parameters">Lista de SqlParameters (puede ser null o vacía).</param>
        /// <param name="commandType">Tipo de comando (por defecto CommandType.StoredProcedure).</param>
        /// <returns>DataTable con los resultados obtenidos.</returns>
        public DataTable ExecuteDataTable(
            string storedProcedureOrQuery,
            List<SqlParameter> parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var dataTable = new DataTable();

            // Si el texto parece una consulta SQL directa (ej. contiene SELECT o FROM), ajustamos el CommandType
            if (commandType == CommandType.StoredProcedure &&
                (storedProcedureOrQuery.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase) ||
                 storedProcedureOrQuery.Trim().StartsWith("WITH", StringComparison.OrdinalIgnoreCase)))
            {
                commandType = CommandType.Text;
            }

            string _connectionString = ConfigurationManager.AppSettings["conexP"].ToString();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureOrQuery, connection))
                {
                    command.CommandType = commandType;
                    command.CommandTimeout = 120; // 2 minutos para consultas extensas

                    // Adjuntar parámetros
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            if (param.Value == null)
                            {
                                param.Value = DBNull.Value;
                            }
                            command.Parameters.Add(param);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        try
                        {
                            // SqlDataAdapter se encarga de abrir y cerrar la conexión automáticamente si está cerrada
                            adapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            throw new Exception($"Error al ejecutar ExecuteDataTable en '{storedProcedureOrQuery}': {ex.Message}", ex);
                        }
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Sobrecarga simplificada para ejecutar consultas directas o SPs sin parámetros.
        /// </summary>
        public DataTable ExecuteDataTable(string queryOrSp)
        {
            return ExecuteDataTable(queryOrSp, null);
        }

    }
}
