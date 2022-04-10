<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Member.master" AutoEventWireup="true" CodeBehind="UsingLog.aspx.cs" Inherits="CD2WebApp.Account.UsingLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Date: "></asp:Label>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;~
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" Text="Search" 
        onclick="Button1_Click" />
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
        TargetControlID="TextBox1">
    </ajaxToolkit:CalendarExtender>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
        TargetControlID="TextBox2">
    </ajaxToolkit:CalendarExtender>
    <hr />
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                GridLines="None" onpageindexchanged="GridView1_PageIndexChanged" 
                onpageindexchanging="GridView1_PageIndexChanging" 
                onrowcommand="GridView1_RowCommand" PageSize="20" Width="560px">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DT") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" 
                                CommandArgument='<%# bind("dt") %>' CommandName="selectLogByDate" 
                                Text='<%# bind("dt") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="src" HeaderText="Peer ID" />
                    <asp:BoundField DataField="DataLength" DataFormatString="{0} Bytes" 
                        HeaderText="Data size" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <EmptyDataTemplate>
                    No Data.
                </EmptyDataTemplate>
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
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                GridLines="None" onpageindexchanged="GridView2_PageIndexChanged" 
                onpageindexchanging="GridView2_PageIndexChanging" PageSize="20" 
                Width="560px" ondatabound="GridView2_DataBound">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ts") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ts") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="src" HeaderText="Peer ID" />
                    <asp:BoundField DataField="datalength" DataFormatString="{0} Bytes" 
                        HeaderText="Data Size" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <EmptyDataTemplate>
                    No Data.
                </EmptyDataTemplate>
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
            <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">&lt; Back</asp:LinkButton>
        </asp:View>
    </asp:MultiView>
</asp:Content>
