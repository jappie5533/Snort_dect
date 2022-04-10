<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="HubLog.aspx.cs" Inherits="CD2WebApp.Admin.HubLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" Width="150px"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" Text="Search" 
        onclick="Button1_Click" />
<hr />
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
    GridLines="None" Width="750px" onpageindexchanged="GridView1_PageIndexChanged" 
        onpageindexchanging="GridView1_PageIndexChanging" PageSize="20">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <Columns>
        <asp:BoundField DataField="IP" HeaderText="IP">
        <HeaderStyle Width="100px" />
        </asp:BoundField>
        <asp:BoundField DataField="Account" HeaderText="Account" />
        <asp:BoundField DataField="StartTime" HeaderText="Start Time" />
        <asp:BoundField DataField="TimePeriod" DataFormatString="{0} seconds" 
            HeaderText="Period of Time" />
        <asp:BoundField DataField="City" HeaderText="City">
        <HeaderStyle Width="80px" />
        </asp:BoundField>
        <asp:BoundField DataField="Country" HeaderText="Country">
        <HeaderStyle Width="60px" />
        </asp:BoundField>
    </Columns>
    <EditRowStyle BackColor="#999999" />
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
