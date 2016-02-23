<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CuentaDigital.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title>Boton Cargar Cuenta Digital</title>

    <script src="http://code.jquery.com/jquery-1.12.0.min.js"></script>

    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" integrity="sha256-7s5uDGW3AHqw6xtJmNNtr+OBRJUlgkNJEo78P4b0yRw= sha512-nNo+yCHEyn0smMxSswnf/OnX6/KwJuZTlNZBjauKhTK0c+zT+q5JOCx0UFhXQ6rJR9jg6Es8gPuD2uZcYDLqSw==" crossorigin="anonymous"/> 
    
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>

</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-xs-4"></div>
                <div class="col-xs-4" style="margin-top: 30px">

                    <asp:Calendar ID="Calendario" Style="width: 100%" runat="server" BackColor="White" BorderColor="#e2e2e2" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
                        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#999999" />
                        <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                        <TitleStyle BackColor="#e2e2e2" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                        <TodayDayStyle BackColor="#CCCCCC" />
                    </asp:Calendar>

                </div>
                <div class="col-xs-4"></div>
            </div>

            <div class="row">
                <div class="col-xs-4"></div>
                <div class="col-xs-4">
                    <asp:Button ID="Button1" runat="server" Text="Actualizar Pago" OnClick="Button1_Click" CssClass="btn btn-success" Style="margin-top: 30px; width: 100%" />
                </div>
                <div class="col-xs-4"></div>
            </div>


        </div>
    </form>
</body>
</html>
