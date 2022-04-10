$(function () {
    $(document).ready(function () {
        $("#MainContent_RightContent_TextBox1").live('focusout', function () {
            $("#MainContent_RightContent_TextBox2").attr('value', $("#MainContent_RightContent_TextBox1").val());
        });
    });
});