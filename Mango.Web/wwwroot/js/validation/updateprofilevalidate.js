$(document).ready(function () {
    $("#firstName, #lastName, #birthday, #phoneNumber, #city, #district, #ward").on("blur change", function () {
        validateRegistrationForm();
    });

    function validateRegistrationForm() {
        let firstName = $("#firstName").val().trim();
        let lastName = $("#lastName").val().trim();
        let birthday = $("#birthday").val();
        let phone = $("#phoneNumber").val().trim();
        let city = $("#city").val();
        let district = $("#district").val();
        let ward = $("#ward").val();
        let specificAddress = $("#specificAddress").val().trim(); // Optional address field
        let isValid = true;

        const maxNameLength = 50;
        const phonePattern = /^0\d{9}$/;

        // Get current date for birthday validation
        const today = new Date();
        const birthDate = new Date(birthday);
        const age = today.getFullYear() - birthDate.getFullYear();
        const monthDifference = today.getMonth() - birthDate.getMonth();
        const dayDifference = today.getDate() - birthDate.getDate();
        const isOldEnough = age > 18 || (age === 18 && (monthDifference > 0 || (monthDifference === 0 && dayDifference >= 0)));

        // Validate First Name
        if (firstName === "" || firstName.length > maxNameLength) {
            $("#firstName").siblings("span").text("First Name cannot be empty and must be less than 50 characters.");
            isValid = false;
        } else {
            $("#firstName").siblings("span").text("");
        }

        // Validate Last Name
        if (lastName === "" || lastName.length > maxNameLength) {
            $("#lastName").siblings("span").text("Last Name cannot be empty and must be less than 50 characters.");
            isValid = false;
        } else {
            $("#lastName").siblings("span").text("");
        }

        // Validate Birthday (must be 18+)
        if (birthday === "" || !isOldEnough) {
            $("#birthday").siblings("span").text("You must be at least 18 years old.");
            isValid = false;
        } else {
            $("#birthday").siblings("span").text("");
        }

        // Validate Phone number
        if (!phonePattern.test(phone)) {
            $("#phoneNumber").siblings("span").text("Phone number must be 10 digits and start with 0.");
            isValid = false;
        } else {
            $("#phoneNumber").siblings("span").text("");
        }

        // Validate Province/City
        if (city === "") {
            $("#city").siblings("span").text("You must select a Province/City.");
            isValid = false;
        } else {
            $("#city").siblings("span").text("");
        }

        // Validate District
        if (district === "") {
            $("#district").siblings("span").text("You must select a District.");
            isValid = false;
        } else {
            $("#district").siblings("span").text("");
        }

        // Validate Ward
        if (ward === "") {
            $("#ward").siblings("span").text("You must select a Ward.");
            isValid = false;
        } else {
            $("#ward").siblings("span").text("");
        }

        // Validate Address (Optional specific address field)
        if (specificAddress !== "") {
            $("#specificAddress").siblings("span").text(""); // Clear any validation message for optional field
        }

        // Enable or disable the submit button based on validation
        $("#updateButton").prop("disabled", !isValid);
    }
});
