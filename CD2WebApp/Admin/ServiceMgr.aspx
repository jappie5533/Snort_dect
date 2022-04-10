<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="ServiceMgr.aspx.cs" Inherits="CD2WebApp.Admin.ServiceMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:Label ID="Label1" runat="server" Font-Size="Large" 
    Text="Bootstrap Service:"></asp:Label>
&nbsp;<asp:Label ID="Label2" runat="server" Font-Size="Large" Text="On"></asp:Label>
&nbsp;<asp:Button ID="Button1" runat="server" Text="Start" Width="100px" />
&nbsp;<asp:Button ID="Button2" runat="server" Text="Stop" Width="100px" />
</asp:Content>
