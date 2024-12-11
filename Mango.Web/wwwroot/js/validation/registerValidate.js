$(document).ready(function () {
    // Add event listener for form submission
    $('#registrationForm').on('submit', function (event) {
        let isValid = validateForm(); // Call validation function

        if (!isValid) {
            event.preventDefault(); // Prevent form submission if not valid
        }
    });

    // Function to validate the form
    function validateForm() {
        let isValid = true;

        // First Name validation
        const firstName = $('#FirstName').val().trim();
        const firstNameError = $('#firstNameError');
        if (!firstName || firstName.length > 50) {
            firstNameError.html('First Name must be under 50 characters and cannot be empty.');
            isValid = false;
        } else {
            firstNameError.html('');
        }

        // Last Name validation
        const lastName = $('#LastName').val().trim();
        const lastNameError = $('#lastNameError');
        if (!lastName || lastName.length > 50) {
            lastNameError.html('Last Name must be under 50 characters and cannot be empty.');
            isValid = false;
        } else {
            lastNameError.html('');
        }

        // Email validation
        const email = $('#Email').val().trim();
        const emailError = $('#emailError');
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) {
            emailError.html('Invalid email format.');
            isValid = false;
        } else {
            emailError.html('');
        }

        // Phone Number validation
        const phone = $('#PhoneNumber').val().trim();
        const phoneError = $('#phoneError');
        const phonePattern = /^[0-9]{10}$/;
        if (!phonePattern.test(phone)) {
            phoneError.html('Phone number must be exactly 10 digits.');
            isValid = false;
        } else {
            phoneError.html('');
        }

        // Password validation
        const password = $('#Password').val();
        const passwordError = $('#passwordError');
        let errorMessages = [];
        const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{6,}$/;

        if (!password) {
            errorMessages.push('Password cannot be null.');
        }
        if (password.length < 6) {
            errorMessages.push('Password must be at least 6 characters long.');
        }
        if (!/[A-Z]/.test(password)) {
            errorMessages.push('Password must contain at least 1 uppercase letter.');
        }
        if (!/\d/.test(password)) {
            errorMessages.push('Password must contain at least 1 number.');
        }
        if (!/[@$!%*?&#]/.test(password)) {
            errorMessages.push('Password must contain at least 1 special character.');
        }

        if (errorMessages.length > 0) {
            passwordError.html(errorMessages.join('<br>'));
            isValid = false;
        } else {
            passwordError.html('');
        }

        // Confirm Password validation
        const confirmPassword = $('#confirmPassword').val();
        const confirmPasswordError = $('#confirmPasswordError');
        if (confirmPassword !== password) {
            confirmPasswordError.html('Passwords do not match.');
            isValid = false;
        } else {
            confirmPasswordError.html('');
        }

        // Get current date for birthday validation
        let birthday = $("#birthday").val();
        const today = new Date();
        const birthDate = new Date(birthday);
        const age = today.getFullYear() - birthDate.getFullYear();
        const monthDifference = today.getMonth() - birthDate.getMonth();
        const dayDifference = today.getDate() - birthDate.getDate();
        const isOldEnough = age > 18 || (age === 18 && (monthDifference > 0 || (monthDifference === 0 && dayDifference >= 0)));
        // Validate Birthday (must be 18+)
        if (birthday === "" || !isOldEnough) {
            $("#birthday").siblings("span").text("You must be at least 18 years old.");
            isValid = false;
        } else {
            $("#birthday").siblings("span").text("");
        }

        let city = $("#city").val();
        let district = $("#district").val();
        let ward = $("#ward").val();
        let specificAddress = $("#specificAddress").val().trim(); // Optional address field
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

        let gender = $("#gender").val();
        if (gender === "" || gender === null) {
            $("#gender").siblings("span").text("You must select a gender.");
            isValid = false;
        } else {
            $("#gender").siblings("span").text("");
        }
        // Terms and Conditions validation
        const termsCheck = $('#termsCheck').is(':checked');
        const termsError = $('#termsError');
        if (!termsCheck) {
            termsError.html('You must accept the terms and conditions.');
            isValid = false;
        } else {
            termsError.html('');
        }

        return isValid;
    }

    // Real-time validation with jQuery event listeners
    $('#FirstName, #LastName, #Email, #PhoneNumber, #Password, #confirmPassword, #gender').on('input', validateForm);
    $('#termsCheck').on('change', validateForm);
});
