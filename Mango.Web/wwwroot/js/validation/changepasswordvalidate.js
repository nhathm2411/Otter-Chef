$(document).ready(function () {
    $("#newPassword, #confirmPassword ,#currentPassword").on("blur", function () {
        validatePasswords();
    });

    function validatePasswords() {
        let currentPassword = $("#currentPassword").val().trim();
        let newPassword = $("#newPassword").val().trim();
        let confirmPassword = $("#confirmPassword").val().trim();
        let isValid = true;

        // Regular expression to check if password contains at least one number and one special character
        const passwordPattern = /^(?=.*[0-9])(?=.*[!@#$%^&*])/;

        // Check if current password is not null and at least 6 characters long
        if (currentPassword === "" || currentPassword.length < 6) {
            $("#currentPassword").siblings("span").text("The current password must be at least 6 characters long.");
            isValid = false;
        } else if (!passwordPattern.test(currentPassword)) {
            $("#currentPassword").siblings("span").text("The password must contain at least one number and one special character.");
            isValid = false;
        } else {
            $("#currentPassword").siblings("span").text("");
        }
        // Check if new password is not null and at least 6 characters long
        if (newPassword === "" || newPassword.length < 6) {
            $("#newPassword").siblings("span").text("The new password must be at least 6 characters long.");
            isValid = false;
        } else if (!passwordPattern.test(newPassword)) {
            $("#newPassword").siblings("span").text("The password must contain at least one number and one special character.");
            isValid = false;
        } else {
            $("#newPassword").siblings("span").text("");
        }

        // Check if confirm password is at least 6 characters long and matches new password
        if (confirmPassword === "" || confirmPassword.length < 6) {
            $("#confirmPassword").siblings("span").text("The confirm password must be at least 6 characters long.");
            isValid = false;
        } else if (newPassword !== confirmPassword) {
            $("#confirmPassword").siblings("span").text("The passwords do not match.");
            isValid = false;
        } else {
            $("#confirmPassword").siblings("span").text("");
        }

        // Enable or disable the submit button
        $("#changedPasswordButton").prop("disabled", !isValid);
    }
});