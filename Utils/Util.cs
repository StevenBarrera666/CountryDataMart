using iTextSharp.text;
using iTextSharp.text.pdf;
using LogicBo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
namespace Utils
{
    public class Util
    {
        PdfPTable table;
        LogicBo.WorkingAtHeightBo _WorkingAtHeightBo;
        LogicBo.IzageBo _IzageBo;
        LogicBo.InspectionIzajeBo _InspectionIzajeBo;

        public void lanzarCertificados(int EncabezadoID)
        {
            DataSet dsResultEnc = new DataSet();

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



            _WorkingAtHeightBo = new LogicBo.WorkingAtHeightBo();
            dsResultEnc.Tables.Add(_WorkingAtHeightBo.CrearCertficados(EncabezadoID));



            int num1 = checked(dsResultEnc.Tables[0].Rows.Count - 1);
            int index = 0;
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["PathPDFCurriculum"].ToString();


            while (index <= num1)
            {
                string Prompt = dsResultEnc.Tables[0].Rows[index]["rfid"].ToString().Replace("/", "_");
                int year = Convert.ToDateTime(dsResultEnc.Tables[0].Rows[index]["FechaInspeccion"].ToString()).Year;
                int int32 = Convert.ToInt32(dsResultEnc.Tables[0].Rows[index]["id"]);
                try
                {
                    Document document = new Document(PageSize.LETTER, 40, 40, 40, 40);


                    PdfWriter.GetInstance(document, (Stream)new FileStream(pathPDF + @"CERT_INS\RG03-PL 260_CERT_" + Prompt + "_" + year.ToString() + ".pdf", FileMode.OpenOrCreate));
                    document.Open();
                    this.table = new PdfPTable(2);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 0;
                    this.table.SetWidths(new int[2] { 150, 300 });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 0;
                    iTextSharp.text.Font font1 = FontFactory.GetFont("Bookman Old Style", 10f, 1);
                    iTextSharp.text.Font font2 = FontFactory.GetFont("Bookman Old Style", 11f, 2);
                    iTextSharp.text.Font font3 = FontFactory.GetFont("Bookman Old Style", 9f, 2);
                    int inspectorID = Convert.ToInt32(dsResultEnc.Tables[0].Rows[index]["InspectorID"]);
                    string imagenEmpresa = string.Empty;
                    string nombreEmpresa = string.Empty;






                    if (inspectorID == 20)
                    {
                        imagenEmpresa = pathPDF + "Content\\Images\\LOgoVerthor.jpg";
                        nombreEmpresa = "VERTHOR SAFETY LATAM SAS";
                    }
                    else if (inspectorID != 23)
                    {
                        imagenEmpresa = pathPDF + "Content\\Images\\LOgoVerthor.jpg";
                        nombreEmpresa = "VERTHOR SAFETY LATAM SAS";
                    }
                    else if (inspectorID == 23)
                    {
                        imagenEmpresa = pathPDF + "Content\\Images\\Logo.png";
                        nombreEmpresa = "Enel Green Power";
                    }



                    iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(imagenEmpresa);
                    instance.BorderWidth = 0.0f;
                    instance.Alignment = 2;
                    double num2 = 100.0 / (double)instance.Width;
                    instance.ScalePercent((float)(num2 * 100.0));
                    this.table.AddCell(new PdfPCell(instance)
                    {
                        Colspan = 1,
                        Rowspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    this.table.AddCell(new PdfPCell(new Phrase("CERTIFICADO DE INSPECCIÓN Y CERTIFICACIÓN", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 5
                    });


                    document.Add((IElement)this.table);





                    this.table = new PdfPTable(4);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[4]
                    {
            150,
            150,
            150,
            150
                    });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;
                    this.table.AddCell(new PdfPCell(new Phrase("Sede", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["sede"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Ubicación", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["ubicacion"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("TAG", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["RFID"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("RFID", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["RFID"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Modelo/Referencia", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["Modelo"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Fecha Fabricación", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["FechaFabricacion"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Marca", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["Marca"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Serial", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["Serial"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Lote", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["Lote"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Fecha Compra", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["FechaCompra"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    document.Add((IElement)this.table);




                    this.table = new PdfPTable(1);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 0;
                    this.table.SetWidths(new int[1] { 300 });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 0;
                    PdfPCell cell = new PdfPCell(new Phrase("FACTORES INSPECCIONADOS", font1));
                    cell.Colspan = 1;
                    cell.Padding = 5f;
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 1;
                    cell.Border = 0;
                    this.table.AddCell(cell);
                    document.Add((IElement)this.table);




                    this.table = new PdfPTable(3);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[3]
                    {
            200,
            100,
            250
                    });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;
                    this.table.AddCell(new PdfPCell(new Phrase("Factores generales", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Estado", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    this.table.AddCell(new PdfPCell(new Phrase("Comentarios", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });



                    _WorkingAtHeightBo = new LogicBo.WorkingAtHeightBo();
                    dsResultEnc.Tables.Add();






                    DataSet dataSet3 = _WorkingAtHeightBo.CargarFactores(Convert.ToInt32(dsResultEnc.Tables[0].Rows[index]["Id"]));

                    for (int i = 0; i < dataSet3.Tables[0].Rows.Count; i++)
                    {
                        string Factor = dataSet3.Tables[0].Rows[i]["Factor"].ToString();

                        this.table.AddCell(new PdfPCell(new Phrase(Factor, font1))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 1,
                            VerticalAlignment = 1
                        });

                        string estado = dataSet3.Tables[0].Rows[i]["descripcion"].ToString();

                        this.table.AddCell(new PdfPCell(new Phrase(estado, font1))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 1,
                            VerticalAlignment = 1
                        });
                        string comentario = dataSet3.Tables[0].Rows[i]["Comentario"].ToString();
                        this.table.AddCell(new PdfPCell(new Phrase(comentario, font1))
                        {
                            Colspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 1,
                            VerticalAlignment = 1
                        });

                    }
                    document.Add((IElement)this.table);



                    this.table = new PdfPTable(1);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[1] { 300 });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;



                    string imagenS = pathPDF + @"Registradas\" + dsResultEnc.Tables[0].Rows[index]["Imagen"].ToString();
                    if (System.IO.File.Exists(imagenS))
                    {



                        iTextSharp.text.Image instance11 = iTextSharp.text.Image.GetInstance(imagenS);
                        instance.BorderWidth = 0.0f;
                        instance.Alignment = 2;
                        double num221 = 100.0 / (double)instance.Width;
                        instance.ScalePercent((float)(num221 * 100.0));
                        this.table.AddCell(new PdfPCell(instance11)
                        {
                            Colspan = 1,
                            Rowspan = 1,
                            Padding = 5f,
                            HorizontalAlignment = 1,
                            VerticalAlignment = 1
                        });
                    }
                    document.Add((IElement)this.table);












                    this.table = new PdfPTable(1);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[1] { 300 });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;
                    cell = new PdfPCell(new Phrase("CONCEPTO", font1));
                    cell.Colspan = 1;
                    cell.Padding = 5f;
                    cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 1;
                    cell.Border = 0;
                    this.table.AddCell(cell);
                    document.Add((IElement)this.table);


                    this.table = new PdfPTable(3);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[3]
                    {
            100,
            100,
            200
                    });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;
                    this.table.AddCell(new PdfPCell(new Phrase("Estado", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase("Concepto", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    this.table.AddCell(new PdfPCell(new Phrase("Fecha Inspección", font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["estado"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["accion"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["FechaInspeccion"].ToString(), font2))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    this.table.AddCell(new PdfPCell(new Phrase("El equipo cumple con los requerimientos de la resolución Resolución 4272 de 2021 y con las especificaciones del fabricante, bajo la norma: " + dsResultEnc.Tables[0].Rows[index]["Norma"].ToString(), font2))
                    {
                        Colspan = 3,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1
                    });

                    document.Add((IElement)this.table);






                    this.table = new PdfPTable(1);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.Border = 0;
                    this.table.SetWidths(new int[1]
                    {
          500,
                    });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    //this.table.DefaultCell.Border = 0;

                    string imagenS1 = pathPDF + @"Firmas\" + dsResultEnc.Tables[0].Rows[index]["InspectorID"].ToString() + ".jpg";

                    if (!System.IO.File.Exists(imagenS1))
                    {
                        imagenS1 = pathPDF + @"Firmas\0.png";
                    }
                    iTextSharp.text.Image instance1 = iTextSharp.text.Image.GetInstance(imagenS1);
                    instance.Border = 0;
                    instance.Alignment = 2;
                    double num22 = 30.0 / (double)instance1.Width;
                    instance.ScalePercent((float)(num22 * 10.0));
                    this.table.AddCell(new PdfPCell(instance1)
                    {
                        Colspan = 1,
                        Rowspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1,
                        Border = 0
                    });
                    this.table.AddCell(new PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[index]["Nombre"].ToString(), font3))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        Border = 0,
                        VerticalAlignment = 1
                    });



                    document.Add((IElement)this.table);
                    this.table = new PdfPTable(1);
                    this.table.TotalWidth = 550f;
                    this.table.LockedWidth = true;
                    this.table.DefaultCell.BorderWidth = 20f;
                    this.table.SetWidths(new int[1] { 300 });
                    this.table.SpacingBefore = 1f;
                    this.table.SpacingAfter = 1f;
                    this.table.DefaultCell.Border = 30;
                    this.table.AddCell(new PdfPCell(new Phrase(nombreEmpresa, font1))
                    {
                        Colspan = 1,
                        Padding = 5f,
                        HorizontalAlignment = 1,
                        VerticalAlignment = 1,
                        Border = 0

                    });
                    document.Add((IElement)this.table);
                    document.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(dsResultEnc.Tables[0].Rows[index]["RFID"].ToString());
                    MessageBox.Show(ex.Message);

                }
                checked { ++index; }
            }

        }
        public void CreateCurriculum(int EquipementID)
        {
            DataSet dsResultEnc = new DataSet();

            ArrayList arrUbicaciones = new ArrayList();
            ArrayList arrModelo = new ArrayList();
            ArrayList arrFechaFab = new ArrayList();
            ArrayList arrMarca = new ArrayList();
            ArrayList arrSerial = new ArrayList();
            ArrayList arrLote = new ArrayList();
            ArrayList arrComentariost = new ArrayList();
            ArrayList arrRFID = new ArrayList();
            ArrayList arrAprovado = new ArrayList();
            ArrayList arrInspector = new ArrayList();
            ArrayList arrFinal = new ArrayList();
            ArrayList arrFechaInsp = new ArrayList();
            string Sede = "";
            string Equipo = "";
            string RFID = "";
            string CodEq = "";
            int i = 0;
            _WorkingAtHeightBo = new LogicBo.WorkingAtHeightBo();
            dsResultEnc.Tables.Add(_WorkingAtHeightBo.GetDatabyElement(EquipementID));

            Sede = dsResultEnc.Tables[0].Rows[0]["Sede"].ToString();
            Equipo = dsResultEnc.Tables[0].Rows[0]["Equipo"].ToString();
            RFID = dsResultEnc.Tables[0].Rows[0]["Consecutivo"].ToString();
            CodEq = dsResultEnc.Tables[0].Rows[0]["CodE"].ToString();

            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["PathPDFCurriculum"].ToString();


            try
            {
                Document document = new Document(PageSize.LETTER, 40, 40, 40, 40);
                PdfWriter pdfWrite = PdfWriter.GetInstance(document, new FileStream(pathPDF + @"HV_EQUIPOS1\RG03-PL 260_HV_" + CodEq + "_" + RFID + ".pdf", FileMode.OpenOrCreate));
                document.Open();
                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                int[] Columns1 = new int[] { 60, 200, 60 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                iTextSharp.text.Font fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 14, iTextSharp.text.Font.BOLD);


                string strRutaImagen1 = pathPDF + "Content\\Images\\Logo.png";
                iTextSharp.text.Image imagen1;
                //  Creamos la imagen y le ajustamos el tama�o
                imagen1 = iTextSharp.text.Image.GetInstance(strRutaImagen1);
                imagen1.BorderWidth = 0;
                imagen1.Alignment = Element.ALIGN_RIGHT;
                float percentage1 = 0.0F;
                percentage1 = (100 / imagen1.Width);
                imagen1.ScalePercent(percentage1 * 100);

                iTextSharp.text.pdf.PdfPCell celda = new iTextSharp.text.pdf.PdfPCell(imagen1);
                celda.Colspan = 1;
                celda.Rowspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);



                celda = new iTextSharp.text.pdf.PdfPCell(new Phrase("Hoja de Vida de Equipos para Trabajo en Altura", fuenteEncabezado));
                celda.Colspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);

                celda = new iTextSharp.text.pdf.PdfPCell(new Phrase("RG03-PL260 Versión 1 05 / 05 / 2017", fuenteEncabezado));
                celda.Colspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);

                document.Add(table);




                table = new PdfPTable(2);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 200, 300 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                iTextSharp.text.pdf.PdfPCell celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de creación Hoja de vida:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.Border = Rectangle.NO_BORDER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["FechaCreacion"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Creador de la hoja de vida:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.Border = Rectangle.NO_BORDER;
                table.AddCell(celda1);



                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Creador"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Nombre:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.Border = Rectangle.NO_BORDER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Nombre"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Cargo:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.Border = Rectangle.NO_BORDER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Cargo"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Cc / Asignado:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.Border = Rectangle.NO_BORDER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Información del Equipo", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                document.Add(table);

                table = new PdfPTable(2);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 300, 300 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Datos del Equipo", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fotografias del Equipo", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Consecutivo:" + dsResultEnc.Tables[0].Rows[i]["Consecutivo"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                string[] SplitFotos;
                int intTotalFotos;
                if (!String.IsNullOrEmpty(dsResultEnc.Tables[0].Rows[0]["Fotos"].ToString()))
                {
                    SplitFotos = dsResultEnc.Tables[0].Rows[i]["Fotos"].ToString().Split('/');
                    intTotalFotos = SplitFotos.Length;
                }
                else
                {
                    intTotalFotos = 0;
                }


                iTextSharp.text.Image imagen;
                string strRutaImagen;

                    string imagenS = pathPDF + @"Registradas\" + dsResultEnc.Tables[0].Rows[i]["Imagen"].ToString();
                
                    if (System.IO.File.Exists(imagenS))
                    {
                        // Creamos la imagen y le ajustamos el tamaño
                        imagen = iTextSharp.text.Image.GetInstance(imagenS);
                        imagen.BorderWidth = 0;
                        imagen.Alignment = Element.ALIGN_RIGHT;
                        double percentage = 0.0F;
                        percentage = 100 / (double)imagen.Width;
                        imagen.ScalePercent((float)percentage * 100);



                        celda1 = new iTextSharp.text.pdf.PdfPCell(imagen);
                        celda1.Colspan = 1;
                        celda1.Rowspan = 9;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;




                        table.AddCell(celda1);
                    }
                    else
                    {
                        celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                        celda1.Colspan = 1;
                        celda1.Rowspan = 9;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(celda1);
                    }
               

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Equipo:" + dsResultEnc.Tables[0].Rows[i]["Equipo"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Referencia:" + dsResultEnc.Tables[0].Rows[i]["Referencia"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Marca:" + dsResultEnc.Tables[0].Rows[i]["Marca"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Serial:" + dsResultEnc.Tables[0].Rows[i]["Serial"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);





                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Lote:" + dsResultEnc.Tables[0].Rows[i]["Lote"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Ubicación:" + dsResultEnc.Tables[0].Rows[i]["Ubicacion"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de Fabricación:" + dsResultEnc.Tables[0].Rows[i]["FechaFabricacion"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Responsable del equipo:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                document.Add(table);


                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 400, 100, 100 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Programa de Inspección", fuenteEncabezado));
                celda1.Colspan = 3;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Realizado Por:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de Inspección", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Resultado", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                int id;

                id = Convert.ToInt32(dsResultEnc.Tables[0].Rows[i]["id"]);
                DataSet dsResultados = new DataSet();
                dsResultEnc.Tables.Add(_WorkingAtHeightBo.GetResultbyElement(id));

                foreach (DataRow oRow in dsResultEnc.Tables[0].Rows)
                {
                    //}

                    //foreach (DataRow oRow in dsResultEnc.Tables[0].Rows)
                    //{
                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(oRow["RealizadoPor"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);


                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(oRow["FechaInspeccion"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(oRow["Estado"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);

                }

                //// 2
                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);


                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);
                //// 3

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);


                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);
                //// 4

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);


                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);
                //// 5

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);


                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);
                //// 6

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);

                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);


                //celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                //celda1.Colspan = 1;
                //celda1.Padding = 5;
                //celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //table.AddCell(celda1);



                document.Add(table);
                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 400, 100, 100 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Programa de mantenimiento", fuenteEncabezado));
                celda1.Colspan = 3;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Descripción del mantenimiento", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de Mantenimiento", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Realizado Por:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                if (!dsResultEnc.Tables[0].Rows[i]["FechaRealizar"].ToString().Contains("1900"))
                {
                    // 1
                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["GestionRealizar"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["FechaRealizar"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["ResponsableRealizar"].ToString(), fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);
                }
                else
                {
                    // 2
                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);


                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);
                }
                // 2
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);
                // 3
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                // 4
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);
                // 5
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                document.Add(table);



                table = new PdfPTable(1);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 500 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Observaciones", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Observaciones"].ToString(), fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                document.Add(table);

                if (intTotalFotos > 1)
                {
                    table = new PdfPTable(1);
                    table.TotalWidth = 550.0F;
                    table.LockedWidth = true;
                    table.DefaultCell.BorderWidth = 20;
                    Columns1 = new int[] { 600 };
                    table.SetWidths(Columns1);
                    table.SpacingBefore = 1.0F;
                    table.SpacingAfter = 1.0F;
                    table.DefaultCell.Border = Element.RECTANGLE;
                    fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Otras Fotografias", fuenteEncabezado));
                    celda1.Colspan = 2;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                    table.AddCell(celda1);

                    int ni = 1;

                    while ((ni < intTotalFotos))
                    {
                        strRutaImagen = @"C:\PERSONAL\Pers_GIT\ENEL_IST_Inspections\ENEL\FOTOS_EQUIPOS\" + dsResultEnc.Tables[0].Rows[i]["Sede"].ToString() + @"\" + dsResultEnc.Tables[0].Rows[i]["Fotos"].ToString().Split('/')[ni];

                        if (System.IO.File.Exists(strRutaImagen))
                        {
                            // Creamos la imagen y le ajustamos el tamaño
                            imagen = iTextSharp.text.Image.GetInstance(strRutaImagen);
                            imagen.BorderWidth = 0;
                            imagen.Alignment = Element.ALIGN_RIGHT;
                            double percentage = 0.0F;
                            percentage = 100 / (double)imagen.Width;
                            imagen.ScalePercent((float)percentage * 100);

                            celda1 = new iTextSharp.text.pdf.PdfPCell(imagen);
                            celda1.Colspan = 1;
                            celda1.Rowspan = 1;
                            celda1.Padding = 5;
                            celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            table.AddCell(celda1);
                        }
                        ni = (ni + 1);
                    }
                    document.Add(table);

                }
                document.Close();
                //fin
            }
            catch (Exception ex)
            {

            }
        }
        public void CreateHVIzaje(int idIzaje, string path)
        {
            DataSet dsResultEnc = new DataSet();

            ArrayList arrUbicaciones = new ArrayList();
            ArrayList arrModelo = new ArrayList();
            ArrayList arrFechaFab = new ArrayList();
            ArrayList arrMarca = new ArrayList();
            ArrayList arrSerial = new ArrayList();
            ArrayList arrLote = new ArrayList();
            ArrayList arrComentariost = new ArrayList();
            ArrayList arrRFID = new ArrayList();
            ArrayList arrAprovado = new ArrayList();
            ArrayList arrInspector = new ArrayList();
            ArrayList arrFinal = new ArrayList();
            ArrayList arrFechaInsp = new ArrayList();
            string Sede = "";
            string Serial = "";
            string Equipo = "";
            string RFID = "";
            string CodEq = "";
            string ImagenEI = "";
            int i = 0;
            _IzageBo = new LogicBo.IzageBo();
            dsResultEnc.Tables.Add(_IzageBo.GetInfo(idIzaje));

            Sede = dsResultEnc.Tables[0].Rows[0]["Sede"].ToString();
            Serial = dsResultEnc.Tables[0].Rows[0]["Serial"].ToString();
            Equipo = dsResultEnc.Tables[0].Rows[0]["Equipo"].ToString();
            RFID = dsResultEnc.Tables[0].Rows[0]["Consecutivo"].ToString();
            CodEq = dsResultEnc.Tables[0].Rows[0]["CodE"].ToString();
            ImagenEI = dsResultEnc.Tables[0].Rows[0]["Imagen"].ToString();

            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["PathPDFIzaje"].ToString();

            try
            {
                Document document = new Document(PageSize.LETTER, 40, 40, 40, 40);
                if (File.Exists(string.Concat(pathPDF, @"Equipo_Izaje\Equipo_Izaje_", Sede, "_", Serial, ".pdf")))
                {
                    File.Delete(string.Concat(pathPDF, @"Equipo_Izaje\Equipo_Izaje_", Sede, "_", Serial, ".pdf"));
                }
                PdfWriter pdfWrite = PdfWriter.GetInstance(document, new FileStream(string.Concat(pathPDF, @"Equipo_Izaje\Equipo_Izaje_", Sede, "_", Serial, ".pdf"), FileMode.OpenOrCreate));
                document.Open();
                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                int[] Columns1 = new int[] { 60, 200, 60 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                iTextSharp.text.Font fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 14, iTextSharp.text.Font.BOLD);

                string strRutaImagen1 = pathPDF + "Content\\Images\\Logo.png";
                iTextSharp.text.Image imagen1;
                //  Creamos la imagen y le ajustamos el tama�o
                imagen1 = iTextSharp.text.Image.GetInstance(strRutaImagen1);
                imagen1.BorderWidth = 0;
                imagen1.Alignment = Element.ALIGN_RIGHT;
                float percentage1 = 0.0F;
                percentage1 = (100 / imagen1.Width);
                imagen1.ScalePercent(percentage1 * 100);

                iTextSharp.text.pdf.PdfPCell celda = new iTextSharp.text.pdf.PdfPCell(imagen1);
                celda.Colspan = 1;
                celda.Rowspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);

                celda = new iTextSharp.text.pdf.PdfPCell(new Phrase("HOJA DE VIDA DEL EQUIPO DE IZAJE", fuenteEncabezado));
                celda.Colspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);

                celda = new iTextSharp.text.pdf.PdfPCell(new Phrase(String.Concat("Equipo_Izaje_", Sede, "_", Serial), fuenteEncabezado));
                celda.Colspan = 1;
                celda.Padding = 5;
                celda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda);

                document.Add(table);

                table = new PdfPTable(4);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 150, 150, 150, 150 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                iTextSharp.text.pdf.PdfPCell celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de creación:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(Convert.ToDateTime(dsResultEnc.Tables[0].Rows[i]["FechaCreacion"].ToString()).Date.ToString("d"), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Responsable:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["AsignadoA"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Consecutivo:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(CodEq, fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Tag ENEL:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Tag"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("1. INFORMACION GENERAL DEL EQUIPO", fuenteEncabezado));
                celda1.Colspan = 4;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                document.Add(table);

                table = new PdfPTable(2);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 300, 300 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Datos del Equipo", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fotografias del Equipo", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Equipo/Elemento:" + dsResultEnc.Tables[0].Rows[i]["Equipo"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                string[] SplitFotos;
                int intTotalFotos;
                if (!String.IsNullOrEmpty(dsResultEnc.Tables[0].Rows[0]["Fotos"].ToString()))
                {
                    SplitFotos = dsResultEnc.Tables[0].Rows[i]["Fotos"].ToString().Split('/');
                    intTotalFotos = SplitFotos.Length;
                }
                else
                {
                    intTotalFotos = 0;
                }

                iTextSharp.text.Image imagen;
                string strRutaImagen;
                if (intTotalFotos >= 1)
                {
                    strRutaImagen = string.Concat(path, ImagenEI);
                    if (System.IO.File.Exists(strRutaImagen))
                    {
                        // Creamos la imagen y le ajustamos el tamaño
                        imagen = iTextSharp.text.Image.GetInstance(strRutaImagen);
                        imagen.BorderWidth = 0;
                        imagen.Alignment = Element.ALIGN_RIGHT;
                        double percentage = 0.0F;
                        percentage = 100 / (double)imagen.Width;
                        imagen.ScalePercent((float)percentage * 200);

                        celda1 = new iTextSharp.text.pdf.PdfPCell(imagen);
                        celda1.Colspan = 1;
                        celda1.Rowspan = 7;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                        table.AddCell(celda1);
                    }
                    else
                    {
                        celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                        celda1.Colspan = 1;
                        celda1.Rowspan = 7;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(celda1);
                    }
                }
                else
                {
                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                    celda1.Colspan = 1;
                    celda1.Rowspan = 7;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    table.AddCell(celda1);
                }

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Marca:" + dsResultEnc.Tables[0].Rows[i]["Marca"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Modelo:" + dsResultEnc.Tables[0].Rows[i]["Modelo"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Serial:" + dsResultEnc.Tables[0].Rows[i]["Serial"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Lote:" + dsResultEnc.Tables[0].Rows[i]["Lote"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Ubicación:" + dsResultEnc.Tables[0].Rows[i]["Ubicacion"].ToString(), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de Fabricación:" + Convert.ToDateTime(dsResultEnc.Tables[0].Rows[i]["FechaFabricacion"].ToString()).Date.ToString("d"), fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Observaciones:" + dsResultEnc.Tables[0].Rows[i]["Observaciones"].ToString(), fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Rowspan = 4;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                document.Add(table);

                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 400, 100, 100 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("2. INSPECCIONES REALIZADAS AL EQUIPO", fuenteEncabezado));
                celda1.Colspan = 3;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Realizado Por:", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Fecha de Inspección", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Resultado", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                _InspectionIzajeBo = new LogicBo.InspectionIzajeBo();
                dsResultEnc.Tables.Add(_InspectionIzajeBo.GetById(idIzaje));

                if (dsResultEnc.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsResultEnc.Tables[1].Rows)
                    {
                        celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dr["Inspector"].ToString(), fuenteEncabezado));
                        celda1.Colspan = 1;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(celda1);


                        celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(Convert.ToDateTime(dr["FechaInspeccion"].ToString()).Date.ToString("d"), fuenteEncabezado));
                        celda1.Colspan = 1;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(celda1);

                        celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dr["EstadoFinal"].ToString(), fuenteEncabezado));
                        celda1.Colspan = 1;
                        celda1.Padding = 5;
                        celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        table.AddCell(celda1);
                    }
                }
                document.Add(table);

                table = new PdfPTable(3);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 400, 100, 100 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);
                // 3
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                // 4
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);
                // 5
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);


                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 1;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                table.AddCell(celda1);

                document.Add(table);

                table = new PdfPTable(1);
                table.TotalWidth = 550.0F;
                table.LockedWidth = true;
                table.DefaultCell.BorderWidth = 20;
                Columns1 = new int[] { 500 };
                table.SetWidths(Columns1);
                table.SpacingBefore = 1.0F;
                table.SpacingAfter = 1.0F;
                table.DefaultCell.Border = Element.RECTANGLE;
                fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Observaciones", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(dsResultEnc.Tables[0].Rows[i]["Observaciones"].ToString(), fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);

                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("", fuenteEncabezado));
                celda1.Colspan = 2;
                celda1.Padding = 5;
                celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                table.AddCell(celda1);
                document.Add(table);

                if (intTotalFotos > 1)
                {
                    table = new PdfPTable(1);
                    table.TotalWidth = 550.0F;
                    table.LockedWidth = true;
                    table.DefaultCell.BorderWidth = 20;
                    Columns1 = new int[] { 600 };
                    table.SetWidths(Columns1);
                    table.SpacingBefore = 1.0F;
                    table.SpacingAfter = 1.0F;
                    table.DefaultCell.Border = Element.RECTANGLE;
                    fuenteEncabezado = FontFactory.GetFont("Bookman Old Style", 9, iTextSharp.text.Font.BOLD);

                    celda1 = new iTextSharp.text.pdf.PdfPCell(new Phrase("Otras Fotografias", fuenteEncabezado));
                    celda1.Colspan = 2;
                    celda1.Padding = 5;
                    celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                    table.AddCell(celda1);

                    int ni = 1;

                    while ((ni < intTotalFotos))
                    {
                        strRutaImagen = @"C:\PERSONAL\Pers_GIT\ENEL_IST_Inspections\ENEL\FOTOS_EQUIPOS\" + dsResultEnc.Tables[0].Rows[i]["Sede"].ToString() + @"\" + dsResultEnc.Tables[0].Rows[i]["Fotos"].ToString().Split('/')[ni];

                        if (System.IO.File.Exists(strRutaImagen))
                        {
                            // Creamos la imagen y le ajustamos el tamaño
                            imagen = iTextSharp.text.Image.GetInstance(strRutaImagen);
                            imagen.BorderWidth = 0;
                            imagen.Alignment = Element.ALIGN_RIGHT;
                            double percentage = 0.0F;
                            percentage = 100 / (double)imagen.Width;
                            imagen.ScalePercent((float)percentage * 100);

                            celda1 = new iTextSharp.text.pdf.PdfPCell(imagen);
                            celda1.Colspan = 1;
                            celda1.Rowspan = 1;
                            celda1.Padding = 5;
                            celda1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            celda1.VerticalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            table.AddCell(celda1);
                        }
                        ni = (ni + 1);
                    }
                    document.Add(table);

                }
                document.Close();
                //fin
            }
            catch (Exception ex)
            {

            }
        }
    }
}
