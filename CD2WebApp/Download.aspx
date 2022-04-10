<%@ Page Title="Download" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="CD2WebApp.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Download
    </h2>
    <hr />
    <div class="subtitle">Latest Version</div>
    <div class="pageinfo">
        <div class="filetitle">Co-Defend Platform</div>
        <div class="fileinfo">Version: 1.0</div>
        <div class="fileinfo">Release date: 2013/03/11</div>
        <div class="fileinfo">Release notes:
            <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/">here</asp:HyperLink>
        </div>
        <div>Runs on Microsoft Windows 8, 7, Vista and XP. Including both 32-bit and 64-bit version.</div>
&nbsp;<asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/">Click here to download</asp:HyperLink>
    </div>
    <div class="subtitle">Old Versions</div>
    <div class="pageinfo">
    </div>
</asp:Content>
