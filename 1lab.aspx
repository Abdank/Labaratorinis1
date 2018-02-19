<%@ Page Language="C#" AutoEventWireup="true" CodeFile="1lab.aspx.cs" Inherits="_1lab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            Duomenys:<asp:Table ID="Table2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" GridLines="Both" Height="30px" HorizontalAlign="Justify" Width="29px">
            </asp:Table>
            <br />
            Rezultatai:<br />
            <asp:Table ID="Table1" runat="server" Height="147px" Width="411px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" GridLines="Both">
            </asp:Table>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Skaičiuoti" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
