<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="HubMgr.aspx.cs" Inherits="CD2WebApp.Admin.HubMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Search" />
    <hr />
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
    GridLines="None" AllowPaging="True" AllowSorting="True" 
        AutoGenerateColumns="False" onpageindexchanged="GridView1_PageIndexChanged" 
        onpageindexchanging="GridView1_PageIndexChanging" onsorted="GridView1_Sorted" 
        onsorting="GridView1_Sorting" Width="600px">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" />
            <asp:BoundField DataField="ip" HeaderText="IP" SortExpression="ip" />
            <asp:BoundField DataField="account" HeaderText="Account" />
            <asp:BoundField DataField="City" HeaderText="City" />
            <asp:BoundField DataField="country" HeaderText="Country" />
        </Columns>
    <EditRowStyle BackColor="#999999" />
        <EmptyDataTemplate>
            <asp:Label ID="Label1" runat="server" Text="No Data."></asp:Label>
        </EmptyDataTemplate>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</asp:Content>
