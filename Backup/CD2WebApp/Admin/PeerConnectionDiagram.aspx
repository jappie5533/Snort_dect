<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="PeerConnectionDiagram.aspx.cs" Inherits="CD2WebApp.PeerConnectionDiagram" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
<div id="peerconnectiondiagram"></div>
<div id="log"></div>
<div id="inner-details"></div>
<script type="text/javascript">
    DrawConnectionDiagram();
</script>
</asp:Content>
