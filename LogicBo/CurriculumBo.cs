using Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
namespace LogicBo
{
    public class CurriculumBo
    {

        #region Properties
        private readonly Entity.ModelEntities entities = new Entity.ModelEntities();
        private readonly ADO.ExecuteProcedures executeProcedures = new ADO.ExecuteProcedures();
        #endregion
        /// <summary>
        /// Get List By Filters
        /// </summary>
        /// <returns></returns>
        public DataTable GetListByElement(int? headquarterId, int? elementId, string rFID, string serial, string precinto)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="sedeId", SqlDbType=SqlDbType.Int,Value=headquarterId},
                new SqlParameter(){ ParameterName="ElementoID", SqlDbType=SqlDbType.Int,Value=elementId},
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=rFID},
                new SqlParameter(){ ParameterName="serial", SqlDbType=SqlDbType.VarChar,Value=serial},
                new SqlParameter(){ ParameterName="precinto", SqlDbType=SqlDbType.VarChar,Value=precinto}
            };
            var result = executeProcedures.DataTable("ENEL_LoadHVFromElement", parameters);
            return result;
        }

        public DataTable GetListCertificatesByElement(int? headquarterId, int? elementId, string rFID, string serial, string precinto)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="sedeId", SqlDbType=SqlDbType.Int,Value=headquarterId},
                new SqlParameter(){ ParameterName="ElementoID", SqlDbType=SqlDbType.Int,Value=elementId},
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=rFID},
                new SqlParameter(){ ParameterName="serial", SqlDbType=SqlDbType.VarChar,Value=serial},
                new SqlParameter(){ ParameterName="precinto", SqlDbType=SqlDbType.VarChar,Value=precinto}
            };
            var result = executeProcedures.DataTable("ENEL_LoadInspectionCErtificateFromElement", parameters);
            return result;
        }





        public int Create(string rFID, string serial, string model, string material, DateTime fabricationDate, string mark, string lot, int elementId, int headquarterId, DateTime purchaseDate, int? areaId, int? employedId, int assignedDate, int ubicacionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=rFID},
                new SqlParameter(){ ParameterName="Serial", SqlDbType=SqlDbType.VarChar,Value=serial},
                new SqlParameter(){ ParameterName="Modelo", SqlDbType=SqlDbType.VarChar,Value=model},
                new SqlParameter(){ ParameterName="Material", SqlDbType=SqlDbType.VarChar,Value=material},
                new SqlParameter(){ ParameterName="FechaFabricacion", SqlDbType=SqlDbType.DateTime,Value=fabricationDate},
                new SqlParameter(){ ParameterName="Marca", SqlDbType=SqlDbType.VarChar,Value=mark},
                new SqlParameter(){ ParameterName="Lote", SqlDbType=SqlDbType.VarChar,Value=lot},
                new SqlParameter(){ ParameterName="ElementoID", SqlDbType=SqlDbType.Int,Value=elementId},
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.Int,Value=headquarterId},
                new SqlParameter(){ ParameterName="FechaCompra", SqlDbType=SqlDbType.DateTime,Value=purchaseDate},
                new SqlParameter(){ ParameterName="AreaID", SqlDbType=SqlDbType.Int,Value=areaId},
                new SqlParameter(){ ParameterName="EmpleadoID", SqlDbType=SqlDbType.Int,Value=employedId},
                new SqlParameter(){ ParameterName="TipoFecha", SqlDbType=SqlDbType.Int,Value=assignedDate},
                new SqlParameter(){ ParameterName="UbicacionID", SqlDbType=SqlDbType.Int,Value=ubicacionID},
            };
                var result = executeProcedures.DataTable("ENEL_SaveElement", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());


                return int.Parse(result.Rows[0][2].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void CreateCurriculum(int EquipementID)
        {
            DataSet dataSet1 = new DataSet();
            ArrayList arrayList1 = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            ArrayList arrayList3 = new ArrayList();
            ArrayList arrayList4 = new ArrayList();
            ArrayList arrayList5 = new ArrayList();
            ArrayList arrayList6 = new ArrayList();
            ArrayList arrayList7 = new ArrayList();
            ArrayList arrayList8 = new ArrayList();
            ArrayList arrayList9 = new ArrayList();
            ArrayList arrayList10 = new ArrayList();
            ArrayList arrayList11 = new ArrayList();
            ArrayList arrayList12 = new ArrayList();
            string str1 = "";
            string str2 = "";
            int index1 = 0;

            SCIST.CapaDatos.clsFunciones clsFunciones = new SCIST.CapaDatos.clsFunciones();
            DataSet dataSet2 = clsFunciones.CrearHOJA(EquipementID);
            str1 = dataSet2.Tables[0].Rows[0]["Sede"].ToString();
            str2 = dataSet2.Tables[0].Rows[0]["Equipo"].ToString();
            string str3 = dataSet2.Tables[0].Rows[0]["Consecutivo"].ToString();
            string str4 = dataSet2.Tables[0].Rows[0]["CodE"].ToString();
            string str5 = "C:\\D\\hv";
            try
            {
                Document document = new Document(PageSize.LETTER, 40f, 40f, 40f, 40f);
                PdfWriter.GetInstance(document, (Stream)new FileStream($"{str5}\\RG03-REN_PL_001_{str4}_{str3}.pdf", FileMode.OpenOrCreate));
                document.Open();
                PdfPTable pdfPtable1 = new PdfPTable(2);
                pdfPtable1.TotalWidth = 550f;
                pdfPtable1.LockedWidth = true;
                ((Rectangle)pdfPtable1.DefaultCell).BorderWidth = 20f;
                int[] numArray1 = new int[2] { 60, 260 };
                pdfPtable1.SetWidths(numArray1);
                pdfPtable1.SpacingBefore = 1f;
                pdfPtable1.SpacingAfter = 1f;
                ((Rectangle)pdfPtable1.DefaultCell).Border = 30;
                Font font1 = FontFactory.GetFont("Bookman Old Style", 14f, 1);
                iTextSharp.text.Image instance1 = iTextSharp.text.Image.GetInstance("C:\\D\\Logo.png");
                ((Rectangle)instance1).BorderWidth = 0.0f;
                instance1.Alignment = 2;
                float num1 = 100f / ((Rectangle)instance1).Width;
                instance1.ScalePercent(num1 * 100f);
                pdfPtable1.AddCell(new PdfPCell(instance1)
                {
                    Colspan = 1,
                    Rowspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable1.AddCell(new PdfPCell(new Phrase("Hoja de Vida de Equipos para Trabajo en Altura", font1))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 5
                });
                document.Add((IElement)pdfPtable1);
                PdfPTable pdfPtable2 = new PdfPTable(2);
                pdfPtable2.TotalWidth = 550f;
                pdfPtable2.LockedWidth = true;
                ((Rectangle)pdfPtable2.DefaultCell).BorderWidth = 20f;
                int[] numArray2 = new int[2] { 200, 300 };
                pdfPtable2.SetWidths(numArray2);
                pdfPtable2.SpacingBefore = 1f;
                pdfPtable2.SpacingAfter = 1f;
                ((Rectangle)pdfPtable2.DefaultCell).Border = 30;
                Font font2 = FontFactory.GetFont("Bookman Old Style", 9f, 1);
                PdfPCell pdfPcell1 = new PdfPCell(new Phrase("Fecha de creación Hoja de vida:", font2));
                pdfPcell1.Colspan = 1;
                pdfPcell1.Padding = 5f;
                pdfPcell1.HorizontalAlignment = 0;
                pdfPcell1.VerticalAlignment = 1;
                ((Rectangle)pdfPcell1).Border = 0;
                pdfPtable2.AddCell(pdfPcell1);
                pdfPtable2.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["FechaCreacion"].ToString(), font2))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                PdfPCell pdfPcell2 = new PdfPCell(new Phrase("Creador de la hoja de vida:", font2));
                pdfPcell2.Colspan = 1;
                pdfPcell2.Padding = 5f;
                pdfPcell2.HorizontalAlignment = 0;
                pdfPcell2.VerticalAlignment = 1;
                ((Rectangle)pdfPcell2).Border = 0;
                pdfPtable2.AddCell(pdfPcell2);
                pdfPtable2.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["Creador"].ToString(), font2))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                PdfPCell pdfPcell3 = new PdfPCell(new Phrase("Nombre:", font2));
                pdfPcell3.Colspan = 1;
                pdfPcell3.Padding = 5f;
                pdfPcell3.HorizontalAlignment = 0;
                pdfPcell3.VerticalAlignment = 1;
                ((Rectangle)pdfPcell3).Border = 0;
                pdfPtable2.AddCell(pdfPcell3);
                pdfPtable2.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["Nombre"].ToString(), font2))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                PdfPCell pdfPcell4 = new PdfPCell(new Phrase("Cargo:", font2));
                pdfPcell4.Colspan = 1;
                pdfPcell4.Padding = 5f;
                pdfPcell4.HorizontalAlignment = 0;
                pdfPcell4.VerticalAlignment = 1;
                ((Rectangle)pdfPcell4).Border = 0;
                pdfPtable2.AddCell(pdfPcell4);
                pdfPtable2.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["Cargo"].ToString(), font2))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                PdfPCell pdfPcell5 = new PdfPCell(new Phrase("Cc / Asignado:", font2));
                pdfPcell5.Colspan = 1;
                pdfPcell5.Padding = 5f;
                pdfPcell5.HorizontalAlignment = 0;
                pdfPcell5.VerticalAlignment = 1;
                ((Rectangle)pdfPcell5).Border = 0;
                pdfPtable2.AddCell(pdfPcell5);
                pdfPtable2.AddCell(new PdfPCell(new Phrase("", font2))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable2.AddCell(new PdfPCell(new Phrase("Información del Equipo", font2))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                document.Add((IElement)pdfPtable2);
                PdfPTable pdfPtable3 = new PdfPTable(2);
                pdfPtable3.TotalWidth = 550f;
                pdfPtable3.LockedWidth = true;
                ((Rectangle)pdfPtable3.DefaultCell).BorderWidth = 20f;
                int[] numArray3 = new int[2] { 300, 300 };
                pdfPtable3.SetWidths(numArray3);
                pdfPtable3.SpacingBefore = 1f;
                pdfPtable3.SpacingAfter = 1f;
                ((Rectangle)pdfPtable3.DefaultCell).Border = 30;
                Font font3 = FontFactory.GetFont("Bookman Old Style", 9f, 1);
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Datos del Equipo", font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Fotografias del Equipo", font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Consecutivo:" + dataSet2.Tables[0].Rows[index1]["Consecutivo"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                string path1 = "C:\\Registradas\\" + dataSet2.Tables[0].Rows[0]["Imagen"].ToString();
                if (System.IO.File.Exists(path1))
                {
                    iTextSharp.text.Image instance2 = iTextSharp.text.Image.GetInstance(path1);
                    ((Rectangle)instance2).BorderWidth = 0.0f;
                    instance2.Alignment = 2;
                    double num2 = 100.0 / (double)((Rectangle)instance2).Width;
                    instance2.ScalePercent((float)num2 * 100f);
                    pdfPtable3.AddCell(new PdfPCell(instance2)
                    {
                        Colspan = 1,
                        Rowspan = 9,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                }
                else
                    pdfPtable3.AddCell(new PdfPCell(new Phrase("", font3))
                    {
                        Colspan = 1,
                        Rowspan = 9,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Equipo:" + dataSet2.Tables[0].Rows[index1]["Equipo"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Referencia:" + dataSet2.Tables[0].Rows[index1]["Referencia"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Marca:" + dataSet2.Tables[0].Rows[index1]["Marca"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Serial:" + dataSet2.Tables[0].Rows[index1]["Serial"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Lote:" + dataSet2.Tables[0].Rows[index1]["Lote"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Ubicación:" + dataSet2.Tables[0].Rows[index1]["Ubicacion"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Fecha de Fabricación:" + dataSet2.Tables[0].Rows[index1]["FechaFabricacion"].ToString(), font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable3.AddCell(new PdfPCell(new Phrase("Responsable del equipo:", font3))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                document.Add((IElement)pdfPtable3);
                PdfPTable pdfPtable4 = new PdfPTable(3);
                pdfPtable4.TotalWidth = 550f;
                pdfPtable4.LockedWidth = true;
                ((Rectangle)pdfPtable4.DefaultCell).BorderWidth = 20f;
                int[] numArray4 = new int[3] { 400, 100, 100 };
                pdfPtable4.SetWidths(numArray4);
                pdfPtable4.SpacingBefore = 1f;
                pdfPtable4.SpacingAfter = 1f;
                ((Rectangle)pdfPtable4.DefaultCell).Border = 30;
                Font font4 = FontFactory.GetFont("Bookman Old Style", 9f, 1);
                pdfPtable4.AddCell(new PdfPCell(new Phrase("Programa de Inspección", font4))
                {
                    Colspan = 3,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable4.AddCell(new PdfPCell(new Phrase("Realizado Por:", font4))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable4.AddCell(new PdfPCell(new Phrase("Fecha de Inspección", font4))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable4.AddCell(new PdfPCell(new Phrase("Resultado", font4))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                int int32 = Convert.ToInt32(RuntimeHelpers.GetObjectValue(dataSet2.Tables[0].Rows[index1]["id"]));
                DataSet dataSet3 = new DataSet();
                DataSet dataSet4 = clsFunciones.ObtenerResultadoPorElemento(int32);
                try
                {
                    foreach (DataRow row in dataSet4.Tables[0].Rows)
                    {
                        pdfPtable4.AddCell(new PdfPCell(new Phrase(row["Nombre"].ToString(), font4))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 0,
                            VerticalAlignment = 1
                        });
                        pdfPtable4.AddCell(new PdfPCell(new Phrase(row["FechaInspeccion"].ToString(), font4))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 0,
                            VerticalAlignment = 1
                        });
                        pdfPtable4.AddCell(new PdfPCell(new Phrase(row["Estado"].ToString(), font4))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 0,
                            VerticalAlignment = 1
                        });
                    }
                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
                document.Add((IElement)pdfPtable4);
                PdfPTable pdfPtable5 = new PdfPTable(3);
                pdfPtable5.TotalWidth = 550f;
                pdfPtable5.LockedWidth = true;
                ((Rectangle)pdfPtable5.DefaultCell).BorderWidth = 20f;
                int[] numArray5 = new int[3] { 400, 100, 100 };
                pdfPtable5.SetWidths(numArray5);
                pdfPtable5.SpacingBefore = 1f;
                pdfPtable5.SpacingAfter = 1f;
                ((Rectangle)pdfPtable5.DefaultCell).Border = 30;
                Font font5 = FontFactory.GetFont("Bookman Old Style", 9f, 1);
                pdfPtable5.AddCell(new PdfPCell(new Phrase("Programa de mantenimiento", font5))
                {
                    Colspan = 3,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("Descripción del mantenimiento", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("Fecha de Mantenimiento", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("Realizado Por:", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                if (!dataSet2.Tables[0].Rows[index1]["FechaRealizar"].ToString().Contains("1900"))
                {
                    pdfPtable5.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["GestionRealizar"].ToString(), font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                    pdfPtable5.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["FechaRealizar"].ToString(), font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                    pdfPtable5.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["ResponsableRealizar"].ToString(), font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                }
                else
                {
                    pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                    pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                    pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 0,
                        VerticalAlignment = 1
                    });
                }
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                pdfPtable5.AddCell(new PdfPCell(new Phrase("", font5))
                {
                    Colspan = 1,
                    Padding = 5f,
                    HorizontalAlignment = 0,
                    VerticalAlignment = 1
                });
                document.Add((IElement)pdfPtable5);
                PdfPTable pdfPtable6 = new PdfPTable(1);
                pdfPtable6.TotalWidth = 550f;
                pdfPtable6.LockedWidth = true;
                ((Rectangle)pdfPtable6.DefaultCell).BorderWidth = 20f;
                int[] numArray6 = new int[1] { 500 };
                pdfPtable6.SetWidths(numArray6);
                pdfPtable6.SpacingBefore = 1f;
                pdfPtable6.SpacingAfter = 1f;
                ((Rectangle)pdfPtable6.DefaultCell).Border = 30;
                Font font6 = FontFactory.GetFont("Bookman Old Style", 9f, 1);
                pdfPtable6.AddCell(new PdfPCell(new Phrase("Observaciones", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase(dataSet2.Tables[0].Rows[index1]["Observaciones"].ToString(), font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase("", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase("", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase("", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase("", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                pdfPtable6.AddCell(new PdfPCell(new Phrase("", font6))
                {
                    Colspan = 2,
                    Padding = 5f,
                    HorizontalAlignment = 1,
                    VerticalAlignment = 1
                });
                document.Add((IElement)pdfPtable6);
                int num3 = 0;
                if (num3 > 1)
                {
                    PdfPTable pdfPtable7 = new PdfPTable(1);
                    pdfPtable7.TotalWidth = 550f;
                    pdfPtable7.LockedWidth = true;
                    ((Rectangle)pdfPtable7.DefaultCell).BorderWidth = 20f;
                    int[] numArray7 = new int[1] { 600 };
                    pdfPtable7.SetWidths(numArray7);
                    pdfPtable7.SpacingBefore = 1f;
                    pdfPtable7.SpacingAfter = 1f;
                    ((Rectangle)pdfPtable7.DefaultCell).Border = 30;
                    pdfPtable7.AddCell(new PdfPCell(new Phrase("Otras Fotografias", FontFactory.GetFont("Bookman Old Style", 9f, 1)))
                    {
                        Colspan = 2,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    int index2 = 1;
                    while (index2 < num3)
                    {
                        string path2 = $"C:\\PERSONAL\\Pers_GIT\\ENEL_IST_Inspections\\ENEL\\FOTOS_EQUIPOS\\{dataSet2.Tables[0].Rows[index1]["Sede"].ToString()}\\{dataSet2.Tables[0].Rows[index1]["Fotos"].ToString().Split('/')[index2]}";
                        if (System.IO.File.Exists(path2))
                        {
                            iTextSharp.text.Image instance3 = iTextSharp.text.Image.GetInstance(path2);
                            ((Rectangle)instance3).BorderWidth = 0.0f;
                            instance3.Alignment = 2;
                            double num4 = 100.0 / (double)((Rectangle)instance3).Width;
                            instance3.ScalePercent((float)num4 * 100f);
                            pdfPtable7.AddCell(new PdfPCell(instance3)
                            {
                                Colspan = 1,
                                Rowspan = 1,
                                Padding = 5f,
                                HorizontalAlignment = 1,
                                VerticalAlignment = 1
                            });
                        }
                        checked { ++index2; }
                    }
                    document.Add((IElement)pdfPtable7);
                }
                document.Close();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                //ProjectData.ClearProjectError();
            }

        }




        public int CreateEncabezado(string rFID,
            string serial,
            string model,
            string mark,
            string lote,
            string elemento,
            string sede,
            string ubicacion,
            string tag,
            string precinto,
            string EstadoFinalId,
            string InspectorID,
            DateTime fechaInspeccion)

        {
            try
            {


                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="RFID", SqlDbType=SqlDbType.VarChar,Value=rFID},
                new SqlParameter(){ ParameterName="Serial", SqlDbType=SqlDbType.VarChar,Value=serial},
                new SqlParameter(){ ParameterName="Modelo", SqlDbType=SqlDbType.VarChar,Value=model},
                new SqlParameter(){ ParameterName="Material", SqlDbType=SqlDbType.VarChar,Value=""},
                new SqlParameter(){ ParameterName="FechaFabricacion", SqlDbType=SqlDbType.DateTime,Value="01/01/2025"},
                new SqlParameter(){ ParameterName="Marca", SqlDbType=SqlDbType.VarChar,Value=mark},
                new SqlParameter(){ ParameterName="Lote", SqlDbType=SqlDbType.VarChar,Value=lote},
                new SqlParameter(){ ParameterName="ElementoID", SqlDbType=SqlDbType.VarChar,Value=elemento},
                new SqlParameter(){ ParameterName="Sedeid", SqlDbType=SqlDbType.VarChar,Value=sede},
                new SqlParameter(){ ParameterName="FechaCompra", SqlDbType=SqlDbType.DateTime,Value="01/01/2025"},
                new SqlParameter(){ ParameterName="Ubicacion", SqlDbType=SqlDbType.VarChar,Value=ubicacion},

                new SqlParameter(){ ParameterName="Observaciones", SqlDbType=SqlDbType.VarChar,Value=""},
                new SqlParameter(){ ParameterName="Tag", SqlDbType=SqlDbType.VarChar,Value=tag},
                new SqlParameter(){ ParameterName="Precinto", SqlDbType=SqlDbType.VarChar,Value=precinto},

            };
                var result = executeProcedures.DataTable("ENEL_CreateEncabezado", parameters);
                int EncabezadoID = int.Parse(result.Rows[0][0].ToString());
                int ElementID = int.Parse(result.Rows[0][2].ToString());
                int intEstadoFinalId = 0;
                switch (EstadoFinalId)
                {
                    case "Aceptado":
                        intEstadoFinalId = 1;
                        break;
                    case "Rechazado":

                        intEstadoFinalId = 2;
                        break;
                    case "Para Mantenimiento":
                        intEstadoFinalId = 3;
                        break;
                    default:
                        intEstadoFinalId = 1;
                        break;
                }


                int AccionTomarId = ((intEstadoFinalId != 1) ? 4 : 3);
                parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="Encabezadoid", SqlDbType=SqlDbType.Int,Value=EncabezadoID},
                new SqlParameter(){ ParameterName="EstadoFinalId", SqlDbType=SqlDbType.Int,Value=intEstadoFinalId},
                new SqlParameter(){ ParameterName="AccionTomarId", SqlDbType=SqlDbType.Int,Value=AccionTomarId},
                new SqlParameter(){ ParameterName="InspectorID", SqlDbType=SqlDbType.VarChar,Value=InspectorID},
                new SqlParameter(){ ParameterName="FechaInspeccion", SqlDbType=SqlDbType.DateTime,Value=fechaInspeccion}
                 };
                var resultRes = executeProcedures.DataTable("ENEL_CreateResultado", parameters);

                parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="ElementID", SqlDbType=SqlDbType.Int,Value=ElementID}
                };
                DataTable resultFac = executeProcedures.DataTable("ENEL_LoadFactorByElement", parameters);

                foreach (DataRow row in resultFac.Rows)
                {
                    var Factorid = Convert.ToInt32(row["id"]);
                    var estadoId = ((intEstadoFinalId != 1) ? 2 : 1);
                    var comentario= ((intEstadoFinalId != 1) ? "Con afectación" : "");

                    parameters = new List<SqlParameter> {
                        new SqlParameter(){ ParameterName="Encabezadoid", SqlDbType=SqlDbType.Int,Value=EncabezadoID},
                        new SqlParameter(){ ParameterName="Factorid", SqlDbType=SqlDbType.Int,Value=Factorid},
                        new SqlParameter(){ ParameterName="Estadoid", SqlDbType=SqlDbType.Int,Value=1},
                        new SqlParameter(){ ParameterName="Comentario", SqlDbType=SqlDbType.VarChar,Value=comentario}
                    };
                    var resultFacEnc = executeProcedures.DataTable("ENEL_CreateFactorEncabezado", parameters);
                }

                //CreateCurriculum(EncabezadoID);

                return EncabezadoID;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int CreateSede(string NombreSede, int Paisid, int SedeCategoriaID, string ubicacion, string codeENEL)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="NombreSede", SqlDbType=SqlDbType.VarChar,Value=NombreSede},
                new SqlParameter(){ ParameterName="Ubicacion", SqlDbType=SqlDbType.VarChar,Value=ubicacion},
                new SqlParameter(){ ParameterName="EnelCode", SqlDbType=SqlDbType.VarChar,Value=codeENEL},
                new SqlParameter(){ ParameterName="SedeCategoriaID", SqlDbType=SqlDbType.Int,Value=SedeCategoriaID},
                new SqlParameter(){ ParameterName="PaisID", SqlDbType=SqlDbType.Int,Value=Paisid}
            };
                var result = executeProcedures.DataTable("ENEL_CreateSede", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());


                return int.Parse(result.Rows[0][2].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }





        public int CreatePais(string pais)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="NombrePais", SqlDbType=SqlDbType.VarChar,Value=pais}
                };
                var result = executeProcedures.DataTable("ENEL_CreatePais", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return int.Parse(result.Rows[0][2].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int CreateSedeCategoria(string NombreCategoriaSede)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter(){ ParameterName="NombreCategoriaSede", SqlDbType=SqlDbType.VarChar,Value=NombreCategoriaSede}
                };
                var result = executeProcedures.DataTable("ENEL_CreateSedeCategoria", parameters);
                if (!Convert.ToBoolean(result?.Rows[0][0].ToString()))
                    throw new Exception(result.Rows[0][1].ToString());

                return int.Parse(result.Rows[0][2].ToString());
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Entity
        #endregion
    }
}
