// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("newPassword").on("change", function () {
        var spanTag = $(this).parents("div").find("span");
        if ($(this).val() === "") {
            spanTag.text("The new password is not valid!");
        } else {
            spanTag.text("");
        }
    })
    $(document).ready(function () {
        $("#changedPasswordButton").on("click", function (e) {
            e.preventDefault();

        })
    });
})