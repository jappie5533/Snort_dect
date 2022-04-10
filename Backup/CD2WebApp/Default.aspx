<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="CD2WebApp._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 351px;
            height: 222px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Collaborative Applications Platform!
    </h2>
    <hr />
    <div class="pageinfo">
        The feature of collaborative applications is to cooperation among joining nodes by utilizing various of resources distributed nodes over internet (or intranet). It is good choice to develop collaborative applications based on P2P networks system because Peer-to-Peer (P2P) networks system can be applied to integrate resources over peers. But most of popular P2P systems are focused on files or content sharing and security problems are not considered seriously. It's not enough to develop collaborative applications by current P2P systems which have been implemented. We setup secure P2P networks by authentication of joining peers, encrypted data communication and peers with three levels of priorities. Based on secure P2P networks, a scalable and flexible collaborative application platform composed of core services and user defined services is built. Various of resources provided by peers can be utilized easily by execution of services. Users can develop their creative collaborative applications by our proposed collaborative applications platform.<br />
        <br />
        <img alt="system_arch" class="style1" 
            src="image/System%20P2P%20Platform%20Architecture.jpg" /><br />
        <br />
        Microsoft Windows 8, Windows 7, Vista, and XP system with .Net framework 4.0 are supported in our collaborative application platform.
    </div>
</asp:Content>
