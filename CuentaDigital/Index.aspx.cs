﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Net.Mail; 

namespace CuentaDigital
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) // se carga la primera vez al abrir la pagina
            {
                Session["Comparar"] = null;
            }
        }

        public class valores
        {

            public string fecha { get; set; }
            public decimal? monto { get; set; }
            public string comision { get; set; }
            public string usuario { get; set; }
            public string tipo { get; set; }
            public int identidad { get; set; }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Cargando el nombre del archivo y armado del nombre
            string fecha = Calendario.SelectedDate.ToShortDateString();

            XmlDocument xDoc = new XmlDocument();

            string[] dato = fecha.Split('/');

           // if (File.Exists(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml"))) // ya se encuentra en el disco c y analiza su contenido
            if (File.Exists("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml")) // ya se encuentra en el disco c y analiza su contenido
            {

                //xDoc.Load(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml"));
                xDoc.Load("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml");


                XmlNodeList pagos = xDoc.GetElementsByTagName("pagos");

                XmlNodeList lista = ((XmlElement)pagos[0]).GetElementsByTagName("pago");

                foreach (XmlElement nodo in lista)
                {
                    int i = 0;

                    XmlNodeList iIdentidad = nodo.GetElementsByTagName("identidad");

                    Session["Comparar"] = iIdentidad[i++].InnerText; // carga la identidad del ultimo
                    
                }

                // crear archivo que pise el original

                List<string> nuevaLista = new List<string>();

                nuevaLista.Add("fecha");
                nuevaLista.Add("monto_bruto");
                nuevaLista.Add("comision");
                nuevaLista.Add("usuario");
                nuevaLista.Add("tipo_pago");
                nuevaLista.Add("identidad");

                XElement xml = new XElement("pagos");
                XElement xmla = new XElement("pagos");

                string url = "https://www.cuentadigital.com/exportacionsandbox.php?control=2b4153d8728a8c0f90b0a5a43db87507&fecha=" + dato[2] + dato[1] + dato[0];

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string HTML = reader.ReadToEnd();

                string[] etiqueta_primaria = HTML.Split('\n');


                foreach (string ep in etiqueta_primaria)  // comienza a crear el xml total
                {

                    if (ep == "")
                    {
                        break;
                    }

                    XElement pago = new XElement("pago");
                    string[] subcadena = ep.Split('|');
                    int pos = 0;

                    foreach (string es in subcadena)
                    {
                        XElement etiqueta_secundaria = new XElement(string.Format(nuevaLista[pos]));
                        etiqueta_secundaria.Add(es);
                        pago.Add(etiqueta_secundaria);
                        pos += 1;
                    }

                    xml.Add(pago);

                }

                
                //xml.Save(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml"));// aca termina con mombre fechatotal.xml
                xml.Save("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml");





                xDoc.Load("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml");
                
                pagos = xDoc.GetElementsByTagName("pagos");

                lista = ((XmlElement)pagos[0]).GetElementsByTagName("pago");

                foreach (XmlElement nodo in lista)
                {
                    int i = 0;

                    XmlNodeList iIdentidad = nodo.GetElementsByTagName("identidad");

                    Session["Nuevo"] = iIdentidad[i++].InnerText; // carga la identidad del ultimo

                }







                int o = 1;
                int j = 0;

                foreach (string ep in etiqueta_primaria) // comienza a crear el xml parcial
                {
                    
                    XElement carga = new XElement("pago");

                    if (ep == "")
                    {
                        break;
                    }


                    string[] subcadena = ep.Split('|');
                    int pos = 0;

                    if (Convert.ToInt32(Session["Nuevo"]) - Convert.ToInt32(Session["Comprar"]) < o)
                    {

                        foreach (string es in subcadena)
                        {
                            j = 1;
                            XElement etiqueta_secundaria = new XElement(string.Format(nuevaLista[pos]));
                            etiqueta_secundaria.Add(es);
                            carga.Add(etiqueta_secundaria);
                            pos += 1;
                        }

                        xmla.Add(carga);
                    }
                    o++;
                }

















                if (j == 1)
                {
                    //xmla.Save(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + ".xml")); // aca termina con el nombre fecha.xml
                    xmla.Save("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + ".xml"); // aca termina con el nombre fecha.xml


                    MailMessage correo = new MailMessage();
                    correo.From = new MailAddress("correodelosprofesores@gmail.com");
                    correo.To.Add("licenciados@outlook.com.ar");


                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("Correodelosprofesores@gmail.com", "qsoiqzuliwweyeog");

                    smtp.EnableSsl = true;

                    correo.Subject = "Confirmacion CuentaDigital - Bien - " + dato[0] + "/"+ dato[1] +"/" + dato[2];
                    string cuerpo = "";

                    foreach (string ep in etiqueta_primaria) // comienza a crear el xml parcial
                    {

                        string[] subcadena = ep.Split('|');

                        if (ep == "")
                        {
                            break;
                        }



                        if (int.Parse(subcadena[5]) > Convert.ToInt32(Session["Comparar"]))
                        {



                            cuerpo = "Carga: " + subcadena[1] + " / Medio de Pago: " + subcadena[4] + " / Usuario: " + subcadena[3] + "\n" + cuerpo;




                        }

                    }

                    smtp.Send("correodelosprofesores@gmail.com", "licenciados@outlook.com.ar", correo.Subject, "CuentaDigital está OK:" + "\n" + cuerpo);

                }
                else
                {
                    File.Delete(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + ".xml"));
                }


                string alerta = @"<script type='text/javascript'>    
                                    alert('Tarea realizada');  
                                    </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", alerta, false);
               


            }
            else
            {

                List<string> nuevaLista = new List<string>();

                nuevaLista.Add("fecha");
                nuevaLista.Add("monto_bruto");
                nuevaLista.Add("comision");
                nuevaLista.Add("usuario");
                nuevaLista.Add("tipo_pago");
                nuevaLista.Add("identidad");

                XElement xml = new XElement("pagos");

                string url = "https://www.cuentadigital.com/exportacionsandbox.php?control=2b4153d8728a8c0f90b0a5a43db87507&fecha=" + dato[2] + dato[1] + dato[0];

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string HTML = reader.ReadToEnd();

                if (HTML == string.Empty)
                {
                    string alerta = @"<script type='text/javascript'>    
                                    alert('Tarea realizada');  
                                    </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "", alerta, false);

                    return;
                }


                string[] etiqueta_primaria = HTML.Split('\n');

                foreach (string ep in etiqueta_primaria)  // comienza a crear el xml total
                {

                    if (ep == "")
                    {
                        break;
                    }

                    XElement pago = new XElement("pago");
                    string[] subcadena = ep.Split('|');
                    int pos = 0;

                    foreach (string es in subcadena)
                    {
                        XElement etiqueta_secundaria = new XElement(string.Format(nuevaLista[pos]));
                        etiqueta_secundaria.Add(es);
                        pago.Add(etiqueta_secundaria);
                        pos += 1;
                    }

                    xml.Add(pago);

                }






                //xml.Save(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml"));

                //xml.Save(Server.MapPath("pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + ".xml"));

                xml.Save("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + "total.xml");

                xml.Save("c:/pagos/cuentadigital/" + dato[2] + dato[1] + dato[0] + ".xml");

                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("correodelosprofesores@gmail.com");
                correo.To.Add("licenciados@outlook.com.ar");


                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("Correodelosprofesores@gmail.com", "qsoiqzuliwweyeog");

                smtp.EnableSsl = true;

                correo.Subject = "Confirmacion CuentaDigital - Bien - " + dato[0] + "/" + dato[1] + "/" + dato[2];
                string cuerpo = "";

                foreach (string ep in etiqueta_primaria) // comienza a crear el xml parcial
                {

                    string[] subcadena = ep.Split('|');

                    if (ep == "")
                    {
                        break;
                    }

                    cuerpo = "Carga: " + subcadena[1] + " / Medio de Pago: " + subcadena[4] + " / Usuario: " + subcadena[3] + "\n" + cuerpo;

                }

                smtp.Send("correodelosprofesores@gmail.com", "licenciados@outlook.com.ar", correo.Subject, "CuentaDigital está OK:" + "\n" + cuerpo);

                 string alerta1 = @"<script type='text/javascript'>    
                                    alert('Tareas Realizadas');  
                                    </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "", alerta1, false);

            }

            return;
        }

        // no se encuntra

      








    }



}

        