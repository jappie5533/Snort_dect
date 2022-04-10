<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Member.master" AutoEventWireup="true" CodeBehind="HubLog.aspx.cs" Inherits="CD2WebApp.Account.HubLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
        GridLines="None" onpageindexchanged="GridView1_PageIndexChanged" 
        onpageindexchanging="GridView1_PageIndexChanging" Width="700px">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="IP" HeaderText="IP">
            <HeaderStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField DataField="StartTime" HeaderText="Start time" />
            <asp:BoundField DataField="TimePeriod" DataFormatString="{0} seconds" 
                HeaderText="Period of Time" />
            <asp:BoundField DataField="City" HeaderText="City">
            <HeaderStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Country" HeaderText="Country">
            <HeaderStyle Width="100px" />
            </asp:BoundField>
        </Columns>
        <EditRowStyle BackColor="#7C6F57" />
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#E3EAEB" />
        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F8FAFA" />
        <SortedAscendingHeaderStyle BackColor="#246B61" />
        <SortedDescendingCellStyle BackColor="#D4DFE1" />
        <SortedDescendingHeaderStyle BackColor="#15524A" />
    </asp:GridView>
</asp:Content>
