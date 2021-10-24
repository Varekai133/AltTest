<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="AltTest.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        var xhr;
        var data = new Object();
        function ProcessImageByUrl() {
            if (xhr && xhr.readyState != 4) {
                xhr.abort();
            }
            data.url = $("#<%=UploadImageTextBox.ClientID%>")[0].value;
            data.dropboxValue = $("#<%=OptionDropDownList.ClientID%> option:selected").val();
            xhr = $.ajax({
                type: "POST",
                url: "index.aspx/ProcessImage",
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#ProcessedImage").attr("src", "data:image/png;base64," + response.d);
                }
            });
        }
    </script>

    <link href ="style.css" rel="stylesheet" />
</head>
<body>
    <h1>Image Processing</h1>
    <hr />
    <form id="mainform" runat="server">
        <div>
            <table class ="center">
                <tr>
                    <td>
                        <asp:TextBox ID="UploadImageTextBox" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="OptionDropDownList" runat="server">
                            <asp:ListItem Text="Gray" Value="GrayValue"></asp:ListItem>
                            <asp:ListItem Text="SwapGandB" Value="SwapGandBValue"></asp:ListItem>
                        </asp:DropDownList>
                        <input type="button" id="ProcessImageByUrlButton" onclick = "ProcessImageByUrl()" value="Process""/>
                    </td>
                </tr>
                <tr>
                    <th align="center"><u>Processed Image</u></th>
                </tr>
                <tr>
                    <td ><img id="ProcessedImage" height ="240px" width ="320px"/></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
