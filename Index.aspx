<% @Page Language="VB" AutoEventWireup="false" CodeFile="~/Index.aspx.vb" Inherits="Index_aspx" %>

<html>
<head>
    <title>PGM Processor</title>
</head>

<body>
    <h1>PGM Processor</h1>

    <form runat="server">
        <input runat="server" id="Operations" type="text" /><br />
        <asp:TextBox TextMode="MultiLine" Columns="80" Rows="30" ID="Pgm" runat="server"></asp:TextBox><br />
        <asp:Button runat="server" id="Submit" Text="Submit" OnClick="Submit_Click"/>
        Transformations performed: <asp:Label ID="NumTransformationsLabel" runat="server"></asp:Label>
    </form>

    

    <textarea cols="80" rows="30" id="Results" runat="server"></textarea>
</body>

</html>