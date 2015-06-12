<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="WFX.aspx.cs" Inherits="WFX" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br /><br />
        <p>Date From: <asp:TextBox ID="txtDateFrom" runat="server"></asp:TextBox></p>
        <p>Date To: <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox></p>

        <asp:Label ID="lblError" runat="server" Text="Label" Visible="false"></asp:Label>

        <asp:Button ID="btnGetTimeForAllStaff" runat="server" Text="GetTimeForAllStaff" OnClick="btnGetTimeForAllStaff_Click" />
    </asp:Content>