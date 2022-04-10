<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Member.master" AutoEventWireup="true" CodeBehind="MemberInfo.aspx.cs" Inherits="CD2WebApp.Account.MemberCenter" %>
<asp:Content ContentPlaceHolderID="RightContent" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
    CellPadding="4" ForeColor="#333333" GridLines="None" Height="50px" 
    Width="500px" ondatabound="DetailsView1_DataBound">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
    <EditRowStyle BackColor="#999999" />
    <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" Width="150px" />
    <Fields>
        <asp:BoundField DataField="account" HeaderText="User name(Account)" />
        <asp:HyperLinkField HeaderText="Password" 
            NavigateUrl="~/Account/ChangePassword.aspx" Text="Change password" />
        <asp:BoundField DataField="email" HeaderText="E-mail" />
        <asp:BoundField DataField="ip" HeaderText="Recently login IP" />
        <asp:BoundField DataField="agent_type" HeaderText="Agent type" />
    </Fields>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
</asp:DetailsView>
</asp:Content>