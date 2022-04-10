<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="CD2WebApp.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <hr />
    <div class="subtitle">Contact us</div>
    <div class="pageinfo">
        
        If you have any inquiries, please feel free to get in touch with us!<br />
        <br />
        <b>Development &amp; tech. support</b><br />
        E-mail: 
        <asp:HyperLink ID="HyperLink4" runat="server" 
            NavigateUrl="mailto:cclien0725@gmail.com">cclien0725@gmail.com</asp:HyperLink><br />
        E-mail: 
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="mailto:rockers7414@gmail.com">rockers7414@gmail.com</asp:HyperLink>
        
    </div>
</asp:Content>
